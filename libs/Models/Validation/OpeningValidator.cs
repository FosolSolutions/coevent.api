using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoEvent.Models.Validation
{
    public class OpeningValidator : AbstractValidator<Opening>
    {
        #region Constructors
        public OpeningValidator()
        {
            RuleFor(m => m.Id).GreaterThanOrEqualTo(0);
        }
        #endregion
    }
}
