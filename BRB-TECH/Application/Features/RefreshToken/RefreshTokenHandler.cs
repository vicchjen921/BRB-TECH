using Application.Common;
using Application.Features.Login;
using Domain.Entities;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System.Collections.Generic;
using System.Security.Claims;
using System.Transactions;

namespace Application.Features.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, AccessToken>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly ILogger<LoginHandler> _logger;
        private readonly IDistributedCache _cache;

        public RefreshTokenHandler(UserManager<ApplicationUser> userManager, IConfiguration config, ILogger<LoginHandler> logger, IDistributedCache cache)
        {
            _userManager = userManager;
            _config = config;
            _logger = logger;
            _cache = cache;
        }

        public async Task<AccessToken> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var username = await _cache.GetStringAsync($"refreshTokens:{request._refreshToken}");

                if (string.IsNullOrEmpty(username))
                {
                    return null;
                }

                var user = await _userManager.FindByNameAsync(username);
                if (user == null) return null;

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
            catch (Exception e)
            {
                _logger.LogError(e, "Error while login");
                throw;
            }
        }
    }
}
