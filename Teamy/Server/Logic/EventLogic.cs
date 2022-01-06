using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Teamy.Server.Data;
using Teamy.Server.Services;
using Teamy.Server.Models;

namespace Teamy.Server.Logic
{
    public interface IManageEvents 
    {
        Task<bool> DeleteRecommendedDate(Guid proposedDateId, string createdById);
        Task<ProposedDate> UpdateRecommendedDate(ProposedDate newDate);
    }
    public class EventLogic : IManageEvents
    {
        ILogger<EventLogic> _logger { get; set; }
        TeamyDbContext _db { get; set; }
        private readonly IMapper _mapper;
        IVoteHub _hub;
        public EventLogic(ILogger<EventLogic> logger,
                                    TeamyDbContext db,
                                    IMapper mapper,
                                    IVoteHub hub)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
            _hub = hub;
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
    }

}