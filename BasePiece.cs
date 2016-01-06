﻿using System;
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

        public BasePiece()
        {
            image = null;
        }

        public virtual bool validMove(Point from, Point to, ref Board board) { return false; }

        public virtual void setValidMoves(ref Board board) { }

        public Image getImage()
        {
            return image;
        }

        public void setImage(String source)
        {
            image = Bitmap.FromFile(source);
        }
    }
}
