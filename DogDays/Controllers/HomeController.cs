using Dapper;
using DogDays.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//Note: the name "User" in sql is a reserved word, if you choose to name you table "User", you need 
//to escapte it like "Select * from [User]"

//the alternative to this is to call it Users 

//Content is also reserved




namespace DogDays.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection Conn = new SqlConnection("Server=(local);Database=Example;Trusted_Connection=True;");
        // GET: Home
        public ActionResult Index()
        {
            var cookie = Request.Cookies["name"];
            if (cookie == null)
                ViewBag.Name = "(unknown)";
            else
                ViewBag.Name = cookie.Value;



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

        public ActionResult DogDetail(int dogID)
        {
            var dog = Conn.Query<Dog>("Select DogID, Name From Dog Where DogID=@DogID",
                new { DogID = dogID }).Single();
            return View(dog);
        }

        public ActionResult Identify()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Identify(SignupViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Name=="The Man")
                {
                    ModelState.AddModelError("Name", "No, there can only be one 'The man', and you are not it.");
                }
            }

            if (ModelState.IsValid)
            {


                var cookie = new HttpCookie("name", model.Name);
                cookie.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(cookie);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult SignOut()
        {
            var c = new HttpCookie("name");
            c.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(c);
            return RedirectToAction("Index");
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