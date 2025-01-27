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
        public ActionResult Create([Bind("Name, Episodes, Score")] Title title)
        {
            if (title.Name != null)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        transaction.CreateSavepoint("SP_1");
                        db.Titles.Add(title);
                        db.SaveChanges();
                        //transaction.RollbackToSavepoint("SP_1");
                        transaction.Commit();
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("", "An error occurred while creating the title.");
                    }
                }
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
        public ActionResult Edit([Bind("Id, Name, Episodes, Score")] Title title)
        {
            if (title.Name != null)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Titles.Update(title);
                        db.SaveChanges();
                        transaction.Commit();
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("", "An error occurred while editing the title.");
                    }
                }
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
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var title = db.Titles.Find(id);

                    if (title != null)
                    {
                        var userTitles = db.UserTitles.Where(ut => ut.TitleId == id).ToList();
                        var characters = db.Characters.Where(c => c.TitleId == id).ToList();
                        db.UserTitles.RemoveRange(userTitles);
                        db.Characters.RemoveRange(characters);

                        foreach (var user in db.Users)
                        {
                            foreach (var usertitle in userTitles)
                            {
                                if (user.Id == usertitle.UserId)
                                {
                                    user.TitlesSum--;
                                }
                            }
                        }

                        db.Titles.Remove(title);
                        db.SaveChanges();
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    ModelState.AddModelError("", "An error occurred while deleting the title.");
                }
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