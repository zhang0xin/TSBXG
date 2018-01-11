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
using System.IO;
using TSBXG.Helper;

namespace TSBXG.Controllers
{
    public class DocumentController : Controller
    {
        private TSBXGContainer db = new TSBXGContainer();

        public ActionResult _PopLogonPartial(string actionViewName)
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Logon(string username, string password, string logonUrl = null, string redirectUrl = null)
        {
            if (LogonHelper.Logon(username, password))
            {
                return Redirect(redirectUrl);
            }
            else
            {
                ModelState.AddModelError("allMessage", "用户名不存在或密码错误");
                return Redirect(logonUrl);
            }
        }

        #region For Html Pages
        public ActionResult ViewHtml(string category)
        {
            return View(db.Document.FirstOrDefault(d => d.Category == category));
        }
        public ActionResult EditHtml(string category)
        {
            return View(db.Document.FirstOrDefault(d => d.Category == category));
        }
        [HttpPost]
        public ActionResult EditHtml(Document documentData)
        {
            return View(UpdateDocument(documentData, "Html"));
        }
        #endregion

        #region For Pdf Pages
        public ActionResult ViewPdfList(string category, int page = 1)
        {
            var docs = db.Document
                .Where(d => d.Category == category)
                .OrderByDescending(d => d.UpdateTime)
                .ToPagedList(page, 20);
            return View(docs);
        }

        public ActionResult ViewPdfContent(int Id, string category, int page = 1)
        {
            return ViewDocument(Id, category, page, "ViewPdfContent", "ViewPdfList");
        }
        #endregion

        #region For File Pages
        public ActionResult ViewFileList(string category, int page = 1)
        {
            var docs = db.Document
                .Where(d => d.Category == category)
                .OrderByDescending(d => d.UpdateTime)
                .ToPagedList(page, 20);
            return View(docs);
        }
        public ActionResult DownloadFile(int id, string category, int page = 1)
        {
            if (!CanAccessView(category))
            {
                var redirectUrl = Request.RawUrl;
                var logonUrl = string.Format("/Document/ViewFileList?category={0}&page={1}", category, page);

                return RedirectToAction("ViewFileList", new { category, requireLogon = true, logonUrl, redirectUrl });
            }

            ResponseFile(db.Document.Find(id));
            return ViewFileList(category, page);
        }
        public FileContentResult FileById(int Id)
        {
            Document doc = db.Document.Find(Id);
            if (doc == null) return null;
            return File(doc.AttachFile, doc.AttachFileType);
        }
        public ActionResult ListFile(string category, string queryTitle = "", int page = 1)
        {
            ViewBag.QueryTitle = queryTitle;
            ViewBag.Sitemap = db.Sitemap.Find(category);

            IQueryable<Document> query = db.Document.Where(d => d.Category == category);
            if (!string.IsNullOrWhiteSpace(queryTitle))
            {
                query = query.Where(d => d.Title.Contains(queryTitle));
            }
            return View(query.OrderByDescending(d => d.UpdateTime).ToPagedList(page, 10));
        }
        public ActionResult CreateFile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateFile(Document document, HttpPostedFileBase File)
        {
            if (ModelState.IsValid)
            {
                if (File != null) SetFile(document, File);
                SetClientInfo(document);
                db.Document.Add(document);

                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;
                return RedirectToAction("ListFile", new { category = document.Category });
            }

            return View(document);
        }
        public ActionResult EditFile(string category, int? id)
        {
            IEnumerable<Document> docs = db.Document;
            if (!string.IsNullOrWhiteSpace(category))
                docs = docs.Where(d => d.Category == category);
            if (id.HasValue)
                docs = docs.Where(d => d.Id == id);

            var document = docs.FirstOrDefault();
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }
        [HttpPost]
        public ActionResult EditFile(Document documentData, HttpPostedFileBase File)
        {
            Document document = UpdateDocumentWithFile(documentData, File, "Title", "AttachFileUrl");
            return RedirectToAction("ListFile", new { category = document.Category });
        }
        public ActionResult DeleteFile(int id = 0)
        {
            Document documnet = db.Document.Find(id);
            if (documnet == null)
            {
                return HttpNotFound();
            }
            return View(documnet);
        }
        [HttpPost, ActionName("DeleteFile")]
        public ActionResult DeleteFileConfirmed(int id, string category)
        {
            Document documnet = db.Document.Find(id);
            db.Document.Remove(documnet);
            db.SaveChanges();
            return RedirectToAction("ListFile", new { category });
        }
        #endregion

        #region For Image Pages
        public FileContentResult ImageById(int Id, double percent = 1.0)
        {
            Document doc = db.Document.FirstOrDefault(d => d.Id == Id);
            if (doc == null) return null;
            return File(doc.ImageData, doc.ImageMimeType);
        }
        public FileContentResult ImageByCategory(string category, int index = 0, double percent = 1.0)
        {
            Document doc = db.Document.Where(d => d.Category == category).ToArray()[index];
            if (doc == null) return null;
            return File(doc.ImageData, doc.ImageMimeType);
        }
        public ActionResult ViewImageList(string category)
        {
            return View(db.Document.Where(d => d.Category == category).ToArray());
        }
        public ActionResult ListImage(string category)
        {
            ViewBag.Sitemap = db.Sitemap.Find(category);
            return View(db.Document.Where(d => d.Category == category));
        }
        public ActionResult CreateImage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateImage(Document document, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null) SetImage(document, ImageFile);
                SetClientInfo(document);
                db.Document.Add(document);
                db.SaveChanges();
                return RedirectToAction("ListImage", new { category = document.Category });
            }

            return View(document);
        }
        public ActionResult EditImage(string category, int? id)
        {
            IEnumerable<Document> docs = db.Document;
            if (!string.IsNullOrWhiteSpace(category))
                docs = docs.Where(d => d.Category == category);
            if (id.HasValue)
                docs = docs.Where(d => d.Id == id);

            var document = docs.FirstOrDefault();
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }
        [HttpPost]
        public ActionResult EditImage(int id, string Title, HttpPostedFileBase ImageFile)
        {
            Document document = db.Document.Find(id);
            document.Title = Title;

            SetClientInfo(document);
            if (ImageFile != null && !string.IsNullOrEmpty(Request["saveImage"]))
            {
                SetImage(document, ImageFile);
            }
            else if (!string.IsNullOrEmpty(Request["clearImage"]))
            {
                document.ImageMimeType = null;
                document.ImageData = null;
            }
            db.SaveChanges();

            return RedirectToAction("ListImage", new { category = Request["category"] });
        }
        public ActionResult DeleteImage(int id = 0)
        {
            Document documnet = db.Document.Find(id);
            if (documnet == null)
            {
                return HttpNotFound();
            }
            return View(documnet);
        }
        [HttpPost, ActionName("DeleteImage")]
        public ActionResult DeleteConfirmed(int id, string category)
        {
            Document documnet = db.Document.Find(id);
            db.Document.Remove(documnet);
            db.SaveChanges();
            return RedirectToAction("ListImage", new { category });
        }
        #endregion

        #region For News Pages
        public ActionResult ViewNewsContent(int Id, string category, int page = 1)
        {
            var redirectUrl = string.Format("/Document/ViewNewsContent/{0}?category={1}&page={2}", Id, category, page);
            var logonUrl = string.Format("/Document/ViewNewsList/{0}?category={1}&page={2}", Id, category, page);
            if (!CanAccessView(category))
            {
                return RedirectToAction("ViewNewsList", new { category, requireLogon = true, logonUrl, redirectUrl });
            }

            var document = db.Document.Find(Id);
            document.Hits++;
            db.SaveChanges();

            return View(document);
        }
        public bool CanAccessView(string category)
        {
            var sitemap = db.Sitemap.Find(category);
            if (sitemap.ViewNeedPermission != "1") return true;

            var currUser = Session["CurrentUser"] as User;
            if (currUser == null) return false;

            currUser = db.User.Find(currUser.Id); //防止user对象被释放不能使用permission属性，重新从数据库获取
            var permission = currUser.Permision.FirstOrDefault(p => p.SitemapId == category && p.Tag == "前台");
            if (permission == null && currUser.UserName != "admin") return false;

            return true;
        }
        public ActionResult ViewNewsList(string category, int page = 1)
        {
            ViewBag.Sitemap = db.Sitemap.Find(category);
            var docs = db.Document.Where(d => d.Category == category)
                .OrderByDescending(d => d.UpdateTime).ToPagedList(page, 20);
            return View(docs);
        }
        public ActionResult ListNews(string category, string queryTitle = "", int page = 1)
        {
            ViewBag.QueryTitle = queryTitle;
            ViewBag.Sitemap = db.Sitemap.Find(category);

            IQueryable<Document> query = db.Document.Where(d => d.Category == category);
            if (!string.IsNullOrWhiteSpace(queryTitle))
            {
                query = query.Where(d => d.Title.Contains(queryTitle));
            }
            return View(query.OrderByDescending(d => d.UpdateTime).ToPagedList(page, 10));
        }
        public ActionResult CreateNews()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateNews(Document document, HttpPostedFileBase ImageFile)
        {
            if (!ModelState.IsValid) return View(document);

            SetClientInfo(document);
            if (ImageFile != null) SetImage(document, ImageFile);
            db.Document.Add(document);
            db.SaveChanges();
            return RedirectToAction("ListNews", new { category = document.Category });
        }
        public ActionResult EditNews(string category, int? id)
        {
            var document = FindDocument(category, id);
            if (document == null) return HttpNotFound();
            return View(document);
        }
        [HttpPost]
        public ActionResult EditNews(Document documentData, HttpPostedFileBase ImageFile)
        {
            if (ImageFile != null) SetImage(documentData, ImageFile);
            Document document = UpdateDocument(documentData,
                "Title", "Category", "Html", "Tag", "ImageMimeType", "ImageData");

            return RedirectToAction("ListNews", new { category = documentData.Category });
        }
        public ActionResult DeleteNews(int id = 0)
        {
            Document documnet = db.Document.Find(id);
            if (documnet == null)
            {
                return HttpNotFound();
            }
            return View(documnet);
        }
        [HttpPost, ActionName("DeleteNews")]
        public ActionResult DeleteNewsConfirmed(int id)
        {
            Document documnet = db.Document.Find(id);
            var category = documnet.Category;
            db.Document.Remove(documnet);
            db.SaveChanges();
            return RedirectToAction("ListNews", new { category });
        }
        #endregion

        #region For Doc Pages

        public ActionResult ListDoc(string category, string queryTitle = "", int page = 1)
        {
            ViewBag.QueryTitle = queryTitle;
            ViewBag.Sitemap = db.Sitemap.Find(category);

            IQueryable<Document> query = db.Document.Where(d => d.Category == category);
            if (!string.IsNullOrWhiteSpace(queryTitle))
            {
                query = query.Where(d => d.Title.Contains(queryTitle));
            }
            return View(query.OrderByDescending(d => d.UpdateTime).ToPagedList(page, 10));
        }
        public ActionResult CreateDoc()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateDoc(Document documnet, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                documnet.CreateTime = DateTime.Now;
                documnet.UpdateTime = documnet.CreateTime;
                if (ImageFile != null)
                {
                    SetImage(documnet, ImageFile);
                }
                db.Document.Add(documnet);
                db.SaveChanges();
                return RedirectToAction("ListDoc", new { category = documnet.Category });
            }

            return View(documnet);
        }
        public ActionResult EditDoc(string category, int? id)
        {
            IEnumerable<Document> docs = db.Document;
            if (!string.IsNullOrWhiteSpace(category))
                docs = docs.Where(d => d.Category == category);
            if (id.HasValue)
                docs = docs.Where(d => d.Id == id);

            var document = docs.FirstOrDefault();
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }
        [HttpPost]
        public ActionResult EditDoc(int id, string Title, string Category, string Html)
        {
            Document documnet = db.Document.Find(id);
            documnet.Title = Title;
            documnet.Category = Category;
            documnet.Html = Html;
            documnet.UpdateTime = DateTime.Now;
            db.SaveChanges();

            return RedirectToAction("ListDoc", new { category = documnet.Category });
        }
        public ActionResult DeleteDoc(int id = 0)
        {
            Document documnet = db.Document.Find(id);
            if (documnet == null)
            {
                return HttpNotFound();
            }
            return View(documnet);
        }
        [HttpPost, ActionName("DeleteDoc")]
        public ActionResult DeleteDocConfirmed(int id, string category)
        {
            Document documnet = db.Document.Find(id);
            db.Document.Remove(documnet);
            db.SaveChanges();
            return RedirectToAction("ListDoc", new { category });
        }
        #endregion

        #region For Default

        public ActionResult Index()
        {
            return View(db.Document.ToList());
        }

        public ActionResult Details(int id = 0)
        {
            Document documnet = db.Document.Find(id);
            if (documnet == null)
            {
                return HttpNotFound();
            }
            return View(documnet);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Document documnet)
        {
            if (ModelState.IsValid)
            {
                db.Document.Add(documnet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(documnet);
        }

        public ActionResult Edit(int id = 0)
        {
            Document documnet = db.Document.Find(id);
            if (documnet == null)
            {
                return HttpNotFound();
            }
            return View(documnet);
        }
        [HttpPost]
        public ActionResult Edit(Document documnet, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    documnet.ImageMimeType = ImageFile.ContentType;
                    documnet.ImageData = new byte[ImageFile.ContentLength];
                    ImageFile.InputStream.Read(documnet.ImageData, 0, ImageFile.ContentLength);
                }
                else
                {
                    //var tempDoc = db.Document.FirstOrDefault(d => d.Id == documnet.Id);
                    //documnet.ImageData = tempDoc.ImageData;
                    //documnet.ImageMimeType = tempDoc.ImageMimeType;
                }
                db.Entry(documnet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(documnet);
        }

        public ActionResult Delete(int id = 0)
        {
            Document documnet = db.Document.Find(id);
            if (documnet == null)
            {
                return HttpNotFound();
            }
            return View(documnet);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Document documnet = db.Document.Find(id);
            db.Document.Remove(documnet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        #endregion

        private void ResponseFile(Document document)
        {
            byte[] bytes = document.AttachFile;
            string filename = document.AttachFileName;
            if (!string.IsNullOrWhiteSpace(document.AttachFileUrl))
            {
                filename = document.AttachFileUrl.Split('/').Last();
                bytes = FileToBytes(document.AttachFileUrl);
            }
            ResponseFile(filename, bytes);
        }
        private void ResponseFile(string fileName, byte[] fileData)
        {
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition",
                "attachment;  filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            Response.BinaryWrite(fileData);
            Response.Flush();
            Response.End();
        }

        private byte[] FileToBytes(string url)
        {
            var fileStream = new FileStream(Server.MapPath(url), FileMode.Open);
            byte[] bytes = new byte[(int)fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            return bytes;
        }

        private ActionResult ViewDocument(int id, string category, int page, string resultView, string loginView)
        {
            if (!CanAccessView(category))
            {
                var loginToAccessUrl = string.Format("/Document/{0}/{1}?category={2}&page={3}", resultView, id, category, page);
                return RedirectToAction(loginView, new { category, requireLogon = true, redirectUrl = loginToAccessUrl });
            }

            var document = db.Document.Find(id);
            document.Hits++;
            db.SaveChanges();

            return View(resultView, document);
        }

        private Document UpdateDocumentWithFile(Document documentData, HttpPostedFileBase file, params string[] fields)
        {
            Document document = db.Document.Find(documentData.Id);
            if (file != null) SetFile(document, file);
            DataTransHelper.CopyProperties(document, documentData, fields);
            SetClientInfo(document);

            db.SaveChanges();
            return document;
        }
        private Document UpdateDocumentWithImage(Document documentData, HttpPostedFileBase imageFile, params string[] fields)
        {
            Document document = db.Document.Find(documentData.Id);
            if (imageFile != null) SetImage(document, imageFile);
            DataTransHelper.CopyProperties(document, documentData, fields);
            SetClientInfo(document);

            db.SaveChanges();
            return document;
        }
        private Document UpdateDocument(Document documentData, params string[] fields)
        {
            Document document = db.Document.Find(documentData.Id);
            DataTransHelper.CopyProperties(document, documentData, fields);
            SetClientInfo(document);

            db.SaveChanges();
            return document;
        }

        private void SetClientInfo(Document document)
        {
            if (document.Id == 0)  // create document id is 0
            {
                document.CreateTime = DateTime.Now;
                document.CreateUser = GetCurrentUserFullName();
                document.CreateIp = Request.UserHostAddress;
            }

            document.UpdateTime = DateTime.Now;
            document.UpdateUser = GetCurrentUserFullName();
            document.UpdateIp = Request.UserHostAddress;
        }
        private string GetCurrentUserFullName()
        {
            if (Session["CurrentUser"] != null)
            {
                var user = (User)Session["CurrentUser"];
                return user.FullName;
            }
            return "";
        }
        private Document FindDocument(string category, int? id)
        {
            IEnumerable<Document> docs = db.Document;
            if (!string.IsNullOrWhiteSpace(category))
                docs = docs.Where(d => d.Category == category);
            if (id.HasValue)
                docs = docs.Where(d => d.Id == id);

            return docs.FirstOrDefault();
        }
        private void SetImage(Document document, HttpPostedFileBase ImageFile)
        {
            document.ImageMimeType = ImageFile.ContentType;
            document.ImageData = new byte[ImageFile.ContentLength];
            ImageFile.InputStream.Read(document.ImageData, 0, ImageFile.ContentLength);
        }
        private void SetFile(Document document, HttpPostedFileBase attachFile)
        {
            document.AttachFileName = attachFile.FileName;
            document.AttachFileType = attachFile.ContentType;
            document.AttachFile = new byte[attachFile.ContentLength];
            attachFile.InputStream.Read(document.AttachFile, 0, attachFile.ContentLength);
        }
    }
}