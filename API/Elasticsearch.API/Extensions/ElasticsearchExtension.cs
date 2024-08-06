using Elasticsearch.Net;
using Nest;

namespace Elasticsearch.API.Extensions;

public static class ElasticsearchExtension
{
    public static void AddElastic(this IServiceCollection services, IConfiguration configuration)
    {
        var pool = new SingleNodeConnectionPool(new Uri(configuration.GetSection("Elastic")["Url"]!));
        var settings = new ConnectionSettings(pool);
        // You could add username and password right here
        // settings.BasicAuthentication(username,password);
        var client = new ElasticClient(settings);
        services.AddSingleton(client);
    }
}
