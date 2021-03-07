using CoEvent.Core.Exceptions;
using CoEvent.Core.Extensions;
using CoEvent.Data.Interfaces;
using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoEvent.Data.Services
{
    /// <summary>
    /// HelperService class, provides functions that perform data population features that are used to speed up testing and development.
    /// </summary>
    public sealed class HelperService : IHelperService
    {
        #region Variables
        private readonly IDataSource _source;
        #endregion

        #region Properties
        /// <summary>
        /// get - The datasource.
        /// </summary>
        private DataSource Source { get { return _source as DataSource; } }

        /// <summary>
        /// get - The DbContext used to communicate with the datasource.
        /// </summary>
        private CoEventContext Context { get { return this.Source.Context; } }

        /// <summary>
        /// get - Whether the user is currently authenticated.
        /// </summary>
        private bool IsAuthenticated { get { return this.Source.Principal.Identity.IsAuthenticated; } }

        /// <summary>
        /// get - Whether the current logged in principal is a participant or not.
        /// </summary>
        /// <returns></returns>
        private bool IsPrincipalAParticipant
        {
            get
            {
                bool.TryParse(this.Source.Principal.GetParticipant()?.Value, out bool participant);
                return participant;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a HelperService object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="source"></param>
        internal HelperService(IDataSource source)
        {
            _source = source;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add questions to the specified account.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public IEnumerable<Question> AddQuestions(int accountId)
        {
            var userId = this.Source.Principal.GetUser()?.Value.ConvertTo<int>() ?? 0;
            var account = this.Context.Accounts.Find(accountId);
            var title = new Question(account, "Title", "What is the title of your class?", AnswerType.String, true, 150) { AddedById = userId };

            this.Context.Questions.Add(title);

            return new[] { title };
        }

        /// <summary>
        /// Creates and adds a new ecclesial calendar with default events, activities, openings and criteria.
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="startOn"></param>
        /// <param name="endOn"></param>
        /// <returns></returns>
        public Models.Calendar AddEcclesialEvents(int calendarId, DateTime? startOn = null, DateTime? endOn = null)
        {
            if (this.IsPrincipalAParticipant) throw new NotAuthorizedException();

            var userId = this.Source.Principal.GetUser()?.Value.ConvertTo<int>() ?? 0;
            var user = this.Context.Users.Include(u => u.Info).SingleOrDefault(u => u.Id == userId) ?? throw new NotAuthorizedException();
            var calendar = this.Context.Calendars.SingleOrDefault(c => c.Id == calendarId) ?? throw new NoContentException(typeof(Calendar));

            // TODO: Need generic fucntion to test permission to allow editing calendar.
            var ownsAccount = this.Context.Accounts.Any(a => a.Id == calendar.AccountId && a.OwnerId == userId);
            if (!ownsAccount) throw new NotAuthorizedException();

            var questions = AddQuestions(calendar.AccountId);
            var qTitle = questions.First(q => q.Caption.Equals("Title"));

            // Determine date range.
            var start = startOn ?? DateTime.UtcNow.Date;
            var end = endOn ?? start.AddDays(365);
            var days = (end - start).Days;

            var events = new List<Event>(days);

            var member = new CriteriaObject("Member", true) { AddedById = userId };
            var brother = new CriteriaObject("Brother", true) { AddedById = userId };
            var sister = new CriteriaObject("Sister", true) { AddedById = userId };
            var presider = new CriteriaObject(new CriteriaValue("Skill", "Preside"), (Criteria)brother, (Criteria)member) { AddedById = userId };
            var pianist = new CriteriaObject(new CriteriaValue("Skill", "Pianist"), (Criteria)member) { AddedById = userId };
            var exhorter = new CriteriaObject(new CriteriaValue("Skill", "Exhort"), (Criteria)brother) { AddedById = userId };
            var reader = new CriteriaObject(new CriteriaValue("Skill", "Read"), (Criteria)brother, (Criteria)member) { AddedById = userId };
            var server = new CriteriaObject(new CriteriaValue("Skill", "Serve"), (Criteria)brother, (Criteria)member) { AddedById = userId };
            var prayer = new CriteriaObject(new CriteriaValue("Skill", "Pray"), (Criteria)brother, (Criteria)member) { AddedById = userId };
            var door = new CriteriaObject(new CriteriaValue("Skill", "Doorman"), (Criteria)brother, (Criteria)member) { AddedById = userId };
            var emblems = new CriteriaObject(new CriteriaValue("Skill", "Emblems"), (Criteria)member) { AddedById = userId };
            var lecturer = new CriteriaObject(new CriteriaValue("Skill", "Lecturer"), (Criteria)brother) { AddedById = userId };
            var study = new CriteriaObject(new CriteriaValue("Skill", "Bible Class"), (Criteria)brother) { AddedById = userId };
            var clean = new CriteriaObject(new CriteriaValue("Skill", "Clean"), (Criteria)member) { AddedById = userId };
            var criteria = new List<CriteriaObject>() { member, brother, sister, pianist, presider, exhorter, reader, server, prayer, door, emblems, lecturer, clean };

            // Sunday Memorial
            var sunday = start.DayOfWeek == DayOfWeek.Sunday ? start : start.AddDays(7 - (int)start.DayOfWeek);
            Event firstSundaySchool = null;
            Event firstMemorial = null;
            while (sunday <= end)
            {
                // Sunday School
                var school = new Event(calendar, "Sunday School", sunday.AddHours(9), sunday.AddHours(10))
                {
                    Description = "Adult Sunday School.",
                    AddedById = userId,
                    ParentEvent = firstSundaySchool,
                    Repetition = EventRepetition.Weekly,
                    RepetitionEndOn = end,
                    RepetitionSize = 1
                };

                var aTeach = new Activity(school, "Class") { AddedById = userId, Sequence = 2 };
                var oTeach = new Opening(aTeach, "Teach", 1, 1, OpeningType.Application, ApplicationProcess.AutoAccept, CriteriaRule.Visibility) { AddedById = userId };
                oTeach.Actions.Add(new Process(oTeach, ActionTrigger.Accept, "Add(Participant.Answers, Opening.Tags, Question.Caption=\"Title\");") { AddedById = userId });
                oTeach.Actions.Add(new Process(oTeach, ActionTrigger.Unapply, "Delete(Opening.Tags, Tag.Key=\"Title\");") { AddedById = userId });
                oTeach.Questions.Add(new OpeningQuestion(oTeach, qTitle));
                aTeach.Openings.Add(oTeach);
                aTeach.Criteria.Add(new ActivityCriteria(aTeach, lecturer));
                school.Activities.Add(aTeach);

                calendar.Events.Add(school);

                if (firstSundaySchool == null)
                {
                    firstSundaySchool = school;
                }

                // Memorial
                var memorial = new Event(calendar, "Memorial Meeting", sunday.AddHours(11), sunday.AddHours(13))
                {
                    Description = "Sunday memorial meeting.",
                    AddedById = userId,
                    ParentEvent = firstMemorial,
                    Repetition = EventRepetition.Weekly,
                    RepetitionEndOn = end,
                    RepetitionSize = 1
                };
                var aPreside = new Activity(memorial, "Presider") { AddedById = userId, Sequence = 1 };
                aPreside.Openings.Add(new Opening(aPreside, "Preside", 1, 1, OpeningType.Application, ApplicationProcess.AutoAccept, CriteriaRule.Visibility) { AddedById = userId });
                aPreside.Criteria.Add(new ActivityCriteria(aPreside, presider));
                memorial.Activities.Add(aPreside);

                var aExhort = new Activity(memorial, "Exhorter") { AddedById = userId, Sequence = 2 };
                aExhort.Openings.Add(new Opening(aExhort, "Exhort", 1, 1, OpeningType.Application, ApplicationProcess.AutoAccept, CriteriaRule.Visibility) { AddedById = userId });
                aExhort.Criteria.Add(new ActivityCriteria(aExhort, exhorter));
                memorial.Activities.Add(aExhort);

                var aPianist = new Activity(memorial, "Pianist") { AddedById = userId, Sequence = 3 };
                aPianist.Openings.Add(new Opening(aPianist, "Pianist", 1, 1, OpeningType.Application, ApplicationProcess.AutoAccept, CriteriaRule.Visibility) { AddedById = userId });
                aPianist.Criteria.Add(new ActivityCriteria(aPianist, pianist));
                memorial.Activities.Add(aPianist);

                var aDoor = new Activity(memorial, "Door Keeper") { AddedById = userId, Sequence = 4 };
                aDoor.Openings.Add(new Opening(aDoor, "Door", 1, 1, OpeningType.Application, ApplicationProcess.AutoAccept, CriteriaRule.Visibility) { AddedById = userId });
                aDoor.Criteria.Add(new ActivityCriteria(aDoor, door));
                memorial.Activities.Add(aDoor);

                var aReadings = new Activity(memorial, "Readings") { AddedById = userId, Sequence = 5 };
                aReadings.Openings.Add(new Opening(aReadings, "1st Reading", 1, 1, OpeningType.Application, ApplicationProcess.AutoAccept, CriteriaRule.Visibility) { AddedById = userId });
                aReadings.Openings.Add(new Opening(aReadings, "2nd Reading", 1, 1, OpeningType.Application, ApplicationProcess.AutoAccept, CriteriaRule.Visibility) { AddedById = userId });
                aReadings.Criteria.Add(new ActivityCriteria(aReadings, reader));
                memorial.Activities.Add(aReadings);

                var aPrayers = new Activity(memorial, "Prayers") { AddedById = userId, Sequence = 6 };
                aPrayers.Openings.Add(new Opening(aPrayers, "Bread", 1, 1, OpeningType.Application, ApplicationProcess.AutoAccept, CriteriaRule.Visibility) { AddedById = userId });
                aPrayers.Openings.Add(new Opening(aPrayers, "Wine", 1, 1, OpeningType.Application, ApplicationProcess.AutoAccept, CriteriaRule.Visibility) { AddedById = userId });
                aPrayers.Openings.Add(new Opening(aPrayers, "Close", 1, 1, OpeningType.Application, ApplicationProcess.AutoAccept, CriteriaRule.Visibility) { AddedById = userId });
                aPrayers.Criteria.Add(new ActivityCriteria(aPrayers, prayer));
                memorial.Activities.Add(aPrayers);

                //var aServing = new Activity(memorial, "Serving") { AddedById = userId, Sequence = 7 };
                //aServing.Openings.Add(new Opening(aServing, "Server", 4, 4, OpeningType.Application, ApplicationProcess.AutoAccept, CriteriaRule.Visibility) { AddedById = userId });
                //aServing.Criteria.Add(new ActivityCriteria(aServing, server));
                //memorial.Activities.Add(aServing);

                calendar.Events.Add(memorial);

                // Lecture
                var lecture = new Event(calendar, "Bible Talk", sunday.AddHours(19), sunday.AddHours(20))
                {
                    Description = "Sunday night Bible talk.",
                    AddedById = userId
                };
                var aPreside2 = new Activity(lecture, "Presider") { AddedById = userId, Sequence = 1 };
                aPreside2.Openings.Add(new Opening(aPreside2, "Presider", 1, 1, OpeningType.Application, ApplicationProcess.AutoAccept, CriteriaRule.Visibility) { AddedById = userId });
                aPreside2.Criteria.Add(new ActivityCriteria(aPreside2, presider));
                lecture.Activities.Add(aPreside2);

                var aLecture = new Activity(lecture, "Lecture") { AddedById = userId, Sequence = 2 };
                var oLecture = new Opening(aLecture, "Speak", 1, 1, OpeningType.Application, ApplicationProcess.AutoAccept, CriteriaRule.Visibility) { AddedById = userId };
                oLecture.Actions.Add(new Process(oLecture, ActionTrigger.Accept, "Add(Participant.Answers, Opening.Tags, Question.Caption=\"Title\");") { AddedById = userId });
                oLecture.Actions.Add(new Process(oLecture, ActionTrigger.Unapply, "Delete(Opening.Tags, Tag.Key=\"Title\");") { AddedById = userId });
                oLecture.Questions.Add(new OpeningQuestion(oLecture, qTitle));
                aLecture.Openings.Add(oLecture);
                aLecture.Criteria.Add(new ActivityCriteria(aLecture, lecturer));
                lecture.Activities.Add(aLecture);

                calendar.Events.Add(lecture);

                if (firstMemorial == null)
                {
                    firstMemorial = memorial;
                }
                sunday = sunday.AddDays(7);
            }

            // Bible Class
            var thursday = start.DayOfWeek switch
            {
                var dayOfWeek when
                    dayOfWeek <= DayOfWeek.Thursday => start.AddDays(4 - (int)start.DayOfWeek),
                DayOfWeek.Friday => start.AddDays(6),
                DayOfWeek.Saturday => start.AddDays(5),
                _ => start
            };
            Event firstBibleClass = null;
            while (thursday <= end)
            {
                // Lecture
                var bibleClass = new Event(calendar, "Bible Class", thursday.AddHours(19), thursday.AddHours(20))
                {
                    Description = "Thursday night Bible class.",
                    AddedById = userId,
                    ParentEvent = firstBibleClass,
                    Repetition = EventRepetition.Weekly,
                    RepetitionEndOn = end,
                    RepetitionSize = 1
                };
                var aPreside = new Activity(bibleClass, "Preside") { AddedById = userId, Sequence = 1 };
                aPreside.Openings.Add(new Opening(aPreside, "Presider", 1, 1, OpeningType.Application, ApplicationProcess.AutoAccept, CriteriaRule.Visibility) { AddedById = userId });
                aPreside.Criteria.Add(new ActivityCriteria(aPreside, presider));
                bibleClass.Activities.Add(aPreside);

                var aSpeak = new Activity(bibleClass, "Speak") { AddedById = userId, Sequence = 2 };
                var oSpeak = new Opening(aSpeak, "Speaker", 1, 1, OpeningType.Application, ApplicationProcess.AutoAccept, CriteriaRule.Visibility) { AddedById = userId };
                oSpeak.Actions.Add(new Process(oSpeak, ActionTrigger.Accept, "Add(Participant.Answers, Opening.Tags, Question.Caption=\"Title\");") { AddedById = userId });
                oSpeak.Actions.Add(new Process(oSpeak, ActionTrigger.Unapply, "Delete(Opening.Tags, Tag.Key=\"Title\");") { AddedById = userId });
                oSpeak.Questions.Add(new OpeningQuestion(oSpeak, qTitle));
                aSpeak.Openings.Add(oSpeak);
                aSpeak.Criteria.Add(new ActivityCriteria(aSpeak, study));
                bibleClass.Activities.Add(aSpeak);

                var aPianist = new Activity(bibleClass, "Pianist") { AddedById = userId, Sequence = 3 };
                aPianist.Openings.Add(new Opening(aPianist, "Pianist", 1, 1, OpeningType.Application, ApplicationProcess.AutoAccept, CriteriaRule.Visibility) { AddedById = userId });
                aPianist.Criteria.Add(new ActivityCriteria(aPianist, pianist));
                bibleClass.Activities.Add(aPianist);

                calendar.Events.Add(bibleClass);

                if (firstBibleClass == null)
                {
                    firstBibleClass = bibleClass;
                }
                thursday = thursday.AddDays(7);
            }

            // Hall Cleaning
            var saturday = start.DayOfWeek == DayOfWeek.Saturday ? start : start.AddDays(DayOfWeek.Saturday - start.DayOfWeek);
            Event firstCleanHall = null;
            while (saturday <= end)
            {
                var cleanHall = new Event(calendar, "Hall Cleaning", saturday.AddHours(8), saturday.AddHours(10))
                {
                    Description = "Hall cleaning and maintenance.",
                    AddedById = userId,
                    ParentEvent = firstCleanHall,
                    Repetition = EventRepetition.Weekly,
                    RepetitionEndOn = end,
                    RepetitionSize = 1
                };
                var cleaning = new Activity(cleanHall, "Cleaning") { AddedById = userId };
                cleaning.Openings.Add(new Opening(cleaning, "Cleaner", 2, 6, OpeningType.Application, ApplicationProcess.AutoAccept, CriteriaRule.Visibility) { AddedById = userId });
                cleaning.Criteria.Add(new ActivityCriteria(cleaning, clean));
                cleanHall.Activities.Add(cleaning);

                calendar.Events.Add(cleanHall);

                if (firstCleanHall == null)
                {
                    firstCleanHall = cleanHall;
                }
                saturday = saturday.AddDays(7);
            }

            using (var transaction = this.Context.Database.BeginTransaction())
            {
                try
                {
                    this.Context.Criteria.AddRange(criteria);
                    this.Context.SaveChanges();

                    this.Context.Calendars.Update(calendar);
                    this.Context.SaveChanges();

                    transaction.Commit();
                }
                catch (DbUpdateException)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return this.Source.Mapper.Map<Models.Calendar>(calendar);
        }

        /// <summary>
        /// Adds a predefined list of participants to the specified calendar.
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        public IEnumerable<Models.Participant> AddParticipants(int calendarId)
        {
            if (this.IsPrincipalAParticipant) throw new NotAuthorizedException();

            var userId = this.Source.Principal.GetUser().Value.ConvertTo<int>();
            var user = this.Context.Users.Include(u => u.Info).SingleOrDefault(u => u.Id == userId) ?? throw new NotAuthorizedException();
            var calendar = this.Context.Calendars.SingleOrDefault(c => c.Id == calendarId) ?? throw new NoContentException(typeof(Calendar));

            // TODO: Need generic function to test permission to allow editing calendar.
            var ownsAccount = this.Context.Accounts.Any(a => a.Id == calendar.AccountId && a.OwnerId == userId);
            if (!ownsAccount) throw new NotAuthorizedException();

            var participants = new List<Participant>();
            using (var transaction = this.Context.Database.BeginTransaction())
            {
                try
                {
                    participants.Add(CreateParticipant(calendar, user, "L Alderson", "Lynette", "Alderson", "lynettealderson@shaw.ca", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "R Bailey", "Rosa", "Bailey", "rosabailey@shaw.ca", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "S Bennett", "Stephen", "Bennett", "stephen.bennett01@gmail.com", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Lecturer, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "J Bennett", "Joan", "Bennett", "joanimbennett@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "M Bennett", "Matthew", "Bennett", "mattsbennett@gmail.com", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Lecturer, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "R Cadieu", "Roberta", "Cadieu", "rjcadieu@shaw.ca", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "L Catchpole", "Larry", "Catchpole", "", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "D Catchpole", "Debbie", "Catchpole", "dcatch@telus.net", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "D Cawston", "Dan", "Cawston", "dancaw@shaw.ca", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Lecturer, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "T Cawston", "Tiana", "Cawston", "dancaw@shaw.ca", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "A Ceron", "Andrea", "Ceron", "1krisely@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "D Clover", "Daniel", "Clover", "danielclover@shaw.ca", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "C Clover", "Carita", "Clover", "danielclover@shaw.ca", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "J Clover", "Jack", "Clover", "jayclo@hotmail.com", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Lecturer, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "V Clover", "Valerie", "Clover", "", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "J Coupar", "Jeni", "Coupar", "jcoupar@shaw.ca", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "B Dangerfield", "Beth", "Dangerfield", "gdanger@shaw.ca", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Pianist, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "C Daniel", "Clive", "Daniel", "clivedaniel@telus.net", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Lecturer, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "J Daniel", "Jennifer", "Daniel", "jenniferdaniel@telus.net", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    // participants.Add(CreateParticipant(calendar, user, "E Daniel", "Eileen", "Daniel", "", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    // participants.Add(CreateParticipant(calendar, user, "G Ferrie", "Gregg", "Ferrie", "gferrie@gmail.com", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Exhort, Skill=Lecturer, Skill=Bible Class"));
                    //participants.Add(CreateParticipant(calendar, user, "V Ferrie", "Vikki", "Ferrie", "vferrie@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "D Foss", "Donna", "Foss", "dfoss@shaw.ca", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "J Foster", "Jeremy", "Foster", "jeremymfoster@hotmail.com", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Lecturer, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "E Foster", "Elizabeth", "Foster", "ejoyfoster@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Pianist, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "L Gilmore", "Linda", "Gilmore", "linda.gilmour@telus.net", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "D Gorman", "Diana", "Gorman", "dianagorman1966@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems"));
                    participants.Add(CreateParticipant(calendar, user, "A Hibbs", "Art", "Hibbs", "art.hibbs@gmail.com", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Lecturer, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "L Hibbs", "Linda", "Hibbs", "2hibbs@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "J Hibbs", "Jeff", "Hibbs", "jeffvictoria09@gmail.com", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Lecturer, Skill=Bible Class, Skill=Pianist, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "V Hibbs", "Victoria", "Hibbs", "jeffvictoria09@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "D Hills", "Diane", "Hills", "", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "C Hutchison", "Cheryalee", "Hutchison", "cheryalee@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "W Hutchison", "William", "Hutchison", "whutchis@shaw.ca", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    // participants.Add(CreateParticipant(calendar, user, "E Jennings", "Elaine", "Jennings", "institches56@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Pianist, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "R Johnston", "Rod", "Johnston", "rodrjohnston@outlook.com", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Lecturer, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "E Johnston", "Elizabeth", "Johnston", "elizabeth-johnston@outlook.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "C Jones", "Carolyn", "Jones", "jonesbc@shaw.ca", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "L Kemp", "Lorraine", "Kemp", "lorraine1134@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "J Kirk", "Joanne", "Kirk", "tekjak@shaw.ca", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "D Knorr", "Denise", "Knorr", "denise00014@hotmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "P Lawrence", "Peter", "Lawrence", "joshua@joshualawrence.ca", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Lecturer, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "H Lawrence", "Hannah", "Lawrence", "hlawrence005@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "M Little", "Mark", "Little", "mwl@netzero.net", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "Z Little", "Zoe", "Little", "zhq.6987@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "M Lucke", "Myra", "Lucke", "myralucke1@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "M Macfarlane", "Mark", "Macfarlane", "macfarlane.9@hotmail.com", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Lecturer, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "C Macfarlane", "Caitlyn", "Macfarlane", "caitlyndaniel2@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Pianist, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "H Macpherson", "Horace", "Macpherson", "smmacpherson4@shaw.ca", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Lecturer, Skill=Bible Class, Skill=Emblems"));
                    participants.Add(CreateParticipant(calendar, user, "S Macpherson", "Sylvia", "Macpherson", "smmacpherson4@shaw.ca", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems"));
                    // participants.Add(CreateParticipant(calendar, user, "B McArthur", "Bertha", "McArthur", "", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "J McStravick", "Joshua", "McStravick", "joshmcstravick@gmail.com", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Lecturer, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "L McStravick", "Leah", "McStravick", "leah.mcstravick@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Pianist, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "M McStravick", "Mike", "McStravick", "mtmcstravick@gmail.com", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Lecturer, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "S McStravick", "Sandra", "McStravick", "ssmcstravick@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "J Morrison", "Janis", "Morrison", "", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "Ja Myren", "Jamie", "Myren", "jamie_my@shaw.ca", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "Jo Myren", "Joe", "Myren", "joseph798185@gmail.com", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Lecturer, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "T Myren", "Trish", "Myren", "trishmyren@icloud.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems"));
                    participants.Add(CreateParticipant(calendar, user, "M Neville", "Matthew", "Neville", "matthew.n.sabrina@gmail.com", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Lecturer, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "S Neville", "Sabrina", "Neville", "matthew.n.sabrina@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Pianist, Skill=Emblems, Skill=Clean"));
                    // participants.Add(CreateParticipant(calendar, user, "A Ormerod", "Alan", "Ormerod", "aormerod@shaw.ca", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Read, Skill=Pray"));
                    participants.Add(CreateParticipant(calendar, user, "R Owens", "Rodney", "Owens", "rodowens@shaw.ca", Gender.Male, "", "", "", "Member=true, Brother=true"));
                    participants.Add(CreateParticipant(calendar, user, "P Pearce", "Peggy", "Pearce", "", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems"));
                    participants.Add(CreateParticipant(calendar, user, "L Pengelly", "Laura", "Pengelly", "laura.myren19@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "M Pilon", "Marianne", "Pilon", "mppilon@shaw.ca", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "M Quindazzi", "Micah", "Quindazzi", "m.j.quindazzi@gmail.com", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Exhort, Skill=Lecturer, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "P Quindazzi", "Philip", "Quindazzi", "", Gender.Male, "", "", "", "Member=true, Brother=true"));
                    participants.Add(CreateParticipant(calendar, user, "A Ralph", "Andrew", "Ralph", "drew.sherry@shaw.ca", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Bible Class, Skill=Emblems"));
                    participants.Add(CreateParticipant(calendar, user, "S Ralph", "Sherry", "Ralph", "drew.sherry@shaw.ca", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems"));
                    participants.Add(CreateParticipant(calendar, user, "P Ratzka", "Paul", "Ratzka", "paulratzka@yahoo.ca", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "B Ratzka", "Bonny", "Ratzka", "sueme38@hotmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "K Ratzka", "Katelin", "Ratzka", "", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "J Rebman", "Joan", "Rebman", "", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "G Salisbury", "Grace", "Salisbury", "normgrace@live.ca", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems"));
                    participants.Add(CreateParticipant(calendar, user, "A Ojamma", "Anne", "Ojamma", "akojamaa@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "G Shrimpton", "Grace", "Shrimpton", "gmcshrimp@shaw.ca", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Pianist, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "W Shrimpton", "Wendy", "Shrimpton", "whitemore@shaw.ca", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "Cl Snobelen", "Clyde", "Snobelen", "csnobelen@csll.ca", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Lecturer, Skill=Bible Class, Skill=Emblems"));
                    participants.Add(CreateParticipant(calendar, user, "E Snobelen", "Evelyn", "Snobelen", "esnobelen@telus.net", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems"));
                    participants.Add(CreateParticipant(calendar, user, "Ch Snobelen", "Chase", "Snobelen", "chase.snobelen@gmail.com", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Lecturer, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "Me Snobelen", "Meagan", "Snobelen", "msmcstravick@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Pianist, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "M Snobelen", "Mark", "Snobelen", "snobelen2716@gmail.com", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Lecturer, Skill=Bible Class, Skill=Pianist, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "Sh Snobelen", "Sherry", "Snobelen", "", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "S Snobelen", "Shawn", "Snobelen", "snobelen@gmail.com", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Lecturer, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "Ma Snobelen", "Marnie", "Snobelen", "marnieleigh@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Pianist, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "T Snobelen", "Taleigh", "Snobelen", "taleigh.rae@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    // participants.Add(CreateParticipant(calendar, user, "A Starcher", "Al", "Starcher", "", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Read, Skill=Pray"));
                    participants.Add(CreateParticipant(calendar, user, "B Stodel", "Bob", "Stodel", "rwstodel@telus.net", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Lecturer, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "D Stodel", "Dianna", "Stodel", "dianastodel@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "V Stodel", "Vic", "Stodel", "marie-vics@shaw.ca", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Read, Skill=Pray"));
                    participants.Add(CreateParticipant(calendar, user, "M Stodel", "Marie", "Stodel", "marie-vics@shaw.ca", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems"));
                    // participants.Add(CreateParticipant(calendar, user, "R Wallace", "Rob", "Wallace", "", Gender.Male, "", "", "", "Member=true, Brother=true"));
                    // participants.Add(CreateParticipant(calendar, user, "L Wallace", "Linda", "Wallace", "", Gender.Female, "", "", "", "Member=true, Sister=true"));
                    participants.Add(CreateParticipant(calendar, user, "P Williamson", "Pat", "Williamson", "pwilliamson369@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "P Willimont", "Pat", "Willimont", "", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems"));
                    participants.Add(CreateParticipant(calendar, user, "A Wood", "Arthur", "Wood", "arthurwood@outlook.com", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Read, Skill=Pray, Skill=Emblems"));
                    participants.Add(CreateParticipant(calendar, user, "S Wood", "Sharon", "Wood", "arthurwood@outlook.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems"));
                    participants.Add(CreateParticipant(calendar, user, "J Woodcock", "Joan", "Woodcock", "", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems"));
                    participants.Add(CreateParticipant(calendar, user, "S Higgs", "Stephen", "Higgs", "stevehiggs@live.ca", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "L Higgs", "Linda", "Higgs", "lin.susan.lh@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "J Hewer", "Judy", "Hewer", "judyhewer@hotmail.ca", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "S Coleswebb", "Sandi", "Coleswebb", "s.coleswebb@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "S Coles", "Sierra", "Coles", "sierracoles1313@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "R Coles", "Rylan", "Coles", "rylan1315@gmail.com", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "N Crawford", "Nathan", "Crawford", "nathanael.crawford@gmail.com", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Preside, Skill=Exhort, Skill=Serve, Skill=Read, Skill=Pray, Skill=Doorman, Skill=Lecturer, Skill=Bible Class, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "S Crawford", "Sarah", "Crawford", "crawfordsarahm@gmail.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Pianist, Skill=Emblems, Skill=Clean"));
                    participants.Add(CreateParticipant(calendar, user, "A Ligay", "Arina", "Ligay", "arinaligay@yahoo.com", Gender.Female, "", "", "", "Member=true, Sister=true, Skill=Emblems, Skill=Clean"));
                    // participants.Add(CreateParticipant(calendar, user, "N Fernando", "Naleen", "Fernando", "naleenandjulie@gmail.com", Gender.Male, "", "", "", "Member=true, Brother=true, Skill=Exhort, Skill=Bible Class"));
                    // participants.Add(CreateParticipant(calendar, user, "N Orsetti", "Gavin", "Orsetti", "jeremymichaelfoster@gmail.com", Gender.Male, "", "", "", "Member=false, Brother=true, Skill=Exhort, Skill=Lecturer, Skill=Bible Class"));
                    // participants.Add(CreateParticipant(calendar, user, "I MacFarlane", "Ian", "MacFarlane", "jeremymichaelfoster@gmail.com", Gender.Male, "", "", "", "Member=false, Brother=true, Skill=Exhort, Skill=Lecturer, Skill=Bible Class"));
                    // participants.Add(CreateParticipant(calendar, user, "M Hunter", "Marc", "Hunter", "jeremymichaelfoster@gmail.com", Gender.Male, "", "", "", "Member=false, Brother=true, Skill=Exhort, Skill=Lecturer, Skill=Bible Class"));
                    this.Context.SaveChanges();

                    transaction.Commit();
                }
                catch (DbUpdateException)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return participants.Select(p => this.Source.Mapper.Map<Models.Participant>(p));
        }

        private Participant CreateParticipant(Calendar calendar, User user, string displayName, string firstName, string lastName, string email, Gender gender, string mobile, string phone, string address, string attributes)
        {
            if (calendar == null) throw new ArgumentNullException(nameof(calendar));
            if (user == null) throw new ArgumentNullException(nameof(user));

            Address home = null;
            if (!String.IsNullOrWhiteSpace(address))
            {
                var parts = address.Split(',').Select(v => v.Trim()).ToArray();
                home = new Address("Home", parts[0], parts[1], parts[2], parts[3], parts[4]);
            }
            var p = new Participant(calendar, displayName, firstName, lastName, email, home)
            {
                AddedById = user.Id,
                Gender = gender
            };
            this.Context.Participants.Add(p);

            if (!String.IsNullOrWhiteSpace(mobile))
            {
                var ci = new ContactInfo(p, "Mobile", ContactInfoType.Mobile, ContactInfoCategory.Personal, mobile) { AddedById = user.Id };
                this.Context.ContactInfo.Add(ci);
            }
            if (!String.IsNullOrWhiteSpace(mobile))
            {
                var ci = new ContactInfo(p, "Home Phone", ContactInfoType.Phone, ContactInfoCategory.Personal, phone) { AddedById = user.Id };
                this.Context.ContactInfo.Add(ci);
            }
            if (!String.IsNullOrWhiteSpace(attributes))
            {
                var parts = attributes.Split(',').Select(v => v.Trim()).ToArray();
                foreach (var part in parts)
                {
                    var kv = part.Split('=');
                    var a = new Entities.Attribute(p, kv[0], kv[1], kv[1].Equals("true") ? typeof(bool) : typeof(string)) { AddedById = user.Id };
                    this.Context.Attributes.Add(a);
                }
            }
            return p;
        }
        #endregion
    }
}
