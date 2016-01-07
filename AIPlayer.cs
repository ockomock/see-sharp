using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Chess
{
    class AIPlayer : Player
    {
        private Color c;

        public AIPlayer(Color color) : base(color)
        {
           
        }

       public override int performMove(ref Board b)
        {
            BasePiece bp = null;
            Random rnd = new Random();
            Point p = new Point();
            Point np = new Point();


            for (int i = 0; i < 1000; i++)
            {
                p.X = rnd.Next(0, 8);
                p.Y = rnd.Next(0, 8);

                bp = b.getPieceAt(p);

                if (bp != null)
                {
                    if (bp.getColor() == this.getColor())
                    {
                        b.setSelectedPiece(bp);
                        bp.setValidMoves(ref b);
                        break;
                    }
                }
            }

            

            for (int i = 0; i < 1000; i++)
            {
                np.X = rnd.Next(0, 8);
                np.Y = rnd.Next(0, 8);

                if (b.getValidMove(np))
                {
                  b.updatePiece(p, np, ref bp);
                   break;
                }
            }
            
            b.setSelectedPiece(null);
            b.resetValidMoves();
            
            return 0;
        }

    }
}
