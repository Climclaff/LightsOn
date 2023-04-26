namespace LightOn.Helpers
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public bool NotFound { get; set; }
        public string ErrorMessage { get; set; } = null;



    }
}
