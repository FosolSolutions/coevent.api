using System;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// OpeningAction class, provides a way to manage actions to perform on openings based on events.
    /// </summary>
    public class Process : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the parent opening.
        /// </summary>
        public int OpeningId { get; set; }

        /// <summary>
        /// get/set - The opening that owns this action.
        /// </summary>
        public Opening Opening { get; set; }

        /// <summary>
        /// get/set - The trigger which will result in this action to be performed.
        /// </summary>
        public ActionTrigger Trigger { get; set; }

        /// <summary>
        /// get/set - The action to perform when the event occurs.  This requires a special syntax to work (i.e. [Action]([Args]))
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// get/set - The sequence the actions should be performed.
        /// </summary>
        public int Sequence { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an OpeningAction object.
        /// </summary>
        public Process()
        { }

        /// <summary>
        /// Creates a new instance of an OpeningAction object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="opening"></param>
        /// <param name="trigger"></param>
        /// <param name="action"></param>
        /// <param name="sequence"></param>
        public Process(Opening opening, ActionTrigger trigger, string action, int sequence = 0)
        {
            if (String.IsNullOrWhiteSpace(action)) throw new ArgumentException("The argument 'action' cannot be null, empty or whitespace.");

            this.OpeningId = opening?.Id ?? throw new ArgumentNullException(nameof(opening));
            this.Opening = opening;
            this.Trigger = trigger;
            this.Action = action;
            this.Sequence = sequence;
        }
        #endregion

        #region Methods
        #endregion
    }
}
