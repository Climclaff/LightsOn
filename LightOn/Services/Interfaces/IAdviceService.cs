using LightOn.Helpers;
using LightOn.Models;
using System.Collections.Concurrent;

namespace LightOn.Services.Interfaces
{
    public interface IAdviceService
    {
        Task<ServiceResponse<ConcurrentDictionary<string, List<string>>>> GenerateTipsAsync(int id);
    }
}
