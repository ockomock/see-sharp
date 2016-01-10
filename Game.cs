using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Chess
{
    class Game
    {
        private Player p1, p2, activePlayer;
        int mode;
        int turnCounter = 1;

        public Game(int gameMode, ref Board b)
        {
            System.Console.WriteLine(gameMode);

            mode = gameMode;

            switch (gameMode)
            {
                case 1:
                    p1 = new HumanPlayer(Color.WHITE, this);
                    p2 = new AIPlayer(Color.BLACK, this);
                    break;
                case 2:
                    p1 = new HumanPlayer(Color.WHITE, this);
                    p2 = new HumanPlayer(Color.BLACK, this);
                    break;
                case 3:
                    p1 = new AIPlayer(Color.WHITE, this);
                    p2 = new AIPlayer(Color.BLACK, this);
                   // PlayAI(ref b);
                    break;
            }

            activePlayer = p1;
        }

        public void changeTurn(Board board)
        {
            // Save game state to file
            board.saveToFile("board.xml", mode, turnCounter);

            // Is it checkmate?
            if (board.checkMate(activePlayer.getColor() == Color.BLACK ? Color.WHITE : Color.BLACK))
            {
                Console.WriteLine("Check mate! :)");
                activePlayer = null;
            }
            else
            {
                turnCounter *= -1;

                if (turnCounter == 1)
                    activePlayer = p1;
                else
                    activePlayer = p2;
            }
        }

        public void PlayAI(ref Board b)
        {
            for (int i = 0; i < 9000; i++)
            {
                if (i % 2 == 0)
                    p1.performMove(ref b);
                else
                    p2.performMove(ref b);
            }

        }

        public void handleClick(Point grid, ref Board b, ref Graphics g)
        {
            if(activePlayer != null)
                activePlayer.handleClick(grid, ref b, ref g);
            b.Draw(g);            
        }      

        public void performActivePlayerMove(Board b, Graphics g)
        {
            if (activePlayer != null)
            {
                activePlayer.performMove(ref b);
            }
            b.Draw(g);
        }       
    }
}
