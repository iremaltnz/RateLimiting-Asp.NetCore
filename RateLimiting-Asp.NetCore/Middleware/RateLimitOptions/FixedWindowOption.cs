namespace RateLimiting_Asp.NetCore.Middleware.RateLimitOptions
{
    public class FixedWindowOption
    {
        public const string MyRateLimit = "MyRateLimit";
        public int PermitLimit { get; set; } = 2;
        public int Window { get; set; } = 15;
        public int QueueLimit { get; set; } = 3;
        public bool AutoReplenishment { get; set; } = false;
    }
}
