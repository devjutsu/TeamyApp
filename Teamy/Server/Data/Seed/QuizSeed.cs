﻿using Teamy.Server.Models;
using Teamy.Server.Models.Quizes;
using Teamy.Server.Models.Templates;
using Teamy.Shared.Common;

namespace Teamy.Server.Data.Seed
{
    public class QuizSeed
    {
        TeamyDbContext db;
        public QuizSeed(TeamyDbContext ctx)
        {
            db = ctx;
        }

        public void Seed()
        {
            var garuda = db.Users.FirstOrDefault(o => o.UserName == "algarud@gmail.com");
            var enemy = db.Users.FirstOrDefault(o => o.UserName == "enemy@teamy.one");
            var slim = db.Users.FirstOrDefault(o => o.UserName == "slim@teamy.one");

            var quiz = db.Quiz.Add(new Quiz()
            {
                CreatorId = garuda.Id,
                Title = "First seeded Quiz ever",
                Image = new ImageModel() { Url = "https://www.incimages.com/uploaded_files/image/1920x1080/getty_495193237_2000148420009280290_181621.jpg" },
                Details = "Please, answer a few questions to help us improve this tool for your needs.",
                Questions = new List<QuizQuestion>()
                {
                    new QuizQuestion()
                    {
                        Order = 0,
                        Question = "Where do you want to go today?",
                        Type = QuizElementType.SingleSelectQuestion,
                        Choices = new List<QuizChoice>()
                        {
                            new QuizChoice() {Choice = "Home"},
                            new QuizChoice() {Choice = "Work"},
                            new QuizChoice() {Choice = "Party"},
                            new QuizChoice() {Choice = "Mob programming"},
                        },
                        Answers = new List<QuizAnswer>()
                        {
                            new QuizAnswer()
                            {
                                Answer = "Mob programming",
                                UserId = garuda.Id,
                            },
                            new QuizAnswer()
                            {
                                Answer = "Home",
                                UserId = enemy.Id,
                            },
                            new QuizAnswer()
                            {
                                Answer = "Party",
                                UserId = slim.Id,
                            }
                        },
                    },
                    new QuizQuestion()
                    {
                        Order = 1,
                        Question = "Do you like talking to people?",
                        Type = QuizElementType.MultiSelectQuestion,
                        Choices= new List<QuizChoice>()
                        {
                            new QuizChoice() { Choice = "Yes"},
                            new QuizChoice() { Choice = "No"},
                            new QuizChoice() { Choice = "Depends"},
                        },
                        Answers= new List<QuizAnswer>()
                        {
                            new QuizAnswer() {Answer = "Depends", UserId = garuda.Id },
                            new QuizAnswer() {Answer = "No", UserId = enemy.Id },
                            new QuizAnswer() {Answer = "Yes", UserId = slim.Id },
                        }
                    },
                    new QuizQuestion()
                    {
                        Order = 2,
                        Question = "What do you think of this?",
                        Type = QuizElementType.FreeTextQuestion,
                        Answers= new List<QuizAnswer>()
                        {
                            new QuizAnswer() { UserId = garuda.Id, Answer = "Hello, World!" },
                            new QuizAnswer() { UserId = enemy.Id, Answer = "Hello teacher what's my lesson?" },
                            new QuizAnswer() { UserId = slim.Id, Answer = "Code time!"}
                        }
                    },
                    new QuizQuestion()
                    {
                        Order = 3,
                        Question = "How are you feeling today?",
                        Type = QuizElementType.GradeQuestion,
                        Answers = new List<QuizAnswer>()
                        {
                            new QuizAnswer() { UserId = garuda.Id, Answer = "5"},
                            new QuizAnswer() { UserId = enemy.Id, Answer = "3"},
                            new QuizAnswer() { UserId = slim.Id, Answer = "4"}
                        }
                    },
                    new QuizQuestion()
                    {
                        Order = 4,
                        Question = "Information block to let you know!",
                        Type = QuizElementType.InformationOnly,
                        Choices = new List<QuizChoice>()
                        {
                            new QuizChoice() {Choice = "1. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},
                            new QuizChoice() {Choice = "2. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},
                            new QuizChoice() {Choice = "3. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},
                        }
                    },
                    new QuizQuestion()
                    {
                        Order = 5,
                        Question = "https://img-9gag-fun.9cache.com/photo/a0PbZdL_460s.jpg",
                        Type = QuizElementType.Picture,
                    },
                },
                Completions = new List<QuizCompletion>()
                {
                    new QuizCompletion()
                    {
                        Status = QuizCompletionStatus.Submitted,
                        UserId = garuda.Id,
                    },
                    new QuizCompletion()
                    {
                        Status = QuizCompletionStatus.Entered,
                        UserId = enemy.Id,
                    },
                    new QuizCompletion()
                    {
                        Status = QuizCompletionStatus.Submitted,
                        UserId = slim.Id,
                    }
                }
            });

            db.QCodes.Add(new QCode()
            {
                Id = Guid.NewGuid().ToString().Substring(0, 8),
                Quiz = quiz.Entity
            });
            db.QCodes.Add(new QCode()
            {
                Id = Guid.NewGuid().ToString().Substring(0, 8),
                Quiz = quiz.Entity
            });
            db.QCodes.Add(new QCode()
            {
                Id = Guid.NewGuid().ToString().Substring(0, 8),
                Quiz = quiz.Entity
            });


            var publicQuiz = db.Quiz.Add(new Quiz()
            {
                CreatorId = garuda.Id,
                Title = "Improving socialization",
                Image = new ImageModel() { Url = "https://www.incimages.com/uploaded_files/image/1920x1080/getty_495193237_2000148420009280290_181621.jpg" },
                Details = "Please, answer a few questions to help us improve this tool for your needs.",
                Questions = new List<QuizQuestion>()
                {
                    new QuizQuestion()
                    {
                        Order = 0,
                        Question = "How are you feeling today?",
                        Type = QuizElementType.GradeQuestion,
                    },
                    new QuizQuestion()
                    {
                        Order= 1,
                        Question = "Do you find it sometimes challenging to plan time together with your family, friends or acquaintances?",
                        Type = QuizElementType.SingleSelectQuestion,
                        Choices = new List<QuizChoice>()
                        { 
                            new QuizChoice() { Choice = "Mostly challenging" },
                            new QuizChoice() { Choice = "Sometimes challenging" },
                            new QuizChoice() { Choice = "No problem at all" },
                            new QuizChoice() { Choice = "I never plan such things, because I have no people to spend time with" },
                            new QuizChoice() { Choice = "I never plan such things, because I'm not interested in socializing" },
                        }
                    },
                    new QuizQuestion()
                    {
                        Order= 2,
                        Question = "What difficulties you might face?",
                        Type = QuizElementType.MultiSelectQuestion,
                        Choices = new List<QuizChoice>()
                        {
                            new QuizChoice() { Choice = "It's hard to find the time that is OK for everyone" },
                            new QuizChoice() { Choice = "Not always clear what exactly to do, what is interesting for everyone" },
                            new QuizChoice() { Choice = "Hard to come up with such event details that is suitable for everyone" },
                            new QuizChoice() { Choice = "It takes too much initiative to create such an event" },
                            new QuizChoice() { Choice = "It might be complicated to ask questions, discuss or notify everyone " },
                            new QuizChoice() { Choice = "It's hard to plan, because we feel very much disconnected" },
                            new QuizChoice() { Choice = "Difficult, because of other reasons" },
                            new QuizChoice() { Choice = "Easy to plan, we are often in-touch" },
                            new QuizChoice() { Choice = "No difficulties, because I only want to meet with people I know very well, and it's easy to arrange" },
                            new QuizChoice() { Choice = "No difficulties, because I just plan and don't ask if someone has different oppinion" },
                            new QuizChoice() { Choice = "No difficulties, because I never plan any activities together" },
                            new QuizChoice() { Choice = "No difficulties, other reasons" },
                        }
                    },
                    new QuizQuestion()
                    {
                        Order = 3,
                        Question = "Any comments on that?",
                        Type = QuizElementType.FreeTextQuestion,
                    },
                    new QuizQuestion()
                    {
                        Order = 4,
                        Question = "If you do so, then what tools do you use for planning?",
                        Type = QuizElementType.MultiSelectQuestion,
                        Choices = new List<QuizChoice>()
                        {
                            new QuizChoice() { Choice = "Phone: I would just call them all" },
                            new QuizChoice() { Choice = "Chats: whatsapp, facebook, telegram, slack, other" },
                            new QuizChoice() { Choice = "Facebook events: First I decide everything and then create an event" },
                            new QuizChoice() { Choice = "Google calendar: Plan details and then create and share event" },
                            new QuizChoice() { Choice = "Email" },
                            new QuizChoice() { Choice = "Doodle.com to plan the appropriate time" },
                            new QuizChoice() { Choice = "Calendly" },
                            new QuizChoice() { Choice = "Other tools" },
                        }
                    },
                    new QuizQuestion()
                    {
                        Order = 6,
                        Question = "Want to tell us more?",
                        Type = QuizElementType.FreeTextQuestion,
                    },
                    new QuizQuestion()
                    {
                        Order = 7,
                        Question = "https://squalldiag.blob.core.windows.net/preload/mountains.jpg",
                        Type = QuizElementType.Picture,
                    },
                     new QuizQuestion()
                    {
                        Order = 8,
                        Question = "What would you like to do together?",
                        Type = QuizElementType.MultiSelectQuestion,
                        Choices = new List<QuizChoice>()
                        {
                            new QuizChoice() { Choice = "Eat out" },
                            new QuizChoice() { Choice = "BBQ" },
                            new QuizChoice() { Choice = "Party or drink out" },
                            new QuizChoice() { Choice = "Celebrate X-mass or New Year" },
                            new QuizChoice() { Choice = "Travel" },
                            new QuizChoice() { Choice = "Hike" },
                            new QuizChoice() { Choice = "Visit nature" },
                            new QuizChoice() { Choice = "Ride a bicycle" },
                            new QuizChoice() { Choice = "Ride a bike" },
                            new QuizChoice() { Choice = "Go skiing" },
                            new QuizChoice() { Choice = "Do sports" },
                            new QuizChoice() { Choice = "Go to SPA or Sauna" },
                            new QuizChoice() { Choice = "Play sport games" },
                            new QuizChoice() { Choice = "Play poker" },
                            new QuizChoice() { Choice = "Play board games" },
                            new QuizChoice() { Choice = "Play computer games" },
                            new QuizChoice() { Choice = "Visit theatre, cinema or similar" },
                        }
                    },
                    new QuizQuestion()
                    {
                        Order = 9,
                        Question = "Would you share your preferences?",
                        Type = QuizElementType.FreeTextQuestion,
                    },

                    new QuizQuestion()
                    {
                        Order = 10,
                        Question = "How likely would you use our tool to get people together?",
                        Type = QuizElementType.GradeQuestion,
                    },
                    new QuizQuestion()
                    {
                        Order = 11,
                        Question = "https://squalldiag.blob.core.windows.net/preload/girlfriends.jpg",
                        Type = QuizElementType.Picture,
                    },
                    new QuizQuestion()
                    {
                        Order = 12,
                        Question = "Would you like to meet new people interested in same activities as you?",
                        Type = QuizElementType.GradeQuestion,
                    },
                    new QuizQuestion()
                    {
                        Order = 13,
                        Question = "Would you prefer using website, mobile app or a chat bot for it?",
                        Type = QuizElementType.SingleSelectQuestion,
                        Choices = new List<QuizChoice>()
                        {
                            new QuizChoice() { Choice = "Web" },
                            new QuizChoice() { Choice = "Mobile app" },
                            new QuizChoice() { Choice = "Chat bot" },
                        }
                    },
                    new QuizQuestion()
                    {
                        Order= 14,
                        Type = QuizElementType.SingleSelectQuestion,
                        Question = "Do you agree that doing something together leads to better quality communication?",
                        Choices = new List<QuizChoice>()
                        {
                            new QuizChoice() { Choice = "Yes" },
                            new QuizChoice() { Choice = "No" },
                            new QuizChoice() { Choice = "Not sure" },
                        }
                    },
                    new QuizQuestion()
                    {
                        Order= 15,
                        Type = QuizElementType.GradeQuestion,
                        Question = "How much has pandemics affected live communication?",
                    },
                    new QuizQuestion()
                    {
                        Order= 16,
                        Type = QuizElementType.SingleSelectQuestion,
                        Question = "Do social networks improve socialization and communication quality?",
                        Choices = new List<QuizChoice>()
                        {
                            new QuizChoice() { Choice = "Yes" },
                            new QuizChoice() { Choice = "No" },
                            new QuizChoice() { Choice = "Not sure" },
                        }
                    },
                    new QuizQuestion()
                    {
                        Order= 17,
                        Type = QuizElementType.SingleSelectQuestion,
                        Question = "Is it somewhat difficult to find common communication context with new people?",
                        Choices = new List<QuizChoice>()
                        {
                            new QuizChoice() { Choice = "Yes" },
                            new QuizChoice() { Choice = "No" },
                            new QuizChoice() { Choice = "Not sure" },
                        }
                    },
                    new QuizQuestion()
                    {
                        Order= 18,
                        Type = QuizElementType.SingleSelectQuestion,
                        Question = "Would you like to get involved into activities with people you don't know yet, if you all love doing the same things?",
                        Choices = new List<QuizChoice>()
                        {
                            new QuizChoice() { Choice = "Yes" },
                            new QuizChoice() { Choice = "No" },
                            new QuizChoice() { Choice = "Not sure" },
                        }
                    },
                    new QuizQuestion()
                    {
                        Order= 19,
                        Type = QuizElementType.SingleSelectQuestion,
                        Question = "Would you like to get recommendations on activities within your set of interests?",
                        Choices = new List<QuizChoice>()
                        {
                            new QuizChoice() { Choice = "Yes" },
                            new QuizChoice() { Choice = "No" },
                            new QuizChoice() { Choice = "Not sure" },
                        }
                    },
                    new QuizQuestion()
                    {
                        Order= 20,
                        Type = QuizElementType.FreeTextQuestion,
                        Question = "Any comments for us to make this project really helpful for socializing?",
                    },
                }
            });

            db.QCodes.Add(new QCode()
            {
                Id = "afafafaf",
                Quiz = publicQuiz.Entity
            });
        }
    }
}
