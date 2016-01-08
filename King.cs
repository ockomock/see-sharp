using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace Chess
{
    public class King : BasePiece
    {
        public King(Color color) : base(color)
        {
            if (color == Color.WHITE)
                setImage("imgs/white_king.bmp");
            else
                setImage("imgs/black_king.bmp");
        }

        public override bool validMove(Point from, Point to, Board board)
        {
            Point delta = new Point(to.X - from.X, to.Y - from.Y);
               
            if (Math.Abs(delta.X) + Math.Abs(delta.Y) <= 2 && Math.Abs(delta.X) <= 1 && Math.Abs(delta.Y) <= 1)
            {            
                if (board.getPieceAt(to) != null)
                {
                    if (board.getPieceAt(to).getColor() == getColor())
                        return false;
                }
            }
            else
                return false;

            return true;
        }

        public override void setValidMoves(Board board)
        {
            Point p = board.getBasePiecePoint(this);

            // Loop through entire board and find valid moves
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Point newPoint = new Point(x, y);
                    if (validMove(p, newPoint, board))
                    {
                        board.setValidMoveTrue(newPoint);
                    }
                }
            }
        }
    }
}
