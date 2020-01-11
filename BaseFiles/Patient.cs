using System;
using System.Serializable;

/// <summary>
/// Summary description for DPatient
/// </summary>
///
/// THIS WILL NOT WORK TIL EVERYTHINGS IN A PROJECT BECAUSE C# IS SPECIAL 
public class DPatient
{
    private list<TestResultGroup> ResultGroups;
    private PatientData Data;
    private int UniquePatientIdentifier;
    private string Diagnosis;

    public DPatient()
    { }
}

public struct DPatientData
{
    public string Name;
    public string PreferredName;
    public string address;
    public string medications;
    public DateTime DateOfBirth;
    public DateTime DateOfTesting;
    public string notes;
    public int MedicalRecordNumber;
    public int ageAtTesting;
}

public struct DTestResultGroup
{
    public TestGroup Data;
    public list<TestResult> tests;

    public int ID;
}

public struct DTestResult
{
    public int RawScore;
    public int ScaledScore;
    public int ZScore;
    public int Percentile;
    public Test relatedTest;

    public int ID;
}