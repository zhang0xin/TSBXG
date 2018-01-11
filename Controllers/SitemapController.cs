using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSBXG.Models;

namespace TSBXG.Controllers
{
    public class SitemapController : Controller
    {
        private TSBXGContainer db = new TSBXGContainer();
        public ActionResult Index()
        {
            return View(db.Sitemap.ToList());
        }
        public ActionResult Details(string id = null)
        {
            Sitemap sitemap = db.Sitemap.Find(id);
            if (sitemap == null)
            {
                return HttpNotFound();
            }
            return View(sitemap);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Sitemap sitemap)
        {
            if (ModelState.IsValid)
            {
                db.Sitemap.Add(sitemap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sitemap);
        }
        public ActionResult Edit(string id = null)
        {
            Sitemap sitemap = db.Sitemap.Find(id);
            if (sitemap == null)
            {
                return HttpNotFound();
            }
            return View(sitemap);
        }
        [HttpPost]
        public ActionResult Edit(Sitemap sitemap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sitemap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sitemap);
        }
        public ActionResult Delete(string id = null)
        {
            Sitemap sitemap = db.Sitemap.Find(id);
            if (sitemap == null)
            {
                return HttpNotFound();
            }
            return View(sitemap);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            Sitemap sitemap = db.Sitemap.Find(id);
            db.Sitemap.Remove(sitemap);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}