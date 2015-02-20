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
    public class MainCharacter : Individual
    {
        #region Fields Region
        private bool _statusJump;
        private float _heightJump;
        private float _startingJumpingPosition;
        private Vector2 _pPosition;
        private Vector2 _initialD;
        #endregion

        #region Properties Region
        public Boolean StatusJump
        {
            get { return _statusJump; }
            protected set { _statusJump = value; }
        }

        public float HeightJump
        {
            get { return _heightJump; }
            protected set { _heightJump = value; }
        }

        public float StartingJumpingPosition
        {
            get { return _startingJumpingPosition; }
            protected set { _startingJumpingPosition = value; }
        }

        public Vector2 PPosition
        {
            get { return _pPosition; }
            protected set { _pPosition = value; }
        }
        #endregion

        #region Builder Region
        public MainCharacter(Texture2D textureImage, Vector2 position, Point frameSize)
        {
            this._textureImage = textureImage;
            Position = position;
            _frameSize = frameSize;
            XDirection=true;
            CollideRectangle = new Rectangle((int)position.X, (int)position.Y, frameSize.X, frameSize.Y);
            _collidePoint = cPointSelect();
            StatusJump = false;
            HeightJump = 90;
            PPosition = Position;
            Accel=new Vector2(0,0);
            Speed = new Vector2(0, 0);
            StartingJumpingPosition = Position.Y;
            _initialD = position;
        }
        #endregion 

        #region Methods Region
        protected override List<Point> cPointSelect()
        {
            List<Point> list = new List<Point>();

            Point pointU1 = new Point(3, 0);
            list.Add(pointU1);

            Point pointU2 = new Point(72, 0);
            list.Add(pointU2);

            Point pointL1 = new Point(1, 74);
            list.Add(pointL1);

            Point pointR1 = new Point(74, 74);
            list.Add(pointR1);

            Point pointD1 = new Point(5, 75);
            list.Add(pointD1);

            Point pointD2 = new Point(70, 75);
            list.Add(pointD2);


            

            return list;
        }

        public override KeyboardState Update(GameTime gameTime, List<Tile> tileList, int numberOfXTiles, float gravity, KeyboardState pState)
        {
            //this.collideRectangle.X = (int)position.X;
            //this.collideRectangle.Y = (int)position.Y;
             
            _pPosition=_position;
            
            List<bool> collisionChecker = collisionDetection(tileList, numberOfXTiles);
            _speed.X = _speed.X + _accel.X;
            _speed.Y = _speed.Y + _accel.Y;
            _accel.Y = _accel.Y + gravity;

            #region Speed/Accel Limiter Region
            if (_speed.X > 7f)
            {
                _speed.X = 7f;
            }
            if (_speed.X < -7f)
            {
                _speed.X = -7f;
            }

            if (_accel.Y > 0.75f)
            {
                _accel.Y = 0.75f;
            }
            
            if (_speed.Y < -15f)
            {
                _speed.Y = -15f;
            }
            #endregion

            #region Collision Limiter Region
            if (collisionChecker[2] == true && _speed.X < 0)
            {
                _speed.X = 0;
            }
            if (collisionChecker[3] == true && _speed.X > 0)
            {
                _speed.X = 0;
            }
            if ((collisionChecker[0] == true || collisionChecker[1] == true) && _speed.Y < 0)
            {
                _speed.Y = 0;
            }
            if ((collisionChecker[4] == true || collisionChecker[5] == true) && _speed.Y > 0)
            {
                _speed.Y = 0;
            }
            #endregion

            #region Position Change Region
            _position += _speed;
            #endregion

            #region Gravity and collision-at-falling repositioning Region 
            if (collisionChecker[4] == false || collisionChecker[5] == false && _statusJump ==false)
            {
                Tile auxTile=tileList[tileDetection(tileList, numberOfXTiles, _collidePoint[4].X, 75+gravity)];
                Tile auxTile2 = tileList[tileDetection(tileList, numberOfXTiles, _collidePoint[5].X, 75 + gravity)];

                if (auxTile.TileType==1 || auxTile2.TileType==1)
                {
                    _position.Y = auxTile.Position.Y-75;
                }
                
                else if (collisionChecker[4] == false && collisionChecker[5] == false)
                {
                    _position.Y = _position.Y + gravity;
                }
            }
            #endregion


            #region Jump Button Region
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !pState.IsKeyDown(Keys.Up) && (collisionChecker[0] == false || collisionChecker[1] == false) && (StatusJump==false))
            {
                _startingJumpingPosition = _position.Y;
                _accel.Y=_accel.Y-3.2f;
            }
            #endregion

            #region Left and right movement Region
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && collisionChecker[2]==false)
            {
                _xDirection = false;
                _accel.X = _accel.X - 3f;
                if (_accel.X < -3f)
                {
                    _accel.X = -3f;
                }

            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && collisionChecker[3] == false)
            {
                _xDirection = true;                
                _accel.X=_accel.X + 3f;
                if (_accel.X > 3f)
                {
                    _accel.X = 3f;
                }

            }
            #endregion

            #region Acceleration breaks Region
            if (_accel.X > 0)
            {
                _accel.X = _accel.X - 1f;
            }
            if (_accel.X < 0)
            {
                _accel.X = _accel.X + 1f;
            }
            #endregion

        
            collisionChecker = collisionDetection(tileList, numberOfXTiles);

            #region Behavior at landing
            if (collisionChecker[4] == true || collisionChecker[5] == true) 
            {
                StatusJump = false ;                               
            }
            if (collisionChecker[4] == false && collisionChecker[5] == false)
            {
                StatusJump = true;
            }
            #endregion

            #region Jumping Limiter
            if ((_startingJumpingPosition - _position.Y) > _heightJump )
            {  
                _accel.Y = 1f;
                //_speed.Y = 0;
            }
            if( collisionChecker[0] == true || collisionChecker[1] == true)
            {
                _position.Y = _position.Y +1;
                _speed.Y = 0;
                _accel.Y = 1f;
            }
            #endregion

            #region One-push Jumping detector
            if (Keyboard.GetState().GetPressedKeys().Count() == 0)
            {
                _speed.X = 0;
                _accel.X = 0;
            }
            #endregion


            return Keyboard.GetState();
            
        }

        protected override List<bool> collisionDetection(List<Tile> tileList, int xTiles)
        {
            List<Tile> nearTilesList = new List<Tile>();
            List<bool> collisionPointList = new List<bool>();

            nearTilesList.Add(tileList[tileDetection(tileList, xTiles, _collidePoint[0].X+2, _collidePoint[0].Y)]);

            nearTilesList.Add(tileList[tileDetection(tileList, xTiles, _collidePoint[1].X-2, _collidePoint[1].Y)]);

            nearTilesList.Add(tileList[tileDetection(tileList, xTiles, _collidePoint[2].X - 1, _collidePoint[2].Y)]);

            nearTilesList.Add(tileList[tileDetection(tileList, xTiles, _collidePoint[3].X +2, _collidePoint[3].Y)]);

            nearTilesList.Add(tileList[tileDetection(tileList, xTiles, _collidePoint[4].X+2, _collidePoint[4].Y + 1)]);

            nearTilesList.Add(tileList[tileDetection(tileList, xTiles, _collidePoint[5].X-2, _collidePoint[5].Y + 1)]);

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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_textureImage, _position, Color.White);
            spriteBatch.End();
        }



        #endregion
    }
}
