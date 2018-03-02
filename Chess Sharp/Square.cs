using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Sharp
{
    class Square : ISquare
    {
        public PieceColour Colour { get; set; }
        public PieceType Type { get; set; }
        public IGridLocation Location { get; set; }
        public bool HasMoved { get; set; }

        public Square(IGridLocation location)
            : this(location, PieceType.None, PieceColour.None)
        {
        }

        public Square(IGridLocation location, PieceType type, PieceColour colour)
        {
            Location = location;
            Type = type;
            Colour = colour;
        }

        public IEnumerable<IGridLocation> AdjacentLocations()
        {
            return Location.AdjacentLocations();
        }

        public IEnumerable<IGridLocation> KnightMoveLocations()
        {
            return Location.KnightMoveLocations();
        }

        public IEnumerable<IGridLocation> LocationsInDirection(sbyte xStep, sbyte yStep)
        {
            return Location.LocationsInDirection(xStep, yStep);
        }

        public IGridLocation RelativeLocation(sbyte xStep, sbyte yStep)
        {
            return Location.RelativeLocation(xStep, yStep);
        }

        public void Clear()
        {
            Type = PieceType.None;
            Colour = PieceColour.None;
        }

        public bool LastRow()
        {
            return Colour == PieceColour.White ? (Location.YCoord == 0) : (Location.YCoord == 7);
        }
    }
}
