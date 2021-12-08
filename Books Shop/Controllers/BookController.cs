using Books_Shop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Books_Shop.Controllers
{
    public class BookController : Controller
    {
        AppDbContext _appDbContext = new AppDbContext();

        // GET: Book
        public ActionResult Index()
        {
            try
            {
                if (Session["UserName"] != null)
                {
                    List<Book> books = _appDbContext.Books.ToList();
                    return View(books);
                }

            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
            }
            return RedirectToAction("Login", "User");

        }

        // Borrow A Book
        public ActionResult Borrow(int Id)
        {
            try
            {
                Book book = _appDbContext.Books.FirstOrDefault(b => b.Id == Id);
                if (book != null)
                {
                    if (book.CurrentEditions == 0)
                    {
                        ViewBag.noLeft = "No Left Editions";
                        return Json(new { success = false, message = "No Left Editions" }, JsonRequestBehavior.AllowGet); ;
                    }

                    string userName = Session["UserName"].ToString();
                    User currentUser = _appDbContext.Users.FirstOrDefault(u => u.UserName == userName);
                    UserBooks user = _appDbContext.UserBooks.FirstOrDefault(u => u.users.UserName == userName);

                    if (user != null)
                    {
                        var existBook = user.users.books.FirstOrDefault(b => b.Id == Id);

                        if (existBook != null)
                        {
                            UserBooks currentBook = _appDbContext.UserBooks.FirstOrDefault(b => b.books.Id == Id && b.users.UserName == userName);
                            currentBook.NumberOfEditions += 1;

                            //Update the users table
                            _appDbContext.Entry(currentBook).State = EntityState.Modified;
                        }

                        //New Book
                        else
                        {
                            _appDbContext.UserBooks.Add(new UserBooks()
                            {
                                users = currentUser,
                                books = book,
                                NumberOfEditions = 1
                            });

                            currentUser.books.Add(book);
                        }
                    }

                    //New User
                    else
                    {
                        _appDbContext.UserBooks.Add(new UserBooks()
                        {
                            users = currentUser,
                            books = book,
                            NumberOfEditions = 1
                        });

                        currentUser.books.Add(book);
                    }

                    book.CurrentEditions -= 1;

                    _appDbContext.Entry(currentUser).State = EntityState.Modified;
                    _appDbContext.SaveChanges();
                }
                return Json(new { success = true, JsonRequestBehavior.AllowGet });
            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Return A Book
        public ActionResult Return(int Id)
        {
            try
            {
                Book book = _appDbContext.Books.FirstOrDefault(b => b.Id == Id);
                if (book != null)
                {
                    // All book Editions returned
                    if (book.CurrentEditions == book.TotalEditions)
                    {
                        ViewBag.noLeft = "No More Edition For This Book, All Edition Returned";
                        return Json(new { success = false, message = "No More Edition For This Book, All Edition Returned" }, JsonRequestBehavior.AllowGet); ;
                    }

                    string userName = Session["UserName"].ToString();
                    UserBooks user = _appDbContext.UserBooks.FirstOrDefault(u=>u.users.UserName == userName);

                    if (user != null)
                    {
                        var existBook = user.users.books.FirstOrDefault(b => b.Code == book.Code);
                        UserBooks currentBook = _appDbContext.UserBooks.FirstOrDefault(b => b.books.Id == Id && b.users.UserName == userName);
                        if (existBook != null && currentBook.NumberOfEditions > 0)
                        {
                           
                            currentBook.NumberOfEditions -= 1;

                            //Update the users table
                            _appDbContext.Entry(currentBook).State = EntityState.Modified;
                        }
                        else if (existBook !=null)
                        {
                            if (currentBook.NumberOfEditions == 0)
                            {
                                user.users.books.Remove(existBook);
                                _appDbContext.Entry(user.users).State = EntityState.Modified;
                                _appDbContext.SaveChanges();

                            }
                            return Json(new { success = false, message = "You returned all borrowed books " }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    book.CurrentEditions += 1;

                    _appDbContext.Entry(book).State = EntityState.Modified;
                    _appDbContext.SaveChanges();
                }
                return Json(new { success = true, JsonRequestBehavior.AllowGet });
            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Dashboard
        public ActionResult Dashboard()
        {
          
            return View();
        }

        public ActionResult ViewAll()
        {
            try
            {
                if (Session["UserName"] == null || Session["UserName"].ToString() != "admin")
                {
                    return RedirectToAction("Login", "User");
                }

                string userName = Session["UserName"].ToString();

                List<Book> books = _appDbContext.Books.ToList();

                return View(books);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return RedirectToAction("Dashboard");
        }



        //GET : AddUpdate
        public ActionResult AddUpdate(int? Id)
        {
            Book currentBook = new Book();
            if (Id != null)
            {
                 currentBook = _appDbContext.Books.FirstOrDefault(b => b.Id == Id);

            }
            return View(currentBook);
        }

        //POST : AddUpdate
        [HttpPost]
        public ActionResult AddUpdate(Book book)
        {
            try
            {
                book.CurrentEditions = book.TotalEditions;
                if(book.Id == 0)
                {
                    _appDbContext.Books.Add(book);
                    _appDbContext.SaveChanges();
                }

                //Update book
                else
                {
                    _appDbContext.Entry(book).State = EntityState.Modified;
                    _appDbContext.SaveChanges();
                }
                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return View();
        }

        // Delete
        public ActionResult Delete(int? Id)
        {
            try
            {
                var existBook = _appDbContext.Books.FirstOrDefault(b=>b.Id==Id);
                if (existBook != null)
                {
                    _appDbContext.Books.Remove(existBook);
                    _appDbContext.SaveChanges();
                }
                return RedirectToAction("Dashboard");

            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return RedirectToAction("Dashboard");
        }
    }
}