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
    public struct PatientViewModel
    {
        public Patient Patient;
        public List<DrDocx_Core.Models.TestGroup> TestGroups;
    }

    public class PatientsController : Controller
    {
        private readonly DatabaseContext _context;

        public PatientsController(DatabaseContext context)
        {
            _context = context;
        }

<<<<<<< HEAD
=======
        // GET: Patients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Patients.ToListAsync());
        }

>>>>>>> 2a1136ebb64ccbdd284662f6b8520a9a0b514624
        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(new PatientViewModel { Patient = patient, TestGroups = await _context.TestGroups.ToListAsync() });
        }

        // GET: Patients/AddTest/5
        public async Task<IActionResult> AddTest(int? id, string TestGroupName)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            //patient.ResultGroups

            return View(new PatientViewModel { Patient = patient, TestGroups = await _context.TestGroups.ToListAsync() });
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Diagnosis,Name,PreferredName,Address,Medications,DateOfBirth,DateOfTesting,Notes,MedicalRecordNumber,AgeAtTesting,Id")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return Redirect("Edit/" + patient.Id);
            }
            return View(patient);
        }


        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Diagnosis,Name,PreferredName,Address,Medications,DateOfBirth,DateOfTesting,Notes,MedicalRecordNumber,AgeAtTesting,Id")] Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View(new PatientViewModel { Patient = patient, TestGroups = await _context.TestGroups.ToListAsync() });
            }
            return View(new PatientViewModel { Patient = patient, TestGroups = await _context.TestGroups.ToListAsync() });
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
