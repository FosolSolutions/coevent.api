using FluentValidation;

namespace CoEvent.Models.Validation
{
    public class AccountValidator : AbstractValidator<Account>
    {
        #region Constructors
        public AccountValidator()
        {
            RuleFor(m => m.Id).GreaterThanOrEqualTo(0);
        }
        #endregion
    }
}
