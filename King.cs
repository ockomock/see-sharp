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
        public King(Color color)
        {
            if (color == Color.WHITE)
                setImage("imgs/white_king.bmp");
            else
                setImage("imgs/black_king.bmp");
        }

        public override bool validMove(Point from, Point to, ref Board board)
        {
            return true;
        }
    }
}
