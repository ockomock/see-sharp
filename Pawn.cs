using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace Chess
{
    public class Pawn : BasePiece
    {
        public Pawn(Color color) : base(color) 
        {
            if(color == Color.WHITE)
                setImage("imgs/white_pawn.bmp");
            else
                setImage("imgs/black_pawn.bmp");
        }

        public override bool validMove(Point from, Point to, Board board)
        {
            Point delta = new Point(to.X - from.X, to.Y - from.Y);

            if (getColor() == Color.WHITE && delta.Y > 0)
                return false;
            else if (getColor() == Color.BLACK && delta.Y < 0)
                return false;

            // Allow 2 steps for the first move
            int stepsAllowed = 1;
            if ((getColor() == Color.BLACK && from.Y == 1) || (getColor() == Color.WHITE && from.Y == 6))
                stepsAllowed = 2;

            // Only move one step in Y
            if(delta.X == 0 && Math.Abs(delta.Y) <= stepsAllowed && Math.Abs(delta.Y) > 0 && board.getPieceAt(to) == null)
            {
                if(stepsAllowed == 2)
                {
                    if (board.getPieceAt(new Point(to.X, to.Y - delta.Y / 2)) != null)
                        return false;
                }

                return true;
            }

            // Diagonal
            if ((delta.X == 1 || delta.X == -1) && Math.Abs(delta.Y) == 1 && board.getPieceAt(to) != null && board.getPieceAt(to).getColor() != getColor())
                return true;

            return false;
        }

        public override void setValidMoves(Board board)
        {
            Point p = board.getBasePiecePoint(this);// board.getSelectedPiece());

            // Loop through entire board and find valid moves
            for(int x = 0; x < 8; x++)
            {
                for(int y = 0; y < 8; y++)
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
