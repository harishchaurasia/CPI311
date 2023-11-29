using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using System.Collections.Generic;
using System;
namespace Lab11
{
    public class Lab11 : Game
    {
        //***Inner Class***************************
        public class Scene
        {
            public delegate void CallMethod();
            public CallMethod Update;
            public CallMethod Draw;
            public Scene(CallMethod update, CallMethod draw)
            { Update = update; Draw = draw; }
        }
        //****************************************



        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        Texture2D texture;
        SpriteFont font;
        Color background = Color.White;
        //Button exitButton;

        CheckBox box;
        Button fullButton;


        // Section D ***************
        Dictionary<String, Scene> scenes;
        Scene currentScene;

        // Section E ***************

        List<GUIElement> guiElements;


        public Lab11()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            Time.Initialize();
            InputManager.Initialize();
            ScreenManager.Initialize(_graphics);

            // Section D ***************
            scenes = new Dictionary<String, Scene>();

            // Section E ***************
            guiElements = new List<GUIElement>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            texture = Content.Load<Texture2D>("Square");
            font = Content.Load<SpriteFont>("Font");


            /*exitButton = new Button();
            exitButton.Texture = texture;
            exitButton.Text = "Exit";
            exitButton.Bounds = new Rectangle(50, 50, 300, 20);*/

            //exitButton.Action += ExitGame;


            // Section D ***************

            scenes.Add("Menu", new Scene(MainMenuUpdate, MainMenuDraw));
            scenes.Add("Play", new Scene(PlayUpdate, PlayDraw));
            currentScene = scenes["Menu"];


            // Section E ***************
            box = new CheckBox();
            box.Box = texture;
            box.Text = "Switch Scene";
            box.Bounds = new Rectangle(50, 50, 300, 10);
            box.Action += SwitchScene;
            guiElements.Add(box);

            fullButton = new Button();
            fullButton.Texture = texture;
            fullButton.Text = "Full Scene Mode";
            fullButton.Bounds = new Rectangle(50, 200, 300, 10);
            fullButton.Action += FullScene;
            guiElements.Add(fullButton);

            //ScreenManager.Setup(false, 600, 400);


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            /*if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();*/

            // TODO: Add your update logic here


            Time.Update(gameTime);
            InputManager.Update();

            currentScene.Update();

            fullButton.Update();
            box.Update();
            //exitButton.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            /*GraphicsDevice.Clear(Color.CornflowerBlue);*/
            GraphicsDevice.Clear(background);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            //exitButton.Draw(_spriteBatch, font);
            box.Draw(_spriteBatch, font);
            fullButton.Draw(_spriteBatch, font);
            PlayDraw();
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void ExitGame(GUIElement element)
        {
            background = (background == Color.White ? Color.Blue : Color.White);
        }

        void MainMenuUpdate()
        {
            foreach (GUIElement element in guiElements)
                element.Update();
        }
        void MainMenuDraw()
        {
            _spriteBatch.Begin();
            foreach (GUIElement element in guiElements)
                element.Draw(_spriteBatch, font);
            _spriteBatch.End();
        }
        void PlayUpdate()
        {
            if (InputManager.IsKeyReleased(Keys.Escape))
                currentScene = scenes["Menu"];
        }
        void PlayDraw()
        {
            //_spriteBatch.Begin();
            _spriteBatch.DrawString(font, "Play Mode! Press \"Esc\" to go back", Vector2.Zero, Color.Black);
            //_spriteBatch.End();
        }

        //Section E ******************************
        void SwitchScene(GUIElement element)
        {
            currentScene = scenes["Play"];
            background = (background == Color.White ? Color.Blue : Color.White);
        }
        void FullScene(GUIElement element)
        {
            ScreenManager.Setup(1920, 1080);
            //ScreenManager.IsFullScreen = !ScreenManager.IsFullScreen;
        }
    }
}