using Application.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.AssignPermissionToUser
{
    public class AssignPermissionToUserRequest : IRequest<IdentityResult>
    {
        public AssignPermissionRequestDTO _assignPermission { get; set; }

        public AssignPermissionToUserRequest(AssignPermissionRequestDTO assignPermission)
        {
            _assignPermission = assignPermission;
        }
    }
}
