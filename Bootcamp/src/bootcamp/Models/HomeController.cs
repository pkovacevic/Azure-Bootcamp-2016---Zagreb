using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using bootcamp.Models;
using System.Net;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using bootcamp.Utilities;
using Microsoft.Extensions.PlatformAbstractions;

namespace bootcamp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApplicationEnvironment _env;

        public HomeController(IApplicationEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            try
            {
                var data = System.IO.File.ReadAllText(Path.Combine(_env.ApplicationBasePath, "data.json"));
                var locationInfo = JsonConvert.DeserializeObject<LocationInfo>(data);
                return View(locationInfo);

            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Verify()
        {
            return View();
        }

        [HttpPost]
        [CustomExceptionFilter]
        public IActionResult VerifyJSON(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("No JSON entered by user");

            try
            {
                var locationInfo = JsonConvert.DeserializeObject<LocationInfo>(value);
                if (locationInfo == null)
                    throw new InvalidOperationException("Could not parse JSON");

                return View("Index", locationInfo);
            }
            catch
            {
                throw;
            }
        }
    }
}
