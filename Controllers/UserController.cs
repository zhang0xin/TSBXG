using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSBXG.Models;
using PagedList;
using PagedList.Mvc;
using TSBXG.Helper;

namespace TSBXG.Controllers
{
    public class UserController : Controller
    {
        private TSBXGContainer db = new TSBXGContainer();


        public ActionResult Logon()
        {
            if (Session["CurrentUser"] != null)
            {
                return RedirectToAction("Manage", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Logon(string username, string password, string redirectUrl = null)
        {
            if (!LogonHelper.Logon(username, password))
            {
                ModelState.AddModelError("allMessage", "用户名不存在或密码错误");
                return  View();
            }
            return RedirectToAction("Manage", "Home");
        }
        public ActionResult Logout()
        {
            Session["CurrentUser"] = null;
            return RedirectToAction("Logon", "User");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string userName, string password, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
            {
                ModelState.AddModelError("newPassword", "新密码不能为空");
                return View();
            }

            var user = db.User.FirstOrDefault(u => u.UserName == userName && u.Password == password);
            if (user == null)
            {
                ModelState.AddModelError("userName", "用户名不存在或密码错误");
                return View();
            }

            user.Password = newPassword;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Permission(int id = 0)
        {
            ViewBag.User = db.User.Find(id);
            if (ViewBag.User == null)
            {
                return RedirectToAction("Index");
            }

            return View(SitemapManager.GetPermissions(id));
        }

        [HttpPost]
        public ActionResult Permission(int id, string[] access)
        {
            var user = db.User.Find(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            var userPermissions = user.Permision.ToArray();
            foreach (var p in userPermissions)
            {
                string idWithCategory = p.Tag + ":" + p.SitemapId;
                if (access.Contains(idWithCategory)) continue;
                db.Permision.Remove(p);
            }
            var sitemapIds = user.Permision.Select(p => p.Tag + ":" + p.SitemapId);
            foreach (var a in access)
            {
                var strs = a.Split(':');
                string category = strs[0];
                string sitemapId = strs[1];
                if (sitemapIds.Contains(a)) continue;
                user.Permision.Add(new Permision() { UserId = user.Id, SitemapId = sitemapId, Tag = category });
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Index(string queryUserName = "", string queryFullName = "", int page = 1)
        {
            ViewBag.QueryUserName = queryUserName;
            ViewBag.QueryFullName = queryFullName;

            IQueryable<User> query = db.User;
            if (!string.IsNullOrWhiteSpace(queryUserName))
            {
                query = query.Where(u => u.UserName.Contains(queryUserName));
            }
            if (!string.IsNullOrWhiteSpace(queryFullName))
            {
                query = query.Where(u => u.FullName.Contains(queryFullName));
            }

            return View(query.OrderByDescending(u => u.Id).ToPagedList(page, 10));
        }
        public ActionResult Details(int id = 0)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var existUser = db.User.FirstOrDefault(u => u.UserName == user.UserName);
                if (existUser == null)
                {
                    db.User.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("UserName", "用户名已经存在，请使用其他用户名");
                }
            }

            return View(user);
        }
        public ActionResult Edit(int id = 0)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }
        public ActionResult Delete(int id = 0)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
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