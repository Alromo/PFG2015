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
        private int _lifes;
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

        public int Lifes
        {
            get { return _lifes; }
            protected set { _lifes = value; }
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
            StatusJump = true;
            HeightJump = 90;
            PPosition = Position;
            Accel=new Vector2(0,0);
            Speed = new Vector2(0, 0);
            StartingJumpingPosition = Position.Y;
            _initialD = position;
            _isMC = true;
            HealthPoints = 50f;
            Lifes = 5;
            
        }
        #endregion 

        #region Methods Region
        protected override List<Point> cPointSelect()
        {
            List<Point> list = new List<Point>();

            Point pointU1 = new Point(3, 3);
            list.Add(pointU1);

            Point pointU2 = new Point(72, 3);
            list.Add(pointU2);

            Point pointL1 = new Point(1, 74);
            list.Add(pointL1);

            Point pointR1 = new Point(74, 74);
            list.Add(pointR1);

            Point pointD1 = new Point(5, 75);
            list.Add(pointD1);

            Point pointD2 = new Point(70, 75);
            list.Add(pointD2);

            Point pointL2 = new Point(1, 3);
            list.Add(pointL2);

            Point pointR2 = new Point(74, 3);
            list.Add(pointR2);

            Point pointL3 = new Point(1, 37);
            list.Add(pointL3);

            Point pointR3 = new Point(74, 37);
            list.Add(pointR3);


            

            return list;
        }

        public override KeyboardState Update(GameTime gameTime, List<Tile> tileList, int numberOfXTiles, int numberOfYTiles, float gravity, KeyboardState pState, GameWindow window)
        {
            if (Position.X > (numberOfXTiles * 75) - FrameSize.X)
                _position.X = (numberOfXTiles * 75) - FrameSize.X;
            if (Position.Y >  (numberOfYTiles*75) - FrameSize.Y)
                _position.Y = (numberOfYTiles * 75) - FrameSize.Y -50;

            if (Position.X < 0)
                _position.X = 0;
            if (Position.Y < 0)
                _position.Y = 0; 
            //this.collideRectangle.X = (int)position.X;
            //this.collideRectangle.Y = (int)position.Y;
            if (HealthPoints <= 0)
            {
                HealthPoints = 50;
                Lifes--;
               
                //_position.X = 75;
                //_position.Y = 225;
            }
            _pPosition=_position;
            
            List<bool> collisionChecker = collisionDetection(tileList, numberOfXTiles);
            _speed.X = _speed.X + _accel.X;
            _speed.Y = _speed.Y + _accel.Y;
            _accel.Y = _accel.Y + gravity;
            _clip = false;

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

            if (_speed.Y > 15f)
            {
                _speed.Y = 15f;
            }
            #endregion

            #region Collision Limiter Region
            if ((collisionChecker[2] == true || collisionChecker[6] == true || collisionChecker[8] == true) && _speed.X < 0)
            {
                _speed.X = 0;
               
            }
            if ((collisionChecker[3] == true || collisionChecker[7] == true || collisionChecker[9] == true) && _speed.X > 0)
            {
                _speed.X = 0;

            }
            if ((collisionChecker[2] == true || collisionChecker[6] == true || collisionChecker[8] == true) && _statusJump == true)
            {
                _position.X += 0.5f;
                _speed.X = 0.5f;
               
            }
            if ((collisionChecker[3] == true || collisionChecker[7] == true || collisionChecker[9] == true) && _statusJump==true)
            {
                _position.X -= 0.5f;
                _speed.X = -0.5f;
                

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

          /*  
            if (collisionChecker[3] == true || collisionChecker[7] == true || collisionChecker[9] == true)
            {
                Tile auxTile = tileList[tileDetection(tileList, numberOfXTiles, 75, _collidePoint[3].Y)];
                Tile auxTile2 = tileList[tileDetection(tileList, numberOfXTiles, 75, _collidePoint[7].Y)];
                Tile auxTile3  = tileList[tileDetection(tileList, numberOfXTiles, 75, _collidePoint[9].Y)];


                if (auxTile.TileType == 1 || auxTile2.TileType == 1 || auxTile2.TileType == 1)
                {
                    _position.X = auxTile.Position.X - 75-3;
                }

            }
            */


            #region Gravity and collision-at-falling repositioning Region
            if ((collisionChecker[4] == false && collisionChecker[5] == true) || (collisionChecker[4] == true && collisionChecker[5] == false) || (collisionChecker[4] == false && collisionChecker[5] == false))
            {
                Tile auxTile=tileList[tileDetection(tileList, numberOfXTiles, _collidePoint[4].X, 75+gravity)];
                Tile auxTile2 = tileList[tileDetection(tileList, numberOfXTiles, _collidePoint[5].X, 75 + gravity)];


                if (collisionChecker[4] == false && collisionChecker[5] == false)
                {
                    _position.Y = _position.Y + gravity;

                }
                else if (auxTile.TileType==1 || auxTile2.TileType==1)
                {
                    _position.Y = auxTile.Position.Y-75;
                }
       
            }

            if ((collisionChecker[4] == true || collisionChecker[5] == true))
            {
                Tile auxTile = tileList[tileDetection(tileList, numberOfXTiles, _collidePoint[4].X, 75 + gravity)];
                Tile auxTile2 = tileList[tileDetection(tileList, numberOfXTiles, _collidePoint[5].X, 75 + gravity)];
                if (auxTile.TileType == 1 || auxTile2.TileType == 1)
                {
                    _position.Y = auxTile.Position.Y - 75;
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
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && collisionChecker[2] == false && collisionChecker[6]==false && collisionChecker[8]==false)
            {
                _xDirection = false;
                _accel.X = _accel.X - 3f;
                if (_accel.X < -3f)
                {
                    _accel.X = -3f;
                }

            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && collisionChecker[3] == false && collisionChecker[7] == false && collisionChecker[9] == false)
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
                StatusJump = false;
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
            if( (collisionChecker[0] == true || collisionChecker[1] == true) && (collisionChecker[4]==false && collisionChecker[5]==false))
            {
                _position.Y = _position.Y +1;
                _speed.Y = 0;
                _accel.Y = 1f;
            }
            #endregion

            #region One-push detector
            if (Keyboard.GetState().IsKeyUp(Keys.Left)&&Keyboard.GetState().IsKeyUp(Keys.Right))
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

            nearTilesList.Add(tileList[tileDetection(tileList, xTiles, _collidePoint[2].X - 4, _collidePoint[2].Y)]);

            nearTilesList.Add(tileList[tileDetection(tileList, xTiles, _collidePoint[3].X +4, _collidePoint[3].Y)]);

            nearTilesList.Add(tileList[tileDetection(tileList, xTiles, _collidePoint[4].X+2, _collidePoint[4].Y + 3)]);

            nearTilesList.Add(tileList[tileDetection(tileList, xTiles, _collidePoint[5].X-2, _collidePoint[5].Y + 3)]);

            nearTilesList.Add(tileList[tileDetection(tileList, xTiles, _collidePoint[6].X-4, _collidePoint[6].Y)]);

            nearTilesList.Add(tileList[tileDetection(tileList, xTiles, _collidePoint[7].X + 4, _collidePoint[7].Y)]);

            nearTilesList.Add(tileList[tileDetection(tileList, xTiles, _collidePoint[8].X - 4, _collidePoint[8].Y)]);

            nearTilesList.Add(tileList[tileDetection(tileList, xTiles, _collidePoint[9].X + 4, _collidePoint[9].Y)]);



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
                if (actualtype == 2)
                {
                    collisionPointList.Add(true);
                    HealthPoints -= t.DamageDealt/8;
                }
            }
            return collisionPointList;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, ContentManager content)
        {
            if (HealthPoints <= 0)
            {
                _textureImage = content.Load<Texture2D>(@"images\fire");

            }

            spriteBatch.Draw(_textureImage, Position, Color.White);
        }



        #endregion
    }
}
