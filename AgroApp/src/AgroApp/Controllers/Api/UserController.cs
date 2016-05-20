﻿using AgroApp.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AgroApp.Controllers.Api
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        // GET: api/values
        [HttpGet("login/{username}/{password}")]
        [AllowAnonymous]
        public async Task<string> Login(string username, string password)
        {
            password = GetEncodedHash(password, "123");

            if (!await IsValid(username, password))
                return "false";

            string name = (await GetUser(username))?.Name ?? "error with loading name";
            List<Claim> claimCollection = new List<Claim> {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Email, username),
                    new Claim(ClaimTypes.Role, "Admin") };

            await HttpContext.Authentication.SignInAsync("AgroAppCookie", new ClaimsPrincipal(new ClaimsIdentity(claimCollection)));
            return "true"; // auth succeed 

        }

        // GET: api/values
        [HttpGet("logout")]
        [Authorize]
        public async Task<string> Logout(string username, string password)
        {
            await HttpContext.Authentication.SignOutAsync("AgroAppCookie");
            return "succes";
        }

        public static async Task<bool> IsValid(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException();

            string query = "SELECT COUNT(*) FROM werknemer WHERE gebruikersnaam=@0 AND wachtwoord=@1;";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", username),
                new MySqlParameter("@1", password)))
            {
                await reader.ReadAsync();
                return reader.GetInt32(0) == 1;
            }
        }

        public static async Task<User> GetUser(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException();

            string query = "SELECT idWerknemer, naam, gebruikersnaam, rol FROM werknemer WHERE gebruikersnaam=@0";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", email)))
            {
                await reader.ReadAsync();
                return reader.HasRows ? new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)) : null;
            }
        }

        public static async Task<User> GetUser(int id)
        {
            if (id < 0)
                return null;

            string query = "SELECT idWerknemer, naam, gebruikersnaam, rol FROM werknemer WHERE idWerknemer=@0";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", id)))
            {
                await reader.ReadAsync();
                return reader.HasRows ? new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)) : null;
            }
        }

        [HttpGet("register/{username}/{password}/{fullname}/{rol}")]
        public async Task<string> AddUser(string username, string password, string fullname, string rol)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(rol))
                return "Een van de opgegeven velden is leeg";

            password = GetEncodedHash(password, "123");
            if (await IsValid(username, password))//GAAT NIET WERKEN, ALS NAAM OF WACHTWOORD MAAR IETS ANDERS IS RETURNED IE FALSE
                return "Gebruiker bestaat al";

            string query = "INSERT INTO werknemer (`naam`, `gebruikersnaam`, `wachtwoord`, `rol`) VALUES (@0, @1, @2, @3);";
            try
            {
                using (MySqlConnection conn = await DatabaseConnection.GetConnection())
                {
                    await MySqlHelper.ExecuteNonQueryAsync(conn, query,
                      new MySqlParameter("@0", fullname),
                      new MySqlParameter("@1", username),
                      new MySqlParameter("@2", password),
                      new MySqlParameter("@3", rol));
                    return "true";
                }
            }
            catch { return "Er is iets misgegaan!"; }
        }

        //[Authorize("Admin")]
        public static async Task<IEnumerable<User>> GetAllUsers()
        {
            string query = "SELECT idWerknemer, naam, gebruikersnaam, rol FROM werknemer";
            List<User> users = new List<User>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query))
                while (await reader.ReadAsync())
                    users.Add(new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
            return users;
        }

        static string GetEncodedHash(string password, string salt)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] digest = md5.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            string base64digest = Convert.ToBase64String(digest, 0, digest.Length);
            return base64digest.Substring(0, base64digest.Length - 2);
        }
    }
}