using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToyRobotSimulator;

namespace ToyRobotSimulatorTests
{
    /// <summary>
    /// Tests the overall inputs and outputs at the console controller level
    /// </summary>
    [TestClass]
    public class ConsoleControllerTests
    {
        private static char[] LineEnds = new char[] { '\r', '\n' };

        private StringWriter output;
        private StringBuilder outputText = new StringBuilder();
        private StringWriter error;
        private StringBuilder errorText = new StringBuilder();
        private ToyRobot robot;
        private ConsoleController controller;
        private TableTop tableTop;
        private Queue<string> userInputQueue = new Queue<string>();

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            // set up test inputs and outputs to simulate console session
            outputText.Clear();
            output = new StringWriter(outputText);
            errorText.Clear();
            error = new StringWriter(errorText);
            userInputQueue.Clear();

            // set up objects for testing
            robot = new ToyRobot();
            tableTop = new TableTop { SizeX = 5, SizeY = 5 };
            controller = new ConsoleController
            {
                Output = output,
                Error = error,
                ReadLine = ReadLine,
                Robot = robot,
                TableTop = tableTop
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            output.Dispose();
            error.Dispose();
        }

        [TestMethod]
        [DeploymentItem("testData.xml")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", 
            "|DataDirectory|\\testData.xml", 
            "Row", 
            DataAccessMethod.Sequential)]
        public void XmlTestCases()
        {
            // read data from row in data.xml
            string description = (string)TestContext.DataRow["Description"];
            string expectedOutput = (string)TestContext.DataRow["Expected"];
            string allCommands = (string)TestContext.DataRow["Commands"];

            // test reading as user input
            foreach (string cmd in allCommands.Split(LineEnds, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!String.IsNullOrWhiteSpace(cmd))
                {
                    userInputQueue.Enqueue(cmd);
                }
            }
            controller.Run();
            Assert.AreEqual(expectedOutput, outputText.ToString().TrimEnd(), description);

            // test reading from file
            outputText.Clear();
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, allCommands);
            controller.Run(new string[] { tempFile });
            Assert.AreEqual(expectedOutput, outputText.ToString().TrimEnd(), description);
        }

        private string ReadLine()
        {
            if (userInputQueue.Count > 0)
            {
                return userInputQueue.Dequeue();
            }
            return null;
        }
    }
}
