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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ScreenManager screenManager;
        Click click;

        Song gameSong;
        bool isGameSong = false;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
              graphics.ToggleFullScreen();
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            screenManager = new ScreenManager(Content, graphics);
            click = new Click();

            spriteBatch = new SpriteBatch(GraphicsDevice);

            gameSong = Content.Load<Song>("Songs\\Theme Song");
            MediaPlayer.IsRepeating = true;

        }

        protected override void UnloadContent()
        {
            
        }


        protected override void Update(GameTime gameTime)
        {
            if (!isGameSong)
            {
                MediaPlayer.Play(gameSong);
                isGameSong = true;
            }

            click.Update();
            ScreenManager.Instance.CurrentScreen.HandleMouseClicks(click);
            ScreenManager.Instance.CurrentScreen.Update(gameTime, click);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Orange);

            spriteBatch.Begin();
            if (ScreenManager.Instance.CurrentScreen.IsContentLoaded)
            {
                ScreenManager.Instance.CurrentScreen.Draw(spriteBatch, GraphicsDevice);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        
    }
}
