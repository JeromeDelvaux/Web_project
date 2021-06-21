using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web_project.Models;
using Web_project.Models.Interfaces;

namespace Web_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly INewsRepository newsRepository;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment, INewsRepository newsRepository)
        {
            _logger = logger;
            this.hostingEnvironment = hostingEnvironment;
            this.newsRepository = newsRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = await newsRepository.GetAllNewsAsync();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Remerciements()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
