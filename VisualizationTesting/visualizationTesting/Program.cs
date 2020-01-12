using System;
using System.IO;
using System.Collections.Generic;
using DrDocx_Core.Models;
using System.Windows.Forms.DataVisualization.Charting;

namespace visualizationTesting
{
    class Program
    {
        public static List<TestResult> DemoGroup = new List<TestResult>(3);
        public static Test Test1 = new Test();
        public static Test Test2 = new Test();
        public static Test Test3 = new Test();

        public static TestResult Test1Result = new TestResult();
        public static TestResult Test2Result = new TestResult();
        public static TestResult Test3Result = new TestResult();

        public static Chart Table = new Chart();
        //public static Series MainSeries = new Series("Test Result Data");
        //public static DataPoint D1 = new DataPoint(MainSeries);
        //public static DataPoint D2 = new DataPoint(MainSeries);
        //public static DataPoint D3 = new DataPoint(MainSeries);

        private static void Main(string[] args)
        {
            Test1.Name = "Test 1";
            Test2.Name = "Test 2";
            Test3.Name = "Test 3";

            Test1Result.RelatedTest = Test1;
            Test2Result.RelatedTest = Test2;
            Test3Result.RelatedTest = Test3;
                
            Test1Result.ZScore = 2;
            Test2Result.ZScore = 1;
            Test3Result.ZScore = -1;

            DemoGroup.Add(Test1Result);
            DemoGroup.Add(Test2Result);
            DemoGroup.Add(Test3Result);

            Table.ChartAreas.Add(new ChartArea());
            Table.Series["Main"].Points.AddXY("Test 1", 1);
            Table.Series["Main"].Points.AddXY("Test 2", 3);

            string dest =
                "C:\\Users\\legom\\source\\repos\\DrDocx\\DrDocx-Core\\VisualizationTesting\\visualizationTesting\\Testing\\image.png";

            Table.SaveImage(dest, System.Drawing.Imaging.ImageFormat.Png);
        }

    }
}
