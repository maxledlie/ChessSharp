using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;

namespace Chess_Sharp
{
    public class Board : IEnumerable
    {
        public ISquare[] Squares;       
        public ISquare EnPassantSquare { get; set; }

        public Board()
        {
            Squares = new ISquare[64];
        }

        public Board Copy()
        {
            Board copy = new Board();
            foreach (ISquare square in this)
            {
                IGridLocation loc = square.Location;
                copy[loc] = new Square(loc, this[loc].Type, this[loc].Colour);
                copy[loc].HasMoved = this[loc].HasMoved;             
            }
            return copy;            
        }

        public IEnumerator GetEnumerator()
        {
            return Squares.GetEnumerator();
        }

        public ISquare this[byte xCoord, byte yCoord]
        {
            get { return Squares[xCoord + 8*yCoord]; }
            set { Squares[xCoord + 8*yCoord] = value; }
        }

        public ISquare this[int xCoord, int yCoord]
        {
            get { return this[(byte)xCoord, (byte)yCoord]; }
            set { this[(byte)xCoord, (byte)yCoord] = value; }
        }

        public ISquare this[IGridLocation location]
        {
            get { return this[location.XCoord, location.YCoord]; }
            set { this[location.XCoord, location.YCoord] = value; }
        }

        public short Value
        {
            get
            {
                if (Checkmated(PieceColour.White))
                    return short.MinValue;

                if (Checkmated(PieceColour.Black))
                    return short.MaxValue;

                short value = 0;

                foreach (ISquare square in this)
                {
                    short minusIfBlack = square.Colour == PieceColour.White ? (short)1 : (short)-1;
                    value += (short)(minusIfBlack * PieceValues.GetValue(square.Type));                    
                }
                return value;
            }
        }

        public bool IsOccupied(IGridLocation location)
        {
            return this[location].Type != PieceType.None;
        }

        public List<Move> AvailableMoves(PieceColour playingColour)
        {                        
            List<Move> allMoves = new List<Move>();
            List<Move> availableMoves = new List<Move>();

            foreach (ISquare mover in this)
                if (mover.Type != PieceType.None && mover.Colour == playingColour)
                    foreach (Move move in AvailableMovesFromSquare(mover))
                        allMoves.Add(move);

            foreach (Move move in allMoves)
            {
                Turn hypotheticalTurn = new Turn(Copy(), playingColour, leaf: true);
                hypotheticalTurn.Execute(move);
                if (!hypotheticalTurn.CurrentBoard.InCheck(this[move.Start].Colour))
                    availableMoves.Add(move);
            }
            return availableMoves;
        }

        public bool Checkmated(PieceColour playingColour)
        {
            return (InCheck(playingColour) && AvailableMoves(playingColour).Count == 0);
        }

        public bool Stalemate(PieceColour playingColour)
        {
            return !InCheck(playingColour) && AvailableMoves(playingColour).Count == 0;
        }

        public IEnumerable<Move> AvailableMovesFromSquare(ISquare mover)
        {
            if (mover.Type == PieceType.None)
                throw new Exception("MoveDestinations was called with an empty square as the mover.");

            switch (mover.Type)
            {
                case PieceType.Pawn:
                    return PawnMoves(mover);
                case PieceType.Knight:
                    return KnightMoves(mover);
                case PieceType.Bishop:
                    return BishopMoves(mover);
                case PieceType.Rook:
                    return RookMoves(mover);
                case PieceType.Queen:
                    return QueenMoves(mover);
                case PieceType.King:
                    return KingMoves(mover);
                default:
                    return new List<Move>();
            }
        }

        public List<IGridLocation> AvailableDestinations(ISquare mover)
        {
            List<IGridLocation> destinations = new List<IGridLocation>();

            foreach (Move move in AvailableMoves(mover.Colour))
                if (move.Start.Equals(mover.Location))
                    destinations.Add(move.End);

            return destinations;
        }  

        /// <summary>
        /// Returns a list of all the squares that are currently attacked by the non-moving player.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IGridLocation> AttackedLocations(PieceColour playingColour)
        {
            List<IGridLocation> attackedLocations = new List<IGridLocation>();
            // For each enemy piece on the board, add its attacked squares to the list.
            foreach (ISquare square in this)
                if (square.Type != PieceType.None && square.Colour != playingColour)
                    attackedLocations.AddRange(LocationsAttackedBy(square));

            return attackedLocations;
        }

        /// <summary>
        /// Returns a list of all the squares that are currently attacked by the piece at the given square.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        private IEnumerable<IGridLocation> LocationsAttackedBy(ISquare attacker)
        {
            if (attacker.Type == PieceType.None)
                throw new Exception("AttackedSquares was called with an empty square as the attacker.");

            switch (attacker.Type)
            {
                case PieceType.Pawn:
                    return PawnAttackedLocations(attacker);
                case PieceType.Knight:
                    return DestinationsFromMoves(KnightMoves(attacker));
                case PieceType.Bishop:
                    return DestinationsFromMoves(BishopMoves(attacker));
                case PieceType.Rook:
                    return DestinationsFromMoves(RookMoves(attacker));
                case PieceType.Queen:
                    return DestinationsFromMoves(QueenMoves(attacker));
                case PieceType.King:
                    return KingAttackedLocations(attacker);
                default:
                    return new List<IGridLocation>();
            }
        }

        private IEnumerable<IGridLocation> DestinationsFromMoves(IEnumerable<Move> moves)
        {
            List<IGridLocation> destinations = new List<IGridLocation>();
            foreach (Move move in moves)
                destinations.Add(move.End);

            return destinations;
        }        

        public static Board InitialSetup()
        {
            Board board = new Board();
            InitialSquareFactory factory = new InitialSquareFactory();

            for (byte i = 0; i < 8; ++i)
                for (byte j = 0; j < 8; ++j)
                {
                    board[i, j] = factory.CreateSquare(new GridLocation(i, j));
                }

            return board;
        }

        public IEnumerable<IGridLocation> PawnAttackedLocations(ISquare pawn)
        {
            AssertType(pawn, PieceType.Pawn);

            PieceColour colour = pawn.Colour;
            byte xCoord = pawn.Location.XCoord;
            byte yCoord = pawn.Location.YCoord;

            List<IGridLocation> attacked = new List<IGridLocation>();

            // If the pawn is on the last row, it's attacking nothing.
            if (LastRow(pawn))
                return attacked;

            sbyte minusIfWhite = (sbyte)((colour == PieceColour.White) ? -1 : 1);

            // Add the diagonal squares to the list if they exist on the board.
            IGridLocation left = new GridLocation((byte)(xCoord - 1), (byte)(yCoord + minusIfWhite));
            IGridLocation right = new GridLocation((byte)(xCoord + 1), (byte)(yCoord + minusIfWhite));

            if (xCoord != 0)
                attacked.Add(left);
            if (xCoord != 7)
                attacked.Add(right);

            return attacked;
        }

        public List<Move> PawnMoves(ISquare pawn)
        {
            AssertType(pawn, PieceType.Pawn);

            byte yCoord = pawn.Location.YCoord;
            byte xCoord = pawn.Location.XCoord;

            List<Move> moves = new List<Move>();

            if (LastRow(pawn))
                return moves;

            sbyte minusIfWhite = (sbyte)((pawn.Colour == PieceColour.White) ? -1 : 1);

            // Advance one square if not blocked by a piece of either colour
            if (this[xCoord, yCoord + minusIfWhite].Type == PieceType.None)
            {
                moves.Add(new Move(pawn.Location, new GridLocation(xCoord, (yCoord + minusIfWhite))));

                // Advance two squares if not blocked and haven't moved yet, and take note of the enPassant square.
                if (pawn.HasMoved == false && this[xCoord, yCoord + 2 * minusIfWhite].Type == PieceType.None)
                    moves.Add(new Move(pawn.Location, new GridLocation(xCoord, (yCoord + 2 * minusIfWhite)),
                        enPassantSquare: this[xCoord, yCoord + minusIfWhite]));
            }

            // If not on the far left, can take the piece to its diagonal left
            if (xCoord != 0)
            {
                ISquare leftTarget = this[xCoord - 1, yCoord + minusIfWhite];
                if (leftTarget.Type != PieceType.None && leftTarget.Colour != pawn.Colour)
                    moves.Add((new Move(pawn.Location, leftTarget.Location)));
                else if (leftTarget == EnPassantSquare)
                    moves.Add(new Move(pawn, EnPassantSquare, sideEffect: new Move(EnPassantSquare.RelativeLocation(0, (sbyte)-minusIfWhite), null)));
            }

            // If not on the far right, can take the piece to its diagonal right
            if (xCoord != 7)
            {
                ISquare rightTarget = this[xCoord + 1, yCoord + minusIfWhite];
                if (rightTarget.Type != PieceType.None && rightTarget.Colour != pawn.Colour)
                    moves.Add(new Move(pawn, rightTarget));
                else if (rightTarget == EnPassantSquare)
                    moves.Add(new Move(pawn, EnPassantSquare, sideEffect: new Move(EnPassantSquare.RelativeLocation(0, (sbyte)-minusIfWhite), null)));
            }
            return moves;
        }

        public IEnumerable<Move> KnightMoves(ISquare knight)
        {
            AssertType(knight, PieceType.Knight);

            IEnumerable<IGridLocation> candidates = knight.KnightMoveLocations();

            IList<Move> moves = new List<Move>();

            foreach (IGridLocation destination in candidates)
                moves.Add(new Move(knight.Location, destination));

            return moves.Where(move => !PiecesOfSameColour(move));
        }

        public IEnumerable<Move> BishopMoves(ISquare piece)
        {
            IList<Move> northWestMoves = UnblockedMoves(piece, -1, -1);
            IList<Move> northEastMoves = UnblockedMoves(piece, +1, -1);
            IList<Move> southWestMoves = UnblockedMoves(piece, -1, +1);
            IList<Move> southEastMoves = UnblockedMoves(piece, +1, +1);

            List<Move> allMoves = new List<Move>();

            allMoves.AddRange(northWestMoves);
            allMoves.AddRange(northEastMoves);
            allMoves.AddRange(southWestMoves);
            allMoves.AddRange(southEastMoves);

            return allMoves;
        }

        public IEnumerable<Move> RookMoves(ISquare piece)
        {
            IList<Move> northMoves = UnblockedMoves(piece, 0, -1);
            IList<Move> eastMoves = UnblockedMoves(piece, 1, 0);
            IList<Move> southMoves = UnblockedMoves(piece, 0, 1);
            IList<Move> westMoves = UnblockedMoves(piece, -1, 0);

            List<Move> allMoves = new List<Move>();

            allMoves.AddRange(northMoves);
            allMoves.AddRange(eastMoves);
            allMoves.AddRange(southMoves);
            allMoves.AddRange(westMoves);

            return allMoves;
        }

         public IEnumerable<Move> QueenMoves(ISquare piece)
         {
             AssertType(piece, PieceType.Queen);
             return BishopMoves(piece).Concat(RookMoves(piece));
         }

        private IList<Move> UnblockedMoves(ISquare start, sbyte xStep, sbyte yStep)
        {
            IList<Move> moves = new List<Move>();
            foreach (IGridLocation destination in start.LocationsInDirection(xStep, yStep))
            {
                Move trialMove = new Move(start.Location, destination);

                if (this.IsOccupied(destination))
                {
                    if (!PiecesOfSameColour(trialMove))
                        moves.Add(trialMove);

                    return moves;
                }
                moves.Add(trialMove);
            }
            return moves;
        }

        public IEnumerable<Move> KingMoves(ISquare piece)
        {
            AssertType(piece, PieceType.King);

            IEnumerable<IGridLocation> adjacentLocations = piece.AdjacentLocations();

            List<Move> moves = new List<Move>();
            foreach (IGridLocation adjacentLocation in adjacentLocations)
                moves.Add(new Move(piece.Location, adjacentLocation));

            byte backRank = BackRank(piece.Colour);

            // Add the castling locations if appropriate            
            if (CanCastleKingSide(piece.Colour))
                moves.Add(new Move(piece.Location, new GridLocation((byte)6, backRank),
                    new Move(new GridLocation((byte)7, BackRank(piece.Colour)), new GridLocation((byte)5, backRank))));

            if (CanCastleQueenSide(piece.Colour))
                moves.Add(new Move(piece.Location, new GridLocation((byte)2, backRank),
                    new Move(new GridLocation((byte)0, backRank), new GridLocation((byte)3, backRank))));

            return moves.Where(move => !AttackedLocations(piece.Colour).Contains(move.End)
                && !PiecesOfSameColour(move));
        }

        public IEnumerable<IGridLocation> KingAttackedLocations(ISquare attacker)
        {
            AssertType(attacker, PieceType.King);
            return attacker.AdjacentLocations();
        }

        private bool CanCastleKingSide(PieceColour castlingColour)
        {
            return CanCastle(castlingColour, true);
        }

        private bool CanCastleQueenSide(PieceColour castlingColour)
        {
            return CanCastle(castlingColour, false);
        }

        private bool CanCastle(PieceColour castlingColour, bool kingSide)
        {
            byte castleRank = castlingColour == PieceColour.White ? (byte)7 : (byte)0;
            byte rookFile = kingSide ? (byte)7 : (byte)0;

            ISquare kingSquare = this[(byte)4, (byte)castleRank];
            ISquare rookSquare = this[(byte)rookFile, (byte)castleRank];

            if (InCheck(castlingColour))
                return false;

            if (rookSquare.Type != PieceType.Rook || rookSquare.Colour != castlingColour || rookSquare.HasMoved)
                return false;

            if (kingSquare.Type != PieceType.King || kingSquare.Colour != castlingColour || kingSquare.HasMoved)
                return false;

            sbyte minusIfQueenSide = (sbyte)(kingSide ? 1 : -1);

            // Can't castle if any of the squares between the king and the rook are occupied.     
            IGridLocation nextLocation = kingSquare.RelativeLocation(minusIfQueenSide, 0);

            while (nextLocation.XCoord != rookFile)
            {
                if (this[nextLocation].Type != PieceType.None)
                    return false;

                if (this.AttackedLocations(castlingColour).Contains(nextLocation))
                    return false;

                nextLocation = nextLocation.RelativeLocation(minusIfQueenSide, 0);
            }
            return true;
        }

        public bool InCheck(PieceColour playingColour)
        {
            foreach (GridLocation location in AttackedLocations(playingColour))
                if (this[location].Colour == playingColour && this[location].Type == PieceType.King)
                    return true;

            return false;
        }

        private byte BackRank(PieceColour playingColour)
        {
            return (byte)(playingColour == PieceColour.White ? 7 : 0);
        }

        private bool PiecesOfSameColour(Move move)
        {
            if (this[move.Start].Type == PieceType.None || this[move.End].Type == PieceType.None)
                return false;

            return (this[move.Start].Colour == this[move.End].Colour);
        }

        private static bool Contains(List<IGridLocation> list, IGridLocation loc)
        {
            return list.Any(x => x.XCoord == loc.XCoord && x.YCoord == loc.YCoord);
        }

        private static void AssertType(ISquare square, PieceType type)
        {
            if (square.Type != type)
                throw new Exception("A piece-specific method was called on the wrong type of piece.");
        }

        public bool LastRow(ISquare piece)
        {
            if ((piece.Colour == PieceColour.White && piece.Location.YCoord == 0)
                || (piece.Colour == PieceColour.Black && piece.Location.YCoord == 7))
            {
                return true;
            }
            return false;
        }
    }   
}
