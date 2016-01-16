using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Chess
{
    public class TextureKey
    {
        public TextureKey(Type _type, Color _color)
        {
            type = _type;
            color = _color;
        }

        public override bool Equals(object obj)
        {
            TextureKey other = obj as TextureKey;

            if (other == null)
                return false;

            return type == other.type && color == other.color;
        }

        public override int GetHashCode()
        {
            return (this.type.ToString() + this.color.ToString()).GetHashCode();
        }

        public Type type;
        public Color color;
    }

    class BoardUI
    {
        Dictionary<TextureKey, Image> textures;
        private Image black, white, selectedpiece;

        public BoardUI()
        {
            // Load piece textures
            textures = new Dictionary<TextureKey, Image>();
            textures.Add(new TextureKey(typeof(Pawn), Color.WHITE), Bitmap.FromFile("imgs/white_pawn.bmp"));
            textures.Add(new TextureKey(typeof(Pawn), Color.BLACK), Bitmap.FromFile("imgs/black_pawn.bmp"));
            textures.Add(new TextureKey(typeof(Rook), Color.WHITE), Bitmap.FromFile("imgs/white_rook.png"));
            textures.Add(new TextureKey(typeof(Rook), Color.BLACK), Bitmap.FromFile("imgs/black_rook.bmp"));
            textures.Add(new TextureKey(typeof(Knight), Color.WHITE), Bitmap.FromFile("imgs/white_knight.bmp"));
            textures.Add(new TextureKey(typeof(Knight), Color.BLACK), Bitmap.FromFile("imgs/black_knight.bmp"));
            textures.Add(new TextureKey(typeof(Bishop), Color.WHITE), Bitmap.FromFile("imgs/white_bishop.bmp"));
            textures.Add(new TextureKey(typeof(Bishop), Color.BLACK), Bitmap.FromFile("imgs/black_bishop.bmp"));
            textures.Add(new TextureKey(typeof(Queen), Color.WHITE), Bitmap.FromFile("imgs/white_queen.bmp"));
            textures.Add(new TextureKey(typeof(Queen), Color.BLACK), Bitmap.FromFile("imgs/black_queen.bmp"));
            textures.Add(new TextureKey(typeof(King), Color.WHITE), Bitmap.FromFile("imgs/white_king.bmp"));
            textures.Add(new TextureKey(typeof(King), Color.BLACK), Bitmap.FromFile("imgs/black_king.bmp"));

            black = Bitmap.FromFile("imgs/black.bmp");
            white = Bitmap.FromFile("imgs/white.bmp");
            selectedpiece = Bitmap.FromFile("imgs/selected.png");
        }

        public void Draw(Graphics graphics, BasePiece[,] pieces, bool[,] validMoves, BasePiece selectedPiece, int gridSize)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    // Draw the white or black background
                    if ((x + y) % 2 == 0)
                    {
                        graphics.DrawImage(black, x * gridSize, y * gridSize, gridSize, gridSize);
                    }
                    else
                    {
                        graphics.DrawImage(white, x * gridSize, y * gridSize, gridSize, gridSize);
                    }

                    // Draw the piece
                    BasePiece piece = pieces[x, y];
                    if (piece != null)
                    {
                        Image image;
                        TextureKey key = new TextureKey(piece.GetType(), piece.getColor());
                        textures.TryGetValue(key, out image);
                        graphics.DrawImage(image, x * gridSize, y * gridSize, gridSize, gridSize);
                    }

                    // Draw selected piece
                    if (selectedPiece != null)
                    {
                        if (pieces[x, y] == selectedPiece)
                            graphics.DrawImage(selectedpiece, x * gridSize, y * gridSize, gridSize, gridSize);

                        // draw valid moves
                        if (validMoves[x, y] == true)
                            graphics.DrawImage(selectedpiece, x * gridSize, y * gridSize, gridSize, gridSize);
                    }
                }
            }
        }
    }
}
