using Application.DTO;
using Domain.Models;
using MediatR;

namespace Application.Features.Login
{
    public class LoginRequest : IRequest<AccessToken>
    {
        public LoginDTO _login { get; set; }

        public LoginRequest(LoginDTO login)
        {
            _login = login;
        }
    }
}
