using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Sharp
{
    public class GameSettings
    {
        public Dictionary<PieceColour, PlayerType> PlayerTypes { get; private set; }

        public GameSettings(PlayerType whiteType, PlayerType blackType)
        {
            PlayerTypes = new Dictionary<PieceColour, PlayerType>();
            PlayerTypes.Add(PieceColour.White, whiteType);
            PlayerTypes.Add(PieceColour.Black, blackType);
        }
    }
}
