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

        [HttpGet]
        [ValidateAntiForgeryToken]
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

        private async Task GeneratePatientReport(Patient patient)
        {
            // Create local report directory
            var strippedPatientName = patient.Name.Replace(" ", "-");
            var workingDir = Directory.CreateDirectory(@"tmp/reports/patient-" + strippedPatientName);
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            var reportGenDirectory = projectDirectory + "/report-gen";
            var reportTemplatePath = reportGenDirectory + "/Report_Template.dotx";
            var reportFileName = "Patient-" + strippedPatientName + patient.DateOfTesting;
            var reportPath = workingDir.Name + reportFileName;
            var reportStaticPath = projectDirectory + "/wwwroot/reports" + reportFileName;
            var visualizationsDirectory = workingDir.CreateSubdirectory("visualizations");

            await Task.WhenAll(GenerateReportSansVisuals(patient, reportTemplatePath, reportPath), GenerateTestVisualizations(patient, workingDir, reportGenDirectory, visualizationsDirectory));
            await CombineReportAndVisualizations(reportPath, visualizationsDirectory.Name);
            bool readyToDelete = await ServeGeneratedReportStatically(reportPath, reportStaticPath);
            workingDir.Delete(readyToDelete);
        }

        private async Task GenerateReportSansVisuals(Patient patient, string templatePath, string reportPath)
        {
            Dictionary<string, string> templateReplacements = new Dictionary<string, string> 
            {
                { "NAME", patient.Name },
                { "PREFERRED_NAME", patient.PreferredName },
                { "MEDICATIONS", patient.Medications },
                { "ADDRESS", patient.Address },
                { "MEDICAL RECORD NUMBER", patient.MedicalRecordNumber.ToString() },
                { "AGE_AT_TESTING", "19" }, // Hardcoded as calculation method does not yet exist
                { "TEST_DATE", patient.DateOfTesting.ToString() }
            };

            await ReportGen.ReportGen.GenerateReport(patient, templatePath, reportPath, templateReplacements);
        }

        private async Task GenerateTestVisualizations(Patient patient, DirectoryInfo tmpDir, string reportGenDirectory, DirectoryInfo visualizationsDir)
        {
            var trgDict = new Dictionary<string, List<TestResult>>();
            foreach (var trGroup in patient.ResultGroups)
            {
                trgDict.Add(trGroup.TestGroupInfo.Name, trGroup.Tests);
            }
            var output = JsonSerializer.Serialize<Dictionary<string, List<TestResult>>>(trgDict);
            var resultJsonPath = tmpDir + "/test-result-data.json";
            System.IO.File.WriteAllText(resultJsonPath, output);

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = $"python chartGen.py {resultJsonPath} {visualizationsDir.FullName}";
            startInfo.WorkingDirectory = reportGenDirectory;
            process.StartInfo = startInfo;
            process.Start();
        }

        private async Task CombineReportAndVisualizations(string reportSansVisualsPath, string visualizationsDirectoryPath)
        {
            var imagesInVisualizationDir = Directory.GetFiles(visualizationsDirectoryPath, "*.png");

        }

        private async Task<bool> ServeGeneratedReportStatically(string reportGenPath, string reportStaticPath)
        {
            System.IO.File.Copy(reportGenPath, reportStaticPath);
            return true;
        }
    }
}