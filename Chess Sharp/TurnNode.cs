using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Sharp
{
    public class TurnNode
    {
        public ITurn Turn { get; private set; }
        public byte Depth { get; private set; }
        public IList<TurnNode> Children { get; private set; }
        public short Value { get; private set; }

        // Constructing a turn node creates nodes for each of its immediate children.
        public TurnNode(ITurn turn, byte depth, byte maxDepth)
        {
            Depth = depth;
            Turn = turn;
            Value = turn.CurrentBoard.Value;

            if (depth >= maxDepth)
                return;

            Children = new List<TurnNode>();

            foreach (Move move in turn.AvailableMoves)
            {
                ITurn hypotheticalTurn = new Turn(turn.CurrentBoard.Copy(), turn.PlayingColour, leaf: true);
                hypotheticalTurn.Execute(move);
                Children.Add(new TurnNode(hypotheticalTurn, (byte)(depth + 1), maxDepth));
            }
        }
    }
}
