﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace Chess
{
    public class Pawn : BasePiece
    {
        public Pawn(Color color) : base(color) 
        {
            if(color == Color.WHITE)
                setImage("imgs/white_pawn.bmp");
            else
                setImage("imgs/black_pawn.bmp");
        }

        public override bool validMove(Point from, Point to, ref Board board)
        {
            Point delta = new Point(to.X - from.X, to.Y - from.Y);

            if (getColor() == Color.WHITE)
                delta.Y *= -1;

            // Allow 2 steps for the first move
            int stepsAllowed = 1;
            if ((getColor() == Color.BLACK && from.Y == 1) || (getColor() == Color.WHITE && from.Y == 6))
                stepsAllowed = 2;

            // Only move one step in Y
            if(delta.X == 0 && delta.Y <= stepsAllowed && delta.Y > 0 && board.getPieceAt(to) == null)
            {
                return true;
            }

            // Diagonal
            if ((delta.X == 1 || delta.X == -1) && delta.Y == 1 && board.getPieceAt(to) != null && board.getPieceAt(to).getColor() != getColor())
                return true;

            return false;
        }
    }
}