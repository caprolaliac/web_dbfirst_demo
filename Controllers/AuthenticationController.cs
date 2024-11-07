using web_db.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth_demo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(UserManager<ApplicationUser> user, RoleManager<IdentityRole> role,IConfiguration config,ILogger<AuthenticationController> logger)
        {
            userManager = user;
            roleManager = role;
            configuration = config;
            _logger = logger;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var userExist = await userManager.FindByNameAsync(model.Username);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error",
                    Message = "User already Exist!"
                });
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username

            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response
                    {
                        Status = "Error",
                        Message = " User Creation Failed! Please check the user details and try again"
                    });
            }
            if (model.Role.ToLower() == "user")
            {
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                }
                if (await roleManager.RoleExistsAsync(UserRoles.User))
                {
                    await userManager.AddToRoleAsync(user, UserRoles.User);
                }
            }
            if (model.Role.ToLower() == "admin")
            {
                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                }
                if (await roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await userManager.AddToRoleAsync(user, UserRoles.Admin);
                }
            }
            return Ok(new Response { Status = "Success", Message = "User Created Successfully" });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel login)
        {
            var userExist = await userManager.FindByNameAsync(login.Username);
            if (userExist != null && await userManager.CheckPasswordAsync(userExist, login.Password))
            {
                var userRoles = await userManager.GetRolesAsync(userExist);
                _logger.LogInformation("User roles: {Roles}", string.Join(", ", userRoles));
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userExist.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
                _logger.LogInformation("User roles: {Roles}", string.Join(", ",
                authClaims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value)));

                var token = new JwtSecurityToken(
                        issuer: configuration["JWT:Issuer"],
                        audience: configuration["JWT:Audience"],
                        expires: DateTime.Now.AddMinutes(20),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
                        );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return StatusCode(StatusCodes.Status401Unauthorized,
                new Response
                {
                    Status = "Unauthorized User",
                    Message = "Enter Valid Username or Password"
                });
        }
    }
}