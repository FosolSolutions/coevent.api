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
    /// OpeningService sealed class, provides a way to manage openings in the datasource.
    /// </summary>
    public sealed class OpeningService : UpdatableService<Entities.Opening, Models.Opening>, IOpeningService
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a OpeningService object, and initalizes it with the specified options.
        /// </summary>
        /// <param name="source"></param>
        internal OpeningService(IDataSource source) : base(source)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the opening for the specified 'id'.
        /// Validates whether the current user is authorized to view the opening.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.Opening Get(int id)
        {
            this.VerifyPrincipal();
            var accountId = this.GetAccountId();
            var opening = this.Find((set) => set
                .Include(o => o.Criteria)
                .Include(o => o.Questions).ThenInclude(oq => oq.Question).ThenInclude(q => q.Options)
                .Include(o => o.Tags)
                .Include(o => o.Participants).ThenInclude(op => op.Participant)
                .Include(o => o.Participants).ThenInclude(op => op.Answers).ThenInclude(oa => oa.Options).ThenInclude(oaqo => oaqo.Option)
                .SingleOrDefault(o => o.Id == id && o.Activity.Event.Calendar.AccountId == accountId));
            return this.Map(opening);
        }

        /// <summary>
        /// Get the openings for the specified 'calendard' and within the specified timeframe.
        /// Validates whether the current use is authorized to view the calendar.
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="startOn"></param>
        /// <param name="endOn"></param>
        /// <returns></returns>
        public IEnumerable<Models.Opening> GetForCalendar(int calendarId, DateTime startOn, DateTime endOn)
        {
            var participantId = this.GetParticipantId();
            var isAuthorized = this.Context.Calendars.Any(c => c.Id == calendarId && c.Participants.Any(p => p.Id == participantId));
            if (!isAuthorized) throw new NotAuthorizedException();

            // Convert datetime to utc.
            var start = startOn.ToUniversalTime();
            var end = endOn.ToUniversalTime();

            var openings = (
                from o in this.Context.Openings
                    .Include(a => a.Criteria).ThenInclude(c => c.Criteria)
                    .Include(o => o.Tags)
                    .Include(o => o.Questions).ThenInclude(q => q.Answers)
                    .Include(o => o.Participants).ThenInclude(op => op.Participant)
                where o.Activity.Event.CalendarId == calendarId
                    && o.Activity.StartOn >= startOn
                    && o.Activity.EndOn <= endOn
                orderby o.Activity.StartOn, o.Activity.Sequence, o.Name
                select o
                ).ToArray()
                .Select(o => this.Map(o));

            return openings;
        }

        /// <summary>
        /// The participant is apply for the opening.
        /// </summary>
        /// <param name="application"></param>
        /// <param name="participant"></param>
        /// <returns></returns>
        public Models.Opening Apply(Models.OpeningApplication application, Models.Participant participant = null)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));

            var userId = this.GetUserId();
            var participantId = participant?.Id ?? this.GetParticipantId() ?? throw new NotAuthorizedException();
            var eparticipant = this.Find<Participant>((set) => set.Include(p => p.Attributes).ThenInclude(a => a.Attribute).SingleOrDefault(p => p.Id == participantId)) ?? throw new NotAuthorizedException();

            var eopening = this.Find((set) => set
                .Include(o => o.Participants).ThenInclude(op => op.Participant)
                .Include(o => o.Questions).ThenInclude(oq => oq.Question)
                .Include(o => o.Criteria).ThenInclude(c => c.Criteria)
                .Include(o => o.Activity).ThenInclude(a => a.Event)
                .SingleOrDefault(o => o.Id == application.OpeningId));
            if (Convert.ToBase64String(eopening.RowVersion) != application.RowVersion) throw new InvalidOperationException($"The opening has been updated recently.  Please resync your version before applying."); // TODO: Resource file for text.

            return Apply(eopening, eparticipant, application.Answers?.ToArray());
        }


        /// <summary>
        /// The participant is apply for the opening.
        /// </summary>
        /// <param name="opening"></param>
        /// <param name="participant"></param>
        /// <param name="answers"></param>
        /// <returns></returns>
        private Models.Opening Apply(Entities.Opening opening, Entities.Participant participant, params Models.Answer[] answers)
        {
            var userId = this.GetUserId();
            var participantCount = opening.Participants.Count(p => p.State == OpeningApplicationState.Accepted);
            if (opening.MaxParticipants <= participantCount) throw new InvalidOperationException($"The opening has been filled, {participantCount} of {opening.MaxParticipants} applications have been accepted.");

            if (opening.Participants.Any(p => p.ParticipantId == participant.Id)) throw new InvalidOperationException($"Cannot apply for the same opening more than once.");

            var openingCriteria = opening.Criteria.ToArray().Select(c => (Criteria)c.Criteria);
            var activityCriteria = this.Context.ActivityCriteria.Include(ac => ac.Criteria).Where(ac => ac.ActivityId == opening.ActivityId).ToArray().Select(ac => (Criteria)ac.Criteria);
            var eventCriteria = this.Context.EventCriteria.Include(ec => ec.Criteria).Where(ec => ec.EventId == opening.Activity.EventId).ToArray().Select(ac => (Criteria)ac.Criteria);
            var calendarCriteria = this.Context.CalendarCriteria.Include(cc => cc.Criteria).Where(cc => cc.CalendarId == opening.Activity.Event.CalendarId).ToArray().Select(ac => (Criteria)ac.Criteria);
            var criteria = openingCriteria.Concat(activityCriteria).Concat(eventCriteria).Concat(calendarCriteria);

            // Validate criteria
            if (!criteria.All(c => c.Validate(participant.Attributes.Select(a => a.Attribute).ToArray()))) throw new InvalidOperationException($"The participant '{participant.DisplayName}' does not have the required attributes to apply to this opening.");

            // Validate questions
            var answeredQuestionIds = answers?.Select(a => a.QuestionId).ToArray() ?? new int[0];
            var hasAnsweredQuestions = opening.Questions.All(q => q.Question.IsRequired && answeredQuestionIds.Contains(q.QuestionId));

            if (!hasAnsweredQuestions) throw new InvalidOperationException($"The application is missing an answer to a question.");

            var state = opening.ApplicationProcess == ApplicationProcess.Review ? OpeningApplicationState.Applied : OpeningApplicationState.Accepted;
            var op = new OpeningParticipant(opening, participant, state) { AddedById = userId };
            var eanswers = answers?.Select(a => new OpeningAnswer() { OpeningId = opening.Id, QuestionId = a.QuestionId, ParticipantId = participant.Id, OpeningParticipant = op, Text = a.Text, AddedById = userId }).ToArray() ?? new OpeningAnswer[0]; // TODO: Save answer options.
            eanswers.ForEach(a => op.Answers.Add(a));
            opening.Participants.Add(op);
            this.Context.OpeningParticipants.Add(op);

            PerformAction(op, ActionTrigger.Apply);

            if (opening.ApplicationProcess == ApplicationProcess.AutoAccept)
            {
                PerformAction(op, ActionTrigger.Accept);
            }

            var result = this.Map(opening);
            Track(opening, result);
            return result;
        }

        /// <summary>
        /// The participant is unapplying to the opening.
        /// </summary>
        /// <param name="opening"></param>
        /// <param name="participants"></param>
        /// <returns></returns>
        public Models.Opening Unapply(Models.Opening opening, params Models.Participant[] participants)
        {
            if (opening == null) throw new ArgumentNullException(nameof(opening));

            // If none are specified select the current user's participant.
            if (participants == null || participants.Count() == 0)
            {
                var participantId = this.GetParticipantId();
                var participant = this.Find<Participant>((set) => set.Find(participantId));
                participants = new[] { new Models.Participant() { Id = participant.Id } };
            }

            var eopening = this.Find((set) => set
                .Include(o => o.Participants).ThenInclude(op => op.Participant)
                .Include(o => o.Questions).ThenInclude(oq => oq.Question)
                .Include(o => o.Criteria).ThenInclude(c => c.Criteria)
                .SingleOrDefault(o => o.Id == opening.Id));
            if (Convert.ToBase64String(eopening.RowVersion) != opening.RowVersion) throw new InvalidOperationException($"The opening has been updated recently.  Please resync your version before unapplying.");

            var applicationIds = participants.Select(a => a.Id).ToArray();
            var applications = eopening.Participants.Where(a => applicationIds.Contains(a.ParticipantId));
            applications.ForEach(op =>
            {
                PerformAction(op, ActionTrigger.Unapply);
                eopening.Participants.Remove(op);

                op.Answers.ForEach(aa =>
                {
                    aa.Options.ForEach(o => this.Context.OpeningAnswerQuestionOptions.Remove(o)); // Delete OpeningAnswerQuestionOption.
                    aa.Options.Clear();
                    this.Context.OpeningAnswers.Remove(aa); // Delete OpeningAnswer.
                });
                op.Answers.Clear();
            });
            this.Context.OpeningParticipants.RemoveRange(applications);

            var result = this.Map(eopening);
            Track(eopening, result);
            return result;
        }

        private void PerformAction(Entities.OpeningParticipant openingParticipant, ActionTrigger trigger)
        {
            var processes = this.Context.Processes.Where(oa => oa.OpeningId == openingParticipant.OpeningId && oa.Trigger == trigger).OrderBy(oa => oa.Sequence);
            processes.ForEach(p => this.Source.Actions.Add(new Helpers.OpeningAction(p, openingParticipant)));
        }
        #endregion
    }
}
