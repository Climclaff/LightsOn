namespace LightOn.Services
{
    public interface ILoggingService
    {
        public void LogError(string message, Exception ex);
    }
}
