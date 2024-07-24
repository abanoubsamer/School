using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School.Data;
using School.Models;

namespace School.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly MyDbContext _context;

        public EnrollmentsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Enrollments
        public IActionResult Index(string? SearchResult)
        {
            var Enrollments = _context.Enrollments.Include(e => e.Course).Include(e => e.Student).ToList();
            if(!string.IsNullOrEmpty(SearchResult))
            {

                Enrollments = Enrollments.Where(name => name.Student.FirstName.Contains(SearchResult)).ToList();

            }



            return View(Enrollments);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound("Id is Invalid");
            }

            try
            {
                var student = await _context.Students
                                            .Include(s => s.Enrollments)
                                            .ThenInclude(e => e.Course)
                                            .FirstOrDefaultAsync(s => s.StudentId == id);

                if (student == null)
                {
                    return NotFound("Student Not Found");
                }

                return View(student);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }


        private void PopulateDropdownLists(Enrollment e=null)
        {
         

            ViewBag.CourseId = new SelectList(_context.Courses, "CourseId", "Tital",e?.CourseId);
            ViewBag.StudentId = new SelectList(_context.Students, "StudentId", "FirstName", e?.StudentId);
        }
        // GET: Enrollments/Create
        public IActionResult Create()
        {
            PopulateDropdownLists();
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Enrollment enrollment)
        {
                // Check if CourseId and StudentId are provided
               

                if (ModelState.IsValid)
                {
                    // If both CourseId and StudentId are provided and valid, proceed to save
                    _context.Enrollments.Add(enrollment);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }

            PopulateDropdownLists(enrollment);

            return View(enrollment);
        }


        // GET: Enrollments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            PopulateDropdownLists(enrollment);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Enrollments.Update(enrollment);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.EnrollmentId))
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
            PopulateDropdownLists(enrollment);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollments.Any(e => e.EnrollmentId == id);
        }

   
    }
}
