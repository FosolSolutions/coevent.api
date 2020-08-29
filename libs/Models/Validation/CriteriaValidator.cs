using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoEvent.Models.Validation
{
    public class CriteriaValidator : AbstractValidator<Criteria>
    {
        #region Constructors
        public CriteriaValidator()
        {
            RuleFor(m => m.Id).GreaterThanOrEqualTo(0);
        }
        #endregion
    }
}
