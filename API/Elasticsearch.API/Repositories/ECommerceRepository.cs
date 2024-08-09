using System.Collections.Immutable;
using Elastic.Clients.Elasticsearch;
using Elasticsearch.API.Models.ECommerce;

namespace Elasticsearch.API.Repositories;

public class ECommerceRepository
{
    private readonly ElasticsearchClient _client;
    private const string indexName = "kibana_sample_data_ecommerce";


    public ECommerceRepository(ElasticsearchClient client)
    {
        _client = client;
    }

    public async Task<ImmutableList<ECommerce>> TermQuery(string customerFirstName)
    {
        var response = await _client.SearchAsync<ECommerce>(x => x.Index(indexName).Query(q => q.Term(t => t.Field("customer_first_name.keyword").Value(customerFirstName))));
        response.Hits.ToList().ForEach(hit => hit.Source.Id = hit.Id);
        return response.Documents.ToImmutableList();
    }
}
