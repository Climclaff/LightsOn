using LightOn.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;
using System.Text.Json;
#pragma warning disable CS8618
#pragma warning disable CS8602
#pragma warning disable CS8600
namespace LightOn.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RequestLimitAttribute : ActionFilterAttribute
    {
        public string Name { get; set; }

        public int Seconds { get; set; }
        public int MaxRequestCount { get; set; }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context,
                                        ActionExecutionDelegate next)
        {
            bool error = false;
            IDistributedCache cache = context.HttpContext.RequestServices.GetService<IDistributedCache>();
            var cacheEntryOptions = new DistributedCacheEntryOptions()
                   .SetAbsoluteExpiration(DateTime.Now.AddSeconds(Seconds));

            var ipAddress = context.HttpContext.Request.HttpContext.Connection.Id;
            var memoryCacheKey = $"{Name}-{ipAddress}";

            var distCacheVal = await cache.GetAsync(memoryCacheKey);

            CachedIP ip;
            if (distCacheVal == null)
            {
                ip = new CachedIP();
                ip.Value = ipAddress.ToString();
                ip.RequestCount = 1;
                await cache.SetAsync(memoryCacheKey, JsonSerializer.SerializeToUtf8Bytes(ip), cacheEntryOptions);
            }
            else
            {
                ip = JsonSerializer.Deserialize<CachedIP>(distCacheVal);
                if (ip.RequestCount > MaxRequestCount)
                {
                    error = true;
                    context.Result = new ContentResult
                    {
                        Content = "Too many requests."
                    };
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                }

            }
            if (!error)
            {
                ip.RequestCount++;
                await cache.SetAsync(memoryCacheKey, JsonSerializer.SerializeToUtf8Bytes(ip), cacheEntryOptions);
                await next();
            }
        }
    }
}

