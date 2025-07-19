using Application.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Register
{
    public class RegisterUserRequest : IRequest<IdentityResult>
    {
        public RegisterUserDTO _registerUser { get; set; }

        public RegisterUserRequest(RegisterUserDTO registerUser)
        {
            _registerUser = registerUser;
        }
    }
}
