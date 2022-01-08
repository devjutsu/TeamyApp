using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Teamy.Server.Data;
using Teamy.Server.Services;
using Teamy.Server.Models;

namespace Teamy.Server.Logic
{
    public interface IManageEvents 
    {
        Task<Event> Get(Guid id, string userId);
        Task<List<Event>> GetUpcoming(string userId);
        Task<Event> CreateEvent(Event evt, string userId, string? imageUrl);
        Task<Event> UpdateEvent(Event evt, string userId, string? imageUrl);
        Task<Event> GetInvitedEvent(string inviteCode);
        Task<ProposedDate> RecommendDate(ProposedDate newDate);
        Task<bool> DeleteRecommendedDate(Guid proposedDateId, string createdById);
        Task<ProposedDate> UpdateRecommendedDate(ProposedDate newDate);
    }

    public class EventLogic : IManageEvents
    {
        ILogger<EventLogic> _logger { get; set; }
        TeamyDbContext _db { get; set; }
        private readonly IMapper _mapper;
        IVoteHub _hub;
        public EventLogic(ILogger<EventLogic> logger, TeamyDbContext db, IMapper mapper, IVoteHub hub)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
            _hub = hub;
        }
        
        public async Task<Event> Get(Guid id, string userId)
        {
            return await _db.Events
                                .Include(_ => _.Participants)
                                .ThenInclude(z => z.User)
                                .Include(_ => _.Polls)
                                .ThenInclude(_ => _.Choices)
                                .ThenInclude(_ => _.Answers)
                                .Include(_ => _.Invites)
                                .Include(_ => _.CoverImage)
                                .Include(_ => _.CreatedBy)
                                .Include(_ => _.ProposedDates.OrderBy(d => d.Date))
                                .ThenInclude(_ => _.Votes)
                                .ThenInclude(_ => _.User)
                                .Where(_ => _.CreatedById == userId || _.Participants.Any(p => p.UserId == userId))
                                .FirstAsync(o => o.Id == id);
        }

        public async Task<List<Event>> GetUpcoming(string userId)
        {
            return await _db.Events
                                .Include(_ => _.Participants)
                                .ThenInclude(z => z.User)
                                .Include(_ => _.Polls)
                                .ThenInclude(_ => _.Choices)
                                .ThenInclude(_ => _.Answers)
                                .Include(_ => _.Invites)
                                .Include(_ => _.CoverImage)
                                .Include(_ => _.CreatedBy)
                                .Include(_ => _.ProposedDates.OrderBy(d => d.Date))
                                .ThenInclude(_ => _.Votes)
                                .ThenInclude(_ => _.User)
                                .Where(_ => _.CreatedById == userId || _.Participants.Any(p => p.UserId == userId))
                                .OrderBy(_ => _.EventDate)
                                .ToListAsync();
        }
        
        public async Task<Event> CreateEvent(Event evt, string userId, string? imageUrl)
        {
            if (string.IsNullOrEmpty(userId))
            {
                var user = await _db.Users.AddAsync(new AppUser()
                {
                    DisplayName = "anonymous"
                });
                userId = user.Entity.Id;
                await _db.SaveChangesAsync();
            }

            evt.CreatedById = userId;
            evt.CreatedBy = null;
            evt.Invites = new List<Invite>() { new Invite() { InviteCode = GenerateInvite(), InvitedById = userId, Public = true } };
            if (string.IsNullOrEmpty(imageUrl))
                evt.CoverImage = null;

            var addedEvt = _db.Events.Add(evt);
            await _db.SaveChangesAsync();
            return addedEvt.Entity;
        }

        public async Task<Event> UpdateEvent(Event evt, string userId, string? imageUrl)
        {
            var existingEvent = _db.Events
                                    .Include(_ => _.Polls)
                                    .ThenInclude(_ => _.Choices)
                                    .ThenInclude(_ => _.Answers)
                                    .Include(_ => _.CoverImage)
                                    .Include(_ => _.ProposedDates)
                                    .First(_ => _.Id == evt.Id);

            if (existingEvent.CoverImage?.Url != imageUrl)
            {
                if (imageUrl == null)
                    existingEvent.CoverImageId = null;
                else
                    existingEvent.CoverImage = new ImageModel() { Url = imageUrl };
            }
            existingEvent.Polls = evt.Polls;

            foreach (var date in evt.ProposedDates)
            {
                foreach (var vote in date.Votes)
                {
                    vote.User = null;
                }
            }
            existingEvent.ProposedDates = evt.ProposedDates;

            existingEvent.Title = evt.Title;
            existingEvent.Description = evt.Description;
            existingEvent.EventDate = evt.EventDate;
            existingEvent.EventDateTo = evt.EventDateTo;
            existingEvent.Where = evt.Where;
            existingEvent.DateStatus = evt.DateStatus;
            existingEvent.DateRecommendationType = evt.DateRecommendationType;

            var existingProposedDates = _db.ProposedDates.Where(o => o.EventId == existingEvent.Id);
            _db.ProposedDates.RemoveRange(existingProposedDates);
            var e = _db.Events.Update(existingEvent);

            await _db.SaveChangesAsync();
            await _hub.EventUpdated(e.Entity.Id);
            return e.Entity;
        }

        public async Task<Event> GetInvitedEvent(string inviteCode)
        {
            var evt = await _db.Events
                            .Include(_ => _.Participants)
                            .ThenInclude(z => z.User)
                            .Include(_ => _.Polls)
                            .ThenInclude(_ => _.Choices)
                            .ThenInclude(_ => _.Answers)
                            .Include(_ => _.Invites)
                            .Include(_ => _.CoverImage)
                            .Include(_ => _.CreatedBy)
                            .Include(_ => _.Participants)
                            .Include(_ => _.ProposedDates)
                            .FirstAsync(o => o.Invites.Any(o => o.InviteCode == inviteCode));

            return evt;
        }

        public async Task<ProposedDate> RecommendDate(ProposedDate newDate)
        {
            var entity = await _db.ProposedDates.AddAsync(newDate);
            await _db.SaveChangesAsync();
            await _hub.EventUpdated(entity.Entity.EventId);
            return entity.Entity;
        }

        public async Task<ProposedDate> UpdateRecommendedDate(ProposedDate newDate)
        {
            var existingDate = await _db.ProposedDates
                                         .Where(o => o.Id == newDate.Id && o.CreatedById == newDate.CreatedById)
                                         .FirstOrDefaultAsync();
            if (existingDate != null)
            {
                existingDate.Date = newDate.Date;
                existingDate.DateTo = newDate.DateTo;

                _db.ProposedDates.Update(existingDate);

                await _db.SaveChangesAsync();
                await _hub.EventUpdated(existingDate.EventId);
                return existingDate;
            }
            else return null;
        }

        public async Task<bool> DeleteRecommendedDate(Guid proposedDateId, string createdById)
        {
            var existingDate = await _db.ProposedDates
                                        .Where(o => o.Id == proposedDateId && o.CreatedById == createdById)
                                        .FirstOrDefaultAsync();
            if (existingDate != null)
            {
                _db.ProposedDates.Remove(existingDate);

                await _db.SaveChangesAsync();
                await _hub.EventUpdated(existingDate.EventId);
                return true;
            }
            else return false;
        }

        private string GenerateInvite()
        {
            // use MlkPwgen
            // PasswordGenerator.Generate(length: 10, allowed: Sets.Alphanumerics);
            var newInvite = Guid.NewGuid().ToString().Substring(0, 8);
            while (_db.Invites.Any(o => o.InviteCode == newInvite))
            {
                newInvite = Guid.NewGuid().ToString().Substring(0, 8);
            }
            return newInvite;
        }
    }

}