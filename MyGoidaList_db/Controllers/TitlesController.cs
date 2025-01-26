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
    public class TitlesController : Controller
    {
        private ApplicationDbContext db;

        public TitlesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ActionResult Index()
        {
            return View(db.Titles.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Name, Episodes, Score")]Title title)
        {
            if (title.Name != null)
            {
                db.Titles.Add(title);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(title);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Edit");
            }
            Title title = db.Titles.Find(id);
            if (title == null)
            {
                return NotFound();
            }
            return View(title);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Name, Episodes, Score")] Title title)
        {
            if (title.Name != null)
            {
                db.Entry(title).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(title);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Delete");
            }
            Title title = db.Titles.Find(id);
            if (title == null)
            {
                return NotFound();
            }
            return View(title);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var title = db.Titles.Find(id);

            if (title != null)
            {
                var userTitles = db.UserTitles.Where(ut => ut.TitleId == id).ToList();
                var characters = db.Characters.Where(c => c.TitleId == id).ToList();
                db.UserTitles.RemoveRange(userTitles);
                db.Characters.RemoveRange(characters);

                db.Titles.Remove(title);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        public ActionResult Titles(int userId)
        {
            var user = db.Users.Include(u => u.UserTitles.Select(ut => ut.Title)).FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}