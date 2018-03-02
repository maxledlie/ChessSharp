using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Chess_Sharp
{
    class GameModel : IGameModel
    {
        public event EventHandler<List<IGridLocation>> HighlightsRequested;
        public event EventHandler MoveMade;
        public event EventHandler<PieceColour> GameOver;

        public ITurn CurrentTurn { get; private set; }

        private List<ITurn> _turns = new List<ITurn>();
        private Dictionary<PieceColour, PlayerType> _playerTypes;

        public GameModel(GameSettings settings)
        {
            _playerTypes = settings.PlayerTypes;
        }

        public void StartGame()
        {
            CurrentTurn = new Turn();

            // Have the initial arrangement rendered.
            if (MoveMade != null)
                MoveMade(this, EventArgs.Empty);

            HookTurnEvents();
            CurrentTurn.GenerateMoves();

            PlayerType playerType = _playerTypes[CurrentTurn.PlayingColour];

            switch (playerType)
            {
                case PlayerType.Chimp:
                    CurrentTurn.MakeRandomMove();
                    return;
                case PlayerType.Computer:
                    MakeBestMove();
                    return;
                default:
                    return;
            }
        }

        public void SquareClicked(IGridLocation location)
        {
            CurrentTurn.SquareClicked(location);
        }

        public void Terminate()
        {
            UnHookTurnEvents();
        }

        private void HookTurnEvents()
        {
            CurrentTurn.MoveMade += MoveMadeHandler;
            CurrentTurn.HighlightsRequested += HighlightsRequestedHandler;
            CurrentTurn.GameOver += GameOverHandler;
        }

        private void UnHookTurnEvents()
        {
            CurrentTurn.MoveMade -= MoveMadeHandler;
            CurrentTurn.HighlightsRequested -= HighlightsRequestedHandler;
            CurrentTurn.GameOver -= GameOverHandler;
        }

        private void MoveMadeHandler(object sender, EventArgs args)
        {
            // Inform the view that a move was made so the form can render the current state of the board.           
            if (MoveMade != null)
                MoveMade(this, EventArgs.Empty);

            Application.DoEvents();

            StartNextTurn();
        }

        private void StartNextTurn()
        {
            // Unsubscribe to the current turn's events.
            UnHookTurnEvents();

            // Add a new turn with the current state of the board.
            PieceColour nextPlayingColour = CurrentTurn.PlayingColour == PieceColour.White ? PieceColour.Black : PieceColour.White;
            Board nextSquares = CurrentTurn.CurrentBoard;
            ISquare nextEnPassantSquare = CurrentTurn.NextEnPassantSquare;
            CurrentTurn = new Turn(nextSquares, nextPlayingColour, nextEnPassantSquare);
            _turns.Add(CurrentTurn);

            // Subscribe to the new turn's events.
            HookTurnEvents();

            CurrentTurn.GenerateMoves();

            PlayerType playerType = _playerTypes[CurrentTurn.PlayingColour];

            if (CurrentTurn.AvailableMoves.Count == 0)
                return;

            switch (playerType)
            {
                case PlayerType.Chimp:
                    CurrentTurn.MakeRandomMove();
                    return;
                case PlayerType.Computer:
                    MakeBestMove();
                    return;
                default:
                    return;
            }
        }

        private void MakeBestMove()
        {
            CurrentTurn.Execute(Minimaxer.BestMove(CurrentTurn, 2));
            MoveMadeHandler(this, EventArgs.Empty);
        }

        private void HighlightsRequestedHandler(object sender, List<IGridLocation> locations)
        {
            if (HighlightsRequested != null)
                HighlightsRequested(this, locations);
        }

        private void GameOverHandler(object sender, PieceColour winningColour)
        {
            if (GameOver != null)
                GameOver(this, winningColour);
        }
    }
}
