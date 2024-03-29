﻿using LightOn.Data;
using LightOn.Exceptions;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
#pragma warning disable 8602
namespace LightOn.Repositories
{
    public class AdviceRepository : IAdviceRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggingService _logger;
        public AdviceRepository(ApplicationDbContext context, ILoggingService logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<List<Building>> GetBuildingsByUsersAsync(List<User> users)
        {

            try
            {
                List<Building> buildings= new List<Building>();
                foreach (var user in users)
                {
                    var building = await _context.Buildings.Where(u => u.Id == user.BuildingId).FirstOrDefaultAsync();
                    buildings.Add(building);
                }
                return buildings;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting buildings by users connected to transformer.", ex);
                throw new RepositoryException("Error getting buildings by users connected to transformer", ex);
            }
        }
        public async Task<int?> GetTransformerByUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new NotFoundException($"User with id {id} not found.");
            }
            try
            {
                var building = await _context.Buildings.Where(u => u.Id == user.BuildingId).FirstOrDefaultAsync();
                return building.TransformerId;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting transformer from database.", ex);
                throw new RepositoryException("Error getting transformer", ex);
            }
        }
        public async Task<List<User>> GetUsersByTransformerAsync(int id)
        {
            var transformer = await _context.Transformers.FindAsync(id);
            if (transformer == null)
            {
                throw new NotFoundException($"Transformer with id {id} not found.");
            }
            try
            {
                /*
                var usersToRemove = new List<User>();
                var users = await _context.Users.Where(u => u.Building.TransformerId == id).ToListAsync();
                foreach (var user in users)
                {
                    var result = await _context.ApplianceUsageHistories
                  .Where(usage => usage.Appliance.UserId == user.Id)
                  .ToListAsync();
                    if (result.Count < 3)
                    {
                        usersToRemove.Add(user);
                    }
                }
                foreach(var userToRemove in usersToRemove)
                {
                    users.Remove(userToRemove);
                }
                return users;
                */
                var userIds = await _context.Users
                    .Where(u => u.Building.TransformerId == id)
                    .Select(u => (int?)u.Id)  
                    .ToListAsync();

                var usageCounts = await _context.ApplianceUsageHistories
                    .Where(usage => userIds.Contains(usage.Appliance.UserId))
                    .GroupBy(usage => usage.Appliance.UserId)
                    .Select(g => new
                    {
                        ApplianceId = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                var filteredUserIds = usageCounts
                    .Where(uc => uc.Count >= 3)
                    .Select(uc => uc.ApplianceId)
                    .ToList();

                var users = await _context.Users
                    .Where(user => filteredUserIds.Contains(user.Id))
                    .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting users from database.", ex);
                throw new RepositoryException("Error getting users", ex);
            }

        }
    }
}
