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
        private Point lastPoint;
        private Player p1, p2;

        public Game(int gameMode, ref Board b)
        {
            lastPoint = new Point(4, 4); // tmp solution 

            System.Console.WriteLine(gameMode);

            switch (gameMode)
            {
                case 1:
                    p1 = new HumanPlayer(Color.WHITE);
                    p2 = new AIPlayer(Color.BLACK);
                    break;
                case 2:
                    p1 = new HumanPlayer(Color.WHITE);
                    p2 = new HumanPlayer(Color.BLACK);
                    break;
                case 3:
                    p1 = new AIPlayer(Color.WHITE);
                    p2 = new AIPlayer(Color.BLACK);
                    PlayAI(ref b);
                    break;
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
            BasePiece bp = null;// new BasePiece();

            /*
             * Check out of boundaries
             */
            if ((grid.X < 0 || grid.Y < 0) || (grid.X > 7 || grid.Y > 7))
            {
                Console.WriteLine("YOU FUCKED IN THE HEAD M8??? OUT OF BOUNDS"); // debug, remove later
                return;
            }

            /*
             * Handle if piece is selected
             */
            if (b.getSelectedPiece() != null)
            {
                if (lastPoint == grid) // in case the user want to put down the selected piece and choose another
                {
                    b.setSelectedPiece(null);
                    b.resetValidMoves();
                    return;
                }

                BasePiece sb = b.getSelectedPiece();

                // Valid move! Move the piece to new position
                if (b.getValidMove(sb, grid))
                {
                    b.updatePiece(lastPoint, grid, ref sb);

                    // Test if the new position results in check
                    // Every piece needs to be looped and tested against the kings position
                    
                    if(b.isChecking(sb.getColor()))
                    {
                        Console.WriteLine("THAT MEANS CHECK!");

                        // Is it checkmate?
                        if (b.checkMate((sb.getColor() == Color.BLACK ? Color.WHITE : Color.BLACK)))
                        {
                            Console.WriteLine("Check mate! :)");
                        }
                    }
                }

                b.setSelectedPiece(null);
                b.resetValidMoves();
                p2.performMove(ref b);
                return;
            }

            /*
             * If no piece is selected
             * Select a piece if piece exists in position point
             */
            bp = b.getPieceAt(grid);
            lastPoint = grid;
            if (bp != null)
            {
                b.setSelectedPiece(b.getPieceAt(grid));
                Console.WriteLine("PIECE SELECTED"); // debug, remove later

                b.setValidMoves(bp);

                //bp.setValidMoves(b);
            }
            else
            {
                
            }
            

            Console.WriteLine("THIS FUCKING SHIT x: " + grid.X + "y: " + grid.Y); // debug, remove later
        }      
    }
}
