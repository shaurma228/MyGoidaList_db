using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyGoidaList.Models;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace MyGoidaList.Controllers
{
    public class CharacterController : Controller
    {
        private ApplicationDbContext db;

        public CharacterController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // Отображение списка персонажей
        public ActionResult Index()
        {
            var characters = db.Characters.Include(c => c.Title).ToList();
            return View(characters);
        }

        public ActionResult Create()
        {
            // Передаем список тайтлов в ViewBag
            ViewBag.TitleId = new SelectList(db.Titles, "Id", "Name");
            return View();
        }

        // Обработка создания нового персонажа
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Character character)
        {
            if (character.Name != null)
            {
                db.Characters.Add(character);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Передаем список тайтлов в случае ошибки
            ViewBag.TitleId = new SelectList(db.Titles, "Id", "Name", character.TitleId);
            return View(character);
        }

        // Страница редактирования персонажа
        public ActionResult Edit(int id)
        {
            // Получаем персонажа по id
            var character = db.Characters.Find(id);

            if (character == null)
            {
                return NotFound();
            }

            // Получаем все тайтлы для выпадающего списка
            ViewBag.Titles = new SelectList(db.Titles, "Id", "Name", character.TitleId);

            // Возвращаем представление с моделью
            return View(character);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id, Name, BirthDate, Description, TitleId, IsMainCharacter, Height, PopularityScore, ScreenTime, Rating")]Character character)
        {
            if (character.Name != null)
            {
                db.Entry(character).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Заполняем ViewBag.Titles снова, чтобы данные были при возврате из формы в случае ошибки
            ViewBag.Titles = new SelectList(db.Titles, "Id", "Name", character.TitleId);

            return View(character);
        }


        // Страница удаления персонажа
        public ActionResult Delete(int id)
        {
            var character = db.Characters.Find(id);
            if (character == null)
            {
                return NotFound();
            }
            return View(character);
        }

        // Подтверждение удаления персонажа
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var character = db.Characters.Find(id);
            db.Characters.Remove(character);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
