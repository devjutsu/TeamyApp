﻿using Teamy.Server.Models;

namespace Teamy.Server.Data.Seed
{
    public class TemplateSeed
    {
        TeamyDbContext db;
        public TemplateSeed(TeamyDbContext ctx)
        {
            db = ctx;
        }

        public void Seed()
        {
            // 0 Custom
            // 1 Food & Drink
            // 2 Arts & Culture
            // 3 Dodamies dabā
            // 4 Travelling
            // 5 Hiking / Walking
            // 6 Sports & Wellness
            // 7 Parties & Nighlife
            // 8 Playing Games
            // + Social activities
            // + Children activities

            var eatAndDrink = db.TemplateCategories.Add(new TemplateCategory() { Title = "Food & Drink" });
            var enjoyArtsAndCulture = db.TemplateCategories.Add(new TemplateCategory() { Title = "Arts & Culture" });
            var nature = db.TemplateCategories.Add(new TemplateCategory() { Title = "Nature" });
            var travel = db.TemplateCategories.Add(new TemplateCategory() { Title = "Travel" });
            var hiking = db.TemplateCategories.Add(new TemplateCategory() { Title = "Hiking" });
            var sports = db.TemplateCategories.Add(new TemplateCategory() { Title = "Sports & Wellness" });
            var nighlife = db.TemplateCategories.Add(new TemplateCategory() { Title = "Parties & Nighlife" });
            var games = db.TemplateCategories.Add(new TemplateCategory() { Title = "Play Games" });


            db.Templates.Add(new Template() 
            { 
                Category = eatAndDrink.Entity,
                Title = "Food & Drink",
                Description = "Have some food and drinks together",
                CoverImage = new ImageModel() { Url = "https://squalldiag.blob.core.windows.net/template-img/113244196food.jpg", 
                                                 Id = Guid.NewGuid() },
                Polls = new List<TemplatePoll>()
                {
                    new TemplatePoll() 
                    { 
                        Question = "Are we going out, ordering in or cooking BBQ?",
                        Choices = new List<TemplatePollChoice>()
                        {
                            new TemplatePollChoice() { Choice = "Eating out" },
                            new TemplatePollChoice() { Choice = "Ordering in" },
                            new TemplatePollChoice() { Choice = "Cooking BBQ" },
                        }
                    },
                    new TemplatePoll() 
                    { 
                        Question = "What kind of food is everybody up to?",
                        FreeTextOption = true,
                        MultiChoice = true,
                        Choices = new List<TemplatePollChoice>()
                        {
                            new TemplatePollChoice() { Choice = "Steak or BBQ" },
                            new TemplatePollChoice() { Choice = "Pizza" },
                            new TemplatePollChoice() { Choice = "Sushi" },
                            new TemplatePollChoice() { Choice = "Beer/coktails" },
                            new TemplatePollChoice() { Choice = "Tea or Coffee" },
                        }
                    }
                },
                Where = "To be decided...",
            });

            db.Templates.Add(new Template()
            {
                Category = enjoyArtsAndCulture.Entity,
                Title = "Arts & Culture",
                Description = "Visit cinema, theatre or museum",
                CoverImage = new ImageModel()
                {
                    Url = "https://squalldiag.blob.core.windows.net/template-img/arts-culture_0.jpg",
                    Id = Guid.NewGuid()
                },
                Polls = new List<TemplatePoll>()
                {
                    new TemplatePoll()
                    {
                        Question = "Where are we going?",
                        Choices = new List<TemplatePollChoice>()
                        {
                            new TemplatePollChoice() { Choice = "Cinema" },
                            new TemplatePollChoice() { Choice = "Concert" },
                            new TemplatePollChoice() { Choice = "Theatre" },
                            new TemplatePollChoice() { Choice = "Exhibition" },
                            new TemplatePollChoice() { Choice = "Museum" },
                            new TemplatePollChoice() { Choice = "Library" },
                        }
                    },
                    new TemplatePoll()
                    {
                        Question = "Interest topic for this time?",
                        FreeTextOption = true,
                        MultiChoice = true,
                        Choices = new List<TemplatePollChoice>()
                        {
                            new TemplatePollChoice() { Choice = "History" },
                            new TemplatePollChoice() { Choice = "Warfare" },
                            new TemplatePollChoice() { Choice = "Arts" },
                            new TemplatePollChoice() { Choice = "Science" },
                            new TemplatePollChoice() { Choice = "Sci-Fi" },
                            new TemplatePollChoice() { Choice = "Fashion" },
                            new TemplatePollChoice() { Choice = "Entertainment" },
                            new TemplatePollChoice() { Choice = "Comedy" },
                        }
                    }
                },
                Where = "To be decided...",
            });

            db.Templates.Add(new Template()
            {
                Category = nature.Entity,
                Title = "Nature",
                Description = "Go to the Nature",
                CoverImage = new ImageModel()
                {
                    Url = "https://squalldiag.blob.core.windows.net/template-img/t3bip9rzxau6hh4kz5hb.jfif",
                    Id = Guid.NewGuid()
                },
                Polls = new List<TemplatePoll>()
                {
                    new TemplatePoll()
                    {
                        Question = "Where are we going?",
                        Choices = new List<TemplatePollChoice>()
                        {
                            new TemplatePollChoice() { Choice = "Barbeque" },
                            new TemplatePollChoice() { Choice = "Walk in the forest" },
                            new TemplatePollChoice() { Choice = "Swimming" },
                            new TemplatePollChoice() { Choice = "Riding a bicycle" },
                        }
                    },
                    new TemplatePoll()
                    {
                        Question = "How far you're ready to walk?",
                        Choices = new List<TemplatePollChoice>()
                        {
                            new TemplatePollChoice() { Choice = "5 km" },
                            new TemplatePollChoice() { Choice = "10 km" },
                            new TemplatePollChoice() { Choice = "20 km" },
                            new TemplatePollChoice() { Choice = "30 km" },
                            new TemplatePollChoice() { Choice = "50 km" },
                        },
                        FreeTextOption = true,
                        MultiChoice = true,
                    }
                },
                Where = "To be decided...",
            });

            db.Templates.Add(new Template()
            {
                Category = travel.Entity,
                Title = "Travel",
                Description = "Travel with good company",
                CoverImage = new ImageModel()
                {
                    Url = "https://squalldiag.blob.core.windows.net/template-img/travel.jpg",
                    Id = Guid.NewGuid()
                },
                Polls = new List<TemplatePoll>()
                {
                    new TemplatePoll()
                    {
                        Question = "Where are we going?",
                        Choices = new List<TemplatePollChoice>()
                        {
                            new TemplatePollChoice() { Choice = "Countryside" },
                            new TemplatePollChoice() { Choice = "Seaside" },
                            new TemplatePollChoice() { Choice = "Mountains" },
                            new TemplatePollChoice() { Choice = "City travel" },
                        }
                    },
                    new TemplatePoll()
                    {
                        Question = "How far you're ready to walk?",
                        Choices = new List<TemplatePollChoice>()
                        {
                            new TemplatePollChoice() { Choice = "5 km" },
                            new TemplatePollChoice() { Choice = "10 km" },
                            new TemplatePollChoice() { Choice = "20 km" },
                            new TemplatePollChoice() { Choice = "30 km" },
                            new TemplatePollChoice() { Choice = "50 km" },
                        },
                        FreeTextOption = true,
                        MultiChoice = true,
                    }
                },
                Where = "To be decided...",
            });

            db.Templates.Add(new Template()
            {
                Category = hiking.Entity,
                Title = "Hiking",
                Description = "Go Hiking and Sightseeing",
                CoverImage = new ImageModel()
                {
                    Url = "https://squalldiag.blob.core.windows.net/template-img/hiking.jpg",
                    Id = Guid.NewGuid()
                },
                Polls = new List<TemplatePoll>()
                {
                    new TemplatePoll()
                    {
                        Question = "How far you're ready to walk?",
                        Choices = new List<TemplatePollChoice>()
                        {
                            new TemplatePollChoice() { Choice = "5 km" },
                            new TemplatePollChoice() { Choice = "10 km" },
                            new TemplatePollChoice() { Choice = "20 km" },
                            new TemplatePollChoice() { Choice = "30 km" },
                            new TemplatePollChoice() { Choice = "50 km" },
                        },
                        FreeTextOption = true,
                        MultiChoice = true,
                    },
                    new TemplatePoll()
                    {
                        Question = "Where should we go walking?",
                        Choices = new List<TemplatePollChoice>()
                        {
                            new TemplatePollChoice() { Choice = "Around the city" },
                            new TemplatePollChoice() { Choice = "Seaside" },
                            new TemplatePollChoice() { Choice = "Green forest" },
                            new TemplatePollChoice() { Choice = "Countryside" },
                            new TemplatePollChoice() { Choice = "Mountains" },
                        }
                    }
                },
                Where = "To be decided...",
            });

            db.Templates.Add(new Template()
            {
                Category = sports.Entity,
                Title = "Sports & Wellness",
                Description = "Do Sports and keep Fit",
                CoverImage = new ImageModel()
                {
                    Url = "https://squalldiag.blob.core.windows.net/template-img/sports.jpg",
                    Id = Guid.NewGuid()
                },
                Polls = new List<TemplatePoll>()
                {
                    new TemplatePoll()
                    {
                        Question = "Where should we go?",
                        Choices = new List<TemplatePollChoice>()
                        {
                            new TemplatePollChoice() { Choice = "Gym" },
                            new TemplatePollChoice() { Choice = "SPA" },
                            new TemplatePollChoice() { Choice = "Dancing" },
                            new TemplatePollChoice() { Choice = "Running" },
                            new TemplatePollChoice() { Choice = "Cycling" },
                            new TemplatePollChoice() { Choice = "Martial arts" },
                            new TemplatePollChoice() { Choice = "Sport games" },
                        },
                        FreeTextOption = true,
                        MultiChoice = true,
                    },
                    new TemplatePoll()
                    {
                        Question = "What kind of sport games do you like?",
                        FreeTextOption = true,
                        MultiChoice = true,
                        Choices = new List<TemplatePollChoice>()
                        {
                            new TemplatePollChoice() { Choice = "Football" },
                            new TemplatePollChoice() { Choice = "Basketball" },
                            new TemplatePollChoice() { Choice = "Hockey" },
                            new TemplatePollChoice() { Choice = "Tennis" },
                            new TemplatePollChoice() { Choice = "Table tennis" },
                            new TemplatePollChoice() { Choice = "Golf" },
                            new TemplatePollChoice() { Choice = "Criquet" },
                            new TemplatePollChoice() { Choice = "Chess" },
                            new TemplatePollChoice() { Choice = "Poker" },
                        }
                    }
                },
                Where = "To be decided...",
            });

            db.Templates.Add(new Template()
            {
                Category = nighlife.Entity,
                Title = "Parties & Nightlife",
                Description = "Have fun and enjoy free time",
                CoverImage = new ImageModel()
                {
                    Url = "https://squalldiag.blob.core.windows.net/template-img/nightlife.jpg",
                    Id = Guid.NewGuid()
                },
                Polls = new List<TemplatePoll>()
                {
                    new TemplatePoll()
                    {
                        Question = "Where should we go?",
                        Choices = new List<TemplatePollChoice>()
                        {
                            new TemplatePollChoice() { Choice = "Coktail/wine party" },
                            new TemplatePollChoice() { Choice = "Salsa evening" },
                            new TemplatePollChoice() { Choice = "Pub hopping" },
                            new TemplatePollChoice() { Choice = "Nightclub" },
                            new TemplatePollChoice() { Choice = "Concert" },
                            new TemplatePollChoice() { Choice = "Strip bar" },
                            new TemplatePollChoice() { Choice = "Cazino" },
                        },
                        
                    },
                    new TemplatePoll()
                    {
                        Question = "How do you like it?",
                        FreeTextOption = true,
                        MultiChoice = true,
                        Choices = new List<TemplatePollChoice>()
                        {
                            new TemplatePollChoice() { Choice = "Loud music and lots of dancing" },
                            new TemplatePollChoice() { Choice = "Not so loud so we can speak" },
                            new TemplatePollChoice() { Choice = "More alcohol" },
                            new TemplatePollChoice() { Choice = "Less alcohol" },
                            new TemplatePollChoice() { Choice = "Games and fun" },
                            new TemplatePollChoice() { Choice = "Meeting new people" },
                        },
                    }
                },
                Where = "To be decided...",
            });

            db.Templates.Add(new Template()
            {
                Category = games.Entity,
                Title = "Games",
                Description = "Play Board, Card, Sport games or even virtual",
                CoverImage = new ImageModel()
                {
                    Url = "https://squalldiag.blob.core.windows.net/template-img/games.png",
                    Id = Guid.NewGuid()
                },
                Polls = new List<TemplatePoll>()
                {
                    new TemplatePoll()
                    {
                        Question = "What games do you like?",
                        FreeTextOption = true,
                        MultiChoice = true,
                        Choices = new List<TemplatePollChoice>()
                        {
                            new TemplatePollChoice() { Choice = "Word games" },
                            new TemplatePollChoice() { Choice = "Quizz games" },
                            new TemplatePollChoice() { Choice = "Logic games" },
                            new TemplatePollChoice() { Choice = "Chess" },
                            new TemplatePollChoice() { Choice = "Poker" },
                            new TemplatePollChoice() { Choice = "Uno" },
                            new TemplatePollChoice() { Choice = "Speed racing" },
                            new TemplatePollChoice() { Choice = "Action" },
                            new TemplatePollChoice() { Choice = "Horrors" },
                            new TemplatePollChoice() { Choice = "Adventures" },
                            new TemplatePollChoice() { Choice = "Puzzles" },
                            new TemplatePollChoice() { Choice = "CS Go" },
                            new TemplatePollChoice() { Choice = "Fortnite" },
                        }
                    },
                    new TemplatePoll()
                    {
                        Question = "What's best for you?",
                        Choices = new List<TemplatePollChoice>()
                        {
                            new TemplatePollChoice() { Choice = "Offline group games" },
                            new TemplatePollChoice() { Choice = "Computer games" },
                            new TemplatePollChoice() { Choice = "Board games" },
                            new TemplatePollChoice() { Choice = "Card games" },
                            new TemplatePollChoice() { Choice = "Quizz games" },
                            new TemplatePollChoice() { Choice = "Logic games" },
                            new TemplatePollChoice() { Choice = "Word games" },
                        }
                    }
                },
                Where = "To be decided...",
            });
        }
    }
}
