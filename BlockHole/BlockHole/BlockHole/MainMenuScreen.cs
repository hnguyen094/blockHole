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
    class MainMenuScreen : AbstractScreen
    {
        TimeSpan timeToClick = TimeSpan.FromSeconds(0.2);

        Button startButton;
        Button exitButton;
        Button extremeButton;

        SpriteFont pixelatedFont;
        public Texture2D PointerSprite { private set; get; }
        Texture2D titleSprite;
        public Rectangle TitleBox;

        Texture2D whiteSquareSprite;
        Vector2 viewPortSize;
        Click click;


        public MainMenuScreen(ContentManager Content, GraphicsDeviceManager graphics)
            : base(Content)
        {
            viewPortSize = new Vector2(graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width, graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height);

            whiteSquareSprite = Content.Load<Texture2D>("Cblock");
            PointerSprite = Content.Load<Texture2D>("pointer");
            titleSprite = Content.Load<Texture2D>("Blockhole");
            pixelatedFont = Content.Load<SpriteFont>("scoreText");

            startButton = new Button(whiteSquareSprite, new Rectangle((int)(viewPortSize.X / 2), (int)(viewPortSize.Y / 2), (int)(viewPortSize.X / 20),
                (int)(viewPortSize.X / 27)), Color.Lerp(Color.Transparent, Color.Red, .9f));
            exitButton = new Button(whiteSquareSprite, new Rectangle((int)(viewPortSize.X / 2), (int)(viewPortSize.Y / 10 * 7), (int)(viewPortSize.X / 20),
                (int)(viewPortSize.X / 27)), Color.Lerp(Color.Transparent, Color.Red, .9f));
            extremeButton = new Button(whiteSquareSprite, new Rectangle((int)(viewPortSize.X / 2), (int)(viewPortSize.Y / 5 * 3), (int)(viewPortSize.X / 20),
                (int)(viewPortSize.X / 27)), Color.Lerp(Color.Transparent, Color.Red, .9f));

            TitleBox = new Rectangle((int)(viewPortSize.X / 2), (int)(viewPortSize.Y / 3), (int)(viewPortSize.X / 3), (int)(viewPortSize.X / 3 / 4.1));


            IsContentLoaded = true;
            entered = true;
        }

   

        public override void ReloadContent()
        {
            IsContentReloaded = true;    
        }

        public void UpdateButtons(Click click)
        {
            startButton.Update(click);
            exitButton.Update(click);
            if(!ScreenManager.Instance.isFirstRUn)
                extremeButton.Update(click);
        }

        public override void Update(GameTime gameTime, Click click)
        {
            this.click = click;
            if (entered)
            {
                entered = false;
            }

            UpdateButtons(click);

            timeToClick = timeToClick.Subtract(gameTime.ElapsedGameTime);   //subtract from the timer
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
        {
            spriteBatch.Draw(titleSprite, TitleBox, null, Color.White, 0, new Vector2(titleSprite.Width / 2, titleSprite.Height / 2), SpriteEffects.None, 0);
            DrawButtons(spriteBatch);
        }

        public void DrawButtons(SpriteBatch spriteBatch)
        {
            startButton.Draw(spriteBatch, "COMMENCE\n<NORMAL>", pixelatedFont);
            exitButton.Draw(spriteBatch, "TERMINATE", pixelatedFont);
            if (!ScreenManager.Instance.isFirstRUn) 
                extremeButton.Draw(spriteBatch, "COMMENCE\n<BLIND>", pixelatedFont);
            spriteBatch.Draw(PointerSprite, new Vector2(click.currentMouse.X, click.currentMouse.Y), Color.Lerp(Color.Transparent, Color.White, .7f));
        }

        public override void HandleMouseClicks(Click click)
        {
            if (startButton.isClicked)
            {
                ScreenManager.Instance.myGameScreen.resetScore();
                ScreenManager.Instance.myGameScreen.isBlindMode = false;
                ScreenManager.Instance.CurrentScreen = ScreenManager.Instance.myGameScreen;
            }
            else if (exitButton.isClicked)
            {
                Environment.Exit(Environment.ExitCode);
            }
            else if (extremeButton.isClicked)
            {
                ScreenManager.Instance.myGameScreen.resetScore();
                ScreenManager.Instance.myGameScreen.isBlindMode = true;
                ScreenManager.Instance.CurrentScreen = ScreenManager.Instance.myGameScreen;
            }
        }
    }
}
