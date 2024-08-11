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

    public async Task<List<Blog>> SearchAsync(string searchText)
    {
        //title
        //content

        // should ((term1 and term2) or term3)

        // title => full text
        // content => full text

        var result = await _elasticsearchClient.SearchAsync<Blog>(s => s
            .Index(indexName)
                .Size(1000)
                    .Query(q => q
                        .Bool(b => b
                            .Should(m => m
                                .Match(s => s
                                    .Field(f => f.Content)
                                    .Query(searchText)),
                                s => s.MatchBoolPrefix(p => p
                                    .Field(p => p
                                        .Title)
                                        .Query(searchText))
                                    ))));

        result.Hits.ToList().ForEach(x => x.Source.Id = x.Id);
        return result.Documents.ToList();
    }
}
