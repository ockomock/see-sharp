using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Chess
{
    public class Rook : BasePiece
    {
        public Rook(Color color) : base(color)
        {
            if (color == Color.WHITE)
                setImage("imgs/white_rook.bmp");
            else
                setImage("imgs/black_rook.bmp");
        }

        public override bool validMove(Point from, Point to, Board board)
        {
            Point delta = new Point(to.X - from.X, to.Y - from.Y);
            Point dir = new Point(0, 0);//

            if (delta.X != 0)
                dir.X = delta.X / Math.Abs(delta.X);

            if (delta.Y != 0)
                dir.Y = delta.Y / Math.Abs(delta.Y);

            // left-right or up-down
            if ((delta.X != 0 && delta.Y == 0) || (delta.X == 0 && delta.Y != 0))
            {
                int max = Math.Abs(delta.X);
                if (delta.X == 0)
                    max = Math.Abs(delta.Y);

                for (int i = 1; i < max; i++)
                {
                    if (board.getPieceAt(new Point(from.X + i * dir.X, from.Y + i * dir.Y)) != null)
                        return false;
                }
            }
            else
                return false;

            BasePiece piece = board.getPieceAt(to);
            if (piece != null && piece.getColor() == getColor())
                return false;

            return true;
        }

        public override void setValidMoves(Board board)
        {
            BasePiece selected = this;// board.getSelectedPiece();
            Point p = board.getBasePiecePoint(selected);

            // up
            while (--p.Y >= 0)
                if (board.getPieceAt(p)  == null)
                    board.setValidMoveTrue(p);
                else
                {
                    if (board.getPieceAt(p).getColor() != selected.getColor())
                        board.setValidMoveTrue(p);

                    break;
                }
                    

            // right
            p = board.getBasePiecePoint(selected);
            while (++p.X < 8)
                if (board.getPieceAt(p) == null)
                    board.setValidMoveTrue(p);
                else
                {
                    if (board.getPieceAt(p).getColor() != selected.getColor())
                        board.setValidMoveTrue(p);

                    break;
                }

            // down
            p = board.getBasePiecePoint(selected);
            while (++p.Y < 8)
                if (board.getPieceAt(p) == null)
                    board.setValidMoveTrue(p);
                else
                {
                    if (board.getPieceAt(p).getColor() != selected.getColor())
                        board.setValidMoveTrue(p);

                    break;
                }

            // left
            p = board.getBasePiecePoint(selected);
            while (--p.X >= 0)
                if (board.getPieceAt(p) == null)
                    board.setValidMoveTrue(p);
                else
                {
                    if (board.getPieceAt(p).getColor() != selected.getColor())
                        board.setValidMoveTrue(p);

                    break;
                }

        }
    }
}
