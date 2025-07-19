using Application.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.RemovePermissionFromUser
{
    public class RemovePermissionRequest : IRequest<IdentityResult>
    {
        public RemovePermissionRequestDTO _removePermission { get; set; }

        public RemovePermissionRequest(RemovePermissionRequestDTO removePermission)
        {
            _removePermission = removePermission;
        }
    }
}
