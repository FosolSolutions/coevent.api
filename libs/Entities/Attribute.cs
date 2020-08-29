using CoEvent.Core.Extensions;
using System;
using System.Collections.Generic;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// Attribute class, provides a way to describe an attribute or qualification that a participant has.
    /// </summary>
    public class Attribute : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - Primary key.  A unique way to identify a attribute.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// get/set - Primary key.  A value to specify the attribute.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// get/set - The datatype of the attribute.
        /// </summary>
        public string ValueType { get; set; }

        /// <summary>
        /// get - A collection of participants associated with this attribute.
        /// </summary>
        public ICollection<ParticipantAttribute> Participants { get; private set; } = new List<ParticipantAttribute>();

        /// <summary>
        /// get - A collection of users associated with this attribute.
        /// </summary>
        public ICollection<UserAttribute> Users { get; private set; } = new List<UserAttribute>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an Attribute object.
        /// </summary>
        public Attribute()
        {

        }

        /// <summary>
        /// Creates a new instance of an Attribute object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public Attribute(string key, string value, Type type)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException($"Argument 'key' cannot be null, empty or whitespace.");
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException($"Argument 'value' cannot be null, empty or whitespace.");

            this.Key = key;
            this.Value = value; // TODO: serialize.
            this.ValueType = type?.FullName ?? throw new ArgumentNullException(nameof(type)); // TODO: handle generic names
        }

        /// <summary>
        /// Creates a new instance of an Attribute object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public Attribute(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException($"Argument 'key' cannot be null, empty or whitespace.");
            if (value == null) throw new ArgumentException($"Argument 'value' cannot be null.");

            this.Key = key;
            this.Value = $"{value}"; // TODO: serialize.
            this.ValueType = value.GetType()?.FullName; // TODO: handle generic names
        }

        /// <summary>
        /// Creates a new instance of an Attribute object, and initializes it with the specified properties.
        /// Links the attribute with the specified participant.
        /// </summary>
        /// <param name="participant"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public Attribute(Participant participant, string key, string value, Type type) : this(key, value, type)
        {
            this.Participants.Add(new ParticipantAttribute(participant ?? throw new ArgumentNullException(nameof(participant)), this));
        }

        /// <summary>
        /// Creates a new instance of an Attribute object, and initializes it with the specified properties.
        /// Links the attribute with the specified user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public Attribute(User user, string key, string value, Type type) : this(key, value, type)
        {
            this.Users.Add(new UserAttribute(user ?? throw new ArgumentNullException(nameof(user)), this));
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get the value after converting it to the configured type.
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            // TODO: deserialize objects.
            return this.Value.ConvertTo(this.GetValueType());
        }

        /// <summary>
        /// Get the type of the value.
        /// </summary>
        /// <returns></returns>
        public Type GetValueType()
        {
            return Type.GetType(this.ValueType);
        }
        #endregion
    }
}
