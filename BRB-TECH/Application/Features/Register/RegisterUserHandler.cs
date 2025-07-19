using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Features.Register
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, IdentityResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterUserHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser { UserName = request._registerUser.Username };
            var result = await _userManager.CreateAsync(user, request._registerUser.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                var claims = new List<Claim>
                {
                    new Claim("Permission", "CanCreateCategory"),
                    new Claim("Permission", "CanUpdateCategory"),
                    new Claim("Permission", "CanReadCategories"),
                    new Claim("Permission", "CanRemoveCategory"),

                    new Claim("Permission", "CanCreateTransaction"),
                    new Claim("Permission", "CanUpdateTransaction"),
                    new Claim("Permission", "CanReadTransactions"),
                    new Claim("Permission", "CanRemoveTransaction"),
                    new Claim("Permission", "CanGetMonthlyBalance"),
                    new Claim("Permission", "CanCreateDocTransfer")
                };

                result = await _userManager.AddClaimsAsync(user, claims);
            }

            return result;
        }
    }
}
