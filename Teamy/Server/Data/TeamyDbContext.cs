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
        public DbSet<PollChoice> PollChoices { get; set; }
        public DbSet<PollAnswer> PollAnswers { get; set; }

        public DbSet<TemplateCategory> TemplateCategories { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<TemplatePoll> TemplatePolls { get; set; }
        public DbSet<TemplatePollChoice> TemplatePollChoices { get; set; }

        public DbSet<Invite> Invites { get; set; }
        public DbSet<Participation> Participation { get; set; }
        public DbSet<ImageModel> Images { get; set; }

        public DbSet<AnonParticipation> AnonParticipation { get; set; }
        public DbSet<ProposedDate> ProposedDates { get; set; }
        public DbSet<DateVote> DateVotes { get; set; }

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
            builder.ApplyConfiguration(new PollChoiceConfiguration());
            builder.ApplyConfiguration(new PollAnswerConfiguration());

            builder.ApplyConfiguration(new TemplateCategoryConfiguration());
            builder.ApplyConfiguration(new TemplateConfiguration());
            builder.ApplyConfiguration(new TemplatePollConfiguration());
            builder.ApplyConfiguration(new TemplatePollChoiceConfiguration());

            builder.ApplyConfiguration(new InviteConfiguration());
            builder.ApplyConfiguration(new ParticipationConfiguration());

            builder.ApplyConfiguration(new ImageModelConfiguration());

            builder.ApplyConfiguration(new InviteConfiguration());
            builder.ApplyConfiguration(new ParticipationConfiguration());
            builder.ApplyConfiguration(new ImageModelConfiguration());

            builder.ApplyConfiguration(new AnonParticipationConfiguration());
            builder.ApplyConfiguration(new ProposedDateConfiguration());
            builder.ApplyConfiguration(new DateVoteConfiguration());
        }
    }
}