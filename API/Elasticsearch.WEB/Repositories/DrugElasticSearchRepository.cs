using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elasticsearch.WEB.Models;

namespace Elasticsearch.WEB.Repositories;

public class DrugElasticSearchRepository
{
    private readonly ElasticsearchClient _elasticSearchClient;

    public DrugElasticSearchRepository(ElasticsearchClient elasticSearchClient)
    {
        _elasticSearchClient = elasticSearchClient;
    }

    private const string IndexName = "drugs";  // Drugs index

    public async Task<(List<DrugsElasticsearch> list, long count)> SearchAsync(DrugSearchViewModel searchViewModel)
    {
        List<Action<QueryDescriptor<DrugsElasticsearch>>> listQuery = new();

        // Eğer arama kriteri yoksa MatchAll ekle
        if (searchViewModel == null ||
            (string.IsNullOrEmpty(searchViewModel.DrugName) &&
             string.IsNullOrEmpty(searchViewModel.Producer) &&
             string.IsNullOrEmpty(searchViewModel.QRCode.ToString())))
        {
            listQuery.Add(g => g.MatchAll());
            return await CalculateResultSet(listQuery);
        }

        if (!string.IsNullOrEmpty(searchViewModel.QRCode?.ToString()))
        {
            listQuery.Add((q) => q.Match(m => m
                .Field(f => f.QRCode.Suffix("keyword"))
                .Query(searchViewModel.QRCode.ToString())
            ));
        }

        // DrugName için kısmi arama yapılacak (tam eşleşme gerekmiyor)
        if (!string.IsNullOrEmpty(searchViewModel.DrugName))
        {
            listQuery.Add((q) => q.Match(m => m
                .Field(f => f.DrugName)
                .Query(searchViewModel.DrugName)
            ));
        }

        // Producer için kısmi arama yapılacak (tam eşleşme gerekmiyor)
        if (!string.IsNullOrEmpty(searchViewModel.Producer))
        {
            listQuery.Add((q) => q.Match(m => m
                .Field(f => f.Producer)
                .Query(searchViewModel.Producer)
            ));
        }

        // Arama kriteri yoksa MatchAll kullanılır
        // if (!listQuery.Any())
        // {
        //     listQuery.Add(g => g.MatchAll());
        // }

        return await CalculateResultSet(listQuery);
    }

    private async Task<(List<DrugsElasticsearch> list, long count)> CalculateResultSet(List<Action<QueryDescriptor<DrugsElasticsearch>>> listQuery)
    {
        var result = await _elasticSearchClient.SearchAsync<DrugsElasticsearch>(s => s.Index(IndexName)
            .Size(1000)
            .Query(q => q.Bool(b => b.Must(listQuery.ToArray())))
        );

        foreach (var hit in result.Hits)
        {
            hit.Source.BarcodeId = long.Parse(hit.Id);
        }

        return (list: result.Documents.ToList(), result.Total);
    }
}