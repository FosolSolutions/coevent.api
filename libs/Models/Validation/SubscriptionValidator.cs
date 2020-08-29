using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoEvent.Models.Validation
{
    public class SubscriptionValidator : AbstractValidator<Subscription>
    {
        #region Constructors
        public SubscriptionValidator()
        {
            RuleFor(m => m.Id).GreaterThanOrEqualTo(0);
        }
        #endregion
    }
}
