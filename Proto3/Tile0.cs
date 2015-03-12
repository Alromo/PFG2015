using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace Proto3
{
    public class Tile0 : Tile
    {
        public Tile0(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed)
        {
            _textureImage = textureImage;
            Position = position;
            _frameSize = frameSize;
            CollideRectangle = new Rectangle((int)position.X, (int)position.Y, frameSize.X, frameSize.Y);
            Speed = speed;
            TileType = 0;
        }

       
    }
}
