using Amazon.Lambda.AspNetCoreServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Todo.Web
{
    public class LambdaEntryPoint : APIGatewayProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            builder.UseStartup<Startup>();
        }

        protected override void PostCreateHost(IHost webHost)
        {
            base.PostCreateHost(webHost);

            _ = webHost.SeedDatabaseAsync();
        }
    }
}
