﻿using System;
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
    public class HomeViewModel
    {
        public IEnumerable<DrDocx_Core.Models.Patient> Patients;
        public IEnumerable<DrDocx_Core.Models.TestGroup> TestGroups;
        public IEnumerable<DrDocx_Core.Models.TestGroupTest> TestGroupTests;
        public IEnumerable<DrDocx_Core.Models.Test> Tests;
    }

    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;

        public HomeController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(new HomeViewModel {
                Patients = await _context.Patients.ToListAsync(),
                TestGroups = await _context.TestGroups.ToListAsync(),
                TestGroupTests = await _context.TestGroupTests.ToListAsync(),
                Tests = await _context.Tests.ToListAsync()
            } );
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}