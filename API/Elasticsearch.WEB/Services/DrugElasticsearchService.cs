using Elasticsearch.WEB.Models;
using Elasticsearch.WEB.Repositories;

namespace Elasticsearch.WEB.Services.DrugElasticsearchService
{
    public class DrugElasticsearchService
    {
        private readonly DrugElasticSearchRepository _repository;

        public DrugElasticsearchService(DrugElasticSearchRepository repository)
        {
            _repository = repository;
        }

        public async Task<(List<DrugsElasticsearch>, long totalCount)> SearchAsync(DrugSearchViewModel searchModel)
        {
            var (drugList, totalCount) = await _repository.SearchAsync(searchModel);

            var drugListViewModel = drugList.Select(x => new DrugsElasticsearch
            {
                BarcodeId = x.BarcodeId,
                CreatedDate = x.CreatedDate,
                DrugBarcode = x.DrugBarcode,
                DrugId = x.DrugId,
                DrugName = x.DrugName,
                Producer = x.Producer,
                QRCode = x.QRCode,
                RecipeType = x.RecipeType,
                Status = x.Status
            }).ToList();

            return (drugListViewModel, totalCount);
        }
    }
}