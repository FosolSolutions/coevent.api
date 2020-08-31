using System;
using System.Collections.Generic;
using System.Text;

namespace CoEvent.Data.Helpers
{
    public struct OpeningAction
    {
        #region Properties
        public Entities.Process Process { get; }

        public Entities.OpeningParticipant OpeningParticipant { get; }
        #endregion

        #region Constructors
        public OpeningAction(Entities.Process process, Entities.OpeningParticipant openingParticipant)
        {
            this.Process = process;
            this.OpeningParticipant = openingParticipant;
        }
        #endregion
    }
}
