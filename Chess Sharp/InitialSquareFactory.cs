using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Sharp
{
    class InitialSquareFactory
    {
        // Given the coordinates of a square, returns the PieceType associated with that square at initital setup.
        public ISquare CreateSquare(GridLocation location)
        {
            byte i = location.XCoord;
            byte j = location.YCoord;

            if (j > 1 && j < 6)
                return new Square(location);

            if (j == 1)
                return new Square(location, PieceType.Pawn, PieceColour.Black);

            if (j == 6)
                return new Square(location, PieceType.Pawn, PieceColour.White);

            PieceColour colour = j > 3 ? PieceColour.White : PieceColour.Black;

            switch (i)
            {
                case 0:
                case 7:
                    return new Square(location, PieceType.Rook, colour);
                case 1:
                case 6:
                    return new Square(location, PieceType.Knight, colour);
                case 2:
                case 5:
                    return new Square(location, PieceType.Bishop, colour);
                case 3:
                    return new Square(location, PieceType.Queen, colour);
                case 4:
                    return new Square(location, PieceType.King, colour);
                default:
                    return new Square(location, PieceType.None, colour);
            }
        }
    }
}
