using Buffett.Endpoint;
using Buffett.Endpoint.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<BuffettSettings>(builder.Configuration);

var app = builder.Build();
app.UseServiceStack(new BuffettHost());
await app.RunAsync();