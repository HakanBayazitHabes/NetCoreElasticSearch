using System.Collections.Immutable;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
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
        /*
        1. Yol
        var response = await _client.SearchAsync<ECommerce>(x => x.Index(indexName).Query(q => q.Term(t => t.Field("customer_first_name.keyword").Value(customerFirstName))));
        
        2. Yol
        var response = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.Term(t => t.CustomerFirstName.Suffix("keyword"), customerFirstName)));
        */

        var termQuery = new TermQuery("customer_first_name.keyword") { Value = customerFirstName, CaseInsensitive = true };

        var response = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(termQuery));

        response.Hits.ToList().ForEach(hit => hit.Source.Id = hit.Id);
        return response.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> TermsQuery(List<string> customerFirstNameList)
    {
        List<FieldValue> terms = [];
        customerFirstNameList.ForEach(x =>
        {
            terms.Add(x);
        });

        /*
        1. Yol
        var termsQuery = new TermsQuery()
        {
            Field = "customer_first_name.keyword",
            Terms = new TermsQueryField(terms.AsReadOnly())
        };

        var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(termsQuery));
        */

        // 2. Yol
        var result = await _client.SearchAsync<ECommerce>(s => s
        .Index(indexName)
        .Size(100)
        .Query(q => q
        .Terms(t => t
        .Field(f => f.CustomerFirstName
        .Suffix("keyword"))
        .Terms(new TermsQueryField(terms.AsReadOnly())))));

        result.Hits.ToList().ForEach(x => x.Source.Id = x.Id);

        return result.Documents.ToImmutableList();
    }
}
