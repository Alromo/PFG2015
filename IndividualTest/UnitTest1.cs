using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proto3;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace IndividualTest
{
    [TestClass]
    public class IndividualTest
    {
        [TestMethod]
        public void TileDetection()
        {
            Tile block = new Tile1(Content.Load<Texture2D>(@"images\block"), new Vector2(countX, countY), new Point(75, 75), 0, new Vector2(0, 0));
        }
    }
}
