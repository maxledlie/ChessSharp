using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess_Sharp
{
    public partial class MainForm : Form, IMainView
    {
        public event EventHandler StartButtonClicked;

        public MainForm()
        {
            InitializeComponent();
            whiteComboBox.SelectedIndex = 0;
            blackComboBox.SelectedIndex = 0;
        }

        public GameSettings SelectedSettings
        {
            get
            {
                PlayerType whiteType = TypeFromText(whiteComboBox.Text);
                PlayerType blackType = TypeFromText(blackComboBox.Text);
                return new GameSettings(whiteType, blackType);
            }
        }

        private void startButton_Click(object sender, EventArgs args)
        {
            if (StartButtonClicked != null)
                StartButtonClicked(this, EventArgs.Empty);
        }

        private PlayerType TypeFromText(string text)
        {
            switch (text)
            {
                case "Human":
                    return PlayerType.Human;
                case "Chimp":
                    return PlayerType.Chimp;
                case "Computer":
                    return PlayerType.Computer;
                default:
                    return PlayerType.Human;
            }
        }
    }
}
