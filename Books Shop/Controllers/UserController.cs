using Books_Shop.Helpers;
using Books_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace Books_Shop.Controllers
{
    public class UserController : Controller
    {
        private AppDbContext _appDbContext = new AppDbContext();

        // GET: User
        public ActionResult Index()
        {
            if (Session["Id"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //GET: Register

        public ActionResult Register()
        {
            return View();
        }


        //POST : Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _user)
        {
            if (ModelState.IsValid)
            {
                // Check if the user already exits or not
                try
                {
                    var userExist = _appDbContext.Users.FirstOrDefault(s => s.UserName == _user.UserName);
                    if (userExist == null)
                    {

                        try
                        {
                            _user.Password = Hashing.getHash(_user.Password);
                            _user.Code = CodeGenerator.generate(8);
                            _user.UserName = _user.UserName;
                            _appDbContext.Users.Add(_user);
                            _appDbContext.SaveChanges();

                            // Add Session
                            Session["UserName"] = _user.UserName;
                            Session["Id"] = _user.Id;
                            Session["Code"] = _user.Code;
                            return RedirectToAction("Index", "Book");
                        }
                        catch (Exception e)
                        {
                            string s = e.Message.ToString();
                        }

                    }
                    else
                    {
                        ViewBag.error = "User Name already exist";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message.ToString();
                }
            }
            return View();
        }

        //GET : Login
        public ActionResult Login()
        {
            return View();
        }

        // POST : Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string userName, string password)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var hashedPassword = Hashing.getHash(password);
                    var user = _appDbContext.Users.FirstOrDefault(u => u.UserName == userName);
                    if(user != null)
                    {
                        if(user.Password != hashedPassword)
                        {
                            ViewBag.error = "Wrong Password";
                            return View();
                        }
                        // Add Session
                        Session["UserName"] = user.UserName;
                        Session["Id"] = user.Id;
                        Session["Code"] = user.Code;
                        if(user.UserName =="admin" && user.Code == "Admin2020")
                        {
                            return RedirectToAction("Dashboard", "Book");
                        }
                        return RedirectToAction("Index", "Book");
                    }
                    else if (user == null)
                    {
                        ViewBag.error = "User Not Found";
                        return View();
                    }
                    
                    else
                    {
                        ViewBag.error = "Login Falid";
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return View();

        }

        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }

        // Borrowed Books
        public ActionResult BorrowedBooks()
        {
            if (Session["UserName"] != null)
            {

                string userName = Session["UserName"].ToString();

                List<UserBooks> books = _appDbContext.UserBooks.Where(u => u.users.UserName == userName && u.NumberOfEditions > 0).ToList();

                return View(books);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }



    }
}
