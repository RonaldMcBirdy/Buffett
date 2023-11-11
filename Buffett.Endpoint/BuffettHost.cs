using Buffett.Endpoint.Cache;
using Buffett.Endpoint.Clients;
using Buffett.Endpoint.Returns;
using Funq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Validation;

namespace Buffett.Endpoint
{
    public class BuffettHost : AppHostBase
    {
        public static string connectionString { get; private set; }
        public BuffettHost() : base("Buffett", typeof(StatusQueryHandler).Assembly)
        {
        }

        public override void Configure(Container container)
        {
            // Load configuration settings from appsettings.json file
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            // Set up database connection
            // connectionString = config.GetConnectionString("MySqlConnection");
            // IDbConnectionFactory dbFactory = new OrmLiteConnectionFactory(connectionString, MySqlDialect.Provider);

            // register services
            container.AddSingleton<IMemoryCache>(new MemoryCache(new MemoryCacheOptions()));
            container.AddSingleton<IReturnsManager, ReturnsManager>();
            container.AddSingleton<IAlphaVantageClient, AlphaVantageClient>();
            container.AddSingleton<ITickerCache, TickerCache>();
        }
    }
}
