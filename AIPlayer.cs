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

        public AIPlayer(Color color, Game g) : base(color, g)
        {
           
        }

        BasePiece getPiece(Board b)
        {
            Point p = new Point();
            Random rnd = new Random();

            while (true)
            {
                p.X = rnd.Next(0, 8);
                p.Y = rnd.Next(0, 8);

                if (b.getPieceAt(p) != null && b.getPieceAt(p).getColor() == this.getColor())
                {
                    break;
                }
            }

            return b.getPieceAt(p);
        }

        Point getMove(Board b, BasePiece bp)
        {
            Point p = new Point();
            List<Point> lp = new List<Point>();
            for (int i = 0; i < 8; i++)
            {
                    for (int j = 0; j < 8; j++)
                    {
                        p.X = i;
                        p.Y = j;
                        if (b.getValidMove(bp, p))
                            lp.Add(p);
                    }
            }
            

            Console.WriteLine("SIZE OF THE LIST OF POINTS " + lp.Count());
            if (lp.Count() == 0)
                return new Point(-1, -1);

            // determine if you can kill someone
            foreach (var np in lp)
            {
                if (b.getPieceAt(np) != null && b.getPieceAt(np).getColor() != this.getColor())
                {
                    Console.WriteLine("HASIDHAISDHAISDHAIH");
                    return np;
                }
            }

            // otherwise return a move
            var kaka = lp.Last();
            return kaka;
        }

        public bool killMove(Point lp, List<BasePiece> lbp, List<Point> lpp, ref Board b)
        {
            Console.WriteLine("SIZE OF LBP IN KILMOVE " + lbp.Count());
            for (int i = 0; i < lpp.Count(); i++)
            {
                BasePiece bp = lbp.ElementAt(i);
                Point np = lpp.ElementAt(i);
                if (b.getPieceAt(np) != null && b.getPieceAt(np).getColor() != this.getColor())
                {
                    b.updatePiece(lp, np, ref bp);
                    return true;
                }
                
            }

            return false;
        }

       public override int performMove(ref Board b)
        {
            Point lp = new Point();
            Point np = new Point();
            BasePiece bp = null;
            List<BasePiece> lbp = new List<BasePiece>();
            List<Point> lpp = new List<Point>();
            for (int i = 0; i < 10; i++)
            {
                do
                {
                    bp = getPiece(b);
                    lp = b.getBasePiecePoint(bp);
                    b.setSelectedPiece(bp);
                    b.setValidMoves(bp);
                    np = getMove(b, bp);
                } while (np.X == -1 || np.Y == -1);

                b.setSelectedPiece(null);
                b.resetValidMoves();
                lbp.Add(bp);
                lpp.Add(np);
            }

            if (killMove(lp, lbp, lpp, ref b))
            {
                b.setSelectedPiece(null);
                b.resetValidMoves();
                game.changeTurn(b);
                return 2;
            }

            b.updatePiece(lp, np, ref bp);
            b.setSelectedPiece(null);
            b.resetValidMoves();
            game.changeTurn(b);
            return 0;
        }

    }
}
