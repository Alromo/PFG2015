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
    public class Individual
    {

        #region Fields Region
        protected Texture2D _textureImage;
        protected Vector2 _position;
        protected Point _frameSize;
        protected Vector2 _speed;
        protected Boolean _xDirection;
        protected Rectangle _collideRectangle;
        protected List<Point> _collidePoint;
        protected Vector2 _accel;
        protected Boolean _isMC;
        protected int _healthPoints;

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

        protected Point FrameSize
        {
            get { return _frameSize; }
        }

        public Vector2 Speed
        {
            get { return _speed; }
            protected set { _speed = value; }
        }

        protected Boolean XDirection
        {
            get { return _xDirection; }
            set { _xDirection = value; }
        }

        protected Rectangle CollideRectangle
        {
            get { return _collideRectangle; }
            set { _collideRectangle = value; }
        }

        protected List<Point> CollidePoint
        {
            get { return _collidePoint; }
        }

        public Vector2 Accel
        {
            get { return _accel; }
            protected set { _accel = value; }
        }
        public Boolean isMC
        {
            get { return _isMC; }
        }
        public int HealthPoints
        {
            get { return _healthPoints; }
            set { _healthPoints = value; }
        }
        #endregion

        #region Builder Region
        public Individual(Texture2D textureImage, Vector2 position, Point frameSize, Vector2 speed, Vector2 accel)
        {
            _textureImage = textureImage;
            Position = position;
            _frameSize = frameSize;
            Speed = speed;
            XDirection=true;
            CollideRectangle = new Rectangle((int)position.X, (int)position.Y, frameSize.X, frameSize.Y);
            _collidePoint = cPointSelect();
            Accel = accel;
        }

        public Individual()
        {
            _textureImage=null;
            Position=new Vector2(0, 0);
            _frameSize = new Point(25, 25);
            Speed = new Vector2(0, 0);
            XDirection = true;
            CollideRectangle = new Rectangle((int)_position.X, (int)_position.Y, _frameSize.X, _frameSize.Y);
            _collidePoint = cPointSelect();
            Accel = new Vector2(0, 0);
            _isMC = false;

        }
        #endregion

        #region Methods Region
        public virtual KeyboardState Update(GameTime gameTime, List<Tile> tileList, int numberOfXTiles, int numberOfYTiles, float gravity, KeyboardState pState, GameWindow window)
        {
            Rectangle auxRectangle = new Rectangle((int)Position.X, (int)Position.Y, CollideRectangle.Width, CollideRectangle.Height);
            CollideRectangle = auxRectangle;
            return Keyboard.GetState();            
        }


        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, ContentManager content)
        {
            
            spriteBatch.Draw(_textureImage, Position, Color.White);
   
        }

        protected virtual List<Point> cPointSelect()
        {
            List<Point> list = new List<Point>();

            Point pointL1 = new Point(0, 75);
            list.Add(pointL1);

            Point pointR1 = new Point(75, 75);
            list.Add(pointR1);
            
            Point pointD1 = new Point(0, 75);
            list.Add(pointD1);

            Point pointD2 = new Point(75, 75);
            list.Add(pointD2);

            return list;
        }
        protected virtual int tileDetection(List<Tile> tileList, int xTiles, float x, float y)
        {
            int posicionX = (int)Position.X+(int)x;
            int posicionY = (int)Position.Y+(int)y;

            int xTiled = 0;
            int yTiled = 0;
            float xRemainder = 0;
            float yRemainder = 0;
            xTiled = posicionX / tileList[0].FrameSize.X;
            yTiled = posicionY / tileList[0].FrameSize.Y;
            xRemainder = posicionX % tileList[0].FrameSize.X;
            yRemainder = posicionY % tileList[0].FrameSize.Y;
            
            if (xRemainder > 0)
                xTiled = xTiled + 1;
            if (yRemainder > 0)
                yTiled = yTiled + 1;

            return (((yTiled - 1) * xTiles) + xTiled)-1;

        }

        protected virtual List<bool> collisionDetection(List<Tile> tileList, int xTiles)
        {
            List<Tile> nearTilesList = new List<Tile>();
            List<bool> collisionPointList = new List<bool>();
           
            
            nearTilesList.Add(tileList[tileDetection(tileList, xTiles, CollidePoint[0].X-1, CollidePoint[0].Y)]);

            nearTilesList.Add(tileList[tileDetection(tileList, xTiles, CollidePoint[1].X+1, CollidePoint[1].Y)]);

            nearTilesList.Add(tileList[tileDetection(tileList, xTiles, CollidePoint[2].X+1, CollidePoint[2].Y+1)]);

            nearTilesList.Add(tileList[tileDetection(tileList, xTiles, CollidePoint[3].X-1, CollidePoint[3].Y+1)]);

            foreach (Tile t in nearTilesList)
            {
                int actualtype = t.TileType;

                if (actualtype == 1)
                {
                    collisionPointList.Add(true);
                } 
                if (actualtype == 0)
                {
                    collisionPointList.Add(false);
                }
                
            }

            return collisionPointList;
            
        }
    }
        #endregion

}
