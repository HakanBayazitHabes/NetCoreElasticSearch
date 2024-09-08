using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Elasticsearch.WEB.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Elasticsearch.WEB.Extensions;

public static class ElasticsearchExtension
{
    public static void AddElastic(this IServiceCollection services, IConfiguration configuration)
    {
        var activeConnection = configuration["ActiveConnection"];
        Console.WriteLine(activeConnection);

        if (true)
        {
            var username = configuration.GetSection("elastic")["Username"];
            var password = configuration.GetSection("elastic")["Password"];
            var settings = new ElasticsearchClientSettings(new Uri(configuration.GetSection("elastic")["Url"]!))
                .Authentication(new BasicAuthentication(username!, password!));
            var client = new ElasticsearchClient(settings);
            services.AddSingleton(client);
        }
        if (true)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            services.AddDbContext<Elasticsearch.WEB.Repositories.AppContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}
