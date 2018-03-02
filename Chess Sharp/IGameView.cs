using System;
using System.Collections.Generic;

namespace Chess_Sharp
{
    public interface IGameView
    {
        event EventHandler<IGridLocation> SquareClicked;
        event EventHandler FormClosed;

        void UpdateView(Board board);
        void HighlightSquares(List<IGridLocation> locations);
        void ClearHighlights();
        void DisplayOutcomeText(string text);
    }
}
