using AutoMapper;
using CoEvent.Core.Extensions;
using CoEvent.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoEvent.Data.Maps
{
    /// <summary>
    /// ModelProfile class, provides a way to map entities and models.
    /// </summary>
    public class ModelProfile : Profile
    {
        #region Properties
        protected IDataSource DataSource { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an ModelProfile object, and initializes it with the specified arguments.
        /// </summary>
        public ModelProfile()
        {
            CreateMap<Entities.BaseEntity, Models.BaseModel>()
                .Include<Entities.Subscription, Models.Subscription>()
                .Include<Entities.User, Models.User>()
                .Include<Entities.Account, Models.Account>()
                .Include<Entities.Calendar, Models.Calendar>()
                .Include<Entities.Participant, Models.Participant>()
                .Include<Entities.Event, Models.Event>()
                .Include<Entities.Activity, Models.Activity>()
                .Include<Entities.Opening, Models.Opening>()
                .Include<Entities.Attribute, Models.Attribute>()
                .Include<Entities.Schedule, Models.Schedule>()
                .Include<Entities.OauthAccount, Models.OauthAccount>()
                .Include<Entities.Question, Models.Question>()
                .Include<Entities.QuestionOption, Models.QuestionOption>()
                .Include<Entities.OpeningParticipant, Models.OpeningApplication>()
                .Include<Entities.OpeningAnswer, Models.Answer>()
                .Include<Entities.Process, Models.Process>()
                .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)))
                .ReverseMap()
                .ForMember(dest => dest.AddedById, opt => opt.MapFrom(src => src.RowVersion == null ? this.DataSource.Principal.GetUser().Value.ConvertTo<int>() : src.AddedById))
                .ForMember(dest => dest.AddedOn, opt => opt.MapFrom(src => src.RowVersion == null ? DateTime.UtcNow : src.AddedOn))
                .ForMember(dest => dest.UpdatedById, opt => opt.MapFrom(src => src.RowVersion == null ? (int?)null : this.DataSource.Principal.GetUser().Value.ConvertTo<int>()))
                .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(src => src.RowVersion == null ? (DateTime?)null : DateTime.UtcNow))
                .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => Convert.FromBase64String(src.RowVersion)));

            // Subscriptions
            CreateMap<Entities.Subscription, Models.Subscription>()
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Id == 0 && src.Id.HasValue || src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));

                        // Schedules
            CreateMap<Entities.Schedule, Models.Schedule>()
                .ForPath(dest => dest.Events, opt => opt.MapFrom(src => src.Events.Select(a => a.Event)))
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Id.HasValue || src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));
            CreateMap<Entities.ScheduleEvent, Models.Event>()
                .ReverseMap()
                .ConvertUsing(ConvertEventToScheduleEvent);

            // Users
            CreateMap<Entities.User, Models.User>()
                .ForPath(dest => dest.Attributes, opt => opt.MapFrom(src => src.Attributes.Select(c => c.Attribute)))
                .ForPath(dest => dest.Gender, opt => opt.MapFrom(src => src.Info.Gender))
                .ForPath(dest => dest.FirstName, opt => opt.MapFrom(src => src.Info.FirstName))
                .ForPath(dest => dest.MiddleName, opt => opt.MapFrom(src => src.Info.MiddleName))
                .ForPath(dest => dest.LastName, opt => opt.MapFrom(src => src.Info.LastName))
                .ForPath(dest => dest.Birthdate, opt => opt.MapFrom(src => src.Info.Birthdate))
                .ForPath(dest => dest.HomeAddress, opt => opt.MapFrom(src => src.Info.HomeAddress))
                .ForPath(dest => dest.WorkAddress, opt => opt.MapFrom(src => src.Info.WorkAddress))
                .ForPath(dest => dest.HomePhone, opt => opt.MapFrom(src => src.Info.HomePhone))
                .ForPath(dest => dest.MobilePhone, opt => opt.MapFrom(src => src.Info.MobilePhone))
                .ForPath(dest => dest.WorkPhone, opt => opt.MapFrom(src => src.Info.WorkPhone))
                .ReverseMap()
                .ForPath(dest => dest.Info.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForPath(dest => dest.Info.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForPath(dest => dest.Info.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForPath(dest => dest.Info.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForPath(dest => dest.Info.Birthdate, opt => opt.MapFrom(src => src.Birthdate))
                .ForPath(dest => dest.Info.HomeAddress, opt => opt.MapFrom(src => src.HomeAddress))
                .ForPath(dest => dest.Info.WorkAddress, opt => opt.MapFrom(src => src.WorkAddress))
                .ForPath(dest => dest.Info.HomePhone, opt => opt.MapFrom(src => src.HomePhone))
                .ForPath(dest => dest.Info.MobilePhone, opt => opt.MapFrom(src => src.MobilePhone))
                .ForPath(dest => dest.Info.WorkPhone, opt => opt.MapFrom(src => src.WorkPhone))
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Id.HasValue || src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));

            CreateMap<Entities.OauthAccount, Models.OauthAccount>()
                .ReverseMap();

            // Accounts
            CreateMap<Entities.Account, Models.Account>()
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Id.HasValue || src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));

            // Calendars
            CreateMap<Entities.Calendar, Models.Calendar>()
                .ForPath(dest => dest.Criteria, opt => opt.MapFrom(src => src.Criteria.Select(c => c.Criteria)))
                .ForPath(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Id.HasValue || src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));

            CreateMap<Entities.CalendarTag, Models.Tag>()
                .ReverseMap();

            // Participants
            CreateMap<Entities.Participant, Models.Participant>()
                .ForPath(dest => dest.ContactInfo, opt => opt.MapFrom(src => src.ContactInfo.Select(a => a.ContactInfo)))
                .ForPath(dest => dest.Attributes, opt => opt.MapFrom(src => src.Attributes.Select(a => a.Attribute)))
                .ForPath(dest => dest.WorkPhone, opt => opt.MapFrom(src => src.WorkPhone))
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Id.HasValue || src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));

            // Events
            CreateMap<Entities.Event, Models.Event>()
                .ForPath(dest => dest.Criteria, opt => opt.MapFrom(src => src.Criteria.Select(c => c.Criteria)))
                .ForPath(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Id.HasValue || src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));

            CreateMap<Entities.EventTag, Models.Tag>()
                .ReverseMap();

            // Activities
            CreateMap<Entities.Activity, Models.Activity>()
                .ForPath(dest => dest.Criteria, opt => opt.MapFrom(src => src.Criteria.Select(c => c.Criteria)))
                .ForPath(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Id.HasValue || src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));

            CreateMap<Entities.ActivityTag, Models.Tag>()
                .ReverseMap();

            // Openings
            CreateMap<Entities.Opening, Models.Opening>()
                .ForPath(dest => dest.Criteria, opt => opt.MapFrom(src => src.Criteria.Select(c => c.Criteria)))
                .ForPath(dest => dest.Applications, opt => opt.MapFrom(src => src.Participants))
                .ForPath(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants.Where(p => p.State == Entities.OpeningApplicationState.Accepted).Select(p => p.Participant)))
                .ForPath(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Id.HasValue || src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));

            CreateMap<Entities.OpeningParticipant, Models.OpeningApplication>()
                .ForMember(dest => dest.OpeningId, opt => opt.MapFrom(src => src.OpeningId))
                .ForMember(dest => dest.Participant, opt => opt.MapFrom(src => src.Participant))
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers))
                .ReverseMap()
                .ForMember(dest => dest.OpeningId, opt => opt.MapFrom(src => src.OpeningId))
                .ForMember(dest => dest.ParticipantId, opt => opt.MapFrom(src => src.Participant.Id))
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State));

            CreateMap<Entities.OpeningQuestion, Models.Question>()
                .ForPath(dest => dest.Id, opt => opt.MapFrom(src => src.QuestionId))
                .ForPath(dest => dest.IsRequired, opt => opt.MapFrom(src => src.Question.IsRequired))
                .ForPath(dest => dest.MaxLength, opt => opt.MapFrom(src => src.Question.MaxLength))
                .ForPath(dest => dest.Sequence, opt => opt.MapFrom(src => src.Question.Sequence))
                .ForPath(dest => dest.Text, opt => opt.MapFrom(src => src.Question.Text))
                .ForPath(dest => dest.Caption, opt => opt.MapFrom(src => src.Question.Caption))
                .ForPath(dest => dest.AccountId, opt => opt.MapFrom(src => src.Question.AccountId))
                .ForPath(dest => dest.Options, opt => opt.MapFrom(src => src.Question.Options))
                .ReverseMap()
                .ConvertUsing(ConvertQuestionToOpenQuestion);

            CreateMap<Entities.OpeningTag, Models.Tag>()
                .ReverseMap();

            CreateMap<Entities.OpeningAnswer, Models.Answer>()
                .ReverseMap();

            // Process
            CreateMap<Entities.Process, Models.Process>()
                .ReverseMap();

            // Questions
            CreateMap<Entities.Question, Models.Question>()
                .ReverseMap();

            CreateMap<Entities.QuestionOption, Models.QuestionOption>()
                .ReverseMap();

            // Criteria
            CreateMap<Entities.CriteriaObject, Models.Criteria>()
                .ConvertUsing(ConvertCriteriaObjectToCriteria);
            CreateMap<Models.Criteria, Entities.CriteriaObject>()
                .ConvertUsing(ConvertCriteriaToCriteriaObject);
            //.ForMember(dest => dest.Criterion, opt => opt.MapFrom(src => src.ToString()))
            //.ReverseMap()
            //.ForMember(dest => dest.Criteria, opt => opt.MapFrom(src => src.Criterion));

            // Attributes
            CreateMap<Entities.Attribute, Models.Attribute>().ReverseMap();
            CreateMap<Entities.ParticipantAttribute, Models.Attribute>()
                .ReverseMap()
                .ConvertUsing(ConvertParticipantAttributeToAttribute);
        }
        #endregion

        #region Methods
        public void BindDataSource(IDataSource datasource)
        {
            this.DataSource = datasource ?? throw new ArgumentNullException(nameof(datasource));
        }

        Entities.OpeningQuestion ConvertQuestionToOpenQuestion(Models.Question src, Entities.OpeningQuestion dest)
        {
            var question = new Entities.Question()
            {
                Id = src.Id
            };
            return new Entities.OpeningQuestion() { Question = question, QuestionId = question.Id };
        }

        Entities.ScheduleEvent ConvertEventToScheduleEvent(Models.Event src, Entities.ScheduleEvent dest)
        {
            var cevent = new Entities.Event()
            {
                Id = src.Id.Value,
                Key = src.Key.Value,
                Name = src.Name,
                Description = src.Description,
                CalendarId = src.CalendarId,
                StartOn = src.StartOn,
                EndOn = src.EndOn,
                State = src.State,
                CriteriaRule = src.CriteriaRule,
                AddedById = src.Id == 0 ? this.DataSource.Principal.GetUser().Value.ConvertTo<int>() : src.AddedById.Value,
                AddedOn = src.Id == 0 ? DateTime.UtcNow : src.AddedOn.Value,
                UpdatedById = src.Id == 0 ? (int?)null : this.DataSource.Principal.GetUser().Value.ConvertTo<int>(),
                UpdatedOn = src.Id == 0 ? (DateTime?)null : DateTime.UtcNow,
                RowVersion = Convert.FromBase64String(src.RowVersion)
            };
            return new Entities.ScheduleEvent() { EventId = src.Id.Value, Event = cevent };
        }
        Models.Criteria ConvertCriteriaObjectToCriteria(Entities.CriteriaObject src, Models.Criteria dest)
        {
            if (src == null) return null;

            var criteria = (Entities.Criteria)src;

            if (criteria is Entities.CriteriaGroup)
            {
                var group = criteria as Entities.CriteriaGroup;
                return new Models.Criteria()
                {
                    Id = src.Id,
                    LogicalOperator = group.LogicalOperator,
                    Conditions = new List<Models.CriteriaValue>(group.Criteria.Select(c => (Entities.CriteriaValue)c).Select(c => new Models.CriteriaValue() { LogicalOperator = c.LogicalOperator, Key = c.Key, Value = c.Value, ValueType = c.ValueType }))
                };
            }

            var value = criteria as Entities.CriteriaValue;
            return new Models.Criteria()
            {
                Id = src.Id,
                Conditions = new List<Models.CriteriaValue>(new[] { new Models.CriteriaValue() { LogicalOperator = value.LogicalOperator, Key = value.Key, Value = value.Value, ValueType = value.ValueType } })
            };
        }

        Entities.CriteriaObject ConvertCriteriaToCriteriaObject(Models.Criteria src, Entities.CriteriaObject dest)
        {
            if (src == null) return null;

            if (src.Conditions.Count > 1)
            {
                var group = new Entities.CriteriaGroup(src.LogicalOperator.Value, src.Conditions.Select(c => new Entities.CriteriaValue(c.LogicalOperator, c.Key, c.Value, Type.GetType(c.ValueType))).ToArray()) { Id = src.Id };
                return new Entities.CriteriaObject(group);
            }

            var criteria = src.Conditions.FirstOrDefault();

            if (criteria == null) return null;
            return new Entities.CriteriaObject(new Entities.CriteriaValue(criteria.LogicalOperator, criteria.Key, criteria.Value, Type.GetType(criteria.ValueType)) { Id = src.Id });
        }

        Entities.ParticipantAttribute ConvertParticipantAttributeToAttribute(Models.Attribute src, Entities.ParticipantAttribute dest)
        {
            var type = String.IsNullOrWhiteSpace(src.ValueType) ? typeof(string) : Type.GetType(src.ValueType) ?? typeof(string);
            var attribute = src.Id == 0 ? new Entities.Attribute(src.Key, src.Value, type)
            {
                AddedById = src.Id == 0 ? this.DataSource.Principal.GetUser().Value.ConvertTo<int>() : src.AddedById.Value,
                AddedOn = src.Id == 0 ? DateTime.UtcNow : src.AddedOn.Value,
                UpdatedById = src.Id == 0 ? (int?)null : this.DataSource.Principal.GetUser().Value.ConvertTo<int>(),
                UpdatedOn = src.Id == 0 ? (DateTime?)null : DateTime.UtcNow,
                RowVersion = src.RowVersion == null ? null : Convert.FromBase64String(src.RowVersion)
            } : null;
            return new Entities.ParticipantAttribute() { AttributeId = src.Id, Attribute = attribute };
        }
        #endregion
    }
}
