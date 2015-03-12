using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace Proto3
{
    public class Level
    {
        public List<Tile> tileList;
        public List<Individual> individualList;
        public int xTiles;
        public int yTiles;
        public String name;
        ContentManager gameContent;

        public Level(String txt, ContentManager gameContent)
        {
            tileList = new List<Tile>();
            individualList = new List<Individual>();
            xTiles = 0;
            yTiles = 0;
            name = txt;
            this.gameContent = gameContent;
        }

        public void charge()
        {
            levelRead(name, gameContent);
        }

        private void levelRead(string levelName, ContentManager content)
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
                    if (i == '0')
                    {
                        Tile0 air = new Tile0(content.Load<Texture2D>(@"images\void"), new Vector2(countX, countY), new Point(75, 75), 0, new Vector2(0, 0));
                        tileList.Add(air);
                    }
                    if (i == '1')
                    {
                        Tile1 block = new Tile1(content.Load<Texture2D>(@"images\block"), new Vector2(countX, countY), new Point(75, 75), 0, new Vector2(0, 0));
                        tileList.Add(block);
                    }
                    if (i == '2')
                    {
                        Tile2 fire = new Tile2(content.Load<Texture2D>(@"images\fire"), new Vector2(countX, countY), new Point(75, 75), 0, new Vector2(0, 0));
                        tileList.Add(fire);
                    }
                    if (i == 'I')
                    {
                        Tile0 air = new Tile0(content.Load<Texture2D>(@"images\void"), new Vector2(countX, countY), new Point(75, 75), 0, new Vector2(0, 0));
                        tileList.Add(air);
                        MainCharacter dragon = new MainCharacter(content.Load<Texture2D>(@"images\dragon2"), new Vector2(countX, countY), new Point(75, 75));
                        individualList.Add(dragon);
                    }
                    countX = countX + 75;
                }
                countY = countY + 75;
            }
            xTiles = 16;
            yTiles = 14;
            file.Close();
        }




    }
}
