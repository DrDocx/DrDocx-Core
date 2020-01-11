using System;

public struct DTest
{
    public string TestName;
    public string Description;

    public int ID;

    public Dtest(string name, string desription)
    { 
        TestName = name;
        Description = description;
    }
}

public struct DTestGroup
{
    public string testGroupName;
    public string testGroupDescription;
    public list<DTest> testGroupTests;

    public int ID;

    // This can be used for when a test group doesn't have tests ready to populate it with
    public DTestGroup(string name, string description)
    {
        testGroupName = name;
        testGroupDescription = description;
        testGroupTests = null;
    }

    public DTestGroup(string name, string description, list<DTest> tests)
    {
        testGroupName = name;
        testGroupDescription = description;
        testGroupTests = tests;
    }
}