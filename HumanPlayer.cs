using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class HumanPlayer : Player
    {
        private Point lastPoint;

        public HumanPlayer(Color color, Game g) : base(color, g)
        {
            lastPoint = new Point(4, 4); // tmp solution 
        }

        public override int performMove(ref Board b) { return 0; } // empty

        public override void handleClick(Point grid, ref Board b, ref Graphics g)
        {
            BasePiece bp = null;// new BasePiece();

            /*
             * Check out of boundaries
             */
            if ((grid.X < 0 || grid.Y < 0) || (grid.X > 7 || grid.Y > 7))
            {              
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
                BasePiece piece = b.getPieceAt(grid);
                bool valid = b.getValidMove(sb, grid);
                bool isKing = (piece != null && piece.GetType() == typeof(King));
                if (b.getValidMove(sb, grid)) // valid && !isKing)  // if(sb.validMove(b.getBasePiecePoint(sb), grid, b)) //
                {
                    b.updatePiece(lastPoint, grid, ref sb);

                    //p2.performMove(ref b);
                    b.setSelectedPiece(null);
                    b.resetValidMoves();
                    game.changeTurn(b);

                    return;
                }

                b.setSelectedPiece(null);
                b.resetValidMoves();
            }

            /*
             * If no piece is selected
             * Select a piece if piece exists in position point
             */
            bp = b.getPieceAt(grid);
            lastPoint = grid;
            if (bp != null && bp.getColor() == getColor())
            {
                b.setSelectedPiece(b.getPieceAt(grid));
                b.setValidMoves(bp);
            }
            else
            {

            }
        }
    }
}
