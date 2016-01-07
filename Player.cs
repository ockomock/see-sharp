using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Player
    {
        public Player(Color color)
        {
            c = color;
        }

        public virtual int performMove(ref Board b) { return 0; }


        private Color c;

        public Color getColor() { return c; }
    }
}
