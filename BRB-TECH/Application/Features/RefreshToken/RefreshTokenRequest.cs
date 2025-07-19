using Domain.Models;
using MediatR;

namespace Application.Features.RefreshToken
{
    public class RefreshTokenRequest : IRequest<AccessToken>
    {
        public string _refreshToken { get; set; }

        public RefreshTokenRequest(string refreshToken)
        {
            _refreshToken = refreshToken;
        }
    }
}
