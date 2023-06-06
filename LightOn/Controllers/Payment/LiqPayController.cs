using LightOn.Data;
using LightOn.Helpers.LIQPAY;
using LightOn.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace LightOn.Controllers.Payment
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiqPayController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        public LiqPayController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("Payment")]
        public async Task<IActionResult> Payment(string data, string signature)
        {
            var request_dictionary = Request.Form.Keys.ToDictionary(key => key, key => Request.Form[key]);


            byte[] request_data = Convert.FromBase64String(request_dictionary["data"]);
            string decodedString = Encoding.UTF8.GetString(request_data);
            var request_data_dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(decodedString);


            var mySignature = LiqPayHelper.GetLiqPaySignature(request_dictionary["data"]);


            if (mySignature != request_dictionary["signature"])
                return BadRequest("Signature mismatch. The provided signature does not match the expected value.");


            if (request_data_dictionary["status"] == "sandbox" || request_data_dictionary["status"] == "success")
            {
                var username = User.FindFirst(ClaimTypes.Name)?.Value;
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    return BadRequest();
                }
                var databaseClaims = await _userManager.GetClaimsAsync(user);
                var premiumClaim = databaseClaims.Where(u => u.Type == "IsPremium").First();
                var value = DateTime.Now.AddMonths(1);
                var newPremiumClaim = new Claim("IsPremium", value.ToString());
                await _userManager.ReplaceClaimAsync(user, premiumClaim, newPremiumClaim);          
                return Ok();
            }

            return StatusCode(500,"An error occured during payment process");


        }
    }
}
