using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace Chess
{
    public enum Color { WHITE, BLACK }

    public class Board
    {
       // private List<BasePiece> pieces;

        private BasePiece [,]  pieces;
        private BasePiece selectedPiece;
        private Image black, white, selectedpiece;
        private bool[,] validMoves;

        private int gridSize;    

        public Board()
        {
            // Init the board 

            selectedPiece = null;// new BasePiece();
            pieces = new BasePiece[8, 8];
            validMoves = new bool[8, 8];
            black = Bitmap.FromFile("imgs/black.bmp");
            white = Bitmap.FromFile("imgs/white.bmp");
            selectedpiece = Bitmap.FromFile("imgs/selected.png");
            gridSize = 64;

            Init();
        }

        public void Init()
        {
            // Add all black pieces
            pieces[0, 0] = new Rook(Color.BLACK);
            pieces[1, 0] = new Knight(Color.BLACK);
            pieces[2, 0] = new Bishop(Color.BLACK);
            pieces[3, 0] = new Queen(Color.BLACK);
            pieces[4, 0] = new King(Color.BLACK);
            pieces[5, 0] = new Bishop(Color.BLACK);
            pieces[6, 0] = new Knight(Color.BLACK);
            pieces[7, 0] = new Rook(Color.BLACK);

            for (int x = 0; x < 8; x++)
                pieces[x, 1] = new Pawn(Color.BLACK);

            // Add all white pieces
            pieces[0, 7] = new Rook(Color.WHITE);
            pieces[1, 7] = new Knight(Color.WHITE);
            pieces[2, 7] = new Bishop(Color.WHITE);
            pieces[3, 7] = new Queen(Color.WHITE);
            pieces[4, 7] = new King(Color.WHITE);
            pieces[5, 7] = new Bishop(Color.WHITE);
            pieces[6, 7] = new Knight(Color.WHITE);
            pieces[7, 7] = new Rook(Color.WHITE);

            for (int x = 0; x < 8; x++)
                pieces[x, 6] = new Pawn(Color.WHITE);


            // Add the empty pieces
            for (int x = 0; x < 8; x++)
            {
                for (int y = 2; y < 6; y++)
                {
                    pieces[x, y] = null;// new BasePiece(Color.WHITE); 
                }
            }
        }

        internal bool checkMate(Color color)
        {
            int numChecks = 0;
            BasePiece checker = getNumCheckers(color, ref numChecks);

            Console.WriteLine("Num checkers: " + numChecks);

            if (numChecks == 0)
                return false;

            resetValidMoves();
            Point kingPos = getKingPos(color);
            setValidMoves(pieces[kingPos.X, kingPos.Y]);

            // Test if the king can move
            bool kingMove = false;
            BasePiece king = pieces[kingPos.X, kingPos.Y];
            if (getValidMove(king, new Point(kingPos.X + 1, kingPos.Y)) || getValidMove(king, new Point(kingPos.X - 1, kingPos.Y)) ||
                getValidMove(king, new Point(kingPos.X, kingPos.Y + 1)) || getValidMove(king, new Point(kingPos.X, kingPos.Y - 1)) ||
                getValidMove(king, new Point(kingPos.X - 1, kingPos.Y - 1)) || getValidMove(king, new Point(kingPos.X + 1, kingPos.Y + 1)) ||
                getValidMove(king, new Point(kingPos.X + 1, kingPos.Y - 1)) || getValidMove(king, new Point(kingPos.X - 1, kingPos.Y + 1)))
            {
                kingMove = true;
            }

            // More than 1 checker and the king can't escape = check mate
            if (numChecks >= 2 && !kingMove)
                return true;

            // Test if the checker can be captured
            Color c = (color == Color.BLACK ? Color.WHITE : Color.BLACK);
            BasePiece pinner = isPinning(c, getBasePiecePoint(checker));
            if (kingMove)
                return false;

            if (pinner != null)
            {
                if(pinner.GetType() != typeof(King))
                    return false;
            }

            // Test if the checking path can be intersected (impossible for knights)
            if (checker.GetType() == typeof(Knight) || !canIntersect(color, checker))
                return true;

            return false;
        }

        private bool canIntersect(Color color, BasePiece checker)
        {
            Point kingPos = getKingPos(color);
            Point checkerPos = getBasePiecePoint(checker);

            Point delta = new Point(checkerPos.X - kingPos.X, checkerPos.Y - kingPos.Y);

            if (Math.Abs(delta.X) + Math.Abs(delta.Y) <= 2 && Math.Abs(delta.X) <= 1 && Math.Abs(delta.Y) <= 1)
            {
                return false;
            }

            int max = 0;
            // up - down
            if (delta.X == 0 && delta.Y != 0)
                max = Math.Abs(delta.Y);
            else if(delta.X != 0 && delta.Y == 0)
                max = Math.Abs(delta.X);
            else if(delta.X != 0 && delta.X == delta.Y)
                max = Math.Abs(delta.X); 

            for (int i = 0; i < max; i++)
            {
                int xdir = (delta.X == 0 ? 0 : delta.X / Math.Abs(delta.X));
                int ydir = (delta.Y == 0 ? 0 : delta.Y / Math.Abs(delta.Y));
                if (isPinning(color, new Point(kingPos.X + i * xdir, kingPos.Y + i * ydir)) != null)
                    return true;
            }

            return false;
        }

        private BasePiece getNumCheckers(Color color, ref int numCheckers)
        {
            BasePiece checker = null;
            Point kingPos = getKingPos(color);

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    BasePiece piece = pieces[x, y];
                    if (piece != null && piece.getColor() != color && piece.validMove(getBasePiecePoint(piece), kingPos, this))
                    {
                        numCheckers++;
                        checker = piece;
                    }
                }
            }

            return checker;
        }

        internal void setValidMoves(BasePiece bp)
        {
            // Let the piece test allowed moves
            bp.setValidMoves(this);
        }

        public void Draw(Graphics graphics)
        {
            for(int x = 0; x < 8; x++)
            {
                for(int y = 0; y < 8; y++)
                {
                    // Draw the white or black background
                    if((x+y) % 2 == 0)
                    {
                        //graphics.DrawImage(black, new Point(x * gridSize, y * gridSize));
                        graphics.DrawImage(black, x * gridSize, y * gridSize, gridSize, gridSize);
                    }
                    else
                    {
                        //graphics.DrawImage(white, new Point(x * gridSize, y * gridSize));
                        graphics.DrawImage(white, x * gridSize, y * gridSize, gridSize, gridSize);
                    }

                    // Draw the piece
                    if(pieces[x, y] != null)
                        graphics.DrawImage(pieces[x, y].getImage(), x * gridSize, y * gridSize, gridSize, gridSize);

                    // Draw selected piece
                    if (getSelectedPiece() != null)
                    {
                        if (pieces[x, y] == getSelectedPiece())
                            graphics.DrawImage(selectedpiece, x * gridSize, y * gridSize, gridSize, gridSize);

                        // draw valid moves
                        if (validMoves[x, y] == true)
                            graphics.DrawImage(selectedpiece, x * gridSize, y * gridSize, gridSize, gridSize);
                    }

                }
            }
        }

        public void resetValidMoves()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    validMoves[i, j] = false;
        }

        public void setValidMoveTrue(Point p)
        {
            validMoves[p.X, p.Y] = true;
        }

        public bool isValidMove(Point p)
        {
            return validMoves[p.X, p.Y];
        }

        public bool getValidMove(BasePiece sb, Point p)
        {
            // OBS!!!! Pieces will dissapear if there is a capture and the move acctually is invalid!!!
            if (p.X < 0 || p.Y < 0 || p.X > 7 || p.Y > 7)
                return false;

            // NOTE: TEMP
            setValidMoves(sb);

            bool valid = validMoves[p.X, p.Y];

            // Test if the move results in check
            if (valid)
            {
                BasePiece backupPiece = pieces[p.X, p.Y];

                // Can't capture the king
                if (backupPiece != null && backupPiece.GetType() == typeof(King))
                    return false;

                // [[Temporarily]] move the piece!!
                Point oldPoint = getBasePiecePoint(sb);
                updatePiece(oldPoint, p, ref sb);

                // Test opponents pieces
                Point kingPos = getKingPos(sb.getColor());
                if (isPinning(sb.getColor(), kingPos) != null)
                {
                    // Move back the piece!
                    updatePiece(p, oldPoint, ref sb);
                    pieces[p.X, p.Y] = backupPiece;

                    Console.WriteLine("Check! Invalid move!");

                    return false;
                }

                // Move back the piece!
                updatePiece(p, oldPoint, ref sb);
                pieces[p.X, p.Y] = backupPiece;
            }

            return valid;
        }

        public Point getKingPos(Color color)
        {
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++)
                {
                    if (pieces[i, j] == null)
                        continue;

                    if (pieces[i, j].GetType() == typeof(King) && pieces[i, j].getColor() == color)
                        return new Point(i, j);
                }
            }

            return new Point(-1, -1);
        }

        public BasePiece isPinning(Color color, Point pos)
        {
            BasePiece pinner = null;
            // Test opponents pieces
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    BasePiece piece = pieces[i, j];
                    if (piece != null && piece.getColor() != color)
                    {                     
                       resetValidMoves();
                        piece.setValidMoves(this);

                        // Pinning!
                        if (validMoves[pos.X, pos.Y])
                        {
                            // This is really stupid but all valid moves have to be reset since
                            // this function is used even for the piece NOT moving to test for check
                           // resetValidMoves();
                            pinner = piece;
                            return pinner;
                        }
                    }
                }
            }

            resetValidMoves();
            return null;
        }

        public BasePiece getPieceAt(int x, int y)
        {
            return pieces[x,y];
        }

        public BasePiece getPieceAt(Point p)
        {
            return pieces[p.X, p.Y];
        }

        public BasePiece getSelectedPiece()
        {
            return selectedPiece;
        }

        public void setSelectedPiece(BasePiece bp)
        {
            selectedPiece = bp;
        }

        public int getGridSize()
        {
            return gridSize;
        }

        public Point getBasePiecePoint(BasePiece b)
        {
            Point p;

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (pieces[i, j] == b)
                    {
                        p = new Point(i, j);
                        return p;
                    }

            return new Point(0, 0);
        }

        public void updatePiece(Point lp, Point np, ref BasePiece bp)
        {
            // TODO!! Create some special event if a piece is captured!
            pieces[lp.X, lp.Y] = null;// ew BasePiece(Color.WHITE);
            pieces[np.X, np.Y] = bp;
            
        }

        public Point pixelToGrid(Point pixel)
        {
            // (Assuming the board starts at (0,0)
            Point grid = new Point(pixel.X / gridSize, pixel.Y / gridSize);

            // Outside the board?
            if (grid.X < 0 || grid.Y < 0 || grid.X > 7 || grid.Y > 7)
                grid = new Point(-1, -1);

            return grid;
        }


    }
}
