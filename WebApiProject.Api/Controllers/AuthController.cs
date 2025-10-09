using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApiProject.Api.CustomActionFilters;
using WebApiProject.Api.Models.DTOs;
using WebApiProject.Api.Reponsitories;

namespace WebApiProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ILogger<AuthController> logger;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ILogger<AuthController> logger, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.tokenRepository = tokenRepository;
        }

        #region POST: https://localhost:{port}/api/Auth/register
        [HttpPost]
        [Route("register")]
        [ValidateModel]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            // Assign the incoming registration request
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            // Create the user
            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            // Log the result
            logger.LogInformation("User {Username} created with result: {Result}", registerRequestDto.Username, identityResult.Succeeded);

            if (identityResult.Succeeded)
            {
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    // Assign roles to the user
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                    // Log the role assignment result
                    logger.LogInformation("Roles {Roles} assigned to user {Username} with result: {Result}",
                        string.Join(", ", registerRequestDto.Roles), registerRequestDto.Username, identityResult.Succeeded);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered! Please login.");
                    }
                }
            }
            return BadRequest("Something went wrong");
        }
        #endregion

        #region POST: https://localhost:{port}/api/Auth/login
        [HttpPost]
        [Route("login")]
        [ValidateModel]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            // Find the user by username
            var user = await userManager.FindByNameAsync(loginRequestDto.Username);

            // Log whether the user was found
            logger.LogInformation("User {Username} found: {Found}", loginRequestDto.Username, user != null);

            if (user != null)
            {
                // Check the password
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                // Log the password check result
                logger.LogInformation("Password check for user {Username} result: {Result}", loginRequestDto.Username, checkPasswordResult);

                if (checkPasswordResult)
                {
                    // Get user roles
                    var roles = await userManager.GetRolesAsync(user);

                    // Log the retrieved roles
                    logger.LogInformation("User {Username} has roles: {Roles}", loginRequestDto.Username, string.Join(", ", roles));

                    if (roles != null)
                    {
                        // Create JWT token
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());

                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken,
                        };

                        return Ok(response);
                    }
                }
            }
            return BadRequest("Username or password incorrect.");
        }
        #endregion
    }
}
