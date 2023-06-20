using LightOn.BLL;
using LightOn.Data;
using LightOn.Exceptions;
using LightOn.Helpers;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;
using LightOn.Websockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Caching.Distributed;
#pragma warning disable 8602
namespace LightOn.Services.HostedServices
{
    public class TransformerLoadService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public TransformerLoadService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    try
                    {
                        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
                        await CalculateTransformerLoad(context, cache);

                        await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
                    }
                    catch (Exception)
                    {
                        throw new Exception($"Error occured during execution of main background service method");
                        
                    }
                }
            }
        }
        private async Task SendSimulationToTransformer(int id, byte[] message)
        {
            try
            {
                await WebSocketHandler.SendTransformerLoadToClient(id, message);
            }
            catch (Exception)
            {
                throw new Exception($"Failed to send simulation data to transformer with id {id}");
            }
        }
        private async Task CalculateTransformerLoad(ApplicationDbContext _context, IDistributedCache cache)
        {
            try
            {
                TransformerLoadCalculator calculator = new TransformerLoadCalculator();
                var transformers = await _context.Transformers.ToListAsync();
                foreach (var transf in transformers) {
                    List<ApplianceUsagePlanned> applianceUsageForTransformer = await _context.ApplianceUsagePlanneds
                    .Where(u => u.Appliance.User.Building.TransformerId == transf.Id)
                    .ToListAsync();
                    List<int?> applianceIds = applianceUsageForTransformer.Select(u => u.ApplianceId).ToList();
                    List<Appliance> appliancesUsed = await _context.Appliances.Where(a => applianceIds.Contains(a.Id)).ToListAsync();
                    var dictionary = calculator.GenerateSegments(transf, applianceUsageForTransformer, appliancesUsed);


                    var cacheEntryOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(900));
                    string jsonDictionary = JsonSerializer.Serialize(dictionary);
                    byte[] byteArray = Encoding.UTF8.GetBytes(jsonDictionary);
                    await cache.SetAsync(transf.Id.ToString() + "Planning", byteArray, cacheEntryOptions);
                    

                    KeyValuePair<DateTime, float> earliestEntry = dictionary.OrderBy(entry => entry.Key).FirstOrDefault();
                    if (earliestEntry.Key != default)
                    {
                        Dictionary<int, float> transformerLoad = new Dictionary<int, float>();
                        transformerLoad.Add(transf.Id, earliestEntry.Value);
                        string json = JsonSerializer.Serialize(transformerLoad);
                        var encodedMessage = Encoding.UTF8.GetBytes(json);
                        await SendSimulationToTransformer(transf.Id, encodedMessage);

                    }
                }
            }
            catch (Exception)
            {
                throw new Exception($"Error during simulation of the data for transformers");
            }
        }

    }




}
