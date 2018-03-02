using System.Windows.Forms;

namespace Chess_Sharp
{
    /// <summary>
    /// Extends the normal PictureBox class by keeping track of its location in a grid.
    /// </summary>
    class PieceBox : PictureBox
    {
        public GridLocation GridLocation { get; private set; }

        public PieceBox(GridLocation location) : base()
        {
            DoubleBuffered = true;
            GridLocation = location;
        }
    }
}