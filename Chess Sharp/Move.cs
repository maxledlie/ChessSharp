namespace Chess_Sharp
{
    public class Move
    {
        public IGridLocation Start;
        public IGridLocation End;
        public Move SideEffect;
        public ISquare EnPassantSquare;

        public Move(IGridLocation start, IGridLocation end, Move sideEffect = null, ISquare enPassantSquare = null)
        {
            Start = start;
            End = end;
            SideEffect = sideEffect;
            EnPassantSquare = enPassantSquare;
        }

        public Move(ISquare mover, ISquare end, Move sideEffect = null) : this(mover.Location, end.Location, sideEffect)
        {
        }

        public bool Equals(Move other)
        {
            return (Start.Equals(other.Start) && End.Equals(other.End));
        }
    }
}