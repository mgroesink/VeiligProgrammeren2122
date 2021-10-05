using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace VeiligProgrammeren2122.Controllers
{
    public class Les2Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Info()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Info(string name , int age)
        {
            long? contentLength = Request.ContentLength;
            if(contentLength > 1000)
            {
                ViewBag.Error = "Too many characters";
            }
            return View();
        }

        public ActionResult Login()
        {
            if(Request.Cookies["vp_username"] != null)
            {
                ViewBag.UserName = Request.Cookies["vp_username"];
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username , string password)
        {
            SqlConnection conn = new SqlConnection();
            string cs = "Server=sql6004.site4now.net;Database=DB_A2A0BC_vp;";
            conn.ConnectionString = cs;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM Users WHERE UserName = '" +
                username + "' AND password = '" + password + "'";
            cmd.Connection = conn;
            conn.Open();
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    CookieOptions option = new CookieOptions();
                    option.Expires = DateTime.Now.AddDays(7);
                    Response.Cookies.Append("vp_username", username , option);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Login failed";
                    return View();
                }
            }
            catch(Exception ex)
            {
                ViewBag.Error = "Error connecting tot database";
                return View();
            }
        }
    }
}
