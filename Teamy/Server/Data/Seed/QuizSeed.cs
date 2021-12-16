using Teamy.Server.Models;
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
        }
    }
}
