using Books.Models;
using Books.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;

namespace Books.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BooksController (){
            _context = new ApplicationDbContext();
         }


        // GET: Books
        public ActionResult Index()
        {
            var Books = _context.Books.Include(m => m.Category).ToList();
            return View(Books);
        }

        public ActionResult Create()
        {
            var viewModel = new BookFormViewModels
            {
                Categories = _context.Categories.Where(m => m.IsActive).ToList() 
            };
            return View("BookForm",viewModel);  
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult save( BookFormViewModels model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _context.Categories.Where(m => m.IsActive).ToList();
               return View("create" , model);
            }
            if(model.Id ==  0)
            {
                var Book = new Book
                {
                    Title = model.Title,
                    CategoryId = model.CategoryId,
                    Author = model.Author,
                    Description = model.Description
                };
                _context.Books.Add(Book);
            }
            else
            {
                var Book = _context.Books.Find(model.Id);
                if (Book == null)
                {
                    return HttpNotFound();

                }

                Book.Title = model.Title;
                Book.Author = model.Author;
                Book.Description = model.Description;
                Book.CategoryId = model.CategoryId;

            }
           


            _context.SaveChanges();
            TempData["Message"] = "Data saved succefully";
            return RedirectToAction("index");
        }

        public ActionResult Edit(int? Id)
        {
            if(Id == null)
            {
                return HttpNotFound();
            }

            var Book = _context.Books.Find(Id);
            if(Book == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var viewModel = new BookFormViewModels
            {
                Id = Book.Id,
                Title = Book.Title,
                CategoryId = Book.CategoryId,
                Author = Book.Author,
                Description = Book.Description, 
                Categories = _context.Categories.Where(m => m.IsActive).ToList() 
            };

            return View("BookForm", viewModel);
        }

        public ActionResult Details(int? id)
        {
            if (id == null) 
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var book = _context.Books.Include(m => m.Category).SingleOrDefault(m => m.Id == id);
            if (book == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(book);
        }
    }
}