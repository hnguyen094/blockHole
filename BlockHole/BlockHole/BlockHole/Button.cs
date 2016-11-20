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
    public class Button
    {
        public Texture2D Sprite { get; private set; }

        public Rectangle Rect { get; private set; }

        public Color Color { get; private set; }

        public bool isClicked { get; private set; }

        public Rectangle TouchRect { get; private set; }

        private Rectangle defaultRect;

        public Button(Texture2D Sprite, Rectangle Rect, Color Color)
        {
            this.Sprite = Sprite;
            this.Rect = Rect;
            this.Color = Color;
            TouchRect = new Rectangle(Rect.X - Rect.Width / 2, Rect.Y - Rect.Height / 2, Rect.Width, Rect.Height);
            defaultRect = Rect;
        }

        public void Update(Click click)
        { 
            Rect = click.mouseRect.Intersects(TouchRect)? 
                new Rectangle(defaultRect.X, defaultRect.Y, defaultRect.Width, defaultRect.Width) : defaultRect;
            TouchRect = new Rectangle(Rect.X - Rect.Width / 2, Rect.Y - Rect.Height / 2, Rect.Width, Rect.Height);

            isClicked = (click.IsLeftClick() && click.mouseRect.Intersects(TouchRect));
        }

        public void Draw(SpriteBatch spriteBatch, string text, SpriteFont SpriteFont)
        {
            //spriteBatch.Draw(Sprite, Rect, Color);
            spriteBatch.Draw(Sprite, Rect, null, Color.Gray, 0, new Vector2(Sprite.Width / 2-10, Sprite.Height / 2-10), SpriteEffects.None, 0);
            spriteBatch.Draw(Sprite, Rect, null, Color, 0, new Vector2(Sprite.Width / 2, Sprite.Height / 2), SpriteEffects.None, 0);
            spriteBatch.DrawString(SpriteFont, text, new Vector2(Rect.X, Rect.Y), Color.Black, 0, new Vector2(SpriteFont.MeasureString(text).X / 2, 
                SpriteFont.MeasureString(text).Y / 2), Rect.Width / 350f, SpriteEffects.None, 0);
            
        }
    }
}
