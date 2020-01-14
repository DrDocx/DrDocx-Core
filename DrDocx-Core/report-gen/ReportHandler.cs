using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrDocx_Core.Models;
using System.IO;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace DrDocx_Core.ReportGen
{
    public class ReportHandler
    {
        private readonly DatabaseContext _context;

        public ReportHandler(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<string> GeneratePatientReport(Patient patient)
        {
            // Create local report directory
            var strippedPatientName = patient.Name.Replace(" ", "-");
            var workingDir = Directory.CreateDirectory(@"./tmp/reports/patient-" + strippedPatientName);
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/DrDocx-Core/DrDocx-Core";
            var reportGenDirectory = projectDirectory + "/report-gen";
            var reportTemplatePath = reportGenDirectory + "/Report_Template.dotx";
            var reportFileName = $"Patient-{strippedPatientName}.docx";
            var reportPath = workingDir.Name + reportFileName;
            var reportStaticPath = projectDirectory + "/wwwroot/reports" + reportFileName;
            var visualizationsDirectory = reportGenDirectory + "/visualizations";

            await Task.WhenAll(GenerateReportSansVisuals(patient, reportTemplatePath, reportPath), GenerateTestVisualizations(patient, workingDir, reportGenDirectory, visualizationsDirectory));
            await CombineReportAndVisualizations(reportPath, visualizationsDirectory);
            bool readyToDelete = await ServeGeneratedReportStatically(reportPath, reportStaticPath);
            //workingDir.Delete(readyToDelete);
            return reportStaticPath;
        }

        private async Task GenerateReportSansVisuals(Patient patient, string templatePath, string reportPath)
        {
            var v = new PatientViewModel
            {
                Patient = patient,
                Tests = await _context.Tests.ToListAsync(),
                TestGroupTests = await _context.TestGroupTests.ToListAsync(),
                TestGroups = await _context.TestGroups.ToListAsync(),
                TestResultGroups = await _context.TestResultGroups.ToListAsync(),
                TestResults = await _context.TestResults.ToListAsync(),
            };
            var testGroups = patient.ResultGroups;
            Dictionary<string, string> templateReplacements = new Dictionary<string, string>
            {
                { "NAME", patient.Name },
                { "PREFERRED_NAME", patient.PreferredName },
                { "MEDICATIONS", patient.Medications },
                { "ADDRESS", patient.Address },
                { "MEDICAL_RECORD_NUMBER", patient.MedicalRecordNumber.ToString() },
                { "AGE_AT_TESTING", "19" }, // Hardcoded as calculation method does not yet exist
                { "TEST_DATE", patient.DateOfTesting.ToString() },
                { "DOB", patient.DateOfBirth.ToString() }
            };

            await global::ReportGen.ReportGen.GenerateReport(patient, templatePath, reportPath, templateReplacements, testGroups);
        }

        private async Task GenerateTestVisualizations(Patient patient, DirectoryInfo tmpDir, string reportGenDirectory, string visualizationsDir)
        {
            var v = new PatientViewModel
            {
                Patient = patient,
                Tests = await _context.Tests.ToListAsync(),
                TestGroupTests = await _context.TestGroupTests.ToListAsync(),
                TestGroups = await _context.TestGroups.ToListAsync(),
                TestResultGroups = await _context.TestResultGroups.ToListAsync(),
                TestResults = await _context.TestResults.ToListAsync(),
            };
            var resultGroups = patient.ResultGroups;

            var trgDict = new Dictionary<string, List<TestResult>>();
            foreach (var trGroup in resultGroups)
            {
                trgDict.Add(trGroup.TestGroupInfo.Name, trGroup.Tests);
            }
            var serializeOptions = new JsonSerializerOptions { MaxDepth = 10 };
            var output = JsonSerializer.Serialize<Dictionary<string, List<TestResult>>>(trgDict, serializeOptions);
            var resultJsonPath = tmpDir.FullName + "/test-result-data.json";
            System.IO.File.WriteAllText(resultJsonPath, output);

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "python.exe";
            startInfo.WorkingDirectory = reportGenDirectory;
            startInfo.Arguments = $"chartGen.py {resultJsonPath} {visualizationsDir}";
            process.StartInfo = startInfo;
            process.Start();
        }

        private async Task CombineReportAndVisualizations(string reportSansVisualsPath, string visualizationDir)
        {
            var imagesInVisualizationDir = Directory.GetFiles(visualizationDir, "*.png");
            await global::ReportGen.ReportGen.CombineReportAndVisualizations(reportSansVisualsPath, imagesInVisualizationDir);
        }

        private async Task<bool> ServeGeneratedReportStatically(string reportGenPath, string reportStaticPath)
        {
            if (System.IO.File.Exists(reportStaticPath))
            {
                System.IO.File.Delete(reportStaticPath);
            }
            System.IO.File.Copy(reportGenPath, reportStaticPath);

            return true;
        }

    }
}
