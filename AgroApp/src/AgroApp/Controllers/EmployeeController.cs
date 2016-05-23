﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: /user/main
        public IActionResult Main()
        {
            ViewData["volledigenaam"] = HttpContext.User.Identity.Name;
            return View();
        }
    }
}