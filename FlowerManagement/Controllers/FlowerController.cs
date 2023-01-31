using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FlowerManagement.Controllers
{
    public class FlowerController : Controller
    {
        //GET: /Flower/
        FlowerDBDataContext _context = new FlowerDBDataContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string rootPath = Server.MapPath("~/");
                string fileName = System.IO.Path.GetFileName(file.FileName);
                string destFile = System.IO.Path.Combine(rootPath, "Assets\\images\\" + fileName);
                file.SaveAs(destFile);
            }
            return View();
        }


        public ActionResult ListFlowers() {
            var flowers = _context.Flowers.ToList();
            return Json(flowers, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Id")] Flower flower)
        {
            if (ModelState.IsValid)
            {
                string rootPath = Server.MapPath("~/");
                string fileName = System.IO.Path.GetFileName(flower.ImagePath);

                flower.ImagePath = fileName;
                _context.Flowers.InsertOnSubmit(flower);
                _context.SubmitChanges();
            }
            return Json(flower, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var flower = _context.Flowers.ToList().Find(f => f.Id == id);
            if (flower != null)
            {
                _context.Flowers.DeleteOnSubmit(flower);
                _context.SubmitChanges();
            }
            return Json(flower, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Get(int id)
        {
            var flower = _context.Flowers.ToList().Find(f => f.Id == id);

            string rootPath = Server.MapPath("~/");
            string fileName = System.IO.Path.GetFileName(flower.ImagePath);
            string destFile = System.IO.Path.Combine(rootPath, "Assets\\images\\" + fileName);
            flower.ImagePath = destFile;

            return Json(flower, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Update(Flower flower)
        {
            if (ModelState.IsValid)
            {
                string rootPath = Server.MapPath("~/");
                string fileName = System.IO.Path.GetFileName(flower.ImagePath);
                flower.ImagePath = fileName;

                Flower f = (from p in _context.Flowers
                                  where p.Id == flower.Id
                                  select p).SingleOrDefault();

                f.FlowerName = flower.FlowerName;
                f.ImagePath = flower.ImagePath;
                f.description = flower.description;
                _context.SubmitChanges();

            }
            return Json(flower, JsonRequestBehavior.AllowGet);
        }
    }
}
