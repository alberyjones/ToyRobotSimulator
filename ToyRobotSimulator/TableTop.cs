
namespace ToyRobotSimulator
{
    /// <summary>
    /// Class encapsulates properties of table top on which the toy
    /// robot simulator runs.
    /// </summary>
    public class TableTop
    {
        /// <summary>
        /// The size of the table top on the X axis
        /// </summary>
        public int SizeX { get; set; }

        /// <summary>
        /// The size of the table top on the Y axis
        /// </summary>
        public int SizeY { get; set; }

        /// <summary>
        /// Returns true if the specified location is valid - i.e. the
        /// robot will not fall if placed there.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsValidLocation(int x, int y)
        {
            return x >= 0 && y >= 0 && x < SizeX && y < SizeY;
        }
    }
}
