using System.Collections.Generic;
using System.Linq;

namespace Chess_Sharp
{
    public class GridLocation : IGridLocation
    {
        public byte XCoord { get; private set; }
        public byte YCoord { get; private set; }

        public GridLocation(byte XCoord, byte YCoord)
        {
            this.XCoord = XCoord;
            this.YCoord = YCoord;
        }

        public GridLocation(int XCoord, int YCoord)
        {
            this.XCoord = (byte)XCoord;
            this.YCoord = (byte)YCoord;
        }

        public bool Equals(IGridLocation other)
        {
            return XCoord == other.XCoord && YCoord == other.YCoord;
        }

        public IEnumerable<IGridLocation> AdjacentLocations()
        {
            IList<IGridLocation> allAdjacents = new List<IGridLocation>
            {
                new GridLocation(XCoord - 1, YCoord - 1),
                new GridLocation(XCoord - 1, YCoord + 1),
                new GridLocation(XCoord - 1, YCoord),
                new GridLocation(XCoord + 1, YCoord + 1),
                new GridLocation(XCoord + 1, YCoord - 1),
                new GridLocation(XCoord + 1, YCoord),
                new GridLocation(XCoord, YCoord + 1),
                new GridLocation(XCoord, YCoord - 1)
            };

            return (IEnumerable<IGridLocation>)OnBoardLocations(allAdjacents);
        }

        // Given an increment of movement defined by an xStep and a yStep, 
        // returns the list of GridLocations that exist in that direction,
        // up to the edge of an 8x8 board.
        public IList<IGridLocation> LocationsInDirection(sbyte xStep, sbyte yStep)
        {
            IList<IGridLocation> destinations = new List<IGridLocation>();

            IGridLocation currentLocation = new GridLocation(XCoord, YCoord);
            currentLocation = currentLocation.RelativeLocation(xStep, yStep);

            while (currentLocation.OnBoard())
            {
                // Add this square to the list of destinations and go on to the next one.
                destinations.Add(currentLocation);
                currentLocation = currentLocation.RelativeLocation(xStep, yStep);
            }
            // If blocked by the edge, return the list of squares we reached
            return destinations;
        }

        public IEnumerable<IGridLocation> LocationsInDirection(int xStep, int yStep)
        {
            return LocationsInDirection((sbyte)xStep, (sbyte)yStep);
        }

        public IEnumerable<IGridLocation> KnightMoveLocations()
        {
            List<IGridLocation> destinations = new List<IGridLocation>();

            destinations.Add(new GridLocation(XCoord - 2, YCoord + 1));
            destinations.Add(new GridLocation(XCoord - 2, YCoord - 1));
            destinations.Add(new GridLocation(XCoord - 1, YCoord + 2));
            destinations.Add(new GridLocation(XCoord - 1, YCoord - 2));
            destinations.Add(new GridLocation(XCoord + 1, YCoord + 2));
            destinations.Add(new GridLocation(XCoord + 1, YCoord - 2));
            destinations.Add(new GridLocation(XCoord + 2, YCoord + 1));
            destinations.Add(new GridLocation(XCoord + 2, YCoord - 1));

            return OnBoardLocations(destinations);  
        }

        /// <summary>
        /// Given a list of GridLocations, returns only those that would lie on an 8x8 board.
        /// </summary>
        /// <param name="allLocations"></param>
        /// <returns></returns>
        private IEnumerable<IGridLocation> OnBoardLocations(IList<IGridLocation> allLocations)
        {
            return allLocations.Where(location => location.OnBoard());
        }

        // Returns true if this GridLocation will exist on an 8x8 board.
        public bool OnBoard()
        {
            return (XCoord >= 0 && XCoord <= 7 && YCoord >= 0 && YCoord <= 7);
        }

        public IGridLocation RelativeLocation(sbyte xStep, sbyte yStep)
        {
            return new GridLocation(XCoord + xStep, YCoord + yStep);
        }
    }
}