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

        public Game()
        {
            lastPoint = new Point(4, 4); // tmp solution 
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
