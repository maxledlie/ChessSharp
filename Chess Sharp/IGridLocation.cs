using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Sharp
{
    public interface IGridLocation
    {
        byte XCoord { get; }
        byte YCoord { get; }

        // While the other collections of locations are enumerables, this one is a list. The order of the locations
        // matters, as once one location is blocked, we know that all further locations will be blocked.
        // Therefore the zeroth element must always be the closest, and the last the furthest away.
        IList<IGridLocation> LocationsInDirection(sbyte xStep, sbyte yStep);

        IEnumerable<IGridLocation> KnightMoveLocations();
        IEnumerable<IGridLocation> AdjacentLocations();
        IGridLocation RelativeLocation(sbyte xStep, sbyte yStep);
        bool OnBoard();

        bool Equals(IGridLocation other);
    }
}
