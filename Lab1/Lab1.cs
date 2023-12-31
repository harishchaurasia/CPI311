﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using System.Drawing;

namespace Lab1
{
    public class Lab1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont font;
        private fraction a = new fraction(3,4);
        private fraction b = new fraction(8,3);

        public Lab1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Font");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.DrawString(font, a + " + " + b + " = " + (a + b), new Vector2(100, 100), Color.Black);
            _spriteBatch.DrawString(font, a + " - " + b + " = " + (a - b), new Vector2(100, 140), Color.Black);
            _spriteBatch.DrawString(font,  a + " * " +b + " = " + (a * b), new Vector2(100, 180), Color.Black);
            _spriteBatch.DrawString(font, a + " / " + b + " = " + (a / b), new Vector2(100, 220), Color.Black);
            


            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}