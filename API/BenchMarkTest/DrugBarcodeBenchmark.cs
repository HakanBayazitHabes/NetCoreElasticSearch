using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Elasticsearch.WEB.Services;
using Elasticsearch.WEB.Services.DrugElasticsearchService;
using Elasticsearch.WEB.Repositories;
using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using AppContext = Elasticsearch.WEB.Repositories.AppContext;
using Elasticsearch.WEB.Models;

namespace BenchMarkTest
{
    public class DrugBarcodeBenchmark
    {
        private readonly DrugBarcodeService _drugBarcodeService;
        private readonly DrugElasticsearchService _drugElasticService;

        public DrugBarcodeBenchmark()
        {
            var options = new DbContextOptions<AppContext>();
            var context = new AppContext(options);
            var drugRepository = new DrugBarcodeRepository(context);
            _drugBarcodeService = new DrugBarcodeService(drugRepository);

            var elasticSearchClient = new ElasticsearchClient();
            var drugElasticRepository = new DrugElasticSearchRepository(elasticSearchClient);
            _drugElasticService = new DrugElasticsearchService(drugElasticRepository);
        }

        [Params("UCB PHARMA")]
        public string Producer { get; set; }

        [Params("KEPPRA 500 MG 50 FILM TABLET")]
        public string DrugName { get; set; }

        [Params("89f07b69-a733-496c-a5d1-269b5b552347")] 
        public string QRCode { get; set; }

        // [Benchmark]
        // public void TestSearchDrugBarcodesEnumerable()
        // {
        //     Guid.TryParse(QRCode, out Guid qrCodeGuid);
        //     _drugBarcodeService.SearchDrugBarcodesWithEnumerable(Producer, DrugName, qrCodeGuid);
        // }

        [Benchmark]
        public void TestSearchDrugBarcodesQueryable()
        {
            Guid.TryParse(QRCode, out Guid qrCodeGuid);
            var result = _drugBarcodeService.SearchDrugBarcodesQueryable(Producer, DrugName, qrCodeGuid).ToList();
        }

        [Benchmark]
        public void TestElasticSearchDrugBarcode()
        { 
            Guid.TryParse(QRCode, out Guid qrCodeGuid);
            DrugSearchViewModel viewModel = new DrugSearchViewModel
            {
                DrugName = DrugName,
                Producer = Producer,
                QRCode = qrCodeGuid
            };

            _drugElasticService.SearchAsync(viewModel).Wait();
        }
    }


}