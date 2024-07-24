using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using School.Data;
using School.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace School.Controllers
{
    public class StudentController : Controller
    {
        private MyDbContext _db;

        public StudentController(MyDbContext db) {
            _db = db;
        }

        //get
        [HttpGet]
        public IActionResult Index(string? SearchResult)
        {
            var std = _db.Students.ToList();
            if(!string.IsNullOrEmpty(SearchResult))
            {
                std=std.Where(s=>s.FirstName.Contains(SearchResult)).ToList();

            }

            return View(std);
        }
        //get
        [HttpGet]
        public IActionResult New()
        {
            return View();
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult New(Student Data, IFormFile imagePath)
        {
          
       
            if (ModelState.IsValid)
            {
                if (imagePath != null && imagePath.Length > 0)
                {
                    // Define the path to save the uploaded file
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Student");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imagePath.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Ensure the directory exists
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                     
                    // Save the file to the specified path
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        imagePath.CopyTo(fileStream);
                    }

                    // Update the data object with the file path
                    Data.ImagePath = "images/Student/" + uniqueFileName;
                }

                _db.Students.Add(Data);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(Data);
        }
    


        //get
        [HttpGet]
        
        public IActionResult Edite(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var student = _db.Students.Find(Id);
            if (student == null) return NotFound();
            return View(student);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edite(Student data, IFormFile? imagePath)
        {
          
            if (ModelState.IsValid)
            {

                var oldStudent = _db.Students.Find(data.StudentId);
                if (oldStudent == null) return NotFound();

                // If a new image is uploaded, process it
                if (imagePath != null && imagePath.Length > 0)
                {
                    // Define the path to save the image        //root                
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Student");
                
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imagePath.FileName);
                
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Ensure the directory exists // hna lw ale folder m4 mwgod h3mloh
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Save the image to the path
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        imagePath.CopyTo(fileStream);
                    }

                    // Delete the old image if it exists
                    if (!string.IsNullOrEmpty(oldStudent.ImagePath))
                    {
                        // hna ana bgyb ale path ale 2dem
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldStudent.ImagePath);
                     
                        // hna bolh lw mwgod ams7oh
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // 
                    // Update the ImagePath property of the student
                    oldStudent.ImagePath = "images/Student/" + uniqueFileName;
                }

                // Update other properties of the student
                oldStudent.FirstName = data.FirstName;
                oldStudent.LastName = data.LastName;
                oldStudent.StartDate = data.StartDate;

                _db.Students.Update(oldStudent);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {

                return View(data);
            }
        }

        // GET: Student/Delete/5
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = _db.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }
            _db.Students.Remove(student);
            _db.SaveChanges();

            // Optionally, delete associated image file if it exists
            if (!string.IsNullOrEmpty(student.ImagePath))
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", student.ImagePath);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            return RedirectToAction("Index");

        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound("id is Invalid");

            try
            {
                var student = await _db.Students.Include(e => e.Enrollments)
                    .ThenInclude(e=>e.Course)
                    .ThenInclude(e=>e.CourseLevel)
                    .FirstOrDefaultAsync(m => m.StudentId == id);
                
                if (student == null)
                    return NotFound("This student does not exist");

                return View(student);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }





    }
}
