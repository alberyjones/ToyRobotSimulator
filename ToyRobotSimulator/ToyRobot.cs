using System;

namespace ToyRobotSimulator
{
    /// <summary>
    /// Encapsulates the functions of the simulated toy robot
    /// </summary>
    public class ToyRobot
    {
        private bool isPlaced;
        private int currentX;
        private int currentY;
        private Direction currentFacing;
        private TableTop currentTableTop;

        /// <summary>
        /// Places the robot on a table top in the specified position, facing the
        /// specified direction.
        /// </summary>
        /// <param name="x">The X position on the table top</param>
        /// <param name="y">The Y position on the table top</param>
        /// <param name="facing">The direction to face</param>
        /// <param name="tableTop">The table top object to act on</param>
        public void Place(int x, int y, Direction facing, TableTop tableTop)
        {
            // can only place if we have a table top and a valid location
            if (tableTop != null && tableTop.IsValidLocation(x, y))
            {
                currentTableTop = tableTop;
                currentX = x;
                currentY = y;
                currentFacing = facing;
                isPlaced = true;
            }
        }

        /// <summary>
        /// Moves the robot forwards one space in the current direction
        /// </summary>
        public void Move()
        {
            // cannot move if robot has not been placed
            if (isPlaced)
            {
                // determine new location
                int newX = currentX;
                int newY = currentY;
                switch (currentFacing)
                {
                    case Direction.NORTH:
                        newY++;
                        break;
                    case Direction.EAST:
                        newX++;
                        break;
                    case Direction.SOUTH:
                        newY--;
                        break;
                    case Direction.WEST:
                        newX--;
                        break;
                }

                // only allow move to a valid location
                if (currentTableTop.IsValidLocation(newX, newY))
                {
                    currentX = newX;
                    currentY = newY;
                }
            }
        }

        /// <summary>
        /// Turns the robot ninety degrees to the left / counterclockwise
        /// </summary>
        public void Left()
        {
            Turn(true);
        }

        /// <summary>
        /// Turns the robot ninety degrees to the right / clockwise
        /// </summary>
        public void Right()
        {
            Turn(false);
        }

        private void Turn(bool turnLeft)
        {
            // cannot turn if robot has not been placed
            if (isPlaced)
            {
                // set new direction
                switch (currentFacing)
                {
                    case Direction.NORTH:
                        currentFacing = turnLeft ? Direction.WEST : Direction.EAST;
                        break;
                    case Direction.EAST:
                        currentFacing = turnLeft ? Direction.NORTH : Direction.SOUTH;
                        break;
                    case Direction.SOUTH:
                        currentFacing = turnLeft ? Direction.EAST : Direction.WEST;
                        break;
                    case Direction.WEST:
                        currentFacing = turnLeft ? Direction.SOUTH : Direction.NORTH;
                        break;
                }
            }
        }

        /// <summary>
        /// Generates and returns a report of the robot's location and direction
        /// </summary>
        /// <returns></returns>
        public string Report()
        {
            if (isPlaced)
            {
                return String.Format("{0},{1},{2}",
                    currentX, currentY, Enum.GetName(typeof(Direction), currentFacing));
            }
            return String.Empty;
        }
    }

    /// <summary>
    /// Possible directions the robot can face
    /// </summary>
    public enum Direction
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }
}
