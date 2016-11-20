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
    public class Click
    {
        public MouseState currentMouse { private set; get; }
        public MouseState wasMouse { private set; get; }
        public KeyboardState currentKeys { private set; get; }
        public KeyboardState wasKeys { private set; get; }
        public Rectangle mouseRect { private set; get; }

        public Click()
        {
            currentKeys = Keyboard.GetState();
            currentMouse = Mouse.GetState();
            mouseRect = new Rectangle(currentMouse.X,currentMouse.Y,1,1);
        }

        public void Update()
        {
            mouseRect = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);
            wasMouse = currentMouse;
            wasKeys = currentKeys;

            currentKeys = Keyboard.GetState();
            currentMouse = Mouse.GetState();

        }

        public bool IsLeftClick()
        {
            if (wasMouse.LeftButton.Equals(ButtonState.Released) && currentMouse.LeftButton.Equals(ButtonState.Pressed))
                return true;
            else
                return false;
        }
        
        public bool IsKeyPressed(Keys key)
        {
            if (currentKeys.IsKeyDown(key) && wasKeys.IsKeyUp(key))
                return true;
            else
                return false;
        }

    }
}
