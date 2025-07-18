﻿using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TheBookHeaven;
using TheBookHeaven.Models;

public class AdminController : Controller
{
    private readonly MyDbContext _context;

    public AdminController(MyDbContext context)
    {
        _context = context;
    }

    // GET: Admin/Dashboard
    public IActionResult Dashboard()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Admin")
        {
            return RedirectToAction("Login", "Account");
        }

        ViewBag.Username = HttpContext.Session.GetString("Username");
        return View();
    }

    // GET: Admin/ViewBooks
    public IActionResult ViewBooks()
    {
        var books = _context.Books.ToList();
        return View(books);
    }

    // GET: Admin/Edit/{id}
    public IActionResult Edit(int id)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // POST: Admin/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Book updatedBook)
    {
        if (id != updatedBook.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var book = _context.Books.Find(updatedBook.Id);
                if (book == null)
                {
                    return NotFound();
                }

                // Update book properties
                book.Title = updatedBook.Title;
                book.Category = updatedBook.Category;
                book.ImageUrl = updatedBook.ImageUrl;
                book.Price = updatedBook.Price;

                _context.SaveChanges();

                return RedirectToAction("ViewBooks");
            }
            catch (Exception ex)
            {
                // Log the exception 
                ModelState.AddModelError("", "An error occurred while saving the book. Please try again.");
                return View(updatedBook);
            }
        }

        // If validation failed, return to view with validation errors
        return View(updatedBook);
    }

    // POST: Admin/DeleteBook/{id}
    [HttpGet]
    public IActionResult DeleteBook(int id)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        _context.Books.Remove(book);
        _context.SaveChanges();

        return RedirectToAction("ViewBooks");
    }

    // GET: Admin/AddBook
    public IActionResult AddBook()
    {
        return View();
    }

    // POST: Admin/AddBook
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddBook(Book book)
    {
        if (ModelState.IsValid)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Book added successfully!";
            return RedirectToAction("ViewBooks");
        }

        return View(book);
    }

    // GET: Admin/ViewUsers
    public IActionResult ViewUsers()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Admin")
        {
            return RedirectToAction("Login", "Account");
        }

        // Fetch all users except admins (optional)
        var users = _context.Users.Where(u => u.Role != "Admin").ToList();

        return View(users);
    }

    // POST: Admin/DeleteUser/{id}
    [HttpPost]
    public IActionResult DeleteUser(int id)
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Admin")
        {
            return RedirectToAction("Login", "Account");
        }

        var user = _context.Users.Find(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        return RedirectToAction("ViewUsers");
    }
}