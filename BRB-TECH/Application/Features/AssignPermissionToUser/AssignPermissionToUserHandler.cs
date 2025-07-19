using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Application.Features.AssignPermissionToUser
{
    public class AssignPermissionToUserHandler : IRequestHandler<AssignPermissionToUserRequest, IdentityResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public AssignPermissionToUserHandler(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<IdentityResult> Handle(AssignPermissionToUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request._assignPermission.Username);
            if (user == null) return null;

            var claim = new Claim("Permission", request._assignPermission.Permission);
            var result = await _userManager.AddClaimAsync(user, claim);

            return result;
        }
    }
}
