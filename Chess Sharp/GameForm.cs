using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Chess_Sharp
{
    public partial class GameForm : Form, IGameView
    {
        private PieceBox[,] _chessBoardPanels;
        private const int GridSize = 8;
        private const int TileSize = 40;
        private const int MarginSize = 40;
        private readonly ImageFactory _imageFactory;

        public event EventHandler<IGridLocation> SquareClicked;
        public event EventHandler FormClosed;

        public GameForm()
        {
            DoubleBuffered = true;
            _imageFactory = new ImageFactory();
            InitializeComponent();
            SetUp();
        }

        private void SetUp()
        {
            // Size the window suitably
            int formSize = (int)(2.5 * MarginSize) + (GridSize * TileSize);
            var size = new Size(formSize, formSize);
            LockFormSize(size);

            // initialize the "chess board"
            _chessBoardPanels = new PieceBox[GridSize, GridSize];

            // double for loop to handle all rows and columns
            for (byte xCoord = 0; xCoord < GridSize; ++xCoord)
            {
                for (byte yCoord = 0; yCoord < GridSize; ++yCoord)
                {
                    // create new Panel control which will be one 
                    // chess board tile
                    var newPieceBox = new PieceBox(new GridLocation(xCoord, yCoord))
                    {
                        Size = new Size(TileSize, TileSize),
                        Location = new Point(TileSize * xCoord + MarginSize, TileSize * yCoord + MarginSize),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                    };
                    newPieceBox.Click += newPieceBox_Click;

                    Controls.Add(newPieceBox);

                    _chessBoardPanels[xCoord, yCoord] = newPieceBox;
                }
            }

            ClearHighlights();
            Show();
        }

        public void ClearHighlights()
        {
            var clr1 = Color.DarkGray;
            var clr2 = Color.White;

            for (byte xCoord = 0; xCoord < GridSize; ++xCoord)
                for (byte yCoord = 0; yCoord < GridSize; ++yCoord)
                {
                    var panel = _chessBoardPanels[xCoord, yCoord];
                    panel.BorderStyle = BorderStyle.None;
                    if (xCoord % 2 == 0)
                        panel.BackColor = yCoord % 2 != 0 ? clr1 : clr2;
                    else
                        panel.BackColor = yCoord % 2 != 0 ? clr2 : clr1;
                }
        }

        void newPieceBox_Click(object sender, EventArgs e)
        {
            PieceBox clickedSquare = sender as PieceBox;

            if (clickedSquare == null)
                return;

            if (SquareClicked != null)
                SquareClicked(this, clickedSquare.GridLocation);
        }

        private void Form_Load(object sender, EventArgs e)
        {

        }

        private void SetSquareImage(int x, int y, Image image)
        {
            PictureBox square = _chessBoardPanels[x, y];
            square.Image = image;
        }

        // Draws the given arrangement of squares on the board
        public void UpdateView(Board board)
        {
            ClearHighlights();
            for (int i = 0; i < GridSize; ++i)
                for (int j = 0; j < GridSize; ++j)
                {
                    Image image = _imageFactory.CreateImage(board[i,j]);
                    if (image != null)
                        SetSquareImage(i, j, image);
                }
        }

        public void HighlightSquares(List<IGridLocation> locations)
        {
            foreach (IGridLocation location in locations)
            {
                var panel = _chessBoardPanels[location.XCoord, location.YCoord];
                panel.BackColor = Color.LightBlue;
                panel.BorderStyle = BorderStyle.Fixed3D;
            }
        }

        public void DisplayOutcomeText(string text)
        {
            MessageBox.Show(text);
        }

        private void LockFormSize(Size size)
        {
            Size = size;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void GameForm_Closed(object sender, EventArgs e)
        {
            if (FormClosed != null)
                FormClosed(this, EventArgs.Empty);
        }
    }
}
