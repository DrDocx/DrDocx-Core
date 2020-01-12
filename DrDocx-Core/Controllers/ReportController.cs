using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DrDocx_Core.Models;
using DocumentFormat.OpenXml.Packaging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DrDocx_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public ReportController(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> GetPatientReport(int patientId)
        {
            if (!_context.Patients.Any(e => e.Id == patientId))
            {
                return NotFound();
            }
            var patient = await _context.Patients.FindAsync(patientId);
            await GeneratePatientReport(patient);
            return NotFound();
        }

        private async Task<WordprocessingDocument> GeneratePatientReport(Patient patient)
        {
            // Create local report directory
            var strippedPatientName = patient.Name.Replace(" ", "-");
            var workingDir = Directory.CreateDirectory(@"reports/patient-" + strippedPatientName);
            var tmpDir = workingDir.CreateSubdirectory("tmp");

            var reportSansVisualsTask = GenerateReportSansVisuals(patient);
            var testResultVisualizationsTask = GenerateTestVisualizations(patient, tmpDir);
            await Task.WhenAll(reportSansVisualsTask, testResultVisualizationsTask);
            var reportSansVisuals = await reportSansVisualsTask;

        }

        private async Task<WordprocessingDocument> GenerateReportSansVisuals(Patient patient)
        {

        }

        private async Task GenerateTestVisualizations(Patient patient, DirectoryInfo tmpDir)
        {
            var testResultGroupsJson = JsonSerializer.Serialize<List<TestResultGroup>>(patient.ResultGroups);
             System.IO.File.WriteAllText(tmpDir + "/test-data.json", testResultGroupsJson);
        }

        private async Task<WordprocessingDocument> CombineReportAndVisualizations(WordprocessingDocument reportSansVisuals, string filePath)
        {
            //TODO
        }
    }
}