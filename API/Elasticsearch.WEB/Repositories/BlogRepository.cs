using Elastic.Clients.Elasticsearch;
using Elasticsearch.WEB.Models;

namespace Elasticsearch.WEB.Repositories;

public class BlogRepository(ElasticsearchClient client)
{
    private readonly ElasticsearchClient _elasticsearchClient = client;
    private const string indexName = "blog";

    public async Task<Blog?> SaveAsync(Blog newBlog)
    {
        newBlog.Created = DateTime.Now;

        var response = await _elasticsearchClient.IndexAsync(newBlog, x => x.Index(indexName));

        if (!response.IsValidResponse) return null;

        newBlog.Id = response.Id;

        return newBlog;
    }
}
