using Teamy.Server.Models;
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
            });;
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
                When = DateTime.Now.AddDays(30),
                Where = "Office conference room",
                Description = "Tournament: The winner takes it all, others just have a good time and some drinks",
                CoverImage = new ImageModel() { Url = "https://images8.alphacoders.com/642/642626.jpg" }
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
                When = DateTime.Now.AddDays(10),
                Description = "I want to ride my bicycle",
                Where = "Somewhere over the rainbow",
                CoverImage = new ImageModel() { Url = "https://img3.goodfon.com/wallpaper/nbig/1/47/nebo-velosiped-uniforma.jpg" }
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
                Question = "Where would you like to go?",
                Choices = new List<PollChoice> {
                                            new PollChoice() { Choice = "Track to Jurmala" },
                                            new PollChoice() { Choice = "Ogres Zilie Kalni" },
                                            new PollChoice() { Choice = "Sigulda" },
                                            new PollChoice() { Choice = "Ergļi" }
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
                Invites = new List<Invite>() { new Invite() { InviteCode = Guid.NewGuid().ToString().Substring(0,8), InvitedById = garuda.Entity.Id, Public = true} },
                CreatedById = garuda.Entity.Id,
                Title = "Hacking night",
                When = DateTime.Now.AddDays(20),
                Description = "Creating cool stuff and Banja right after",
                Where = "YogaLab countryside workshop facility",
                CoverImage = new ImageModel() { Url = "https://i.pinimg.com/originals/71/85/0c/71850c4d6cd020d740e21c0b2b030acb.jpg" }
            });

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
