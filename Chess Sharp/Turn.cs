using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Chess_Sharp
{
    class Turn : ITurn
    {
        public Board CurrentBoard { get; private set; }
        
        public ISquare NextEnPassantSquare { get; private set; }

        public bool SquareSelected { get; private set; }
        public ISquare SelectedSquare { get; private set; }
        public PieceColour PlayingColour { get; private set; }

        public event EventHandler MoveMade;
        public event EventHandler<List<IGridLocation>> HighlightsRequested;
        public event EventHandler<PieceColour> GameOver;

        private bool _leaf;

        /// <summary>
        /// When a turn is constructed with no parameters, set the CurrentBoard to the initial setup for a game.
        /// </summary>
        public Turn() : this(Board.InitialSetup(), PieceColour.White)
        {
        }

        public Turn(Board board, PieceColour playingColour, ISquare enPassantSquare = null, bool leaf = false)
        {
            CurrentBoard = board;
            PlayingColour = playingColour;
            CurrentBoard.EnPassantSquare = enPassantSquare;
            _leaf = leaf;
        }

        public void GenerateMoves()
        {
            if (!_leaf)
            {
                if (CurrentBoard.Checkmated(PlayingColour))
                    SendGameOver(NonPlayingColour());
                else if (CurrentBoard.Stalemate(PlayingColour))
                    SendGameOver(PieceColour.None);
            }
        }

        public IList<Move> AvailableMoves
        {
            get { return CurrentBoard.AvailableMoves(PlayingColour); }
        }       

        public void MakeRandomMove()
        {
            if (AvailableMoves.Count == 0)
            {
                SendGameOver(PieceColour.White);
                return;
            }
            Random random = new Random();
            int moveIndex = random.Next(AvailableMoves.Count);
            Move randomMove = AvailableMoves[moveIndex];
            Execute(randomMove);
            SendMoveMade();
        }

        public void SquareClicked(IGridLocation clickedLocation)
        {
            ISquare clickedSquare = CurrentBoard[clickedLocation];

            // If a piece is selected, try to move to the chosen location.
            if (SquareSelected)
            {
                Move attemptedMove = new Move(SelectedSquare.Location, clickedLocation);

                // If this is a possible move, execute the move. Then clear the selection either way.
                foreach (Move move in AvailableMoves.Where(move => move.Equals(attemptedMove)))
                {                  
                    Execute(move);
                    SendMoveMade();
                    return;
                }
            }
            if (clickedSquare.Type != PieceType.None && clickedSquare.Colour == PlayingColour)
            {
                SelectedSquare = clickedSquare;
                SquareSelected = true;
                SendHighlightsRequested(CurrentBoard.AvailableDestinations(SelectedSquare));
                return;
            }
        }

        private void ClearSelection()
        {
            SquareSelected = false;
            SelectedSquare = null;
        }   

        public void Execute(Move move)
        {
            IGridLocation start = move.Start;
            IGridLocation end = move.End;

            // A move with null as end represents the disappearance of a piece by itself with no replacement (en Passant)
            if (end == null)
            {
                CurrentBoard[start].Clear();
                return;
            }

            CurrentBoard[end].Colour = CurrentBoard[start].Colour;
            CurrentBoard[end].Type = CurrentBoard[start].Type;

            // Queen a pawn if on the final rank.
            if (CurrentBoard[start].Type == PieceType.Pawn && CurrentBoard[end].LastRow())           
                CurrentBoard[end].Type = PieceType.Queen;

            CurrentBoard[end].HasMoved = true;
            CurrentBoard[start].Clear();

            NextEnPassantSquare = move.EnPassantSquare;

            if (move.SideEffect != null)
                Execute(move.SideEffect);
        }

        private static void AssertType(ISquare square, PieceType type)
        {
            if (square.Type != type)
                throw new Exception("A piece-specific method was called on the wrong type of piece.");
        }

        private void SendMoveMade()
        {
            if (MoveMade != null)
                MoveMade(this, EventArgs.Empty);
        }

        private void SendHighlightsRequested(List<IGridLocation> locations)
        {
            if (HighlightsRequested != null)
                HighlightsRequested(this, locations);
        }
       
        private PieceColour NonPlayingColour()
        {
            return PlayingColour == PieceColour.White ? PieceColour.Black : PieceColour.White;
        }

        private void SendGameOver(PieceColour winningColour)
        {
            if (GameOver != null)
                GameOver(this, winningColour);
        }

        private bool PiecesOfSameColour(Move move)
        {
            if (CurrentBoard[move.Start].Type == PieceType.None || CurrentBoard[move.End].Type == PieceType.None)
                return false;

            return (CurrentBoard[move.Start].Colour == CurrentBoard[move.End].Colour);
        }
    }
}
