using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Teamy.Server.Models;

namespace Teamy.Server.Data
{
    public class TeamyDbContext : ApiAuthorizationDbContext<AppUser>
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<PollChoice> PollsOptions { get; set; }
        public DbSet<PollAnswer> PollsAnswers { get; set; }

        public TeamyDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new AppUserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());

            builder.ApplyConfiguration(new EventConfiguration());
            builder.ApplyConfiguration(new PollConfiguration());
            
        }
    }
}