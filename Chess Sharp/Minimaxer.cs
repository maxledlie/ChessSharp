using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Chess_Sharp
{
    /// <summary>
    /// The class that uses the Minimax algorithm to determine the best move for the active player
    /// on a given turn.
    /// </summary>
    public static class Minimaxer
    {
        public static Move BestMove(ITurn turn, byte depth)
        {
            // Generate the turn tree
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            TurnNode rootNode = new TurnNode(turn, 0, depth);
            stopwatch.Stop();
            Random random = new Random();
            short randIndex = (short)random.Next(turn.AvailableMoves.Count);

            Board board = turn.CurrentBoard;
            short bestValue = board.Value;
            Move bestMove = null;
            foreach (Move move in turn.AvailableMoves)
            {
                Turn hypotheticalTurn = new Turn(board.Copy(), turn.PlayingColour, leaf: true);
                hypotheticalTurn.Execute(move);
                short value = hypotheticalTurn.CurrentBoard.Value;
                switch (turn.PlayingColour)
                {
                    case PieceColour.White:
                        if (value > bestValue)
                        {
                            bestValue = value;
                            bestMove = move;
                        }
                        break;

                    case PieceColour.Black:
                        if (value < bestValue)
                        {
                            bestValue = value;
                            bestMove = move;
                        }
                        break;
                }
            }
            if (bestMove != null)
                return bestMove;

            return turn.AvailableMoves[randIndex];
        }
    }
}
