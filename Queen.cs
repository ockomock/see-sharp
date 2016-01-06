using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace Chess
{
    public class Queen : BasePiece
    {
        public Queen(Color color) : base(color)
        {
            if (color == Color.WHITE)
                setImage("imgs/white_queen.bmp");
            else
                setImage("imgs/black_queen.bmp");
        }

        public override void setValidMoves(ref Board board)
        {
            BasePiece selected = board.getSelectedPiece();
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
            p = board.getBasePiecePoint(board.getSelectedPiece());
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
            p = board.getBasePiecePoint(board.getSelectedPiece());
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
            p = board.getBasePiecePoint(board.getSelectedPiece());
            while (++p.X < 8 && ++p.Y < 8)
                if (board.getPieceAt(p) == null)
                    board.setValidMoveTrue(p);
                else
                {
                    if (board.getPieceAt(p).getColor() != selected.getColor())
                        board.setValidMoveTrue(p);

                    break;
                }

            p = board.getBasePiecePoint(selected);

            // up
            while (--p.Y >= 0)
                if (board.getPieceAt(p) == null)
                    board.setValidMoveTrue(p);
                else
                {
                    if (board.getPieceAt(p).getColor() != selected.getColor())
                        board.setValidMoveTrue(p);

                    break;
                }


            // right
            p = board.getBasePiecePoint(board.getSelectedPiece());
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
            p = board.getBasePiecePoint(board.getSelectedPiece());
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
            p = board.getBasePiecePoint(board.getSelectedPiece());
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
