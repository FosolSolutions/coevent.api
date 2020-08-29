using CoEvent.Core.Extensions;
using CoEvent.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CoEvent.Data.Services
{
    /// <summary>
    /// UserService sealed class, provides a way to manage users in the datasource.
    /// </summary>
    public sealed class UserService : UpdatableService<Entities.User, Models.User>, IUserService
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a UserService object, and initalizes it with the specified options.
        /// </summary>
        /// <param name="source"></param>
        internal UserService(IDataSource source) : base(source)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Verify the user, or oauth account with the specified email exists.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public int Verify(string email)
        {
            return this.Context.Users.Where(u => u.Email == email || u.OauthAccounts.Any(a => a.Email == email)).Select(u => u.Id).SingleOrDefault();
        }

        /// <summary>
        /// Get the user for the specified 'key'.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Models.User Get(Guid key)
        {
            return this.Map(this.Find((set) => set.Include(u => u.Info).SingleOrDefault(u => u.Key == key)));
        }

        /// <summary>
        /// Get the user for the specified 'id'.
        /// Validates whether the current user is authorized to view the user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.User Get(int id)
        {
            return this.Map(this.Find((set) => set.Include(u => u.Info).SingleOrDefault(u => u.Id == id)));
        }

        /// <summary>
        /// Get the claimed identity of the user for the specified 'userId'.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Claim> GetClaims(int userId)
        {
            var user = this.Find((set) => set.Include(u => u.Attributes).ThenInclude(a => a.Attribute).Include(u => u.OwnedAccounts).Include(u => u.Info).SingleOrDefault(u => u.Id == userId));

            var claims = new List<Claim>(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Key}", typeof(Guid).FullName, "CoEvent"),
                new Claim(ClaimTypes.Email, user.Email, typeof(string).FullName, "CoEvent"),
                new Claim(ClaimTypes.Name, user.Username ?? "", typeof(string).FullName, "CoEvent"),
                new Claim(ClaimTypes.GivenName, user.Info.FirstName ?? "", typeof(string).FullName, "CoEvent"),
                new Claim(ClaimTypes.Surname, user.Info.LastName ?? "", typeof(string).FullName, "CoEvent"),
                new Claim(ClaimTypes.Gender, $"{user.Info.Gender}", typeof(Entities.Gender).FullName, "CoEvent"),
                new Claim("User", $"{user.Id}", typeof(int).FullName, "CoEvent"),
                new Claim("Account", $"{user.DefaultAccountId ?? user.OwnedAccounts.FirstOrDefault().Id}", typeof(int).FullName, "CoEvent"),
                new Claim("display_name", user.Info.DisplayName ?? "", typeof(string).FullName, "CoEvent")
            });

            foreach (var attr in user.Attributes)
            {
                claims.Add(new Claim(attr.Attribute.Key, attr.Attribute.Value, attr.Attribute.ValueType, "CoEvent"));
            }

            return claims;
        }


        /// <summary>
        /// Add the specified model to the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <param name="model"></param>
        public override void Add(Models.User model)
        {
            var entity = this.Map(model);
            entity.AddedById = this.GetUserId(); // Oauth users will be null.

            if (entity.Info != null)
            {
                entity.Info.AddedBy = entity;
            }
            if (entity.OauthAccounts.Count() > 0)
            {
                entity.OauthAccounts.ForEach(a =>
                {
                    a.User = entity;
                    a.AddedBy = entity;
                });
            }
            this.Add(entity);
            Track(entity, model);
        }
        #endregion
    }
}
