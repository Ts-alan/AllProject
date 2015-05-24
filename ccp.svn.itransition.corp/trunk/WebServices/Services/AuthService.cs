using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CCP.DAL.DataModels;

namespace CCP.WebApi.Services
{
    public partial class UserService
    {
        private CCPEntities context = new CCPEntities();

        public Client FindClient(string clientId)
        {
            var client = context.Clients.Find(clientId);
            return client;
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

            var existingToken = context.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            context.RefreshTokens.Add(token);

            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await context.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                context.RefreshTokens.Remove(refreshToken);
                return await context.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            context.RefreshTokens.Remove(refreshToken);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await context.RefreshTokens.FindAsync(refreshTokenId);
            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return context.RefreshTokens.ToList();
        }
    }
}