using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimePunch.Models;

namespace TimePunch.Controllers
{
    public class HomeController : Controller
    {
        private TimePunchContext db = new TimePunchContext();

        // GET: TimeEntries
        public ActionResult Index()
        {
            IEnumerable<TimeEntry> table = db.TimeEntries.ToList();

            if (table.Any())
            {
                Session["lastRowId"] = table.Last().ID;
            }
            else
            {
                Session["lastRowId"] = 0;
            }

            return View(table);
        }

        // "New" button pressed
        public ActionResult NewEntry()
        {
            TimeEntry timeEntry = new TimeEntry();

            // End previous entry if still open (EndTime == null)
            End();

            timeEntry.StartTime = DateTime.Now;
            timeEntry.EndTime = null;

            db.TimeEntries.Add(timeEntry);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // "End" button pressed
        public ActionResult EndEntry()
        {
            End();

            db.SaveChanges();
            
            return RedirectToAction("Index");
        }


        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        // Helper function to test for null EndTime in last table entry and set it to current time if null
        public void End()
        {
            int lastRowId = (int)Session["lastRowId"];
            TimeEntry lastRow = (from e in db.TimeEntries
                                 where e.ID == lastRowId
                                 select e).FirstOrDefault();

            if (lastRow != null)
            {
                if (lastRow.EndTime == null)
                {
                    lastRow.EndTime = DateTime.Now;
                }
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeEntry timeEntry = db.TimeEntries.Find(id);
            if (timeEntry == null)
            {
                return HttpNotFound();
            }
            return View(timeEntry);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,StartTime,EndTime,Notes")] TimeEntry timeEntry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timeEntry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(timeEntry);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeEntry timeEntry = db.TimeEntries.Find(id);
            if (timeEntry == null)
            {
                return HttpNotFound();
            }
            return View(timeEntry);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TimeEntry timeEntry = db.TimeEntries.Find(id);
            db.TimeEntries.Remove(timeEntry);
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