using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using MyGoidaList.Models;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace MyGoidaList.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db;

        public UsersController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Nickname")] User user)
        {
            if (user.Nickname != null)
            {
                user.TitlesSum = 0;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            Console.WriteLine("INVALID VALUE IN USER CREATING");
            return View(user);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Edit");
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Nickname")] User user)
        {
            if (user.Nickname != null)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Delete");
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = db.Users.Find(id);

            if (user != null)
            {
                var userTitles = db.UserTitles.Where(ut => ut.UserId == id).ToList();
                db.UserTitles.RemoveRange(userTitles);

                db.Users.Remove(user);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}