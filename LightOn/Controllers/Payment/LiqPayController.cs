using LightOn.Data;
using LightOn.Helpers.LIQPAY;
using LightOn.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
#pragma warning disable CS8602
namespace LightOn.Controllers.Payment
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiqPayController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        public LiqPayController(UserManager<User> userManager, ApplicationDbContext context)
        {

            _userManager = userManager;
            _context = context;
        }


        [HttpPost]
        [Route("Payment")]
        public async Task<IActionResult> Payment()
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
                string liqPhone = "+" + request_data_dictionary["sender_phone"];
                var user = await _context.Users.Where(x => x.PhoneNumber == liqPhone).FirstOrDefaultAsync();
                if (user == null)
                {
                    return BadRequest();
                }
                var databaseClaims = await _userManager.GetClaimsAsync(user);
                var premiumClaim = databaseClaims.Where(u => u.Type == "IsPremium").First();
                if (premiumClaim.Value == "false")
                {
                    var value = DateTime.Now.AddMonths(1);
                    var newPremiumClaim = new Claim("IsPremium", value.ToString());
                    await _userManager.ReplaceClaimAsync(user, premiumClaim, newPremiumClaim);
                    return Ok(new
                    {
                        isPremium = newPremiumClaim.Value
                    });
                }
                else
                {
                    var value = Convert.ToDateTime(premiumClaim.Value).AddMonths(1);
                    var newPremiumClaim = new Claim("IsPremium", value.ToString());
                    await _userManager.ReplaceClaimAsync(user, premiumClaim, newPremiumClaim);
                    return Ok(new
                    {
                        isPremium = newPremiumClaim.Value
                    });
                }
            }

            return StatusCode(500,"An error occured during payment process");
        }
    }
}
