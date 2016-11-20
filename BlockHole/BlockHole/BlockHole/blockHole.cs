using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BlockHole
{
    public class blockHole
    {
        private Texture2D sprite;
        public Rectangle Rect;
        public Vector2 CenterPosition;
        public float Mass { get; private set; }
        public float M { get; private set; }

        public blockHole(Texture2D sprite, Vector2 position, int side)
        {
            Mass = 60f;
            M = 60f;
            Rect = new Rectangle((int)position.X, (int)position.Y, side, side);
            CenterPosition = position;
            this.sprite = sprite;
        }

        public void Update(Click click)
        {
            CenterPosition = new Vector2(click.currentMouse.X, click.currentMouse.Y);
            Rect.X = click.currentMouse.X - Rect.Width / 2;
            Rect.Y = click.currentMouse.Y - Rect.Height / 2;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, Rect, Color.Black);
        }
    }
}
