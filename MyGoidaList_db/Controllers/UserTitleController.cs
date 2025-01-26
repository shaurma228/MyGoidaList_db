using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using MyGoidaList.Models;
using System.Net;
using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;

namespace MyGoidaList.Controllers
{
    public class UserTitleController : Controller
    {
        private ApplicationDbContext db;

        public UserTitleController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ActionResult Index(int userId)
        {
            var user = db.Users
                .Include(u => u.UserTitles)       // Включаем коллекцию UserTitles
                .ThenInclude(ut => ut.Title)      // Включаем связанные Title для каждого UserTitle
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return NotFound(); // Обработка случая, если пользователь не найден
            }

            ViewBag.UserId = userId;
            ViewBag.UserName = user.Nickname;

            // Передаём связанные UserTitles в представление
            return View(user.UserTitles);
        }


        public ActionResult Edit(int userId, int titleId)
        {
            var userTitle = db.UserTitles
                .FirstOrDefault(ut => ut.UserId == userId && ut.TitleId == titleId);

            if (userTitle == null)
            {
                return NotFound();
            }

            var title = db.Titles.FirstOrDefault(t => t.Id == titleId);
            if (title == null)
            {
                return NotFound();
            }

            var viewModel = new EditTitleViewModel
            {
                UserId = userTitle.UserId,
                TitleId = userTitle.TitleId,
                Episodes = userTitle.Episodes,
                Score = userTitle.Score,
                MaxEpisodes = title.Episodes
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditTitleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userTitle = db.UserTitles
                .FirstOrDefault(ut => ut.UserId == model.UserId && ut.TitleId == model.TitleId);

            if (userTitle == null)
            {
                return NotFound();
            }

            var title = db.Titles.FirstOrDefault(t => t.Id == model.TitleId);
            if (title == null)
            {
                return NotFound();
            }

            if (model.Episodes > title.Episodes)
            {
                ModelState.AddModelError("Episodes", "The number of episodes cannot exceed the total episodes for the title.");
                return View(model);
            }

            userTitle.Status = model.Status;
            userTitle.Episodes = model.Episodes;
            userTitle.Score = model.Score;

            db.SaveChanges();

            return RedirectToAction("Index", new { userId = model.UserId });
        }


        public ActionResult Delete(int userId, int titleId)
        {
            var userTitle = db.UserTitles.FirstOrDefault(ut => ut.UserId == userId && ut.TitleId == titleId);

            if (userTitle == null)
            {
                return NotFound();
            }

            return View(userTitle);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int userId, int titleId)
        {
            var userTitle = db.UserTitles
                .FirstOrDefault(ut => ut.UserId == userId && ut.TitleId == titleId);

            if (userTitle == null)
            {
                return NotFound();
            }

            db.UserTitles.Remove(userTitle);

            var user = db.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null && user.TitlesSum > 0)
            {
                user.TitlesSum--;
            }

            db.SaveChanges();

            return RedirectToAction("Index", new { userId = userId });
        }


        public ActionResult AddTitle(int userId)
        {
            var titles = db.Titles.ToList();

            var userTitles = db.UserTitles.Where(ut => ut.UserId == userId).Select(ut => ut.TitleId).ToList();

            var availableTitles = titles.Where(t => !userTitles.Contains(t.Id)).ToList();

            var model = new AddTitleListViewModel
            {
                UserId = userId,
                Titles = availableTitles
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTitle(AddTitleViewModel model)
        {
            if (model.TitleId != null && model.UserId != null)
            {
                var userTitle = new UserTitle
                {
                    UserId = model.UserId,
                    TitleId = model.TitleId,
                    Status = model.Status,
                    Episodes = model.Episodes,
                    Score = model.Score
                };

                db.UserTitles.Add(userTitle);
                db.SaveChanges();

                var user = db.Users.FirstOrDefault(u => u.Id == model.UserId);
                if (user != null)
                {
                    user.TitlesSum += 1;
                    db.SaveChanges();
                }

                return RedirectToAction("AddTitle", new { userId = model.UserId });
            }

            return View(model);
        }

        public ActionResult AddTitleForm(int userId, int titleId)
        {
            var title = db.Titles.FirstOrDefault(t => t.Id == titleId);

            if (title == null)
            {
                return NotFound();
            }

            var model = new AddTitleViewModel
            {
                UserId = userId,
                TitleId = titleId,
                MaxEpisodes = title.Episodes
            };

            return View(model);
        }
    }
}