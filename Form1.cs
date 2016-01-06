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

        public Form1()
        {
            InitializeComponent();

            // Hook the Idle event
            Application.Idle += HandleApplicationIdle;

            graphics = this.CreateGraphics();
            board = new Board();
            game = new Game();
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


            game.handleClick(grid, ref board, ref graphics);
            board.Draw(graphics); //update board, move this function call?
        }

        public void UpdateGame()
        {
                    
        }

        public void DrawGame()
        {
                     
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


    }
}
