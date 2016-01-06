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
        public Bishop(Color color)
        {
            if (color == Color.WHITE)
                setImage("imgs/white_bishop.bmp");
            else
                setImage("imgs/black_bishop.bmp");
        }

        public override bool validMove(Point from, Point to, ref Board board)
        {
            if (from == to)
                return false;
            else if (from.X == to.X || from.Y == to.Y)
                return false;


            return true;
        }

        public override void setValidMoves(ref Board board)
        {
            Point p = board.getBasePiecePoint(board.getSelectedPiece());

            //up left
            while (--p.X >= 0 && --p.Y >= 0)
                if (board.getPieceAt(p).getImage() == null)
                    board.setValidMoveTrue(p);
                else
                    break;

            //up right
            p = board.getBasePiecePoint(board.getSelectedPiece());
            while (--p.X >= 0 && ++p.Y < 8)
                if (board.getPieceAt(p).getImage() == null)
                    board.setValidMoveTrue(p);
                else
                    break;

            //down left
            p = board.getBasePiecePoint(board.getSelectedPiece());
            while (++p.X < 8 && --p.Y >= 0)
                if (board.getPieceAt(p).getImage() == null)
                    board.setValidMoveTrue(p);
                else
                    break;

            //down right
            p = board.getBasePiecePoint(board.getSelectedPiece());
            while (++p.X < 8 && ++p.Y < 8)
                if (board.getPieceAt(p).getImage() == null)
                    board.setValidMoveTrue(p);
                else
                    break;
        }
    }
}
