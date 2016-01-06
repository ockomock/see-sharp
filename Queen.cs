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

        public override bool validMove(Point from, Point to, ref Board board)
        {
            return true;
        }
    }
}
