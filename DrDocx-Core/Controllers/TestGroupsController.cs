using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DrDocx_Core;
using DrDocx_Core.Models;

namespace DrDocx_Core.Controllers
{
    public class TestGroupsController : Controller
    {
        private readonly DatabaseContext _context;

        public TestGroupsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: TestGroups
        public async Task<IActionResult> Index()
        {
            return View(await _context.TestGroups.ToListAsync());
        }

        // GET: TestGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testGroup = await _context.TestGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testGroup == null)
            {
                return NotFound();
            }

            return View(testGroup);
        }

        // GET: TestGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TestGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Id")] TestGroup testGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testGroup);
        }

        // GET: TestGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testGroup = await _context.TestGroups.FindAsync(id);
            if (testGroup == null)
            {
                return NotFound();
            }
            return View(testGroup);
        }

        // POST: TestGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,Id")] TestGroup testGroup)
        {
            if (id != testGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestGroupExists(testGroup.Id))
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
            return View(testGroup);
        }

        // GET: TestGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testGroup = await _context.TestGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testGroup == null)
            {
                return NotFound();
            }

            return View(testGroup);
        }

        // POST: TestGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testGroup = await _context.TestGroups.FindAsync(id);
            _context.TestGroups.Remove(testGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestGroupExists(int id)
        {
            return _context.TestGroups.Any(e => e.Id == id);
        }
    }
}
