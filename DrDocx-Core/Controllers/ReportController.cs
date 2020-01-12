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
using ReportGen;

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
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            var reportGenDirectory = projectDirectory + "/report-gen";
            var reportTemplatePath = reportGenDirectory + "/Report_Template.dotx";
            var reportPath = tmpDir.Name + "Patient-" + strippedPatientName + patient.DateOfTesting;
            await Task.WhenAll(GenerateReportSansVisuals(patient, reportTemplatePath, reportPath), GenerateTestVisualizations(patient, tmpDir));

        }

        private async Task GenerateReportSansVisuals(Patient patient, string templatePath, string reportPath)
        {
            await ReportGen.ReportGen.GenerateReport(patient, templatePath, reportPath);
        }

        private async Task GenerateTestVisualizations(Patient patient, DirectoryInfo tmpDir)
        {
            var trgDict = new Dictionary<string, List<TestResult>>();
            foreach (var trGroup in patient.ResultGroups)
            {
                trgDict.Add(trGroup.TestGroupInfo.Name, trGroup.Tests);
            }
            var output = JsonSerializer.Serialize<Dictionary<string, List<TestResult>>>(trgDict);
            System.IO.File.WriteAllText(tmpDir + "/test-result-data.json", output);
            tmpDir.CreateSubdirectory("visualizations");
        }

        private async Task CombineReportAndVisualizations(string reportSansVisualsPath, string visualizationsDirectoryPath)
        {
            var imagesInVisualizationDir = Directory.GetFiles(visualizationsDirectoryPath, "*.png");

        }
    }
}