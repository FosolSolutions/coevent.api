using System.Collections.Generic;

namespace CoEvent.Core.Mvc.Middleware
{
    public class ResponseHeaderPolicy
    {
        #region Properties
        public IDictionary<string, string> SetHeaders { get; } = new Dictionary<string, string>();
        public ISet<string> RemoveHeaders { get; } = new HashSet<string>();
        #endregion
    }
}
