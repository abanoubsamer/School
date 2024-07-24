using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School.Data;
using School.Models;

namespace School.Controllers
{
    public class CoursesController : Controller
    {
        private readonly MyDbContext _context = new MyDbContext(); // hna ana bft7 ale conection by ale data base



        private void PopulateDropdownLists(CourseLevel e = null)
        {   
            ViewBag.CourseLevel = new SelectList(_context.CourseLevel, "CourseLevelId", "Level", e?.CourseLevelId);

        }

        //public CoursesController(MyDbContext context)
        //{
        //    _context = context;
        //}

        // GET: Courses
        //public IActionResult Index()
        //{

        //     PopulateDropdownLists();
        //    var courses = _context.Courses
        //        .Include(c => c.CourseLevel) // Include CourseLevel navigation property
        //        .ToList();
        //    return View(courses);
        //}

        public IActionResult Index(string? SearchResult)
        {

            PopulateDropdownLists();
            var courses = _context.Courses
                .Include(c => c.CourseLevel) // Include CourseLevel navigation property
                .ToList();

            if (!string.IsNullOrEmpty(SearchResult))
            {
                courses = courses.Where(c=>c.Tital.Contains(SearchResult)).ToList();
            }


            return View(courses);
        }
      


        public IEnumerable<Course> GetCoursesByTitle(string title)
        {
            var courses = _context.Courses.FromSqlRaw("SELECT * FROM dbo.FunGetCourseById({0})", title).ToList();
            return courses;
        }
        public IActionResult Test(string title)
        {
            return View(GetCoursesByTitle(title));
        }
        

        // GET: Courses/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = _context.Courses
            .Include(c => c.CourseLevel) // Include CourseLevel navigation property
            .FirstOrDefault(c => c.CourseId == id);
            if (course == null)// hna lw ml2t4 asln ale course
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            PopulateDropdownLists();
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        ///////////////////////////////////////// hna ana lw m4 3awz ab3t ale object colh fa bolh htb3t ale atribute dh bs
        public  IActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Courses.Add(course);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }


            PopulateDropdownLists(course.CourseLevel);
            return View(course);
        }

        // GET: Courses/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound("Invalid id");
            }

            var course = _context.Courses.Find(id);
            if (course == null)
            {
                return NotFound("Course does not exist");
            }

            // Assuming PopulateDropdownLists populates dropdowns related to CourseLevel
            PopulateDropdownLists(course.CourseLevel);

            return View(course);
        }


        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Edit(int id,  Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                     _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateDropdownLists(course.CourseLevel);
            return View(course);
        }

        // GET: Courses/Delete/5
        public  IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course =  _context.Courses
                .Find(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var course = _context.Courses.Find(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // POST: Courses/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteAjax(int id)
        {
            try
            {
                var course = _context.Courses.Find(id);
                if (course == null)
                {
                    return Json(new { success = false, message = "Course not found." });
                }

                _context.Courses.Remove(course);
                _context.SaveChanges();
               
                return Json(new { success = true, message = "Course deleted successfully." });
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                return Json(new { success = false, message = "An error occurred while deleting the course." });
            }
        }



        // POST: Courses/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteMultiAjax(int[] ids)
        {
            try
            {
                // Find all courses with the given IDs
                var courses = _context.Courses.Where(c => ids.Contains(c.CourseId)).ToList();

                if (courses == null || courses.Count == 0)
                {
                    return Json(new { success = false, message = "Courses not found." });
                }

                // Remove all found courses
                _context.Courses.RemoveRange(courses);
                _context.SaveChanges();

                return Json(new { success = true, message = "Courses deleted successfully." });
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                return Json(new { success = false, message = "An error occurred while deleting the courses." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetSearchVlueAjax(string SearchResult)
        {
            try
            {
                // Find all courses where the title contains the search result
                var courses = _context.Courses
                    .Where(c => c.Tital.Contains(SearchResult))
                    .ToList();

                return Json(courses);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                return Json(new { success = false, message = "An error occurred while searching for the courses." });
            }
        }
        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }

        protected override void Dispose(bool disposing)
        {// hna ana bolh lw ale disposing true y3ne mfto7 
            if (disposing) {
                _context.Dispose();// hna bolh 2fl ale conction lma t5ls
            }
            base.Dispose(disposing);
        }

    }
}
