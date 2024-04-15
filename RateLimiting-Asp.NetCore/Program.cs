using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using NuGet.Common;
using RateLimiting_Asp.NetCore.Middleware.RateLimitOptions;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var myOptions = new RateLimitOption();
builder.Configuration.GetSection(RateLimitOption.MyRateLimit).Bind(myOptions);

var fixedPolicy = "fixed";
var slidingPolicy = "sliding";
var tokenPolicy = "token";


builder.Services.AddRateLimiter(_ =>
{
    _.AddFixedWindowLimiter(policyName: fixedPolicy, options =>
    {
        options.PermitLimit = myOptions.PermitLimit;
        options.Window = TimeSpan.FromSeconds(myOptions.Window);
        //options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        //options.QueueLimit = myOptions.QueueLimit;
    });

    _.OnRejected = async (context, token) =>
    {      
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsync($"Too many requests. Please try later again...", cancellationToken: token);

    };
});

builder.Services.AddRateLimiter(_ =>
{
    _.AddSlidingWindowLimiter(slidingPolicy, options =>
    {
        options.PermitLimit = myOptions.PermitLimit;
        options.Window = TimeSpan.FromSeconds(myOptions.Window);
        options.SegmentsPerWindow = myOptions.SegmentsPerWindow;
    });

    _.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsync($"Too many requests. Please try later again...", cancellationToken: token);

    };
}) ;

builder.Services.AddRateLimiter(_ =>
{ 
   _.AddTokenBucketLimiter(policyName: tokenPolicy, options =>
    {
        options.TokenLimit = myOptions.TokenLimit;
        options.ReplenishmentPeriod = TimeSpan.FromSeconds(myOptions.ReplenishmentPeriod);
        options.TokensPerPeriod = myOptions.TokensPerPeriod;
    });

    _.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsync($"Too many requests. Please try later again...", cancellationToken: token);

    };

}) ;


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRateLimiter();

app.MapGet("/FixedWindow", () => Results.Ok($"Fixed Window Limit"))
                           .RequireRateLimiting(fixedPolicy);

app.MapGet("/SlidingWindow", () => Results.Ok($"Sliding Window Limit"))
                           .RequireRateLimiting(slidingPolicy);

app.MapGet("/TokenBucket", () => Results.Ok($"Token Bucket Limit"))
                           .RequireRateLimiting(tokenPolicy);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
