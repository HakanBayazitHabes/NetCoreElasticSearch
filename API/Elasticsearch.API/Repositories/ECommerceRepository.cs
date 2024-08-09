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

        var response = await _client.SearchAsync<ECommerce>(s => s
        .Index(indexName)
            .Query(termQuery));

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

    public async Task<ImmutableList<ECommerce>> PrefixQueryAsync(string customerFullName)
    {
        var response = await _client.SearchAsync<ECommerce>(s => s
        .Index(indexName)
            .Query(q => q
                .Prefix(t => t
                    .Field(f => f.CustomerFullName
                        .Suffix("keyword"))
                            .Value(customerFullName))));

        return response.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> RangeQueryAsync(double fromPrice, double toPrice)
    {
        var response = await _client.SearchAsync<ECommerce>(s => s
        .Index(indexName)
            .Query(q => q
                .Range(t => t
                    .NumberRange(nr => nr
                        .Field(f => f.TaxFulTotalPrice)
                            .Gte(fromPrice)
                                .Lte(toPrice)))));

        return response.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> MatchAllQueryAsync()
    {
        var result = await _client.SearchAsync<ECommerce>(s => s
        .Index(indexName)
            .Size(100)
                .Query(q => q
                    .MatchAll()));

        result.Hits.ToList().ForEach(x => x.Source.Id = x.Id);

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> PaginationQueryAsync(int page, int pageSize)
    {
        var pageFrom = (page - 1) * pageSize;
        var result = await _client.SearchAsync<ECommerce>(s => s
        .Index(indexName)
            .Size(pageSize).From(pageFrom)
                .Query(q => q
                    .MatchAll()));

        result.Hits.ToList().ForEach(x => x.Source.Id = x.Id);

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> WildCardQueryAsync(string customerFullName)
    {
        var result = await _client.SearchAsync<ECommerce>(s => s
        .Index(indexName)
            .Query(q => q
                .Wildcard(w => w
                    .Field(f => f.CustomerFullName
                        .Suffix("keyword"))
                            .Wildcard(customerFullName))));

        result.Hits.ToList().ForEach(x => x.Source.Id = x.Id);
        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> FuzzyQueryAsync(string customerName)
    {
        var result = await _client.SearchAsync<ECommerce>(s => s
        .Index(indexName)
            .Query(q => q
                .Fuzzy(fu => fu
                    .Field(f => f.CustomerFirstName
                        .Suffix("keyword"))
                            .Value(customerName)
                                .Fuzziness(new Fuzziness(1))))
                                    .Sort(sort => sort
                                        .Field(f => f.TaxFulTotalPrice, new FieldSort() { Order = SortOrder.Desc })));

        result.Hits.ToList().ForEach(x => x.Source.Id = x.Id);
        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> MatchQueryFullTextAsync(string categoryName)
    {
        var result = await _client.SearchAsync<ECommerce>(s => s
        .Index(indexName)
            .Size(1000)
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Category)
                            .Query(categoryName).Operator(Operator.And))));

        result.Hits.ToList().ForEach(x => x.Source.Id = x.Id);
        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> MatchBoolPrefixFullTextAsync(string customerFullName)
    {
        var result = await _client.SearchAsync<ECommerce>(s => s
        .Index(indexName)
            .Size(1000)
                .Query(q => q
                    .MatchBoolPrefix(m => m
                        .Field(f => f.CustomerFullName)
                            .Query(customerFullName))));

        result.Hits.ToList().ForEach(x => x.Source.Id = x.Id);
        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> MatchPhraseFullTextAsync(string customerFullName)
    {
        var result = await _client.SearchAsync<ECommerce>(s => s
        .Index(indexName)
            .Size(1000)
                .Query(q => q
                    .MatchPhrase(m => m
                        .Field(f => f.CustomerFullName)
                            .Query(customerFullName))));

        result.Hits.ToList().ForEach(x => x.Source.Id = x.Id);
        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> CompoundQueryExampleOneAsync(string cityName, double TaxFulTotalPrice, string categoryName, string menufacturer)
    {
        var result = await _client.SearchAsync<ECommerce>(s => s
            .Index(indexName)
                .Size(1000)
                    .Query(q => q
                        .Bool(b => b
                            .Must(m => m
                                .Term(t => t
                                    .Field("geoip.city_name")
                                    .Value(cityName)))

                            .MustNot(mn => mn
                                .Range(r => r
                                    .NumberRange(nr => nr
                                        .Field(f => f.TaxFulTotalPrice)
                                        .Lte(TaxFulTotalPrice))))
                            .Should(s => s
                                .Term(t => t
                                    .Field(f => f.Category.Suffix("keyword"))
                                    .Value(categoryName)))
                            .Filter(f => f
                                .Term(t => t
                                    .Field("manufacturer.keyword")
                                    .Value(menufacturer))))));

        result.Hits.ToList().ForEach(x => x.Source.Id = x.Id);
        return result.Documents.ToImmutableList();

    }
}
