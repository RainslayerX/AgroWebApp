﻿using AgroApp.Controllers.Api;
using AgroApp.Models;
using Microsoft.AspNet.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers
{
    [Route("[controller]")]
    public class WerknemerController : Controller
    {
        // GET: /user/usermain
        [HttpGet("menu")]
        public IActionResult Index()
        {
            ViewData["volledigenaam"] = HttpContext.User.Identity.Name;
            return View("../werknemer/main");
        }

        [HttpGet("werkboninvullen/")]
        public IActionResult werkbontoevoegen()
        {
            return View("../admin/werkbonadd");

        }


        [HttpGet("gebruikerbeheer")]
        public IActionResult Accountbeheren()
        {
            return View("../werknemer/Manage");
        }

        [HttpGet("opdrachten")]
        public IActionResult Opdrachten()
        {
            return View("../werknemer/Assignment");
        }

        [HttpGet("getopdrachten")]
        public async Task<IEnumerable<Opdracht>> GetOpdrachten()
        {
            string query = "SELECT * FROM Opdracht";
            List<Opdracht> opdrachten = new List<Opdracht>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", 1)))
                while (await reader.ReadAsync())
                    opdrachten.Add(new Opdracht(
                        idOpdracht: reader["idOpdracht"] as int? ?? -1,
                        locatie: reader["locatie"] as string,
                        beschrijving: reader["beschrijving"] as string,
                        datum: reader["datum"] as DateTime? ?? DateTime.MinValue));
            return opdrachten;
        }

        [HttpGet("GetGebruikerOpdrachten")]
        public async Task<IEnumerable<OpdrachtWerknemer>> GetGebruikerOpdrachten()
        {
            string query = "SELECT Opdracht.idOpdracht, Opdracht.locatie, Opdracht.beschrijving, Opdracht.datum, Klant.naam, Klant.adres, OpdrachtWerknemer.idOpdrachtWerknemer" +
                " FROM Opdracht JOIN Klant ON Klant.idKlant = Opdracht.idKlant JOIN OpdrachtWerknemer ON Opdracht.idOpdracht = OpdrachtWerknemer.idOpdracht " +
                "WHERE OpdrachtWerknemer.idWerknemer = @0";
            List<OpdrachtWerknemer> OpdrachtWerknemer = new List<OpdrachtWerknemer>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", HttpContext.Request.Cookies["idUser"])))
                while (await reader.ReadAsync())
                    OpdrachtWerknemer.Add(new OpdrachtWerknemer()
                    {
                        Opdracht = new Opdracht(
                        idOpdracht: reader["idOpdracht"] as int? ?? -1,
                        selectedKlant: new Customer(Name: reader["naam"] as string, Adress: reader["adres"] as string),
                        locatie: reader["locatie"] as string,
                        beschrijving: reader["beschrijving"] as string,
                        datum: reader["datum"] as DateTime? ?? DateTime.MinValue),
                        idOpdrachtWerknemer = reader ["idOpdrachtWerknemer"] as int? ?? -1
                    });
            return OpdrachtWerknemer;
        }

        [HttpGet("getOpdrachtWerknemer/{id}")]
        public async Task<OpdrachtWerknemer> GetOpdrachtWerknemer(int id)
        {
            string query = "SELECT Opdracht.idOpdracht, Opdracht.locatie, Opdracht.beschrijving, Opdracht.datum, Klant.naam, Klant.adres, OpdrachtWerknemer.idWerknemer" +
                " FROM Opdracht LEFT JOIN Klant ON Klant.idKlant = Opdracht.idKlant LEFT JOIN OpdrachtWerknemer ON Opdracht.idOpdracht = OpdrachtWerknemer.idOpdracht " +
                "WHERE OpdrachtWerknemer.idOpdrachtWerknemer = @0";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", id)))
                while (await reader.ReadAsync())
                    return new OpdrachtWerknemer()
                    {
                        Opdracht = new Opdracht(
                        idOpdracht: reader["idOpdracht"] as int? ?? -1,
                        selectedKlant: new Customer(Name: reader["naam"] as string, Adress: reader["adres"] as string),
                        locatie: reader["locatie"] as string,
                        beschrijving: reader["beschrijving"] as string,
                        datum: reader["datum"] as DateTime? ?? DateTime.MinValue),
                        Werknemer = new User() { IdWerknemer = reader["idWerknemer"] as int? ?? -1 },
                        idOpdrachtWerknemer = id as int? ?? -1
                    };
            return null;
        }

        [HttpGet("getopdracht")]
        private async Task<Opdracht> _GetOpdracht(int id)
        {
            return await _GetOpdracht(id);
        }

        public static async Task<Opdracht> GetOpdracht(int id)
        {
            if (id < 0)
                return null;

            string query = "SELECT idOpdracht, locatie, beschrijving,  datum FROM Opdracht WHERE idOpdracht=@0";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", id)))
            {
                await reader.ReadAsync();
                return new Opdracht(reader["idOpdracht"] as int? ?? 0, reader["locatie"] as string ?? "", reader["beschrijving"] as string ?? "", reader["datum"] as DateTime?);
            }
        }

        [HttpGet("assignmentedit/{id}")]
        public async Task<IActionResult> GebruikerWijzigen(int id)
        {
            Opdracht opdracht = await GetOpdracht(id);
            ViewData["id"] = id;
            ViewData["locatie"] = opdracht.locatie;
            ViewData["beschrijving"] = opdracht.beschrijving;
            ViewData["datum"] = opdracht?.datum.ToString() ?? "";
            return View("assignmentedit");
        }

    }

}
