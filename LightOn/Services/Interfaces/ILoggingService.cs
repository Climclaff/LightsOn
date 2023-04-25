namespace LightOn.Services.Interfaces
{
    public interface ILoggingService
    {
        public void LogError(string message, Exception ex);
    }
}
