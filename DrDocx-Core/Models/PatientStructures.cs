using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrDocx_Core.Models
{

    /// <summary>
    /// Summary description for Patient
    /// </summary>
    ///
    /// THIS WILL NOT WORK TIL EVERYTHINGS IN A PROJECT BECAUSE C# IS SPECIAL 
    public class Patient
    {
        public List<TestResultGroup> ResultGroups { get; set; }
        public string Diagnosis { get; set; }
        public string Name { get; set; }
        public string PreferredName { get; set; }
        public string Address { get; set; }
        public string Medications { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfTesting { get; set; }
        public string Notes { get; set; }
        public int MedicalRecordNumber { get; set; }
        public int AgeAtTesting { get; set; }
        public int Id { get; set; }
    }

    public class TestResultGroup
    {
        public TestGroup TestGroupInfo { get; set; }
        public List<TestResult> Tests { get; set; }

        public int Id { get; set; }
    }

    public class TestResult
    {
        public int RawScore { get; set; }
        public int ScaledScore { get; set; }
        public int ZScore { get; set; }
        public int Percentile { get; set; }
        public Test RelatedTest { get; set; }

        public int ID { get; set; }
    }
    public struct PatientViewModel
    {
        public Patient Patient;
        public List<DrDocx_Core.Models.TestGroup> TestGroups;
        public List<DrDocx_Core.Models.TestGroupTest> TestGroupTests;
        public List<DrDocx_Core.Models.TestResult> TestResults;
        public List<DrDocx_Core.Models.Test> Tests;
        public List<DrDocx_Core.Models.TestResultGroup> TestResultGroups;
    }
}
