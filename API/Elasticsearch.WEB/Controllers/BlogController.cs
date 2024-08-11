using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.WEB.Models;
using Elasticsearch.WEB.Services;
using Elasticsearch.WEB.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Elasticsearch.WEB.Controllers
{
    public class BlogController(BlogService blogService) : Controller
    {
        private readonly BlogService _blogService = blogService;

        public async Task<IActionResult> SearchAsync()
        {
            return View(await _blogService.SearchAsync(string.Empty));
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchText)
        {
            ViewBag.searchText = searchText;
            var response = await _blogService.SearchAsync(searchText);

            return View(response);
        }

        public IActionResult Save()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(BlogCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _blogService.SaveAsync(model);

            if (!response)
            {
                TempData["result"] = "Kayıt başarısız";
                return RedirectToAction(nameof(BlogController.Save));
            }

            TempData["result"] = "Kayıt başarılı";
            return RedirectToAction(nameof(BlogController.Save));
        }

    }
}