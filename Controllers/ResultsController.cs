using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ResultManagement.Helpers;
using ResultManagement.Models;
using ResultManagement.Models.Core;
using ResultManagement.ViewModel;

namespace ResultManagement.Controllers
{
    [Authorize(Roles="Manager")]
    public class ResultsController : Controller
    {
        private static List<string> studentIds = new List<string>()
        {
            "150408011","150408013","140805003","130492020",
            "150308011","150508014","140805004","130492075",
            "150408011","150408013","140805012","130492030",
            };
        public ResultsController()
        {
            
        }
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Results
        public ActionResult Index(string searchString, string semesterSearch, string unitCodeSearch)
        {
            var result = (from s in db.Result
                          select s).Include(s=>s.Unit);

            List<string> semesters = new List<string>(){
            "All","1","2"
        };

            List<string> unitCodes = new List<string>(){
            "All"
        };

            List<string> _unitCodes = (from s in db.Unit
                                       select s.UnitCode.ToString()).ToList();

            foreach(var item in _unitCodes)
            {
                unitCodes.Add(item);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                result = result.Where(s => s.StudentId.ToString().ToLower().Contains(searchString.ToLower())
                                       || s.UnitCode.ToString().Contains(searchString)
                                       || s.Semester.ToString().ToLower().Contains(searchString.ToLower())
                                         || s.Year.ToString().ToLower().Contains(searchString.ToLower())
                                       );
            }
            if (!(semesterSearch == "All" || string.IsNullOrEmpty(semesterSearch)))
            {
                result = result.Where(s => s.Semester.ToString() == semesterSearch);
            }
            if (!(unitCodeSearch == "All" || (string.IsNullOrEmpty(unitCodeSearch))))
            {
                result = result.Where(s => s.UnitCode.ToString() == unitCodeSearch);
            }


            ViewBag.Search = searchString;

            ViewBag.SelectedSemester = semesterSearch;
            semesters.Remove(semesterSearch);
            ViewBag.SemesterSearch = semesters;

            ViewBag.SelectedUnitCode = unitCodeSearch;
            unitCodes.Remove(unitCodeSearch);
            ViewBag.UnitCodesSearch = unitCodes;

            ViewBag.CountResult = result.ToList().Count();

            double total = 0;
            foreach(var item in result.ToList())
            {
                double assignment1 = item.AssessmentScore1;
                double assignment2 = item.AssessmentScore2;
                double exam = item.ExamScore;
                total += (assignment1 + assignment2 + exam);
            }

            ViewBag.Average = result.ToList().Count <= 0 ? 0 : (total) / result.ToList().Count();
            return View(result.ToList());
        }

        // GET: Results/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Result.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // GET: Results/Create
        public ActionResult Create()
        {
            ViewBag.UnitCodes = (from s in db.Unit
                                select s.UnitCode).ToList();

       //     List<string> studentId = new List<string>()
       //{
       //     "150408011","150408013","140805003","130492020",
       //     "150308011","150508014","140805004","130492075",
       //     "150408011","150408013","140805012","130492030",
       // };

            ViewBag.StudentIds = studentIds;
            return View();
        }

        // POST: Results/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UnitCode,StudentId,Semester,Year,AssessmentScore1,AssessmentScore2,ExamScore,ImgFile")] Result result)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (result.ImgFile != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(result.ImgFile.FileName);
                        string extension = Path.GetExtension(result.ImgFile.FileName);
                        if (!(extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg"))
                        {
                            ViewBag.ImgError = "File is not an image";
                            return View(result);
                        }
                        fileName = fileName + DateTime.Now.ToString("yyyymmddhhmmssfff") + extension;
                        result.ImgPath = "~/Content/IMAGES/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Content/IMAGES/"), fileName);
                        result.ImgFile.SaveAs(fileName);
                    }

                    result.UnitId = db.Unit.Where(r => r.UnitCode == result.UnitCode).FirstOrDefault().Id;
                    result.Unit = db.Unit.Where(r => r.UnitCode == result.UnitCode).FirstOrDefault();

                    db.Result.Add(result);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return View(result);
        }

        // GET: Results/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Result.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }

            ResultViewModel viewModel = new ResultViewModel
            {
                Id = result.Id,
                UnitCode = result.UnitCode,
                //StudentId = result.StudentId,
                Year = result.Year,
                Semester = result.Semester,
                AssessmentScore1 =result.AssessmentScore1,
                AssessmentScore2 =result.AssessmentScore2,
                ExamScore = result.ExamScore,
                ImgPath=result.ImgPath,
            };
            List<string> _unitCodes = (from s in db.Unit
                                       select s.UnitCode).ToList();
            ViewBag.SelectedUnitCode = result.UnitCode;
            _unitCodes.Remove(result.UnitCode);
            ViewBag.UnitCodes = _unitCodes;

            ViewBag.SelectedStudent = result.StudentId.ToString();
            List<string> _studentIds = studentIds;
            _studentIds.Remove(result.StudentId.ToString());
            ViewBag.StudentIds = _studentIds;
            return View(viewModel);
        }

        // POST: Results/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UnitCode,StudentId,Semester,Year,AssessmentScore1,AssessmentScore2,ExamScore,ImgFile")] ResultViewModel resultModel)
        {
            Result result = db.Result.Find(resultModel.Id);
            if (ModelState.IsValid)
            {
                try
                {
                    if (resultModel.ImgFile != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(resultModel.ImgFile.FileName);
                        string extension = Path.GetExtension(resultModel.ImgFile.FileName);
                        if (!(extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg"))
                        {
                            ViewBag.ImgError = "File is not an image";
                            return View(resultModel);
                        }
                        fileName = fileName + DateTime.Now.ToString("yyyymmddhhmmssfff") + extension;
                        resultModel.ImgPath = "~/Content/IMAGEs/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Content/IMAGEs/"), fileName);
                        resultModel.ImgFile.SaveAs(fileName);

                        result.ImgPath = resultModel.ImgPath;
                        result.ImgFile = resultModel.ImgFile;
                    }

                    result.UnitId = db.Unit.Find(result.UnitCode).Id;
                    result.Unit = db.Unit.Find(result.UnitCode);
                    result.UnitCode = resultModel.UnitCode;
                    result.StudentId = resultModel.StudentId;
                    result.Semester = resultModel.Semester;
                    result.Year = resultModel.Year;
                    result.AssessmentScore1 = resultModel.AssessmentScore1;
                    result.AssessmentScore2 = resultModel.AssessmentScore2;
                    result.ExamScore = resultModel.ExamScore;




                    db.Entry(result).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return View(result);
        }

        // GET: Results/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Result.Find(id);

            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // POST: Results/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                
                Result result = db.Result.Find(id);

                db.Result.Remove(result);
                db.SaveChanges();
                string path = Request.MapPath(result.ImgPath);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                
            }catch(Exception ex)
            {

            }
            return RedirectToAction("Index");
        }

        public ActionResult StudentResults(int studentId = 0)
        {
            var result = (from s in db.Result
                          select s).Include(s => s.Unit).Where(s=>s.StudentId==studentId);

            ViewBag.StudentId = studentId;

            ViewBag.CountResult = result.ToList().Count();

            double total = 0;
            foreach (var item in result.ToList())
            {
                double assignment1 = item.AssessmentScore1;
                double assignment2 = item.AssessmentScore2;
                double exam = item.ExamScore;
                total += (assignment1 + assignment2 + exam);
            }

            ViewBag.Average = result.ToList().Count <= 0 ? 0 : (total) / result.ToList().Count();
            return View(result.ToList());
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
