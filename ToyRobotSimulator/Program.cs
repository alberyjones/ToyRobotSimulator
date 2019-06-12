using System;

namespace ToyRobotSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a controller, robot and table top
            var controller = new ConsoleController {
                Output = Console.Out,
                Error = Console.Error,
                ReadLine = Console.ReadLine,
                Robot = new ToyRobot(),
                TableTop = new TableTop { SizeX = 5, SizeY = 5 }
            };

            // run the application via the controller
            controller.Run(args);
        }
    }
}
