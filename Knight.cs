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
            return true;
        }
    }
}
