using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;
using CoEvent.Data.Configurations;
using CoEvent.Data.Entities;

namespace CoEvent.Data
{
    /// <summary>
    /// CoEventContext sealed class, provides a database context for the CoEventtoria web database.
    /// </summary>
    public sealed class CoEventContext : DbContext
    {
        #region Variables
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Properties

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityCriteria> ActivityCriteria { get; set; }
        public DbSet<ActivityTag> ActivityTags { get; set; }

        public DbSet<Entities.Attribute> Attributes { get; set; }

        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<CalendarCriteria> CalendarCriteria { get; set; }
        public DbSet<CalendarTag> CalendarTags { get; set; }

        public DbSet<ContactInfo> ContactInfo { get; set; }

        public DbSet<CriteriaObject> Criteria { get; set; }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventCriteria> EventCriteria { get; set; }
        public DbSet<EventTag> EventTags { get; set; }

        public DbSet<Opening> Openings { get; set; }
        public DbSet<OpeningCriteria> OpeningCriteria { get; set; }
        public DbSet<OpeningParticipant> OpeningParticipants { get; set; }
        public DbSet<OpeningTag> OpeningTags { get; set; }
        public DbSet<OpeningQuestion> OpeningQuestions { get; set; }
        public DbSet<OpeningAnswer> OpeningAnswers { get; set; }
        public DbSet<OpeningAnswerQuestionOption> OpeningAnswerQuestionOptions { get; set; }

        public DbSet<Process> Processes { get; set; }

        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }

        public DbSet<Participant> Participants { get; set; }
        public DbSet<ParticipantAttribute> ParticipantAttributes { get; set; }
        public DbSet<ParticipantContactInfo> ParticipantContactInfo { get; set; }

        public DbSet<Entities.Schedule> Schedules { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a CoEventContext object, initializes with the specified arguments.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="httpContextAccessor"></param>
        public CoEventContext(DbContextOptions<CoEventContext> options, IHttpContextAccessor httpContextAccessor = null) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Configures the DbContext with the specified options.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.EnableSensitiveDataLogging();
            }

            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// Creates the datasource.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations(typeof(CalendarConfiguration), this);

            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            modelBuilder.UseValueConverterForType(typeof(DateTime), dateTimeConverter);

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Save the entities with who created them or updated them.
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            // get entries that are being Added or Updated
            var modifiedEntries = ChangeTracker.Entries()
                    .Where(x => (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                // if (entry.Entity is BaseEntity entity)
                // {
                //     if (entry.State == EntityState.Added)
                //     {
                //         entity.CreatedOn = DateTime.UtcNow;
                //     }
                //     else if (entry.State != EntityState.Deleted)
                //     {
                //         entity.UpdatedOn = DateTime.UtcNow;
                //     }
                // }
            }

            return base.SaveChanges();
        }

        /// <summary>
        /// Wrap the save changes in a transaction for rollback.
        /// </summary>
        /// <returns></returns>
        public int CommitTransaction()
        {
            var result = 0;
            using (var transaction = this.Database.BeginTransaction())
            {
                try
                {
                    result = this.SaveChanges();
                    transaction.Commit();
                }
                catch (DbUpdateException)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return result;
        }
        #endregion
    }
}
