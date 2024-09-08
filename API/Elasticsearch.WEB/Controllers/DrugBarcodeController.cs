using System.Diagnostics;
using Elasticsearch.WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.WEB.Controllers;

public class DrugBarcodeController : Controller
{
    private readonly DrugBarcodeService _drugBarcodeService;

    public DrugBarcodeController(DrugBarcodeService drugBarcodeService)
    {
        _drugBarcodeService = drugBarcodeService;
    }

    [HttpGet]
    public IActionResult SearchDrugBarcodesWithEnumerable()
    {
        // Stopwatch stopwatch = new Stopwatch();
        // stopwatch.Start();

        // var drugBarcodes = _drugBarcodeService.SearchDrugBarcodesWithEnumerable(producer, drugName, qrCode);

        // stopwatch.Stop();
        // ViewData["ElapsedTime"] = stopwatch.ElapsedMilliseconds;

        return View();

    }

    [HttpPost]
    public IActionResult SearchDrugBarcodesWithEnumerable(string producer, string drugName, Guid qrCode)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var drugBarcodes = _drugBarcodeService.SearchDrugBarcodesWithEnumerable(producer, drugName, qrCode);

        stopwatch.Stop();
        ViewData["ElapsedTime"] = stopwatch.ElapsedMilliseconds;

        return View(drugBarcodes);

    }

    [HttpGet]
    public IActionResult SearchDrugBarcodesQueryable()
    {
        // Stopwatch stopwatch = new Stopwatch();
        // stopwatch.Start();

        // var drugBarcodes = _drugBarcodeService.SearchDrugBarcodesQueryable(producer, drugName, qrCode);

        // stopwatch.Stop();
        // ViewData["ElapsedTime"] = stopwatch.ElapsedMilliseconds;

        return View();
    }

    [HttpPost]
    public IActionResult SearchDrugBarcodesQueryable(string producer, string drugName, Guid qrCode)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var drugBarcodes = _drugBarcodeService.SearchDrugBarcodesQueryable(producer, drugName, qrCode);

        stopwatch.Stop();
        ViewData["ElapsedTime"] = stopwatch.ElapsedMilliseconds;

        return View(drugBarcodes);
    }
}