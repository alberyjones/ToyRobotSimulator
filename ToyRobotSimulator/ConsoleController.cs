using System;
using System.IO;

namespace ToyRobotSimulator
{
    /// <summary>
    /// Class models the input and output for use directly in the console application,
    /// implemented as a discrete controller class for ease of unit testing.
    /// </summary>
    public class ConsoleController
    {
        /// <summary>
        /// The robot currently in use
        /// </summary>
        public ToyRobot Robot { get; set; }

        /// <summary>
        /// The table top the robot should act on
        /// </summary>
        public TableTop TableTop { get; set; }

        /// <summary>
        /// The output stream to write to
        /// </summary>
        public TextWriter Output { get; set; }

        /// <summary>
        /// The error stream to write to
        /// </summary>
        public TextWriter Error { get; set; }

        /// <summary>
        /// A function to read an input line
        /// </summary>
        public Func<string> ReadLine { get; set; }

        /// <summary>
        /// Runs the controller
        /// </summary>
        public void Run(string[] args = null)
        {
            // try processing arguments as a file
            if (args == null || args.Length == 0 || !ProcessFile(args[0]))
            {
                // if that was unsuccessful, assume command line operation
                // handle user input until ProcessCommand returns false
                string nextCommand = ReadLine();
                while (ProcessCommand(nextCommand))
                {
                    nextCommand = ReadLine();
                }
            }
        }

        /// <summary>
        /// Processes input from a file
        /// </summary>
        /// <param name="file">The path to a text file containing commands to execute</param>
        /// <returns>True if the specified file was found and processed</returns>
        public bool ProcessFile(string file)
        {
            if (!String.IsNullOrEmpty(file) && File.Exists(file))
            {
                try
                {
                    string[] lines = File.ReadAllLines(file);
                    if (lines != null)
                    {
                        foreach (string line in lines)
                        {
                            if (!String.IsNullOrWhiteSpace(line))
                            {
                                ProcessCommand(line);
                            }
                        }
                    }
                    return true;
                }
                catch (Exception e) 
                {
                    // catching all exceptions like this is questionable in production code
                    // but, on balance, whatever issue occurs we want to handle it and just 
                    // report the details to the user
                    Error.WriteLine("Error processing file: " + file + "; " + e.Message);
                }
            }
            return false;
        }

        /// <summary>
        /// Finds the definition for an individual command and invokes the
        /// appropriate function.
        /// </summary>
        /// <param name="command">The command to be processed as entered by the user or read from file</param>
        /// <returns>True if execution should continue</returns>
        public bool ProcessCommand(string command)
        {
            if (CommandDefinitions.Find(command, out CommandDefinition definition, out string args))
            {
                definition.Invoke(this.Robot, this, args);
                return true;
            }
            return false;
        }
    }
}
