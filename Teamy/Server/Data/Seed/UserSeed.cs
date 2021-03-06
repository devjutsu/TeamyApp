using Teamy.Server.Models;
using Teamy.Server.Models.Polls;
using Teamy.Shared.Common;

namespace Teamy.Server.Data.Seed
{
    public class UserSeed
    {
        TeamyDbContext db;
        public UserSeed(TeamyDbContext ctx)
        {
            db = ctx;
        }

        public void Seed()
        {
            // Users

            var garuda = db.Users.Add(new AppUser
            {
                Email = "algarud@gmail.com",
                DisplayName = "Albert Garuda",
                UserName = "algarud@gmail.com",
                PasswordHash = "AQAAAAEAACcQAAAAEGLLKzxeUXNc64N7RepBTAjo/hV1aaRD504PZxTgy8bklhCDiyXQnfNTZUJPypSuCQ==",
                EmailConfirmed = true,
                NormalizedEmail = "algarud@gmail.com".ToUpper(),
                NormalizedUserName = "algarud@gmail.com".ToUpper(),
            });
            var enemy = db.Users.Add(new AppUser
            {
                Email = "enemy@teamy.one",
                DisplayName = "Public Enemy",
                UserName = "enemy@teamy.one",
                PasswordHash = "AQAAAAEAACcQAAAAEInNniuMwdacNlHRWsJTXVyfp3MqZcTzl/DeDGGCICWKk+ZNKF/1HVdx2oh/UujC+g==",
                EmailConfirmed = true,
                NormalizedEmail = "enemy@teamy.one".ToUpper(),
                NormalizedUserName = "enemy@teamy.one".ToUpper()
            });
            var slim = db.Users.Add(new AppUser
            {
                Email = "slim@teamy.one",
                DisplayName = "Slim Shady",
                UserName = "slim@teamy.one",
                PasswordHash = "AQAAAAEAACcQAAAAEInNniuMwdacNlHRWsJTXVyfp3MqZcTzl/DeDGGCICWKk+ZNKF/1HVdx2oh/UujC+g==",
                EmailConfirmed = true,
                NormalizedEmail = "slim@teamy.one".ToUpper(),
                NormalizedUserName = "slim@teamy.one".ToUpper()
            });

            // Poker Event

            var poker = db.Events.Add(new Event
            {
                Invites = new List<Invite>() { new Invite() { InviteCode = Guid.NewGuid().ToString().Substring(0, 8), InvitedById = garuda.Entity.Id, Public = true } },
                CreatedById = garuda.Entity.Id,
                Title = "Poker Night",
                EventDate = null,
                ProposedDates = new List<ProposedDate>() { 
                    new ProposedDate() { Date = DateTime.Today.AddDays(7).AddHours(18).AddMinutes(30), DateTo = DateTime.Today.AddDays(7).AddHours(20).AddMinutes(30)},
                    new ProposedDate() { Date = DateTime.Today.AddDays(10).AddHours(18), DateTo = DateTime.Today.AddDays(10).AddHours(20) }, 
                    new ProposedDate() { Date = DateTime.Today.AddDays(15).AddHours(15), DateTo = DateTime.Today.AddDays(15).AddHours(17) } 
                },
                DateStatus = EventDateStatus.Voting,
                Where = "Office conference room",
                Description = "Tournament: The winner takes it all, others just have a good time and some drinks",
                CoverImage = new ImageModel() { Url = "https://squalldiag.blob.core.windows.net/template-img/pokah.jpg" }
            });

            db.Participation.Add(new Participation
            {
                EventId = poker.Entity.Id,
                InviteId = poker.Entity.Invites.First().Id,
                Status = ParticipationStatus.Accept,
                UserId = enemy.Entity.Id,
            });

            db.Participation.Add(new Participation
            {
                EventId = poker.Entity.Id,
                InviteId = poker.Entity.Invites.First().Id,
                Status = ParticipationStatus.Accept,
                UserId = slim.Entity.Id,
            });

            db.Polls.Add(new Poll()
            {
                EventId = poker.Entity.Id,
                Question = "What stakes?",
                Choices = new List<PollChoice> {
                                            new PollChoice() { Choice = "Eur 10 buy in" },
                                            new PollChoice() { Choice = "Eur 20 buy in" },
                                            new PollChoice() { Choice = "Eur 50 buy in" },
                                        },
                MultiChoice = false,
                FreeTextOption = false
            });

            db.Polls.Add(new Poll()
            {
                EventId = poker.Entity.Id,
                Question = "Drink preferrences?",
                Choices = new List<PollChoice> {
                                            new PollChoice() { Choice = "Lots of coffee" },
                                            new PollChoice() { Choice = "Beer" },
                                            new PollChoice() { Choice = "Coca cola" } ,
                                            new PollChoice() { Choice = "Jack Daniels" }
                                        },
                MultiChoice = true,
                FreeTextOption = true
            });

            // Bicycle Ride

            var bicycle = db.Events.Add(new Event
            {
                Invites = new List<Invite>() { new Invite() { InviteCode = Guid.NewGuid().ToString().Substring(0, 8), InvitedById = garuda.Entity.Id, Public = true } },
                CreatedById = garuda.Entity.Id,
                Title = "Bicycle ride",
                ProposedDates = new List<ProposedDate>() { 
                    new ProposedDate() { Date = DateTime.Today.AddDays(7).AddHours(15), DateTo = DateTime.Today.AddDays(7).AddHours(17)},
                    new ProposedDate() { Date = DateTime.Today.AddDays(10).AddHours(13).AddMinutes(30), DateTo = DateTime.Today.AddDays(10).AddHours(15).AddMinutes(30) }, 
                    new ProposedDate() { Date = DateTime.Today.AddDays(15).AddHours(13), DateTo = DateTime.Today.AddDays(17).AddHours(13) } 
                },
                DateStatus = EventDateStatus.Voting,
                Description = "I want to ride my bicycle",
                Where = "Somewhere over the rainbow",
                CoverImage = new ImageModel() { Url = "https://squalldiag.blob.core.windows.net/template-img/bicycle.jpg" }
            });

            db.Participation.Add(new Participation
            {
                EventId = bicycle.Entity.Id,
                InviteId = bicycle.Entity.Invites.First().Id,
                Status = ParticipationStatus.Accept,
                UserId = enemy.Entity.Id,
            });

            db.Participation.Add(new Participation
            {
                EventId = bicycle.Entity.Id,
                InviteId = bicycle.Entity.Invites.First().Id,
                Status = ParticipationStatus.Accept,
                UserId = slim.Entity.Id,
            });

            db.Polls.Add(new Poll()
            {
                EventId = bicycle.Entity.Id,
                Question = "How far you want to go?",
                Choices = new List<PollChoice> {
                                            new PollChoice() { Choice = "20 km" },
                                            new PollChoice() { Choice = "30 km" },
                                            new PollChoice() { Choice = "50 km" },
                                            new PollChoice() { Choice = "100 km" },
                                            new PollChoice() { Choice = "Till I fall" }
                                        },
                MultiChoice = true,
                FreeTextOption = true
            });

            db.Polls.Add(new Poll()
            {
                EventId = bicycle.Entity.Id,
                Question = "Your preferred riding style?",
                Choices = new List<PollChoice> {
                                            new PollChoice() { Choice = "Sightseeng and chatting" },
                                            new PollChoice() { Choice = "Speed travel" },
                                            new PollChoice() { Choice = "Crazy hills workout" },
                                        },
                MultiChoice = false,
                FreeTextOption = false
            });

            // Hacking Night

            var hacking = db.Events.Add(new Event
            {
                Invites = new List<Invite>() { new Invite() { InviteCode = Guid.NewGuid().ToString().Substring(0, 8), InvitedById = garuda.Entity.Id, Public = true } },
                CreatedById = garuda.Entity.Id,
                Title = "Hacking night",
                EventDate = DateTime.Now.AddDays(20).AddHours(20),
                EventDateTo = DateTime.Now.AddDays(20).AddHours(23).AddMinutes(59),
                DateStatus = EventDateStatus.Locked,
                Description = "Creating cool stuff and Banja right after",
                Where = "YogaLab countryside workshop facility",
                CoverImage = new ImageModel() { Url = "https://squalldiag.blob.core.windows.net/template-img/hackerman.jpg" }
            }); ;

            db.Participation.Add(new Participation
            {
                EventId = hacking.Entity.Id,
                InviteId = hacking.Entity.Invites.First().Id,
                Status = ParticipationStatus.Reject,
                UserId = enemy.Entity.Id,
            });

            db.Participation.Add(new Participation
            {
                EventId = hacking.Entity.Id,
                InviteId = hacking.Entity.Invites.First().Id,
                Status = ParticipationStatus.Reject,
                UserId = slim.Entity.Id,
            });

            db.Polls.Add(new Poll()
            {
                EventId = hacking.Entity.Id,
                Question = "What would you like to focus on?",
                Choices = new List<PollChoice> {
                                            new PollChoice() { Choice = "Sliced Architecture and Refactoring" },
                                            new PollChoice() { Choice = "Infrastructure code" },
                                            new PollChoice() { Choice = "Frontend" } ,
                                            new PollChoice() { Choice = "Usability and Product Design" }
                                        },
                MultiChoice = true,
                FreeTextOption = true
            });

            db.Polls.Add(new Poll()
            {
                EventId = hacking.Entity.Id,
                Question = "Spaces or Tabs?",
                Choices = new List<PollChoice> {
                                            new PollChoice() { Choice = "Spaces" },
                                            new PollChoice() { Choice = "Tabs" },
                                            new PollChoice() { Choice = "Jack Daniels" }
                                        },
                MultiChoice = false,
                FreeTextOption = false
            });
        }
    }
}
