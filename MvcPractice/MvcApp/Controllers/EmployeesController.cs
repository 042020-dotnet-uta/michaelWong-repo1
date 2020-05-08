  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcApp.DataAccess;
using MvcApp.Domain;

namespace MvcApp
{
    public class EmployeesController : Controller
    {
        private readonly MvcAppContext _context;
        public EmployeesController(MvcAppContext mvcAppContext)
        {
            this._context = mvcAppContext;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.Include(e => e.Department).ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.DepartmentId = new SelectList(await _context.Departments.ToListAsync(), nameof(Department.Id), nameof(Department.Name));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName, LastName, DepartmentId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.Department = _context.Departments.Find(employee.DepartmentId);
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.DepartmentId = new SelectList(_context.Departments, nameof(Department.Id), nameof(Department.Name));
            return View(employee);
        }
    }
}