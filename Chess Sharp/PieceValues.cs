using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Sharp
{
    public static class PieceValues
    {
        private static Dictionary<PieceType, short> _values = new Dictionary<PieceType, short>
        {
            {PieceType.Pawn, 1},
            {PieceType.Knight, 3},
            {PieceType.Bishop, 3},
            {PieceType.Rook, 5},
            {PieceType.Queen, 9},
            {PieceType.King, 0},
            {PieceType.None, 0}
        };

        public static short GetValue(PieceType type)
        {
            return _values[type];
        }
    }
}
