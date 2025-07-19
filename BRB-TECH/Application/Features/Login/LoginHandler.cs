using Application.Common;
using Domain.Entities;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Application.Features.Login
{
    public class LoginHandler : IRequestHandler<LoginRequest, AccessToken>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly ILogger<LoginHandler> _logger;
        private readonly IDistributedCache _cache;

        public LoginHandler(UserManager<ApplicationUser> userManager, IConfiguration config, ILogger<LoginHandler> logger, IDistributedCache cache)
        {
            _userManager = userManager;
            _config = config;
            _logger = logger;
            _cache = cache;
        }

        public async Task<AccessToken> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request._login.Username);
                if (user == null) return null;

                if (await _userManager.CheckPasswordAsync(user, request._login.Password))
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                    };
                    claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

                    var dbClaims = await _userManager.GetClaimsAsync(user);
                    claims.AddRange(dbClaims);

                    var token = new AccessToken()
                    {
                        Token = TokenHelper.GenerateAccessToken(claims, _config["Jwt:Key"]),
                        RefreshToken = TokenHelper.GenerateRefreshToken(),
                        ExpiresIn = 3600000,
                        RefreshTokenExpiresIn = 604800000
                    };

                    var cacheOptions = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7) };
                    await _cache.SetStringAsync($"refreshTokens:{token.RefreshToken}", user.UserName, cacheOptions);

                    return token;
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error while login");
                throw;
            }

            return null;
        }
    }
}
