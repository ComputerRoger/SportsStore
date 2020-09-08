using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerApp.Models;

namespace ServerApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            IActionResult iActionResult;
            bool usePlaceHolder;

            usePlaceHolder = false;
            if (usePlaceHolder)
            {
                ViewResult viewResult;
                string viewName = "Placeholder";

                //  The default ActionResult will be the Placeholder view.
                viewResult = View(viewName);
                iActionResult = viewResult;
            }
            else
            {
                //  Return the default view.
                iActionResult = View();
            }
            return ( iActionResult );
        }

        public IActionResult Privacy()
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
