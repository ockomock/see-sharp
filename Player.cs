using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Player
    {
        protected Game game;

        public Player(Color color, Game g)
        {
            c = color;
            game = g;
        }

        public virtual int performMove(ref Board b) { return 0; }
        public virtual void handleClick(Point grid, ref Board b, ref Graphics g) { }

        private Color c;

        public Color getColor() { return c; }
    }
}
