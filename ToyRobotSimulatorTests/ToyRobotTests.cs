using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToyRobotSimulator;

namespace ToyRobotSimulatorTests
{
    /// <summary>
    /// Testing at this level is something that we might choose not to bother
    /// with at this level of complexity, in favour of just testing at the controller
    /// level.  However, as the object functionality grows, testing at the object
    /// level becomes more and more important.  Hence we start as we mean to go on
    /// and run tests at this level as well.
    /// </summary>
    [TestClass]
    public class ToyRobotTests
    {
        private static char[] CommaOrSpace = { ',', ' ' };
        private static char[] LineEnds = new char[] { '\r', '\n' };

        private ToyRobot robot;
        private TableTop tableTop;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            // set up objects for testing
            robot = new ToyRobot();
            tableTop = new TableTop { SizeX = 5, SizeY = 5 };
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

            // interpret user input - this avoids duplicating the XML 
            // test data in a more explicit form
            foreach (string cmd in allCommands.Split(LineEnds, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!String.IsNullOrWhiteSpace(cmd))
                {
                    string normalisedCmd = cmd.Trim().ToUpperInvariant();
                    if (normalisedCmd.StartsWith("PLACE"))
                    {
                        string args = normalisedCmd.Substring(5).Trim();
                        string[] tokens = args.Split(CommaOrSpace, StringSplitOptions.RemoveEmptyEntries);
                        if (tokens != null && tokens.Length == 3)
                        {
                            if (Int32.TryParse(tokens[0], out int x) &&
                                Int32.TryParse(tokens[1], out int y) &&
                                Enum.TryParse<Direction>(tokens[2].ToUpperInvariant(), out Direction facing))
                            {
                                robot.Place(x, y, facing, tableTop);
                            }
                        }
                    }
                    else if (normalisedCmd.StartsWith("LEFT"))
                    {
                        robot.Left();
                    }
                    else if (normalisedCmd.StartsWith("RIGHT"))
                    {
                        robot.Right();
                    }
                    else if (normalisedCmd.StartsWith("MOVE"))
                    {
                        robot.Move();
                    }
                }
            }

            // check output
            Assert.AreEqual(expectedOutput, robot.Report(), description);
        }
    }
}
