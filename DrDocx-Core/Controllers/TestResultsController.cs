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
    public class TestResultsController : Controller
    {
        private readonly DatabaseContext _context;

        public TestResultsController(DatabaseContext context)
        {
            _context = context;
        }

        // POST: TestResults/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RawScore,ScaledScore,ZScore,Percentile,ID")] TestResult testResult)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testResult);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testResult);
        }

        // POST: TestResults/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RawScore,ScaledScore,ZScore,Percentile,ID")] TestResult testResult)
        {
            if (id != testResult.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(testResult);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestResultExists(testResult.ID))
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
            return View(testResult);
        }

        // POST: TestResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testResult = await _context.TestResults.FindAsync(id);
            _context.TestResults.Remove(testResult);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestResultExists(int id)
        {
            return _context.TestResults.Any(e => e.ID == id);
        }
    }
}
