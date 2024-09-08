using System.Diagnostics;
using Elasticsearch.WEB.Models;
using Elasticsearch.WEB.Services.DrugElasticsearchService;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.WEB.Controllers
{
    public class DrugsElasticSearchController : Controller
    {
        private readonly DrugElasticsearchService _service;

        public DrugsElasticSearchController(DrugElasticsearchService service)
        {
            _service = service;
        }

        // Arama işlemi için endpoint
        public async Task<IActionResult> Search([FromQuery] DrugSearchPageViewModel searchPageView)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            // Service üzerinden arama işlemi gerçekleştiriliyor
            var (drugList, totalCount) = await _service.SearchAsync(searchPageView.SearcViewModel);
            stopwatch.Stop();
            ViewData["ElapsedTime"] = stopwatch.ElapsedMilliseconds;
            // Arama sonuçlarını view modelde sakla
            searchPageView.List = drugList;
            searchPageView.TotalCount = totalCount;

            // Sonuçları view'e dön
            return View(searchPageView);
        }

    }
}