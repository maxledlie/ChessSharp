using System;
using System.Collections.Generic;
namespace Chess_Sharp
{
    public interface ITurn
    {
        Board CurrentBoard { get; }
        ISquare SelectedSquare { get; }
        bool SquareSelected { get; }
        PieceColour PlayingColour { get; }
        ISquare NextEnPassantSquare { get; }
        IList<Move> AvailableMoves { get; }

        event EventHandler MoveMade;
        event EventHandler<List<IGridLocation>> HighlightsRequested;
        event EventHandler<PieceColour> GameOver;

        void SquareClicked(IGridLocation location);
        void MakeRandomMove();
        void Execute(Move move);
        void GenerateMoves();
    }
}
