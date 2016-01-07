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
        private Image image;
        private Color color;

        public BasePiece(Color c)
        {
            image = null;
            color = c;
        }

        public virtual bool validMove(Point from, Point to, Board board) { return false; }

        public virtual void setValidMoves(Board board) { }

        public Image getImage()
        {
            return image;
        }

        public void setImage(String source)
        {
            image = Bitmap.FromFile(source);
        }

        public Color getColor()
        {
            return color;
        }
    }
}
