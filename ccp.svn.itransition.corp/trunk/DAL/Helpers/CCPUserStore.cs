using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCP.DAL.DataModels;
using Microsoft.AspNet.Identity;

namespace CCP.DAL.Helpers
{
    public class CCPUserStore :
        IUserClaimStore<User, long>, 
        IUserEmailStore<User, long>, 
        IUserLoginStore<User, long>, 
        IUserRoleStore<User, long>, 
        IUserPasswordStore<User, long>, 
        IUserSecurityStampStore<User, long>
    {
        private readonly CCPEntities _ccpContext;

        public CCPUserStore()
        {
            _ccpContext = new CCPEntities();
        }
        public CCPUserStore(CCPEntities dbContext)
        {
            _ccpContext = dbContext;
        }

        public async Task CreateAsync(User user)
        {
            _ccpContext.Entry(user).State = EntityState.Added;
            
            await _ccpContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _ccpContext.Entry(user).State = EntityState.Deleted;

            await _ccpContext.SaveChangesAsync();
        }

        public async Task<User> FindByIdAsync(long userId)
        {
            return await _ccpContext.Users.FindAsync(userId);
        }

        public async Task<User> FindByNameAsync(string userName)
        {
            var us = await _ccpContext.Users.FirstOrDefaultAsync(u => String.Equals(u.Email, userName));
            return us;
        }

        public async Task UpdateAsync(User user)
        {
            _ccpContext.Entry(user).State = EntityState.Modified;

            await _ccpContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _ccpContext.Dispose();
        }

        public Task AddClaimAsync(User user, System.Security.Claims.Claim claim)
        {
            throw new NotImplementedException();
        }

        public Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimAsync(User user, System.Security.Claims.Claim claim)
        {
            throw new NotImplementedException();
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _ccpContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public Task<string> GetEmailAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailAsync(User user, string email)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            throw new NotImplementedException();
        }

        public Task AddLoginAsync(User user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(User user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task AddToRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            return new List<string>() {user.Role.RoleName};
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName)
        {
            return user.Role.RoleName == roleName;
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetPasswordHashAsync(User user)
        {
            return user.PasswordHash;
            //var u = await _ccpContext.Users.FindAsync(user.Id);
            //return u.PasswordHash;
        }

        public async Task<bool> HasPasswordAsync(User user)
        {
            return user.PasswordHash != null;
            //var u = await _ccpContext.Users.FindAsync(user.Id);
            //return u.PasswordHash != null;
        }

        public async Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            _ccpContext.Entry(user).State = EntityState.Modified;
            await _ccpContext.SaveChangesAsync();
        }

        public Task<string> GetSecurityStampAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task SetSecurityStampAsync(User user, string stamp)
        {
            throw new NotImplementedException();
        }
    }
}
