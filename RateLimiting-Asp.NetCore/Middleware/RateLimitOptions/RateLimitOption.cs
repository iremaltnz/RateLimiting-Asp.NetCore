namespace RateLimiting_Asp.NetCore.Middleware.RateLimitOptions
{
    public class RateLimitOption
    {
        public const string MyRateLimit = "MyRateLimit";
        public int PermitLimit { get; set; } = 5;
        public int Window { get; set; } = 30;
        public int QueueLimit { get; set; } = 3;
        public bool AutoReplenishment { get; set; } = false;

        #region Sliding Window
        public int SegmentsPerWindow { get; set; } = 3;
        #endregion

        #region TokenBucket
        public int TokenLimit { get; set; } = 5;
        public int ReplenishmentPeriod { get; set; } = 10;
        public int TokensPerPeriod { get; set; } = 2;
        #endregion
    }
}
