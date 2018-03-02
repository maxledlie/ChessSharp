using System.Drawing;

namespace Chess_Sharp
{
    class ImageFactory
    {
        public Image CreateImage(ISquare square)
        {
            if (square.Type == PieceType.None)
                return Chess_Sharp.Properties.Resources.NoPiece;

            if (square.Colour == PieceColour.White)
            {
                switch (square.Type)
                {
                    case (PieceType.Pawn):
                        return Chess_Sharp.Properties.Resources.WhitePawn;
                    case (PieceType.Knight):
                        return Chess_Sharp.Properties.Resources.WhiteKnight;
                    case (PieceType.Bishop):
                        return Chess_Sharp.Properties.Resources.WhiteBishop;
                    case (PieceType.Rook):
                        return Chess_Sharp.Properties.Resources.WhiteRook;
                    case (PieceType.Queen):
                        return Chess_Sharp.Properties.Resources.WhiteQueen;
                    case (PieceType.King):
                        return Chess_Sharp.Properties.Resources.WhiteKing;
                    default:
                        return Chess_Sharp.Properties.Resources.NoPiece;
                }
            }
            switch (square.Type)
            {
                case (PieceType.Pawn):
                    return Chess_Sharp.Properties.Resources.BlackPawn;
                case (PieceType.Knight):
                    return Chess_Sharp.Properties.Resources.BlackKnight;
                case (PieceType.Bishop):
                    return Chess_Sharp.Properties.Resources.BlackBishop;
                case (PieceType.Rook):
                    return Chess_Sharp.Properties.Resources.BlackRook;
                case (PieceType.Queen):
                    return Chess_Sharp.Properties.Resources.BlackQueen;
                case (PieceType.King):
                    return Chess_Sharp.Properties.Resources.BlackKing;
                default:
                    return Chess_Sharp.Properties.Resources.NoPiece;            
            }
        }
    }
}
