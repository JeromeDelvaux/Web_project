using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Web_project.Models;
using Web_project.Models.Interfaces;

namespace Web_project.Controllers.Utilisateurs
{
    public class NewsController : Controller
    {
        private readonly INewsRepository newsRepository;

        public NewsController(INewsRepository newsRepository)
        {
            this.newsRepository = newsRepository;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = await newsRepository.GetAllNewsAsync();
                return View(model);
            }
            catch (Exception ex)
            {
                ErrorViewModel erreur = new ErrorViewModel() { RequestId = ex.Message };
                return View("Error", erreur);
            }
        }
    }
}