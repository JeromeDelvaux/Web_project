using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_project.ViewsModels;

namespace Web_project.Models.Interfaces
{
    public interface IShortUrlRepository
    {
        Task<string> GetShortURL(string longUrl);
    }
}
