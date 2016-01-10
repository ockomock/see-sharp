using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
- Board har en lista med alla pjäser, tom plats -> nullptr
- Spelaren klickar någonstans på brädet, pjäsen på platsen returneras
- Spelaren klickar igen på platsen man vill gå till, validMove() körs för att se om det är möjligt
- Pjäsen flyttas och det är andra spelarens tur


*/

namespace Chess
{
    public partial class Form1 : Form
    {
        private Board board;
        private Graphics graphics;
        private Game game;
        private int gameMode;

        public Form1()
        {
            InitializeComponent();

            // Hook the Idle event
            Application.Idle += HandleApplicationIdle;

            graphics = this.CreateGraphics();
           board = new Board();

        }

        bool IsApplicationIdle()
        {
            NativeMessage result;
            return PeekMessage(out result, IntPtr.Zero, (uint)0, (uint)0, (uint)0) == 0;
        }

        void HandleApplicationIdle(object sender, EventArgs e)
        {
            while (IsApplicationIdle())
            {
                UpdateGame();
                DrawGame();
            }
        }


        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            Point cursor = PointToClient(Cursor.Position);
            Point grid = board.pixelToGrid(cursor);

            if(game != null)
                game.handleClick(grid, ref board, ref graphics);
        }

        public void UpdateGame()
        {

        }

        public void DrawGame()
        {
            // board.Draw(graphics); //update board, move this function call?
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NativeMessage
        {
            public IntPtr Handle;
            public uint Message;
            public IntPtr WParameter;
            public IntPtr LParameter;
            public uint Time;
            public Point Location;
        }

        [DllImport("user32.dll")]
        public static extern int PeekMessage(out NativeMessage message, IntPtr window, uint filterMin, uint filterMax, uint remove);

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (gameMode == 0)
                gameMode = 1;
            board = new Board();
            game = new Game(gameMode, ref board);
            board.Draw(graphics);
            disableButtons();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            board = new Board();
            game = new Game(1, ref board);
            board.Draw(graphics);
            disableButtons();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            board = new Board();
            game = new Game(2, ref board);
            board.Draw(graphics);
            disableButtons();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            board = new Board();
            game = new Game(3, ref board);
            board.Draw(graphics);
            disableButtons();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            game.performActivePlayerMove(board, graphics);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int turn = 0;
            int mode = board.loadFromFile("board.xml", ref turn);
            game = new Game(mode, ref board, turn);
            board = new Board();
            board.Draw(graphics);
            disableButtons();
        }

        private void disableButtons()
        {
            button6.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
        }

        private void enableButtons()
        {
            button6.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            enableButtons();
        }
    }
}
