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
    class ScreenManager
    {
        public static ScreenManager Instance;

        public AbstractScreen CurrentScreen { get; set; }
        public GameScreen myGameScreen { get; private set; }
        public MainMenuScreen myMainMenuScreen { get; private set; }
        public GameOverScreen myGameOverScreen { get; private set; }

        public bool isFirstRUn = true;


        public ScreenManager(ContentManager Content, GraphicsDeviceManager graphics)
        {
            Instance = this;

            myMainMenuScreen = new MainMenuScreen(Content,graphics);

            myGameScreen = new GameScreen(Content, graphics);

            myGameOverScreen = new GameOverScreen(Content, graphics);

            CurrentScreen = myMainMenuScreen;
        }

    }
}
