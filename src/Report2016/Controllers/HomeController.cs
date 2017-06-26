using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Report2016.Controllers
{
	
    public class HomeController : Controller
    {
	    
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/Result")]
		public IActionResult Result()
		{
			return View();
		}

		[HttpGet("/Success")]
		public IActionResult Success()
		{
			return View();
		}

		[HttpGet("/Signin")]
		public IActionResult Signin()
		{
			return View();
		}

    }
	
}
