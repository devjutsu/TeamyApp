using Microsoft.EntityFrameworkCore;
using Teamy.Server.Data.Seed;

namespace Teamy.Server.Data
{
    public interface IDbInitializer
    {
        void Initialize();
        void SeedData();
    }

    public class DbInitializer : IDbInitializer
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DbInitializer(IServiceScopeFactory scopeFactory)
        {
            this._scopeFactory = scopeFactory;
        }

        public void Initialize()
        {
            using var serviceScope = _scopeFactory.CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<TeamyDbContext>();
            context.Database.Migrate();
        }

        public void SeedData()
        {
            using var serviceScope = _scopeFactory.CreateScope();
            using var db = serviceScope.ServiceProvider.GetService<TeamyDbContext>();

            if (!db.Users.Any())
            {
                new UserSeed(db).Seed();
            }

            db.SaveChanges();

            if (!db.Templates.Any())
            {
                new TemplateSeed(db).Seed();
            }

            if(!db.Quiz.Any())
            {
                new QuizSeed(db).Seed();
            }

            db.SaveChanges();
        }
    }
}
