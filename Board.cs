using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Xml.Linq;


namespace Chess
{
    public enum Color { WHITE, BLACK }

    public class Board
    {
        private BasePiece [,]  pieces;
        private BasePiece selectedPiece;
        private Image black, white, selectedpiece;
        private bool[,] validMoves;

        private int gridSize;    

        public Board()
        {
            // Init the board 
            selectedPiece = null;
            pieces = new BasePiece[8, 8];
            validMoves = new bool[8, 8];
            black = Bitmap.FromFile("imgs/black.bmp");
            white = Bitmap.FromFile("imgs/white.bmp");
            selectedpiece = Bitmap.FromFile("imgs/selected.png");
            gridSize = 64;

            Init();
        }

        public void clearBoard()
        {
            // Add the empty pieces
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    pieces[x, y] = null;// new BasePiece(Color.WHITE); 
                }
            }
        }

        public void Init()
        {
            clearBoard();

            // Add all black pieces
            addPiece("Chess.Rook", Color.BLACK, 0, 0);
            addPiece("Chess.Knight", Color.BLACK, 1, 0);
            addPiece("Chess.Bishop", Color.BLACK, 2, 0);
            addPiece("Chess.Queen", Color.BLACK, 3, 0);
            addPiece("Chess.King", Color.BLACK, 4, 0);
            addPiece("Chess.Bishop", Color.BLACK, 5, 0);
            addPiece("Chess.Knight", Color.BLACK, 6, 0);
            addPiece("Chess.Rook", Color.BLACK, 7, 0);

            for (int x = 0; x < 8; x++)
                addPiece("Chess.Pawn", Color.BLACK, x, 1); // pieces[x, 1] = new Pawn(Color.BLACK);

            // Add all white pieces
            addPiece("Chess.Rook", Color.WHITE, 0, 7);
            addPiece("Chess.Knight", Color.WHITE, 1, 7);
            addPiece("Chess.Bishop", Color.WHITE, 2, 7);
            addPiece("Chess.Queen", Color.WHITE, 3, 7);
            addPiece("Chess.King", Color.WHITE, 4, 7);
            addPiece("Chess.Bishop", Color.WHITE, 5, 7);
            addPiece("Chess.Knight", Color.WHITE, 6, 7);
            addPiece("Chess.Rook", Color.WHITE, 7, 7);

            for (int x = 0; x < 8; x++)
                addPiece("Chess.Pawn", Color.WHITE, x, 6); //pieces[x, 6] = new Pawn(Color.WHITE);  
        }

        public bool lastManStanding(Color c)
        {
            int count = 0;
            Point p = new Point();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    p.X = i;
                    p.Y = j;
                    if (getPieceAt(p) != null && getPieceAt(p).getColor() == c)
                        count++;
                }
            }

            return count == 1;
        }

        internal bool checkMate(Color color)
        {
            int numChecks = 0;
            BasePiece checker = getNumCheckers(color, ref numChecks);

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

            // Reset king valid moves
            resetValidMoves();

            // More than 1 checker and the king can't escape = check mate
            if ((numChecks >= 2 && !kingMove) || (lastManStanding(king.getColor()) && !kingMove))
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
            else if(delta.X != 0 && Math.Abs(delta.X) == Math.Abs(delta.Y))
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
                        if(piece.validMove(getBasePiecePoint(piece), pos, this))
                        {
                            pinner = piece;
                            return pinner;
                        }                    
                    }
                }
            }

           // resetValidMoves();
            return null;
        }

        public void saveToFile(String filename, int mode, int turn)
        {
            int[] numbers = new int[7] { 41, 24, 16, 7, 10, 2, 17 };

            XElement gameElement = new XElement("Game");

            XElement modeElement = new XElement("Mode", mode);
            XElement turnElement = new XElement("Turn", turn);
            gameElement.Add(modeElement);
            gameElement.Add(turnElement);

            XElement board = new XElement("Board");

            // Loop over pieces
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if(pieces[x,y] != null)
                    {
                        XElement piece = new XElement("Piece");
                        piece.Add(new XAttribute("Name", pieces[x,y].GetType()));
                        piece.Add(new XAttribute("Color", pieces[x,y].getColor()));
                        piece.Add(new XAttribute("Posx", x));
                        piece.Add(new XAttribute("Posy", y));
                        board.Add(piece);
                    }
                }
            }

            gameElement.Add(board);
            gameElement.Save(filename);
        }

        public int loadFromFile(String filename, ref int turn)
        {
            // Set all pieces = null
            clearBoard();

            XDocument xdoc = XDocument.Load(filename);

            var a = xdoc.Descendants("Game").Select(s => new
            {
                Mode = s.Element("Mode"),
                Turn = s.Element("Turn")
            }).FirstOrDefault();

            Console.WriteLine(a.Mode.Value + " " + a.Turn.Value);

            int mode = Int32.Parse(a.Mode.Value);
            turn = Int32.Parse(a.Turn.Value);

            // Query piece information from the XML file
            var lv1s = from lv1 in xdoc.Descendants("Piece")
                       select new
                       {
                           Name = lv1.Attribute("Name").Value,
                           Color = lv1.Attribute("Color").Value,
                           Posx = lv1.Attribute("Posx").Value,
                           Posy = lv1.Attribute("Posy").Value
                       };

            // Loop over pieces and add them to pieces[x,y]
            foreach (var lv1 in lv1s)
            {
                Color c = Color.WHITE;
                if (lv1.Color == "BLACK")
                    c = Color.BLACK;

                addPiece(lv1.Name, c, Int32.Parse(lv1.Posx), Int32.Parse(lv1.Posy));

                Console.WriteLine(lv1.Name + " " + lv1.Color + " " + lv1.Posx + " " + lv1.Posy);
            }

            return mode;
        }

        private void addPiece(string name, Color color, int posx, int posy)
        {
            // Invalid position
            if (posx < 0 || posy < 0 || posx > 7 || posy > 7) {
                return;
            }

            BasePiece piece = null;
            if (name == "Chess.Pawn")
                piece = new Pawn(color);
            else if (name == "Chess.Rook")
                piece = new Rook(color);
            else if(name == "Chess.Knight")
                piece = new Knight(color);
            else if(name == "Chess.Bishop")
                piece = new Bishop(color);
            else if(name == "Chess.Queen")
                piece = new Queen(color);
            else if(name == "Chess.King")
                piece = new King(color);         

            if(piece != null)
            {
                pieces[posx, posy] = piece;
            }
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
