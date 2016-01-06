using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace Chess
{
    public class Knight : BasePiece
    {
        public Knight(Color color) : base(color)
        {
            if (color == Color.WHITE)
                setImage("imgs/white_knight.bmp");
            else
                setImage("imgs/black_knight.bmp");
        }

        public override bool validMove(Point from, Point to, ref Board board)
        {
            bool valid = false;
            Point delta = new Point(to.X - from.X, to.Y - from.Y);

            // Valid move
            if((Math.Abs(delta.X) == 1 && Math.Abs(delta.Y) == 2) || (Math.Abs(delta.X) == 2 && Math.Abs(delta.Y) == 1))
            {
                // Make sure it's not a piece with the same color
                if (board.getPieceAt(to) != null && board.getPieceAt(to).getColor() != getColor())
                    valid= true;
                else if (board.getPieceAt(to) == null)
                    valid = true;
            }

            return valid;
        }

        public override void setValidMoves(ref Board board)
        {
            Point p = board.getBasePiecePoint(board.getSelectedPiece());

            // Loop through entire board and find valid moves
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Point newPoint = new Point(x, y);
                    if (validMove(p, newPoint, ref board))
                    {
                        board.setValidMoveTrue(newPoint);
                    }
                }
            }
        }
    }
}
