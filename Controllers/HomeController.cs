using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSBXG.Models;

namespace TSBXG.Controllers
{
    public class HomeController : Controller
    {
        private TSBXGContainer db = new TSBXGContainer();
        public ActionResult Index()
        {
            ViewBag.LunXianTuPian = db.Document.Where(d => d.Category == "lunxiantupian").ToArray();
            ViewBag.XinWen = db.Document.Where(d => d.Category == "xinwen").OrderByDescending(d => d.UpdateTime).Take(10).ToArray();
            ViewBag.ChanPinZhanShi = db.Document.Where(d => d.Category == "chanpinzhanshi" && d.Tag == "ShowImage").Take(4);
            return View();
        }
        public ActionResult Manage()
        {
            if (Session["CurrentUser"] == null) return RedirectToAction("Logon", "User");
            return View();
        }

        public ActionResult _ProductShowPartial()
        {
            var products = db.Document.Where(d => d.Category == "chanpinzhanshi" && d.Tag == "ShowImage").ToArray();
            return PartialView(products);
        }

        public ActionResult _TabContentPartial(ICollection<TabContent> tabs)
        {
            foreach (var tab in tabs)
            {
                tab.Text = db.Sitemap.Find(tab.Category).Text;
                tab.Content = db.Document
                    .Where(d => d.Category == tab.Category)
                    .OrderByDescending(o => o.UpdateTime)
                    .ToList();
            }
            return PartialView(tabs);
        }

        public ActionResult _CarouselPartial(string category, bool taged = true, bool withLink = true)
        {
            var list = db.Document.Where(d => d.Category == category);
            if (taged)
            {
                list = list.Where(d => d.Tag == "ShowImage");
            }
            ViewBag.WithLink = withLink;
            return PartialView(list.ToArray());
        }

        public ActionResult _HomeMenuPartial()
        {
            return PartialView(TSBXG.Models.SitemapManager.GetViewSitemapTree());
        }

        public ActionResult _ImageNewsPartial()
        {
            var xinWenTuPian = db.Document.Where(
                d => d.Category == "xinwen" && d.Tag == "ShowImage").OrderByDescending(d => d.UpdateTime).ToArray();

            return PartialView(xinWenTuPian);
        }

        public ActionResult _TabNewsPartial(string category, bool withDate=true, bool withTime=true)
        {
            ViewBag.WithDate = withDate;
            ViewBag.WithTime = withTime;
            return PartialView(GetSubOrCurrCategoryDocuments(category));
        }

        public ActionResult _TabNewsOnCategorysPartial(string[] categorys, bool withDate = true, bool withTime = true)
        {
            ViewBag.WithDate = withDate;
            ViewBag.WithTime = withTime;

            return PartialView("_TabNewsPartial", GetCategoryDocuments(categorys));
        }

        public ActionResult _TabFilePartial(string category, bool withDate = true, bool withTime = true)
        {
            return PartialView(GetSubOrCurrCategoryDocuments(category));
        }

        private Document[] GetSubOrCurrCategoryDocuments(string category)
        {
            var categorys = db.Sitemap.Where(s => s.ParentId == category).OrderBy(i => i.DisplayIndex).Select(t => t.Id).ToList();
            if (categorys.Count == 0) categorys.Add(category);
            return GetCategoryDocuments(categorys);
        }
        private Document[] GetCategoryDocuments(ICollection<string> categorys)
        {
            var categoryDocs = db.Document.Where(
                d => categorys.Contains(d.Category)).OrderByDescending(o => o.UpdateTime).Take(10).ToArray();
            return categoryDocs;
        }

        public ActionResult _NoticePartial()
        {
            var gongGaoLan = db.Document.Where(
                d => d.Category == "gonggaolan").OrderByDescending(d => d.UpdateTime).Take(10).ToArray();

            return PartialView(gongGaoLan);
        }

        public ActionResult _BreadcrumbPartial(int id=0)
        {
            var list = SitemapManager.GetParentViewSitemapList(Request["category"]);
            var doc = db.Document.Find(id);
            if (doc != null)
            { 
                list.Add(new Sitemap(){
                    ViewUrl = string.Format("/Document/ViewNewsContent/{0}?category={1}", doc.Id, doc.Category),
                    Text = doc.Title
                });
            }
            return PartialView(list);
        }

        public ActionResult Test(HttpPostedFileBase ImageData)
        {
            return View();
        }
    }
}
