using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Teamy.Server.Models;
using Teamy.Server.Models.Quizes;
using Teamy.Server.Models.Polls;
using Teamy.Server.Models.Templates;

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

        public DbSet<ChatMessage> Chat { get; set; }

        public DbSet<Quiz> Quiz { get; set; }
        public DbSet<QCode> QCodes { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<QuizChoice> QuizChoices { get; set; }
        public DbSet<QuizAnswer> QuizAnswers { get; set; }
        public DbSet<QuizCompletion> QuizCompletions { get; set; }

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

            builder.ApplyConfiguration(new ChatMessageConfiguration());

            builder.ApplyConfiguration(new QuizConfiguration());
            builder.ApplyConfiguration(new QuizCompletionConfiguration());
            builder.ApplyConfiguration(new QCodeConfiguration());
            builder.ApplyConfiguration(new QuizAnswerConfiguration());
            builder.ApplyConfiguration(new QuizChoiceConfiguration());
            builder.ApplyConfiguration(new QuizQuestionConfiguration());
        }

        public override int SaveChanges()
        {
            SetProperties();
            return base.SaveChanges();
        }

        public sealed override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetProperties();
            return base.SaveChangesAsync(cancellationToken);
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SetProperties();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void SetProperties()
        {
            foreach (var entity in ChangeTracker.Entries().Where(p => p.State == EntityState.Added))
            {
                var created = entity.Entity as IDateCreated;
                if (created != null)
                {
                    created.DateCreated = DateTime.Now;
                }
            }

            foreach (var entity in ChangeTracker.Entries().Where(p => p.State == EntityState.Modified))
            {
                var updated = entity.Entity as IDateUpdated;
                if (updated != null)
                {
                    updated.DateUpdated = DateTime.Now;
                }
            }
        }
    }
}