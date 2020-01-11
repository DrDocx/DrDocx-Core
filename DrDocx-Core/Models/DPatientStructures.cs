using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DrDocx_Core.Models
{

    /// <summary>
    /// Summary description for DPatient
    /// </summary>
    ///
    /// THIS WILL NOT WORK TIL EVERYTHINGS IN A PROJECT BECAUSE C# IS SPECIAL 
    public class DPatient
    {
        public List<DTestResultGroup> ResultGroups { get; set; }
        public DPatientData Data { get; set; }
        public string Diagnosis { get; set; }
        public int Id { get; set; }
    }

    public struct DPatientData
    {
        public string Name { get; set; }
        public string PreferredName { get; set; }
        public string Address { get; set; }
        public string Medications { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfTesting { get; set; }
        public string Notes { get; set; }
        public int MedicalRecordNumber { get; set; }
        public int AgeAtTesting { get; set; }
    }

    public struct DTestResultGroup
    {
        public DTestGroup Data { get; set; }
        public List<DTestResult> Tests { get; set; }

        public int Id { get; set; }
    }

    public struct DTestResult
    {
        public int RawScore { get; set; }
        public int ScaledScore { get; set; }
        public int ZScore { get; set; }
        public int Percentile { get; set; }
        public DTest RelatedTest { get; set; }

        public int ID { get; set; }
    }
}
