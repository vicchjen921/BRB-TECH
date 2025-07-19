using Application.DTO;
using Application.Features.AssignPermissionToUser;
using Application.Features.RefreshToken;
using Application.Features.Register;
using Application.Features.RemovePermissionFromUser;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = Application.Features.Login.LoginRequest;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly IMediator _mediator;

        public AuthController(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              IConfiguration config,
                              IMediator mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO registerUserRequest)
        {
            var result = await _mediator.Send(new RegisterUserRequest(registerUserRequest));
            
            if(result.Succeeded)
                return Ok("User registered");

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginRequest)
        {
            var result = await _mediator.Send(new LoginRequest(loginRequest));

            if(result != null)
                return Ok(result);

            return Unauthorized();
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDTO refreshTokenRequest)
        {
            var result = await _mediator.Send(new RefreshTokenRequest(refreshTokenRequest.RefreshToken));

            if (result != null)
                return Ok(result);

            return Unauthorized();
        }

        [HttpPost("assign-permission")]
        public async Task<IActionResult> AssignPermissionToUser([FromBody] AssignPermissionRequestDTO request)
        {
            var result = await _mediator.Send(new AssignPermissionToUserRequest(request));
            if (result == null) return NotFound("User not found");

            return result.Succeeded ? Ok("Permission added") : BadRequest(result.Errors);
        }

        [HttpPost("remove-permission")]
        public async Task<IActionResult> RemoveUserPermission([FromBody] RemovePermissionRequestDTO request)
        {
            var result = await _mediator.Send(new RemovePermissionRequest(request));
            if (result == null) return NotFound("User not found");

            return result.Succeeded ? Ok("Permission removed") : BadRequest(result.Errors);
        }
    }
}
