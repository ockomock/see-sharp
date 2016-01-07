using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace Chess
{
    public class Bishop : BasePiece
    {
        public Bishop(Color color) : base(color)
        {
            if (color == Color.WHITE)
                setImage("imgs/white_bishop.bmp");
            else
                setImage("imgs/black_bishop.bmp");
        }

        public override bool validMove(Point from, Point to, Board board)
        {
            if (from == to)
                return false;
            else if (from.X == to.X || from.Y == to.Y)
                return false;


            return true;
        }

        public override void setValidMoves(Board board)
        {
            BasePiece selected = this;// board.getSelectedPiece();
            Point p = board.getBasePiecePoint(selected);

            //up left
            while (--p.X >= 0 && --p.Y >= 0)
                if (board.getPieceAt(p) == null)
                    board.setValidMoveTrue(p);
                else
                {
                    if (board.getPieceAt(p).getColor() != selected.getColor())
                        board.setValidMoveTrue(p);

                    break;
                }

            //up right
            p = board.getBasePiecePoint(selected);
            while (--p.X >= 0 && ++p.Y < 8)
                if (board.getPieceAt(p) == null)
                    board.setValidMoveTrue(p);
                else
                {
                    if (board.getPieceAt(p).getColor() != selected.getColor())
                        board.setValidMoveTrue(p);

                    break;
                }

            //down left
            p = board.getBasePiecePoint(selected);
            while (++p.X < 8 && --p.Y >= 0)
                if (board.getPieceAt(p) == null)
                    board.setValidMoveTrue(p);
                else
                {
                    if (board.getPieceAt(p).getColor() != selected.getColor())
                        board.setValidMoveTrue(p);

                    break;
                }

            //down right
            p = board.getBasePiecePoint(selected);
            while (++p.X < 8 && ++p.Y < 8)
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
