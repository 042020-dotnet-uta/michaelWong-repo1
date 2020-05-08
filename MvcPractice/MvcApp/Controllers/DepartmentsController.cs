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
    public class DepartmentsController : Controller
    {
        private readonly MvcAppContext _context;

        public DepartmentsController(MvcAppContext mvcAppContext)
        {
            _context = mvcAppContext;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Departments.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Name")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            return View(department);
        }
    }
}