using Microsoft.AspNetCore.Mvc;

using School.Data;
using School.Models;
namespace School.Controllers
{
    public class CourseLevelController : Controller
    {

        private readonly MyDbContext _context = new MyDbContext();

        public IActionResult Index()
        {
            var levels = _context.CourseLevel.ToList();
            return View(levels);
        }


        public IActionResult Create()
        {

           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CourseLevel courseLevel)
        {
            if (ModelState.IsValid)
            {
                _context.CourseLevel.Add(courseLevel);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(courseLevel);
        }


        public IActionResult Edit(int?id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var level=_context.CourseLevel.Find(id);
            if(level == null)
            {
                return NotFound();
            }    

            return View(level);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? id, CourseLevel courseLevel)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
 

                    if (id!= courseLevel.CourseLevelId)
                    {
                        return NotFound();
                    }

               
                    _context.Update(courseLevel);
                    _context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
            
                catch (Exception e)
                {
                    // Handle other unexpected exceptions
                    return NotFound(e.Message);
                }
            }

            return View(courseLevel);
        }
        public IActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound("Id is Invaild");
            }

            try
            {
                var levels = _context.CourseLevel.Find(id);
                if (levels == null)
                {
                    return NotFound("Not Found Level");

                }
                return View(levels);
            }
            catch (Exception e) {

                return NotFound(e.Message);

            }

           
        }



        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("Id is Invaild");
            }

            try
            {
                var levels = _context.CourseLevel.Find(id);
                if (levels == null)
                {
                    return NotFound("Not Found Level");

                }

                _context.CourseLevel.Remove(levels);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {

                return NotFound(e.Message);

            }
        }

    }
}
