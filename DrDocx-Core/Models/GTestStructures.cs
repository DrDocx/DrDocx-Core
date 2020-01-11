using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrDocx_Core.Models
{
    public struct DTest
    {
        public string TestName;
        public string Description;

        public int Id;

        //public DTest(string name, string description)
        //{
        //    TestName = name;
        //    Description = description;
        //}
    }

    public struct DTestGroup
    {
        public string Name;
        public string Description;
        public List<DTest> Tests;

        public int Id;

        // This can be used for when a test group doesn't have tests ready to populate it with
        //public DTestGroup(string name, string description, int id)
        //{
        //    Name = name;
        //    Description = description;
        //    Tests = null;
        //    Id = id;
        //}

        //public DTestGroup(string name, string description, List<DTest> tests, int id)
        //{
        //    Name = name;
        //    Description = description;
        //    Tests = tests;
        //    Id = id;
        //}
    }
}
