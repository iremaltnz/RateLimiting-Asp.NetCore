namespace RateLimiting_Asp.NetCore.Middleware.RateLimitOptions
{
    public class FixedWindowOption
    {
        public const string MyRateLimit = "MyRateLimit";
        public int PermitLimit { get; set; } = 5;
        public int Window { get; set; } = 30;
        public int QueueLimit { get; set; } = 3;
        public bool AutoReplenishment { get; set; } = false;

        #region Sliding Window
        public int SegmentsPerWindow { get; set; } = 3;
        #endregion
    }
}
