using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoEvent.Models.Validation
{
    public class AddressValidator : AbstractValidator<Address>
    {
        #region Constructors
        public AddressValidator()
        {
        }
        #endregion
    }
}
