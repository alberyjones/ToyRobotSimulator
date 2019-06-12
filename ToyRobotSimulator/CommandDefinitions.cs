using System;
using System.Collections.Generic;

namespace ToyRobotSimulator
{
    /// <summary>
    /// Encapsulates the definition of commands that can be invoked and allows
    /// lookup based on supplied command text. This is arguably overkill for the
    /// limited number of commands we currently need to support, but is put in place 
    /// with future extensibility in mind. As the number of commands increases, a
    /// developer should only need to add code to the PopulateCommandDefinitions
    /// method (and any supporting methods in the robot class itself) and this will
    /// be reflected in the usage message and command line / file support for the command.
    /// </summary>
    public class CommandDefinitions
    {
        private static char[] CommaOrSpace = { ',', ' ' };
        private static Dictionary<string, CommandDefinition> definitions;

        private CommandDefinitions() {} // Private constructor - static methods only

        /// <summary>
        /// The full set of command definitions available, keyed by name.
        /// </summary>
        public static IDictionary<string, CommandDefinition> Definitions
        {
            get
            {
                if (definitions == null)
                {
                    // one-time load of definitions
                    definitions = new Dictionary<string, CommandDefinition>();
                    PopulateCommandDefinitions();
                }
                return definitions;
            }
        }

        /// <summary>
        /// Finds the appropriate command definition for the supplied
        /// command and also splits out the arguments from the command
        /// line.
        /// </summary>
        /// <param name="command">The command to be processed as entered by the user or read from file</param>
        /// <param name="definition">The appropriate matching definition or null</param>
        /// <param name="args">The arguments supplied after the command or null</param>
        /// <returns>True if a matching definition was found</returns>
        public static bool Find(string command, out CommandDefinition definition, out string args)
        {
            definition = null;
            args = null;
            if (!String.IsNullOrEmpty(command))
            {
                string key;
                string trimmed = command.Trim();
                int firstSpace = trimmed.IndexOf(' ');
                if (firstSpace < 0)
                {
                    key = trimmed.ToUpperInvariant();
                }
                else
                {
                    args = trimmed.Substring(firstSpace + 1);
                    key = trimmed.Substring(0, firstSpace).ToUpperInvariant();
                }
                return Definitions.TryGetValue(key, out definition);
            }
            return false;
        }

        /// <summary>
        /// Hard coded population of commands - this could be read from elsewhere
        /// if needed in future - e.g. configuration, data store, etc.
        /// </summary>
        private static void PopulateCommandDefinitions()
        {
            // PLACE command
            AddCommandDefinition(
                "PLACE", "<XPos>,<YPos>,<Facing>",
                (robot, controller, args) =>
                {
                    if (robot != null && controller != null && !String.IsNullOrEmpty(args))
                    {
                        string[] tokens = args.Split(CommaOrSpace, StringSplitOptions.RemoveEmptyEntries);
                        if (tokens != null && tokens.Length == 3)
                        {
                            if (Int32.TryParse(tokens[0], out int x) &&
                                Int32.TryParse(tokens[1], out int y) &&
                                Enum.TryParse<Direction>(tokens[2].ToUpperInvariant(), out Direction facing))
                            {

                                robot.Place(x, y, facing, controller.TableTop);
                            }
                        }
                    }
                    return false;
                });

            // LEFT command
            AddCommandDefinition(
                "LEFT", "",
                (robot, controller, args) =>
                {
                    if (robot != null)
                    {
                        robot.Left();
                    }
                    return false;
                });

            // RIGHT command
            AddCommandDefinition(
                "RIGHT", "",
                (robot, controller, args) =>
                {
                    if (robot != null)
                    {
                        robot.Right();
                    }
                    return false;
                });

            // MOVE command
            AddCommandDefinition(
                "MOVE", "",
                (robot, controller, args) =>
                {
                    if (robot != null)
                    {
                        robot.Move();
                    }
                    return false;
                });

            // REPORT command
            AddCommandDefinition(
                "REPORT", "",
                (robot, controller, args) =>
                {
                    if (robot != null && controller != null)
                    {
                        string report = robot.Report();
                        if (!String.IsNullOrEmpty(report))
                        {
                            controller.Output.WriteLine(report);
                        }
                    }
                    return false;
                });

            // HELP command
            AddCommandDefinition(
                "HELP", "",
                (robot, controller, args) =>
                {
                    if (controller != null)
                    {
                        // build a list of usage details and output message
                        controller.Output.WriteLine("Usage:");
                        controller.Output.WriteLine("-----");
                        foreach (var def in CommandDefinitions.Definitions.Values)
                        {
                            controller.Output.WriteLine(def.Name +
                                (!String.IsNullOrEmpty(def.UsageArgs) ? " " + def.UsageArgs : ""));
                        }
                        controller.Output.WriteLine("-----");
                    }
                    return false;
                });
        }

        private static void AddCommandDefinition(string name, string usage, 
            Func<ToyRobot, ConsoleController, string, bool> invoke)
        {
            if (!String.IsNullOrEmpty(name))
            {
                definitions.Add(
                    name.ToUpperInvariant(),
                    new CommandDefinition { Name = name, UsageArgs = usage, Invoke = invoke });
            }
        }
    }

    /// <summary>
    /// Encapsulates the definition of a command that can be executed
    /// </summary>
    public class CommandDefinition
    {
        /// <summary>
        /// The name of the command used to invoke it
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Example usage arguments for the help message
        /// </summary>
        public string UsageArgs { get; set; }

        /// <summary>
        /// Function that is called when the command is invoked - takes
        /// three parameters: the robot to act on, the controller being used
        /// and any arguments passed to the command.  Returns true if
        /// command was successfully invoked.
        /// </summary>
        public Func<ToyRobot, ConsoleController, string, bool> Invoke { get; set; }
    }
}
