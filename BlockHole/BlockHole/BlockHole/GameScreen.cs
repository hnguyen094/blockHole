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
    class GameScreen : AbstractScreen
    {
        TimeSpan timeToClick = TimeSpan.FromSeconds(0.2);   //A timer so you can't click right away when you enter a screen

        SpriteFont pixelatedFont;

        Texture2D whiteSquareSprite;
        Texture2D whiteScreen;
        SpriteFont tinyFont;
        Random rand = new Random();
        public float Score = 0;
        private float wasScore;
        float spawnTimer = 0f;
        float scoreTimer = 0f;
        float elaspedTime = 0f;

        blockHole blockHole;
        List<blonet> blonets = new List<blonet>();
        Button restartButton;
        Vector2 viewPortSize;

        bool isGameOver = false;
        public bool isBlindMode = false;

        float blonetLvl4Timer = 0f;

        Click click;

        public GameScreen(ContentManager Content, GraphicsDeviceManager graphics)
            : base(Content)
        {
            entered = true;
            IsContentLoaded = true;

            whiteSquareSprite = Content.Load<Texture2D>("Cblock");
            whiteScreen = Content.Load<Texture2D>("White pic");

            viewPortSize = new Vector2(graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width, graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height);

            pixelatedFont = Content.Load<SpriteFont>("scoreText");
            tinyFont = Content.Load<SpriteFont>("tinyText");
            blockHole = new blockHole(whiteSquareSprite, Vector2.Zero, (int)viewPortSize.X / 46);
            
            restartButton = new Button(whiteSquareSprite, new Rectangle((int)(viewPortSize.X - (viewPortSize.X / 40)), (int)(viewPortSize.Y - (viewPortSize.X / 40)),
    (int)viewPortSize.X / 25, (int)viewPortSize.X / 40), Color.Lerp(Color.Transparent, Color.Red, .9f));
            wasScore = Score;
            
        }

        public override void ReloadContent()
        {
            IsContentReloaded = true;    

        }

        public void reset()
        {
            isGameOver = false;
            blonets.RemoveRange(0, blonets.Count);
            spawnTimer = 0;
        }
        public void resetScore()
        {
            ScreenManager.Instance.myGameOverScreen.IsScoreSaved = false;
            Score = 0;
            wasScore = 0;
        }

        public override void Update(GameTime gameTime, Click click)
        {
            elaspedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (entered)
                entered = false;
            blockHole.Update(click);
            if (blonets.Count == 4)
                blonetLvl4Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;


            if ((!((elaspedTime < 5 && elaspedTime > 0) || (elaspedTime < 15 && elaspedTime > 10) || (blonetLvl4Timer < 5 && blonetLvl4Timer > 0)) && ScreenManager.Instance.isFirstRUn) || !ScreenManager.Instance.isFirstRUn)
            {
                timeToClick = timeToClick.Subtract(gameTime.ElapsedGameTime);   //subtract from the timer

                this.click = click;

                blockHole.Update(click);

                restartButton.Update(click);
                if (restartButton.isClicked)
                    reset();

                spawnTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;


                if (wasScore == Score)
                    scoreTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    scoreTimer = 0;
                if (scoreTimer > 5)
                    Score -= (float)gameTime.ElapsedGameTime.TotalMinutes * 100 * blonets.Count;


                for (int i = 0; i < blonets.Count; i++)
                {
                    if (blonets[i].Activated)
                    {
                        blonets[i].UpdateCollision(blockHole);
                        if (blonets[i].IsTouched)
                        {
                            blonets.RemoveAt(i);
                            i--;
                            isGameOver = true;
                            if (i == -1)
                                break;
                        }
                        Score += !blonets[i].IsFadingIn ? Math.Abs(blonets[i].DistanceAway) < 100 ? (125 * blonets[i].multiplier/ blonets[i].DistanceAway) : 0 : 0;
                    }
                }

                if (spawnTimer <= 0)
                {
                    spawnTimer = 7f;
                    blonets.Add(new blonet(whiteSquareSprite,
                        new Vector2(rand.Next((int)viewPortSize.X), rand.Next((int)viewPortSize.Y)),
                        (int)viewPortSize.X / 46, tinyFont));
                }
                foreach (blonet item in blonets)
                {
                    item.Update(gameTime, blockHole, blonets);
                }
                Score = MathHelper.Clamp(Score, -999999, 999999);
                wasScore = Score;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
        {
            GraphicsDevice.Clear(isBlindMode ? Color.Black : Color.Orange);
            restartButton.Draw(spriteBatch, "RETURN", pixelatedFont);
            foreach (blonet item in blonets)
                item.Draw(spriteBatch);
            if(!isBlindMode)
                blockHole.Draw(spriteBatch);
            else
                spriteBatch.Draw(ScreenManager.Instance.myMainMenuScreen.PointerSprite, new Vector2(click.currentMouse.X, click.currentMouse.Y), Color.Black);
           
            spriteBatch.DrawString(pixelatedFont, "Score: " + (int)Score, new Vector2(10, 0), Color.White,
                0, Vector2.Zero, viewPortSize.X / (viewPortSize.X * 3), SpriteEffects.None, 0);
            spriteBatch.DrawString(pixelatedFont, "Level: " + blonets.Count, new Vector2(10, 20), Color.White,
                0, Vector2.Zero, viewPortSize.X / (viewPortSize.X * 3), SpriteEffects.None, 0);


           


            if ((elaspedTime < 5 && elaspedTime > 0) && ScreenManager.Instance.isFirstRUn)
            {
                spriteBatch.Draw(whiteScreen, Vector2.Zero, Color.Lerp(Color.Transparent,Color.White, (5 - elaspedTime) / 10));
                spriteBatch.DrawString(pixelatedFont, "AS A BLOCKHOLE, KEEP AWAY FROM THE WHITE BLONETS \n BUT STAY TOO FAR AND YOU'LL BE SUBTRACTED POINTS", new Vector2(10, 40),
                    Color.White, 0, Vector2.Zero, viewPortSize.X / (viewPortSize.X * 2), SpriteEffects.None, 0);
            }
            else if ((elaspedTime < 15 && elaspedTime > 10) && ScreenManager.Instance.isFirstRUn)
            {
                spriteBatch.Draw(whiteScreen, Vector2.Zero, Color.Lerp(Color.Transparent,Color.White, (15 - elaspedTime) / 10));
                spriteBatch.DrawString(pixelatedFont, "MORE WHITE BLONETS WILL SPAWN! THEY WILL ONLY GET FASTER!", new Vector2(10, 40),
                    Color.White, 0, Vector2.Zero, viewPortSize.X / (viewPortSize.X * 2), SpriteEffects.None, 0);
            }
            else if ((blonetLvl4Timer < 5 && blonetLvl4Timer > 0) && ScreenManager.Instance.isFirstRUn)
            {
                spriteBatch.Draw(whiteScreen, Vector2.Zero, Color.Lerp(Color.Transparent, Color.White, (5-blonetLvl4Timer) / 10));
                spriteBatch.DrawString(pixelatedFont, "BLUE BLONETS CAN EAT WHITE ONES TO BECOME WHITE!", new Vector2(10, 40),
                    Color.White, 0, Vector2.Zero, viewPortSize.X / (viewPortSize.X * 2), SpriteEffects.None, 0);
            }
            else if (ScreenManager.Instance.isFirstRUn)
                spriteBatch.DrawString(pixelatedFont, "TUTORIAL MODE: DON'T DIE UNTIL LEVEL 4 TO SEE ALL THE TUTORIALS", new Vector2(200, 0),
                    Color.White, 0, Vector2.Zero, viewPortSize.X / (viewPortSize.X * 2), SpriteEffects.None, 0);
        
        
            

        }

        public override void HandleMouseClicks(Click click)
        {
            if (isGameOver)
            {
                ScreenManager.Instance.myGameScreen.reset();
                ScreenManager.Instance.CurrentScreen = ScreenManager.Instance.myGameOverScreen;
            }
            if (restartButton.isClicked)
            {
                reset();
                ScreenManager.Instance.CurrentScreen = ScreenManager.Instance.myMainMenuScreen;
            }
        }
    }
}
