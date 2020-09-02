using CoEvent.Core.Mvc;
using CoEvent.Core.Mvc.Filters;
using CoEvent.Data.Interfaces;
using CoEvent.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoEvent.Api.Areas.Data.Controllers
{
    /// <summary>
    /// AccountsController sealed class, provides API endpoints for calendar accounts.
    /// </summary>
    [Produces("application/json")]
    [Area("manage")]
    [Route("[area]/[controller]")]
    [Authorize]
    [ValidateModelFilter]
    public sealed class AccountsController : ApiController
    {
        #region Variables
        private readonly IDataSource _dataSource;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a AccountsController object.
        /// </summary>
        /// <param name="datasource"></param>
        public AccountsController(IDataSource datasource)
        {
            _dataSource = datasource;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Returns a calendar account for the specified 'id'.
        /// </summary>
        /// <param name="id">The primary key for the account.</param>
        /// <returns>An account for the specified 'id'.</returns>
        [HttpGet("{id}")]
        public IActionResult GetAccount(int id) // TODO: Should I use async?
        {
            var account = _dataSource.Accounts.Get(id);
            return Ok(account);
        }

        /// <summary>
        /// Adds the new account to the datasource.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost()]
        public IActionResult AddAccount([FromBody] Account account)
        {
            _dataSource.Accounts.Add(account);
            _dataSource.CommitTransaction();

            return Created(Url.RouteUrl(nameof(GetAccount), new { account.Id }), account);
        }

        /// <summary>
        /// Updates the specified account in the datasource.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult UpdateAccount(int id, [FromBody] Account account)
        {
            _dataSource.Accounts.Update(account);
            _dataSource.CommitTransaction();

            return Ok(account);
        }

        /// <summary>
        /// Deletes the specified account from the datasource.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id, [FromBody] Account account)
        {
            _dataSource.Accounts.Remove(account);
            _dataSource.CommitTransaction();

            return Ok();
        }
        #endregion
    }
}
