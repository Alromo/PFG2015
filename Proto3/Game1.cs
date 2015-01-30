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
        List<Tile> tileList;
        List<Individual> individualList;
        int xTiles;
        int yTiles;
        KeyboardState pState=Keyboard.GetState();
        

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
            tileList = new List<Tile>();
            individualList = new List<Individual>();
            xTiles = 0;
            yTiles = 0;
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
            levelRead("lvl.txt");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
           
            foreach (Tile t in tileList)
            {
                t.Update(gameTime);
            }

            foreach (Individual i in individualList)
            {
                pState=i.Update(gameTime, tileList, xTiles, 0.4f, pState);
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
            
            foreach (Tile t in tileList)
            {
                t.Draw(gameTime, spriteBatch);
            }

            foreach (Individual i in individualList)
            {
                i.Draw(gameTime, spriteBatch);
            }
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        protected void levelRead(string levelName)
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
            xTiles = 14;
            yTiles = 10;
            file.Close();
        }
    }
}
