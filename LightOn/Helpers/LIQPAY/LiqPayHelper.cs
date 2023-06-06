using System.Security.Cryptography;
using System.Text;

namespace LightOn.Helpers.LIQPAY
{
    public class LiqPayHelper
    {
        static private readonly string _private_key;
        static private readonly string _public_key;

        static LiqPayHelper()
        {
            _public_key = "sandbox_i34434641483";
            _private_key = "sandbox_03mT7ZD8LEuetmD4234EAqmvENokys5jUS1OWYx8";
        }

        static public string GetLiqPaySignature(string data)
        {
            return Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(_private_key + data + _private_key)));
        }

    }
}
