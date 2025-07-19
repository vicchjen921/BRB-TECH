using Application.Features.AssignPermissionToUser;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Application.Features.RemovePermissionFromUser
{
    public class RemovePermissionHandler : IRequestHandler<RemovePermissionRequest, IdentityResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public RemovePermissionHandler(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<IdentityResult> Handle(RemovePermissionRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request._removePermission.Username);
            if (user == null) return null;

            var claim = new Claim("Permission", request._removePermission.Permission);
            var result = await _userManager.RemoveClaimAsync(user, claim);

            return result;
        }
    }
}
