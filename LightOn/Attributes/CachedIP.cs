namespace LightOn.Attributes
#pragma warning disable CS8618
{
    [Serializable]
    public class CachedIP
    {
        public string Value { get; set; }

        public int RequestCount { get; set; }

    }
}
