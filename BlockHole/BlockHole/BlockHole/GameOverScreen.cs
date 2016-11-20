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
    class GameOverScreen : AbstractScreen
    {
        TimeSpan timeToClick = TimeSpan.FromSeconds(0.2);
        SpriteFont pixelatedFont;
        Vector2 viewPortSize;


        readonly string highScoresFileName = "highscores.txt";
        List<HighScore> highScores = new List<HighScore>();
        public bool IsScoreSaved = false;


        public GameOverScreen(ContentManager Content, GraphicsDeviceManager graphics)
            : base(Content)
        {
            entered = true;
            IsContentLoaded = true;
            pixelatedFont = Content.Load<SpriteFont>("scoreText");
            viewPortSize = new Vector2(graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width, graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height);

            highScores = Leaderboard.Load(highScoresFileName);
        }

        public override void ReloadContent()
        {
         
            IsContentReloaded = true;
        }

        public override void Update(GameTime gameTime, Click click)
        {
            if (entered)
                entered = false;

            if (ScreenManager.Instance.isFirstRUn)
                ScreenManager.Instance.isFirstRUn = false;
            if (!IsScoreSaved)
                {
                    Leaderboard.Save(highScoresFileName, new HighScore("", (int)ScreenManager.Instance.myGameScreen.Score), 1);
                    highScores = Leaderboard.Load(highScoresFileName);
                    IsScoreSaved = true;
            }

            ScreenManager.Instance.myMainMenuScreen.UpdateButtons(click);

            timeToClick = timeToClick.Subtract(gameTime.ElapsedGameTime);   //subtract from the timer
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
        {
            spriteBatch.DrawString(pixelatedFont, "High Score: " + highScores[0] +"\n" + "Score: " + (int)ScreenManager.Instance.myGameScreen.Score, new Vector2(ScreenManager.Instance.myMainMenuScreen.TitleBox.X, ScreenManager.Instance.myMainMenuScreen.TitleBox.Y), Color.White,
                                   0, new Vector2(pixelatedFont.MeasureString("Score: " + (int)ScreenManager.Instance.myGameScreen.Score).X / 2, pixelatedFont.MeasureString("Score: " + (int)ScreenManager.Instance.myGameScreen.Score).Y / 2),
                                   viewPortSize.X / (viewPortSize.X * 2), SpriteEffects.None, 0);
            ScreenManager.Instance.myMainMenuScreen.DrawButtons(spriteBatch);
        }

        public override void HandleMouseClicks(Click click)
        {
            ScreenManager.Instance.myMainMenuScreen.HandleMouseClicks(click);
        }
    }
}
