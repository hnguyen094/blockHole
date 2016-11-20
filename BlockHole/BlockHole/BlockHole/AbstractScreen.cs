using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace BlockHole
{
    abstract class AbstractScreen
    {
        public bool IsContentLoaded { get; set; }
        public bool IsContentReloaded { get; set; }
        protected SpriteFont Font { get; set; }
        public bool entered;

        public AbstractScreen(ContentManager Content)
        {
            IsContentLoaded = false;
            IsContentReloaded = false;
        }

        public abstract void ReloadContent();
        public abstract void Update(GameTime gameTime, Click click);
        public abstract void Draw(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice);
        public abstract void HandleMouseClicks(Click click); 
    }
}
