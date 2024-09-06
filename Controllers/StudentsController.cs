using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Techzar.Models;

namespace Techzar.Controllers
{
    public class StudentsController : Controller
    {
        private readonly PostgresContext _context;

        public StudentsController(PostgresContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var postgresContext = _context.Students.Include(s => s.Course).Include(s => s.Department);
            return View(await postgresContext.ToListAsync());
        }

        // GET: Students/Details/5
       /* public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Course)
                .Include(s => s.Department)
                .FirstOrDefaultAsync(m => m.Studentid == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        } */

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["Courses"] = new SelectList(_context.Courses, "Courseid", "Coursename");
            ViewData["Departments"] = new SelectList(_context.Departments, "Departmentid", "Departmentname"); // Display department names
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Studentid,Studentname,Departmentid,Courseid")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Courseid"] = new SelectList(_context.Courses, "Courseid", "Courseid", student.Courseid);
            ViewData["Departmentid"] = new SelectList(_context.Departments, "Departmentid", "Departmentid", student.Departmentid);
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["Courseid"] = new SelectList(_context.Courses, "Courseid", "Courseid", student.Courseid);
            ViewData["Departmentid"] = new SelectList(_context.Departments, "Departmentid", "Departmentid", student.Departmentid);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Studentid,Studentname,Departmentid,Courseid")] Student student)
        {
            if (id != student.Studentid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Studentid))
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
            ViewData["Courseid"] = new SelectList(_context.Courses, "Courseid", "Courseid", student.Courseid);
            ViewData["Departmentid"] = new SelectList(_context.Departments, "Departmentid", "Departmentid", student.Departmentid);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Course)
                .Include(s => s.Department)
                .FirstOrDefaultAsync(m => m.Studentid == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Studentid == id);
        }

        [HttpGet]
        public IActionResult GetCoursesByDepartment(int departmentId)
        {
            var courses = _context.Courses
                                  .Where(c => c.Departmentid == departmentId)
                                  .Select(c => new SelectListItem
                                  {
                                      Value = c.Courseid.ToString(),
                                      Text = c.Coursename
                                  })
                                  .ToList();
            return Json(courses);
        }
    }
}
