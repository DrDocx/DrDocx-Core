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
    public class PatientInfosController : Controller
    {
        private readonly DatabaseContext _context;

        public PatientInfosController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: PatientInfoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.PatientInfo.ToListAsync());
        }

        // GET: PatientInfos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientInfo = await _context.PatientInfo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patientInfo == null)
            {
                return NotFound();
            }

            return View(patientInfo);
        }

        // GET: PatientInfos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PatientInfos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,PreferredName,Address,Medications,DateOfBirth,DateOfTesting,Notes,MedicalRecordNumber,AgeAtTesting")] PatientInfo patientInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patientInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patientInfo);
        }

        // GET: PatientInfos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientInfo = await _context.PatientInfo.FindAsync(id);
            if (patientInfo == null)
            {
                return NotFound();
            }
            return View(patientInfo);
        }

        // POST: PatientInfos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PreferredName,Address,Medications,DateOfBirth,DateOfTesting,Notes,MedicalRecordNumber,AgeAtTesting")] PatientInfo patientInfo)
        {
            if (id != patientInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientInfoExists(patientInfo.Id))
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
            return View(patientInfo);
        }

        // GET: PatientInfos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientInfo = await _context.PatientInfo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patientInfo == null)
            {
                return NotFound();
            }

            return View(patientInfo);
        }

        // POST: PatientInfos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patientInfo = await _context.PatientInfo.FindAsync(id);
            _context.PatientInfo.Remove(patientInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientInfoExists(int id)
        {
            return _context.PatientInfo.Any(e => e.Id == id);
        }
    }
}
