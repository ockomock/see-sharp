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
            if (from == to)
                return false;
            else if (from.X != to.X && from.Y != to.Y) // rook can't move in diagonal
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
