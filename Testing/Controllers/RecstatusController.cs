//============================================
// David Zientara
// 11-25-2022
// ASP.NET application to add a web interface 
// to my stream recorder 
//
// RecstatusController.cs
// This code allows the user to view the data
// in Recstatus
//--------------------------------------------

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SSR.Models;

namespace SSR.Controllers
{
    public class RecstatusController : Controller
    {
        private readonly IRecstatusRepository repo;
        public RecstatusController(IRecstatusRepository repo)
        {
            this.repo = repo;
        }


        public IActionResult ViewLastmod(int id)
        {
            var lastmod = repo.GetLastmod(id);

            return View(lastmod);
        }


        public IActionResult Index()
        {
            var status = repo.GetAllRecstatuses();

            return View(status);
        }

    }
}
