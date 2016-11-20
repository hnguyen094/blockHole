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
using System.Diagnostics;

namespace BlockHole
{
    public class blonet
    {
        Texture2D sprite;
        SpriteFont tinyFont;

        Rectangle rect;
        Rectangle touchRect;

        Vector2 position;

        Vector2 velocity;
        Vector2 speed;
        Vector2 scale;
        Vector2 aimScale;

        public bool IsTouched = false;
        public float TimeExisted { get; private set; }
        Vector2 distanceAwayXY = new Vector2();
        public float DistanceAway = 0;

        public float multiplier = 1f;

        const float m = 10f;
        float g;
        const float G = 10f;
        float r;

        float fadingIn;
        public bool IsFadingIn { private set; get; }

        public bool IsTurnBlue = false;
        float turnBlueTimer = 28f;
        public bool Activated = true;

        public blonet(Texture2D sprite, Vector2 position, int side, SpriteFont tinyFont)
        {
            this.sprite = sprite;
            this.position = position;
            this.tinyFont = tinyFont;

            rect = new Rectangle((int)position.X, (int)position.Y, side, side);
            touchRect = new Rectangle(rect.X - rect.Width / 2, rect.Y - rect.Height / 2, rect.Width, rect.Height);
            IsFadingIn = false;
            speed.X = -1f;
            speed.Y = -1f;
            
        }

        public void UpdateCollision(blockHole blockHole)
        {
            if (!IsFadingIn)
            {
                IsTouched = blockHole.Rect.Intersects(touchRect) && Activated;
            }
        }

        public void Update(GameTime gameTime, blockHole blockHole, List<blonet> blonets)
        {
            if (Activated)
            {
                TimeExisted += (float)gameTime.ElapsedGameTime.TotalSeconds;
                turnBlueTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (turnBlueTimer < 0)
                {
                    turnBlueTimer = 28f;
                    IsTurnBlue = true;
                }
                fadingIn = TimeExisted / 2;
                IsFadingIn = TimeExisted < 2f;

                distanceAwayXY.X = position.X - blockHole.CenterPosition.X;
                distanceAwayXY.Y = position.Y - blockHole.CenterPosition.Y;
                DistanceAway = (float)Math.Sqrt(distanceAwayXY.X * distanceAwayXY.X + distanceAwayXY.Y * distanceAwayXY.Y);

                if (!IsFadingIn)
                {
                    if (!blockHole.Rect.Intersects(touchRect))
                    {
                        physics4(blockHole);
                        physics2(blockHole);

                        rect.X = (int)position.X;
                        rect.Y = (int)position.Y;
                        touchRect.X = rect.X - rect.Width / 2;
                        touchRect.Y = rect.Y - rect.Height / 2;

                        if (IsTurnBlue)
                            feeding(blonets);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Activated)
            {
                if (IsFadingIn)
                {
                    spriteBatch.DrawString(tinyFont, "Spawn Time:\n" + Math.Round((2 - TimeExisted), 1, MidpointRounding.AwayFromZero).ToString(), new Vector2(touchRect.X, touchRect.Y - 2 * tinyFont.LineSpacing), Color.Black);
                }
                else
                    spriteBatch.DrawString(tinyFont, "X: " + position.X + "\nY: " + position.Y, new Vector2(touchRect.X, touchRect.Y - 2 * tinyFont.LineSpacing), Color.Black);
                if (!IsTurnBlue)
                    spriteBatch.Draw(sprite, touchRect, null, Color.Lerp(Color.Transparent, Color.White, fadingIn), 0, Vector2.Zero, SpriteEffects.None, 0);
                else
                    spriteBatch.Draw(sprite, touchRect, null, Color.Blue, 0, Vector2.Zero, SpriteEffects.None, 0);
            }
        }
        private void feeding(List<blonet> blonets)
        {
            for (int i = blonets.Count - 1; i >= 0; i--)
            {
                if (touchRect.Intersects(blonets[i].touchRect) &&blonets[i].Activated && !blonets[i].IsTurnBlue && blonets[i]!= this)
                {
                    multiplier = 2f;
                    blonets[i].Activated = false;
                    IsTurnBlue = false;
                    break;
                }
            }
        }

        private void physics(blockHole blockHole)
        {
            g = speed.X * speed.Y / 500f;

            scale.X = (float)Math.Cos(Math.Atan2((position.Y - blockHole.CenterPosition.Y), (position.X - blockHole.CenterPosition.X)));
            scale.Y = (float)Math.Sin(Math.Atan2((position.Y - blockHole.CenterPosition.Y), (position.X - blockHole.CenterPosition.X)));

            speed.X += g;
            speed.Y += g;

            velocity.X = speed.X * scale.X;
            velocity.Y = speed.Y * scale.Y;

            position.X -= velocity.X;
            position.Y -= velocity.Y;

            // Debug.WriteLine(position);

        }

        private void physics2(blockHole blockHole)
        {
            r = (float)Math.Sqrt(distanceAwayXY.X * distanceAwayXY.X + distanceAwayXY.Y * distanceAwayXY.Y);

            g = - G * blockHole.Mass / r / r;

            aimScale.X = (float)Math.Cos(Math.Atan2(distanceAwayXY.Y, distanceAwayXY.X));
            aimScale.Y = (float)Math.Sin(Math.Atan2(distanceAwayXY.Y, distanceAwayXY.X));

            scale.X -= Math.Abs(aimScale.X - scale.X) > 0.1f ? (scale.X - aimScale.X) / 20 : 0;
            scale.Y -= Math.Abs(aimScale.Y - scale.Y) > 0.1f ? (scale.Y - aimScale.Y) / 20 : 0;

            speed.X += g;
            speed.Y += g;

            velocity.X = speed.X * scale.X * m;
            velocity.Y = speed.Y * scale.Y * m;

            position.X += velocity.X;
            position.Y += velocity.Y;
        }

        private void physics3(blockHole blockHole)
        {
            r = (float)Math.Sqrt((position.X - blockHole.CenterPosition.X) * (position.X - blockHole.CenterPosition.X) + (position.Y - blockHole.CenterPosition.Y) * (position.Y - blockHole.CenterPosition.Y));

            g = -G * blockHole.Mass / r / r * TimeExisted;


            position.X -= g*(blockHole.CenterPosition.X - position.X);

            position.Y -= g* (blockHole.CenterPosition.Y - position.Y);
        }

        private void physics4(blockHole blockHole)
        {
            r = (float)Math.Sqrt(distanceAwayXY.X * distanceAwayXY.X + distanceAwayXY.Y * distanceAwayXY.Y);
            double theta = -Math.Atan2(distanceAwayXY.Y, distanceAwayXY.X);
            double magVelocity =  2* Math.Sqrt(G * m / r);
            velocity.Y = (float)(magVelocity * Math.Cos(theta));
            velocity.X = (float)(magVelocity * Math.Sin(theta));

            position.X += velocity.X;
            position.Y += velocity.Y;
        }
    }
}
