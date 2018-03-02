using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Sharp
{
    public interface IMainView
    {
        GameSettings SelectedSettings { get; }

        event EventHandler StartButtonClicked;        
    }
}
