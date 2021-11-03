using Teamy.Server.Models;

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
            });

            db.Templates.Add(new Template()
            {
                Category = travel.Entity,
                Title = "Travel",
                Description = "Travel with good company",
                CoverImage = new ImageModel()
                {
                    Url = "https://squalldiag.blob.core.windows.net/template-img/t3bip9rzxau6hh4kz5hb.jfif",
                    Id = Guid.NewGuid()
                },
            });

            db.Templates.Add(new Template()
            {
                Category = hiking.Entity,
                Title = "Hiking",
                Description = "Go Hiking and Sightseeing",
                CoverImage = new ImageModel()
                {
                    Url = "https://squalldiag.blob.core.windows.net/template-img/t3bip9rzxau6hh4kz5hb.jfif",
                    Id = Guid.NewGuid()
                },
            });

            db.Templates.Add(new Template()
            {
                Category = sports.Entity,
                Title = "Sports & Wellness",
                Description = "Do Sports and keep Fit",
                CoverImage = new ImageModel()
                {
                    Url = "https://squalldiag.blob.core.windows.net/template-img/t3bip9rzxau6hh4kz5hb.jfif",
                    Id = Guid.NewGuid()
                },
            });

            db.Templates.Add(new Template()
            {
                Category = nighlife.Entity,
                Title = "Parties & Nightlife",
                Description = "Have fun and enjoy free time",
                CoverImage = new ImageModel()
                {
                    Url = "https://squalldiag.blob.core.windows.net/template-img/t3bip9rzxau6hh4kz5hb.jfif",
                    Id = Guid.NewGuid()
                },
            });

            db.Templates.Add(new Template()
            {
                Category = games.Entity,
                Title = "Games",
                Description = "Play Board, Card, Sport games or even virtual",
                CoverImage = new ImageModel()
                {
                    Url = "https://squalldiag.blob.core.windows.net/template-img/t3bip9rzxau6hh4kz5hb.jfif",
                    Id = Guid.NewGuid()
                },
            });
        }
    }
}
