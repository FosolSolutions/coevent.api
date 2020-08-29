using System;
using System.Collections.Generic;
using System.Text;

namespace CoEvent.Models
{
    public class Process : BaseModel
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
        /// get/set - The trigger which will result in this action to be performed.
        /// </summary>
        public Data.Entities.ActionTrigger Trigger { get; set; }

        /// <summary>
        /// get/set - The action to perform when the event occurs.  This requires a special syntax to work (i.e. [Action]([Args]))
        /// </summary>
        public string Statement { get; set; }

        /// <summary>
        /// get/set - The sequence the actions should be performed.
        /// </summary>
        public int Sequence { get; set; }
        #endregion
    }
}
