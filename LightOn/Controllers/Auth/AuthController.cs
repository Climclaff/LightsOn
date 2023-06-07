using LightOn.Models;
using LightOn.Models.Auth;
using LightOn.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace LightOn.Controllers.Auth
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IProfileService _profileService;
        public AuthController(UserManager<User> userManager, IProfileService profileService, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            _profileService = profileService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Email);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                { Status = "Error", Message = "User already exists" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(
                new ValidationProblemDetails(ModelState));
            }
            User user = new User()
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                RegionId = model.RegionId,
                DistrictId = model.DistrictId,
                TownId = model.TownId,
                StreetId = model.StreetId,
                BuildingId = model.BuildingId,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new
                { Status = "Something went wrong", Message = "Make sure the password contains numbers, upper and lower case letters, special symbols" });
            }
            await _userManager.AddClaimAsync(user, new Claim("IsAdmin", "false"));
            await _userManager.AddClaimAsync(user, new Claim("IsPremium", "false"));
            return Ok(new { Status = "Success", Message = "User created" });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password) != false)
            {
                var databaseClaims = await _userManager.GetClaimsAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    
                };
                for(int i= 0; i<databaseClaims.Count; i++)
                {
                    authClaims.Add(databaseClaims[i]);
                }
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
                if(authClaims.Count > 0)
                {
                    var adminClaim = authClaims.Where(u => u.Type == "IsAdmin").First();
                    var premiumClaim = authClaims.Where(u => u.Type == "IsPremium").First();
                    DateTime dateTime;
                    if (DateTime.TryParse(premiumClaim.Value, out dateTime))
                    {
                        
                        if (dateTime > DateTime.Now)
                        {
                            var newPremiumClaim = new Claim("IsPremium", "false");
                            await _userManager.ReplaceClaimAsync(user, premiumClaim, newPremiumClaim);
                            return Ok(new
                            {
                                token = new JwtSecurityTokenHandler().WriteToken(token),
                                isAdmin = adminClaim.Value,
                                isPremium = newPremiumClaim.Value,
                                userId = user.Id
                            });
                        }                      
                    }
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        isAdmin = adminClaim.Value,
                        isPremium = premiumClaim.Value,
                        userId = user.Id
                    });
                    }
                
            }
            return Unauthorized();
        }
        

        [AllowAnonymous]
        [HttpGet]
        [Route("GetRegions")]
        public async Task<IActionResult> GetRegions()
        {
            var result = await _profileService.GetRegionsAsync();

            if (result.NotFound)
            {
                return NotFound();
            }
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(500, result.ErrorMessage);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetDistrictsOfRegion")]
        public async Task<IActionResult> GetDistrictsOfRegion([FromQuery] int id)
        {
            var result = await _profileService.GetDistrictsOfRegionAsync(id);

            if (result.NotFound)
            {
                return NotFound();
            }
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(500, result.ErrorMessage);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetTownsOfDistrict")]
        public async Task<IActionResult> GetTownsOfDistrict([FromQuery] int id)
        {
            var result = await _profileService.GetTownsOfDistrictAsync(id);

            if (result.NotFound)
            {
                return NotFound();
            }
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(500, result.ErrorMessage);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetStreetsOfTown")]
        public async Task<IActionResult> GetStreetsOfTown([FromQuery] int id)
        {
            var result = await _profileService.GetStreetsOfTownAsync(id);

            if (result.NotFound)
            {
                return NotFound();
            }
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(500, result.ErrorMessage);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetBuildingsOfStreet")]
        public async Task<IActionResult> GetBuildingsOfStreet([FromQuery] int id)
        {
            var result = await _profileService.GetBuildingsOfStreetAsync(id);

            if (result.NotFound)
            {
                return NotFound();
            }
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(500, result.ErrorMessage);
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("ExternalLoginCallBack")]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var info = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
            var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var databaseClaims = await _userManager.GetClaimsAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                };
                for (int i = 0; i < databaseClaims.Count; i++)
                {
                    authClaims.Add(databaseClaims[i]);
                }
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
                if (authClaims.Count > 0)
                {
                    if (authClaims.First().Value == "true")
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            isAdmin = true
                        });
                }

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    isAdmin = false
                });
            }

            // Create a new user with the necessary information
            var newUser = new User
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };

            return Ok(newUser);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("GoogleLogin")]
        public IActionResult GoogleLogin()
        {

            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("ExternalLoginCallback", "Auth")
            };


            return Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("CompleteGoogleRegister")]
        public async Task<IActionResult> CompleteGoogleRegister([FromBody] GoogleRegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Email);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                { Status = "Error", Message = "User already exists" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(
                new ValidationProblemDetails(ModelState));
            }
            User user = new User()
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                RegionId = model.RegionId,
                DistrictId = model.DistrictId,
                TownId = model.TownId,
                StreetId = model.StreetId,
                BuildingId = model.BuildingId,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new
                { Status = "Something went wrong", Message = "Make sure the password contains numbers, upper and lower case letters, special symbols" });
            }
            await _userManager.AddClaimAsync(user, new Claim("IsAdmin", "false"));
            return Ok(new { Status = "Success", Message = "User created" });
        }

    }
}