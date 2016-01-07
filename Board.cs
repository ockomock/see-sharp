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

        public bool getValidMove(BasePiece sb, Point p)
        {
            bool valid = validMoves[p.X, p.Y];

            // Test if the move results in check
            if (valid)
            {
                // [[Temporarily]] move the piece!!
                Point oldPoint = getBasePiecePoint(sb);
                updatePiece(oldPoint, p, ref sb);

                // Test opponents pieces
                if (isChecking((sb.getColor() == Color.BLACK ? Color.WHITE : Color.BLACK)))
                {
                    // Move back the piece!
                    updatePiece(p, oldPoint, ref sb);

                    Console.WriteLine("Check! Invalid move!");

                    return false;
                }

                // Move back the piece!
                updatePiece(p, oldPoint, ref sb);
            }

            return valid;
        }

        private Point getKingPos(Color color)
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

        public bool isChecking(Color color)
        {
            Color c = (color == Color.BLACK ? Color.WHITE : Color.BLACK);
            Point kingPos = getKingPos(c);

            // Test opponents pieces
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    BasePiece piece = pieces[i, j];
                    if (piece != null && piece.getColor() != c)
                    {
                        resetValidMoves();
                        piece.setValidMoves(this);

                        // Check!
                        if (validMoves[kingPos.X, kingPos.Y])
                            return true;
                    }
                }
            }

            return false;
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
