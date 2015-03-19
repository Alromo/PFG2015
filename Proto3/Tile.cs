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
    public class Tile
    {
        #region Fields Region
        protected Texture2D _textureImage;
        protected Vector2 _position;
        protected Point _frameSize;
        protected Vector2 _speed;
        protected Rectangle _collideRectangle;
        protected int _tileType;
        protected float _damageDealt;
        #endregion

        #region Properties Region
        protected Texture2D TextureImage
        {
            get { return _textureImage; }
        }

        public Vector2 Position
        {
            get { return _position; }
            protected set { _position = value; }
        }

        public Point FrameSize
        {
            get { return _frameSize; }
        }


        public Vector2 Speed
        {
            get { return _speed; }
            protected set { _speed = value; }
        }

        protected Rectangle CollideRectangle
        {
            get { return _collideRectangle; }
            set { _collideRectangle = value; }
        }

        public int TileType
        {
            get { return _tileType; }
            protected set { _tileType = value; }
        }

        public float DamageDealt
        {
            get { return _damageDealt; }
            set { _damageDealt = value; }
        }
        #endregion


        #region Builder Region
        public Tile()
        {
            _textureImage = null;
            Position = new Vector2(0, 0);
            Speed = new Vector2(0, 0);
            _frameSize = new Point(25, 25);
            CollideRectangle = new Rectangle((int)_position.X, (int)_position.Y, _frameSize.X, _frameSize.Y);
            TileType = -1;
            DamageDealt = 0;
        }

        public Tile(Texture2D textureImage, Vector2 position, Point frameSize, Vector2 speed)
        {
            _textureImage = textureImage;
            Position = position;
            _frameSize = frameSize;
            Speed = speed;
            CollideRectangle = new Rectangle((int)position.X, (int)position.Y, frameSize.X, frameSize.Y);
            TileType = -1;
        }
        #endregion

        #region Methods Region
        public virtual void Update(GameTime gameTime)
        {
            Rectangle auxRectangle = new Rectangle((int)Position.X, (int)Position.Y, CollideRectangle.Width, CollideRectangle.Height);
            CollideRectangle = auxRectangle;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureImage, Position, Color.White);
        }

        public virtual Boolean Collide(Tile another)
        {
            if (CollideRectangle.Intersects(another.CollideRectangle))
                return true;
            return false;
        }
        #endregion

    }
}
