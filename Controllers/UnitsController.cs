using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ResultManagement.Models;
using ResultManagement.Models.Core;
using ResultManagement.ViewModel;

namespace ResultManagement.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UnitsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Units
        public ActionResult Index()
        {
            return View(db.Unit.ToList());
        }

        // GET: Units/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit unit = db.Unit.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }

        // GET: Units/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Units/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UnitCode,UnitTitle,UnitCordinator,PdfFile")] Unit unit)
        {
            if (ModelState.IsValid)
            {
                if (unit.PdfFile!=null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(unit.PdfFile.FileName);
                    unit.PdfTitle = fileName;
                    string extension = Path.GetExtension(unit.PdfFile.FileName);
                    if(extension.ToLower() != ".pdf")
                    {
                        ViewBag.PdfError = "File is not a pdf";
                        return View(unit);
                        
                    }
                    fileName = fileName + DateTime.Now.ToString("yyyymmddhhmmssfff") + extension;
                    unit.PdfPath = "~/Content/PDFs/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/PDFs/"), fileName);
                    unit.PdfFile.SaveAs(fileName); 
                }
                
                db.Unit.Add(unit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(unit);
        }

        // GET: Units/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit unit = db.Unit.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }


            UnitViewModel unitViewModel = new UnitViewModel
            {
                Id = unit.Id,
                UnitCode = unit.UnitCode,
                UnitTitle = unit.UnitTitle,
                UnitCordinator = unit.UnitCordinator,
                PdfTitle = unit.PdfTitle,
            };

            return View(unitViewModel);
        }

        // POST: Units/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UnitCode,UnitTitle,UnitCordinator,PdfFile")] UnitViewModel unitModel)
        {
            if (ModelState.IsValid)
            {
                Unit unit = db.Unit.Find(unitModel.Id);
                if (unitModel.PdfFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(unitModel.PdfFile.FileName);
                    unitModel.PdfTitle = fileName;
                    string extension = Path.GetExtension(unitModel.PdfFile.FileName);
                    if (extension.ToLower() != ".pdf")
                    {
                        ViewBag.PdfError = "File is not a pdf";
                        return View(unitModel);

                    }
                    fileName = fileName + DateTime.Now.ToString("yyyymmddhhmmssfff") + extension;
                    unitModel.PdfPath = "~/Content/PDFs/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/PDFs/"), fileName);
                    unitModel.PdfFile.SaveAs(fileName);

                    unit.PdfFile = unitModel.PdfFile;
                    unit.PdfTitle = unitModel.PdfTitle;
                    unit.PdfPath = unitModel.PdfPath;
                }


                unit.UnitCode = unitModel.UnitCode;
                unit.UnitTitle = unitModel.UnitTitle;
                unit.UnitCordinator = unitModel.UnitCordinator;




                db.Entry(unit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(unitModel);
        }

        // GET: Units/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit unit = db.Unit.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }

        // POST: Units/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Unit unit = db.Unit.Find(id);
            db.Unit.Remove(unit);
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
