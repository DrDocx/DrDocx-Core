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
    public class TestResultGroupsController : Controller
    {
        private readonly DatabaseContext _context;

        public TestResultGroupsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: TestResultGroups
        public async Task<IActionResult> Index()
        {
            return View(await _context.TestResultGroups.ToListAsync());
        }

        // GET: TestResultGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testResultGroup = await _context.TestResultGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testResultGroup == null)
            {
                return NotFound();
            }

            return View(testResultGroup);
        }

        // GET: TestResultGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TestResultGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] TestResultGroup testResultGroup, int testGroupId, int patientId)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testResultGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testResultGroup);
        }

        // GET: TestResultGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testResultGroup = await _context.TestResultGroups.FindAsync(id);
            if (testResultGroup == null)
            {
                return NotFound();
            }
            return View(testResultGroup);
        }

        // POST: TestResultGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] TestResultGroup testResultGroup)
        {
            if (id != testResultGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testResultGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestResultGroupExists(testResultGroup.Id))
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
            return View(testResultGroup);
        }

        // GET: TestResultGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testResultGroup = await _context.TestResultGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testResultGroup == null)
            {
                return NotFound();
            }

            return View(testResultGroup);
        }

        // POST: TestResultGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testResultGroup = await _context.TestResultGroups.FindAsync(id);
            _context.TestResultGroups.Remove(testResultGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestResultGroupExists(int id)
        {
            return _context.TestResultGroups.Any(e => e.Id == id);
        }
    }
}
