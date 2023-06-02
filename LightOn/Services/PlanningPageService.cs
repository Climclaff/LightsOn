using LightOn.Exceptions;
using LightOn.Helpers;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;

namespace LightOn.Services
{
    public class PlanningPageService : IPlanningPageService
    {
        private readonly IPlanningPageRepository _repository;
        private readonly ILoggingService _logger;
        public PlanningPageService(IPlanningPageRepository repository, ILoggingService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ServiceResponse<int?>> GetTransformerByUserAsync(int id)
        {
            try
            {
                var result = await _repository.GetTransformerByUserAsync(id);
                if (result == null)
                {
                    return new ServiceResponse<int?> { Success = false, ErrorMessage = $"Transformer of user with ID {id} was not found", NotFound = true };
                }
                return new ServiceResponse<int?> { Success = true, Data = result };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<int?> { Success = false, ErrorMessage = ex.Message, NotFound = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<int?> { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}
