using System;
using System.Collections.Generic;

namespace Chess_Sharp
{
    class GamePresenter
    {
        private IGameModel _model;
        private IGameView _view;

        public GamePresenter(GameSettings settings)
        {
            _model = new GameModel(settings);
            _view = new GameForm();
            HookViewEvents();
            HookModelEvents();
            _model.StartGame();
        }

        private void HookViewEvents()
        {
            _view.SquareClicked += SquareClickedHandler;
            _view.FormClosed += FormClosedHandler;
        }

        private void UnhookViewEvents()
        {
            _view.SquareClicked -= SquareClickedHandler;
            _view.FormClosed -= FormClosedHandler;
        }

        private void HookModelEvents()
        {
            _model.HighlightsRequested += HighlightsRequestedHandler;
            _model.MoveMade += MoveMadeHandler;
            _model.GameOver += GameOverHandler;
        }

        private void UnhookModelEvents()
        {
            _model.HighlightsRequested -= HighlightsRequestedHandler;
            _model.MoveMade -= MoveMadeHandler;
            _model.GameOver -= GameOverHandler;
        }

        private void SquareClickedHandler(object sender, IGridLocation location)
        {
            _model.SquareClicked(location);
        }

        private void HighlightsRequestedHandler(object sender, List<IGridLocation> locations)
        {
            _view.ClearHighlights();
            _view.HighlightSquares(locations);
        }

        private void MoveMadeHandler(object sender, EventArgs args)
        {
            _view.UpdateView(_model.CurrentTurn.CurrentBoard);
        }

        private void GameOverHandler(object sender, PieceColour winningColour)
        {
            if (winningColour == PieceColour.None)
                _view.DisplayOutcomeText("Stalemate!");
            else if (winningColour == PieceColour.White)
                _view.DisplayOutcomeText("White wins!");
            else
                _view.DisplayOutcomeText("Black wins!");
        }

        private void FormClosedHandler(object sender, EventArgs args)
        {
            _model.Terminate();
            UnhookViewEvents();
            UnhookModelEvents();            
        }
    }
}
