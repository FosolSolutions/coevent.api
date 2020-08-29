using System.Collections.Generic;

namespace CoEvent.Api.Models
{
    public class PageModel<T>
    {
        #region Properties
        public int Page { get; set; }
        public int Total { get; set; }
        public IEnumerable<T> Items { get; set; }
        #endregion

        #region Constructors
        public PageModel() { }

        public PageModel(int page, int total, IEnumerable<T> items)
        {
            this.Page = page;
            this.Total = total;
            this.Items = items;
        }
        #endregion
    }
}
