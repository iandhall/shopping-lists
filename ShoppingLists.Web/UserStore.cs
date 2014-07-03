using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using ShoppingLists.BusinessLayer;
using ShoppingLists.Web.Models;

namespace ShoppingLists.Web
{
    public class UserStore : IUserStore<AspNetUser>, IUserPasswordStore<AspNetUser>, IUserSecurityStampStore<AspNetUser>
    {
        private UserService service;

        public UserStore(UserService service)
        {
            this.service = service;
        }

        public void Dispose()
        {
        }

        #region IUserStore
        public virtual Task CreateAsync(AspNetUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            return Task.Factory.StartNew(() =>
            {
                var entityUser = user.GetEntityUser();
                service.Create(entityUser);
                user.Id = entityUser.Id;
            });
        }

        public virtual Task DeleteAsync(AspNetUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            return Task.Factory.StartNew(() =>
            {
                service.Delete(user.Id);
            });
        }

        public virtual Task<AspNetUser> FindByIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException("userId");
            Guid parsedUserId;
            if (!Guid.TryParse(userId, out parsedUserId)) throw new ArgumentOutOfRangeException("userId", string.Format("'{0}' is not a valid GUID.", new { userId }));
            return Task.Factory.StartNew(() =>
            {
                return AspNetUser.EntityToAspNetUser(service.Get(userId));
            });
        }

        public virtual Task<AspNetUser> FindByNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException("userName");
            return Task.Factory.StartNew(() =>
            {
                return AspNetUser.EntityToAspNetUser(service.GetByName(userName));
            });
        }

        public virtual Task UpdateAsync(AspNetUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            return Task.Factory.StartNew(() =>
            {
                service.Update(user.GetEntityUser());
            });
        }
        #endregion

        #region IUserPasswordStore
        public virtual Task<string> GetPasswordHashAsync(AspNetUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            return Task.FromResult(user.PasswordHash);
        }

        public virtual Task<bool> HasPasswordAsync(AspNetUser user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public virtual Task SetPasswordHashAsync(AspNetUser user, string passwordHash)
        {
            if (user == null) throw new ArgumentNullException("user");
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }
        #endregion

        #region IUserSecurityStampStore
        public virtual Task<string> GetSecurityStampAsync(AspNetUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            return Task.FromResult(user.SecurityStamp);
        }

        public virtual Task SetSecurityStampAsync(AspNetUser user, string stamp)
        {
            if (user == null) throw new ArgumentNullException("user");
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }
        #endregion
    }
}