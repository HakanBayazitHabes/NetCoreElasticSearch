using System.Collections.Immutable;
using Elasticsearch.API.Models;
using Nest;

namespace Elasticsearch.API.Repositories;

public class ProductRepository
{
    private readonly ElasticClient _client;
    private const string indexName = "products";
    public ProductRepository(ElasticClient client)
    {
        _client = client;
    }

    public async Task<Product?> SaveAsync(Product newProduct)
    {
        newProduct.Created = DateTime.Now;

        var response = await _client.IndexAsync(newProduct, x => x.Index(indexName).Id(Guid.NewGuid().ToString()));

        if (!response.IsValid) return null;

        newProduct.Id = response.Id;

        return newProduct;
    }

    public async Task<ImmutableList<Product>> GetAllAsync()
    {
        var response = await _client.SearchAsync<Product>(x => x.Index(indexName).Query(q => q.MatchAll()));

        response.Hits.ToList().ForEach(hit => hit.Source.Id = hit.Id);

        return response.Documents.ToImmutableList();
    }

    public async Task<Product?> GetByIdAsync(string id)
    {
        var response = await _client.GetAsync<Product>(id, x => x.Index(indexName));

        if (!response.IsValid) return null;

        response.Source.Id = response.Id;

        return response.Source;
    }

}
