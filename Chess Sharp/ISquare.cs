using System.Collections.Generic;

namespace Chess_Sharp
{
    public interface ISquare
    {
        IGridLocation Location { get; set; }
        PieceType Type { get; set; }
        PieceColour Colour { get; set; }
        bool HasMoved { get; set; }

        IEnumerable<IGridLocation> KnightMoveLocations();
        IEnumerable<IGridLocation> AdjacentLocations();
        IEnumerable<IGridLocation> LocationsInDirection(sbyte xStep, sbyte yStep);
        IGridLocation RelativeLocation(sbyte xStep, sbyte yStep);
        void Clear();
        bool LastRow();
    }
}
