using System;
using System.Collections.Generic;

namespace Chess_Sharp
{
    public interface IGameModel
    {
        ITurn CurrentTurn { get; }

        event EventHandler<List<IGridLocation>> HighlightsRequested;
        event EventHandler MoveMade;
        event EventHandler<PieceColour> GameOver;

        void StartGame();
        void SquareClicked(IGridLocation location);
        void Terminate();
    }
}
