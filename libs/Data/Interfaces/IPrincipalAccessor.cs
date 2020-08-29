using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace CoEvent.Data.Interfaces
{
    public interface IPrincipalAccessor
    {
        #region Properties
        ClaimsPrincipal Principal { get; }
        #endregion
    }
}
