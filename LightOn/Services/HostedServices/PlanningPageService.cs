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
#pragma warning disable 8602
namespace LightOn.Services.HostedServices
{
    public class PlanningPageService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public PlanningPageService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    await CalculateTransformerLoad(1, context);

                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
            }
        }
        private async Task SendSimulationToTransformer(int id, byte[] message)
        {
            try
            {
                await WebSocketHandler.SendTransformerLoadToClient(1, message);
            }
            catch (Exception)
            {
                throw new Exception($"Failed to send simulation data to transformer with id {id}");
            }
        }
        public async Task CalculateTransformerLoad(int id, ApplicationDbContext _context)
        {
            try
            {
                TransformerLoadCalculator calculator = new TransformerLoadCalculator();

                var transf = await _context.Transformers.FindAsync(1);
                List<ApplianceUsagePlanned> applianceUsageForTransformer = await _context.ApplianceUsagePlanneds
                .Where(u => u.Appliance.User.Building.TransformerId == transf.Id)
                .ToListAsync();
                List<int?> applianceIds = applianceUsageForTransformer.Select(u => u.ApplianceId).ToList();
                List<Appliance> appliancesUsed = await _context.Appliances.Where(a => applianceIds.Contains(a.Id)).ToListAsync();

                var dictionary = calculator.GenerateSegments(transf, applianceUsageForTransformer, appliancesUsed);
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
            catch (Exception)
            {
                throw new Exception($"Error during simulation of the data for transformer with id {id}");
            }
        }

    }




}
