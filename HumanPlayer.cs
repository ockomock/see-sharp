using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class HumanPlayer : Player
    {
        public HumanPlayer(Color color) : base(color) { }

        public override int performMove(ref Board b) { return 0; } // empty
    }
}
