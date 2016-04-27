using Dapper;
using DogDays.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DogDays.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection Conn = new SqlConnection("Server=(local);Database=Example;Trusted_Connection=True;");
        // GET: Home
        public ActionResult Index()
        {
                var list = Conn.Query<Dog>("Select DogID, Name from Dog");
                return View(list);
        }

        public ActionResult Index2()
        {
            List<Dog> list = new List<Dog>();
            var cmd = new SqlCommand("Select DogID, Name from Dog", Conn);
            Conn.Open();

            using (var rd = cmd.ExecuteReader())
            {
                while (rd.Read())
                {
                    list.Add(new Dog
                    {
                        DogId = rd.GetInt32(0),
                        Name = (string)rd["Name"]
                    });
                }
            }

            return View("Index", list);
        }



        public ActionResult AddDog()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddDog(Dog dog)
        {
            Conn.Execute("insert dog(name) values(@Name);",
                new { Name = dog.Name });


            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Conn != null)
            {
                Conn.Dispose();
                Conn = null;

            }

            base.Dispose(disposing);
        }

    }
}