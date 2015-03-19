#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Proto3
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;        
        KeyboardState pState=Keyboard.GetState();
        Camera2d cam = new Camera2d();
        Level actualLevel;
        SpriteFont hudFont;
        int gameState;
       //Boolean FUNFLAG = true; ///////////////////////////////////////////////////////////
        

        public Game1(): base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here            
            cam.Pos = new Vector2(500.0f, 200.0f);
            gameState = 0;
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            switch (gameState)
            {
                case 1:
                    actualLevel = new Level("lvl.txt", Content, 0);
                    break;
                default:
                    actualLevel = new Level("lvl.txt", Content, 0);
                    break;

            }
            hudFont = Content.Load<SpriteFont>("Fuente");
            actualLevel.charge();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            actualLevel = new Level("lvl.txt", Content,0);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            gameState = actualLevel.LevelNum;
            foreach (Tile t in actualLevel._tileList)
            {
                t.Update(gameTime);
            }
            
            foreach (Individual i in actualLevel._individualList)
            {
                pState = i.Update(gameTime, actualLevel._tileList, actualLevel._xTiles, actualLevel._yTiles, 0.4f, pState, Window);
                if(i.isMC==true){
                    cam.Update(i.Position, 0f, 0f, GraphicsDevice.Viewport, actualLevel._xTiles, actualLevel._yTiles, 75, 75);
                }
            }
            
           // if (Keyboard.GetState().IsKeyDown(Keys.R))
            //{
           
            
            //if(cam.Rotation<Math.PI-0.007)
             //  cam.Rotation += 0.01f;
            
            //}
           
          
            /*
            if (cam.Zoom > 2)
            {
                FUNFLAG = false;
            }
            if (cam.Zoom == 1)
            {
                FUNFLAG = true;
            }
            if (FUNFLAG==true)
            {
                cam.Zoom += 0.1f;

            }
            else
            {
                cam.Zoom -= 0.1f;
            }*/
           

            if (keyboardState.IsKeyDown(Keys.T))
            {
                UnloadContent();
                LoadContent();
            }

            if (keyboardState.IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);
            MainCharacter MC=null;
            spriteBatch.Begin(SpriteSortMode.Immediate,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        cam.get_transformation(GraphicsDevice));
            foreach (Tile t in actualLevel._tileList)
            {
                t.Draw(gameTime, spriteBatch);
            }

            foreach (Individual i in actualLevel._individualList)
            {
                if (i.isMC){
                    MC = (MainCharacter)i;
                }
                i.Draw(gameTime, spriteBatch, Content);
            }
            // TODO: Add your drawing code here
            Vector2 fontPosLeft1 = new Vector2(cam.Pos.X - GraphicsDevice.Viewport.Width/2 +50, cam.Pos.Y - GraphicsDevice.Viewport.Height/2+50);
            Vector2 fontPosRight1 = new Vector2(cam.Pos.X + GraphicsDevice.Viewport.Width / 4 , cam.Pos.Y - GraphicsDevice.Viewport.Height / 2 + 50);

            spriteBatch.DrawString(hudFont, "Vida: "+Math.Round(MC.HealthPoints,0).ToString(), fontPosLeft1, Color.White);
            spriteBatch.DrawString(hudFont, "Vidas: " + MC.Lifes.ToString(), fontPosRight1, Color.White);



            base.Draw(gameTime);
            spriteBatch.End();

        }

  /*      protected void levelRead(string levelName)
        {
            string line;
            int countY = 0;
            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(@"C:\Users\Nerviosillo\lvl.txt");
            while ((line = file.ReadLine()) != null)
            {
                int countX = 0;
                foreach (char i in line)
                {
                    if (i=='0')
                    {
                        Tile2 air = new Tile2(Content.Load<Texture2D>(@"images\void"), new Vector2(countX, countY), new Point(75, 75), 0, new Vector2(0, 0));
                        tileList.Add(air);
                    }
                    if (i=='1')
                    {
                        Tile1 block = new Tile1(Content.Load<Texture2D>(@"images\block"), new Vector2(countX, countY), new Point(75, 75), 0, new Vector2(0, 0));
                        tileList.Add(block);
                    }
                    if (i == 'I')
                    {
                        Tile2 air = new Tile2(Content.Load<Texture2D>(@"images\void"), new Vector2(countX, countY), new Point(75, 75), 0, new Vector2(0, 0));
                        tileList.Add(air);
                        MainCharacter dragon = new MainCharacter(Content.Load<Texture2D>(@"images\dragon2"), new Vector2(countX, countY), new Point(75, 75));
                        individualList.Add(dragon);                        
                    }
                    countX = countX + 75;
                }
                countY = countY + 75;
            }
            xTiles = 16;
            yTiles = 14;
            file.Close();
        }*/
    }
}
