using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace Chess
{
    public class BasePiece
    {
        private Color color;

        public BasePiece(Color c)
        {
            color = c;
        }

        public virtual bool validMove(Point from, Point to, Board board) { return false; }

        public virtual void setValidMoves(Board board) { }      

        public Color getColor()
        {
            return color;
        }
    }
}
