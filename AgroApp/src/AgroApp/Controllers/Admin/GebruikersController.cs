﻿using AgroApp.Controllers.Api;
using AgroApp.Models;
using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers
{
    public class GebruikersController : Controller
    {
        // GET: /<controller>/
        [HttpGet("admin/gebruikerbeheer")]
        public IActionResult Index()
        {
            return View("../admin/gebruikerbeheer/gebruikerbeheer");
        }

        [HttpGet("admin/gebruikerbeheer/wijzigen/{id}")]
        public async Task<IActionResult> GebruikerWijzigen(int id)
        {
            User user = await UserController.GetUser(id);
            ViewData["id"] = user.IdWerknemer;
            ViewData["naam"] = user.Name;
            ViewData["email"] = user.Username;
            ViewData["rol"] = user.Rol;
            return View("../admin/gebruikerbeheer/gebruikeredit");
        }

        [HttpGet("admin/gebruikerbeheer/toevoegen")]
        public IActionResult GebruikerToevoegen()
        {
            return View("../admin/gebruikerbeheer/gebruikeradd");
        }

        [HttpGet("admin/gebruikerbeheer/archief")]
        public IActionResult GebruikerTerugHalen()
        {
            return View("../admin/gebruikerbeheer/gebruikerArchief");
        }
    }
}
