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
    public class OutLinkController : Controller
    {
        private TSBXGContainer db = new TSBXGContainer();

        public ActionResult _FlowTabLinks()
        {
            return PartialView(db.OutLink.OrderBy(l=>l.DispalyIndex));
        }
        public ActionResult _TabLinks()
        {
            return PartialView(db.OutLink.OrderBy(l=>l.DispalyIndex));
        }
        public ActionResult Index()
        {
            return View(db.OutLink.ToList());
        }

        public ActionResult ViewLinkList()
        {
            return View(db.OutLink.ToList());
        }
        public ActionResult Details(int id = 0)
        {
            OutLink outlink = db.OutLink.Find(id);
            if (outlink == null)
            {
                return HttpNotFound();
            }
            return View(outlink);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(OutLink outlink)
        {
            if (ModelState.IsValid)
            {
                db.OutLink.Add(outlink);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(outlink);
        }
        public ActionResult Edit(int id = 0)
        {
            OutLink outlink = db.OutLink.Find(id);
            if (outlink == null)
            {
                return HttpNotFound();
            }
            return View(outlink);
        }
        [HttpPost]
        public ActionResult Edit(OutLink outlink)
        {
            if (ModelState.IsValid)
            {
                db.Entry(outlink).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(outlink);
        }
        public ActionResult Delete(int id = 0)
        {
            OutLink outlink = db.OutLink.Find(id);
            if (outlink == null)
            {
                return HttpNotFound();
            }
            return View(outlink);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            OutLink outlink = db.OutLink.Find(id);
            db.OutLink.Remove(outlink);
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