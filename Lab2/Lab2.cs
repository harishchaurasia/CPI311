using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using CPI311.GameEngine;

namespace Lab2
{
    public class Lab2 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Sprite sprite;
        //KeyboardState prevState;
        SpiralMove sprite;

        public Lab2()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //prevState  = Keyboard.GetState();

            InputManager.Initialize();
            Time.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D texture = Content.Load<Texture2D>("Square");
            //sprite = new Sprite(texture);

            sprite = new SpiralMove(texture, new Vector2(300, 300));
           // sprite.Position = new Vector2(0, 0);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            InputManager.Update();
            Time.Update(gameTime);


            sprite.Update();


            /*
            if (InputManager.IsKeyPressed(Keys.Left)) 
            {
                sprite.Position += Vector2.UnitX * -5;
            }
            if (InputManager.IsKeyPressed(Keys.Right))
            {
                sprite.Position -= Vector2.UnitX * -5;
            }
            if (InputManager.IsKeyPressed(Keys.Up))
            {
                sprite.Position += Vector2.UnitY * -5;
            }
            if (InputManager.IsKeyPressed(Keys.Down))
            {
                sprite.Position -= Vector2.UnitY * -5;
            }
            if (InputManager.IsKeyDown(Keys.Space))
            {
                sprite.Ro
            */

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            //sprite.Draw(_spriteBatch); //Sprite class Method: Draw
            sprite.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}