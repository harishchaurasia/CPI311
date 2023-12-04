using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using CPI311.GameEngine;

namespace Assignment1
{
    public class Assignment1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont font;
        private Sprite clock;
        private Texture2D background;
        private AnimatedSprite animatedChar;
        private ProgressBar distanceBar;
        private ProgressBar timeBar;
        private float speed;
        private float distanceTraveled;
        private float goalDistance;
        private float givenTime;
        private float totalTime;

        private Random random;
        public Assignment1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            random = new Random();
            speed = 100f;
            goalDistance = 8000;
            givenTime = 0;
            totalTime = 45;
            //addedTime = 15;
            InputManager.Initialize();
            Time.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Font");
            animatedChar = new AnimatedSprite(Content.Load<Texture2D>("Explorer"));
            background = Content.Load<Texture2D>("usethis");

            timeBar = new ProgressBar(Content.Load<Texture2D>("Square"), Color.Red, 0, 0);
            timeBar.Position = new Vector2(150, 20);
            timeBar.Scale = new Vector2(7.5f, 0.5f);

            distanceBar = new ProgressBar(Content.Load<Texture2D>("Square"), Color.Green, 0, 0);
            distanceBar.Position = new Vector2(450, 20);
            distanceBar.Scale = new Vector2(7.5f, 0.5f);

            clock = new Sprite(Content.Load<Texture2D>("clock"));
            clock.Scale = new Vector2(0.025f, 0.025f);
            clock.Position = new Vector2(150, 200);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            InputManager.Update();
            Time.Update(gameTime);

            givenTime += Time.ElapsedGameTime;
            timeBar.Value = 1 - (givenTime / totalTime);

            if (InputManager.IsKeyDown(Keys.W))
            {
                animatedChar.Position -= Vector2.UnitY * speed * Time.ElapsedGameTime;
                animatedChar.currentY = 0;
                if (distanceTraveled <= goalDistance)
                {
                    distanceTraveled += speed * Time.ElapsedGameTime;
                }
                distanceBar.Value = distanceTraveled / goalDistance;
            }
            if (InputManager.IsKeyDown(Keys.S))
            {
                animatedChar.currentY = 32;
                animatedChar.Position += Vector2.UnitY * speed * Time.ElapsedGameTime;
                if (distanceTraveled <= goalDistance)
                {
                    distanceTraveled += speed * Time.ElapsedGameTime;
                }
                distanceBar.Value = distanceTraveled / goalDistance;
            }

            if (InputManager.IsKeyDown(Keys.A))
            {
                animatedChar.currentY = 64;
                animatedChar.Position -= Vector2.UnitX * speed * Time.ElapsedGameTime;
                if (distanceTraveled <= goalDistance)
                {
                    distanceTraveled += speed * Time.ElapsedGameTime;
                }
                distanceBar.Value = distanceTraveled / goalDistance;
            }

            if (InputManager.IsKeyDown(Keys.D))
            {
                animatedChar.currentY = 96;
                animatedChar.Position += Vector2.UnitX * speed * Time.ElapsedGameTime;
                if (distanceTraveled <= goalDistance)
                {
                    distanceTraveled += speed * Time.ElapsedGameTime;
                }
                distanceBar.Value = distanceTraveled / goalDistance;
            }

            if ((animatedChar.Position.Y - clock.Position.Y) <= 10 && (animatedChar.Position.Y - clock.Position.Y) >= -10 && (animatedChar.Position.X - clock.Position.X) <= 10 && (animatedChar.Position.X - clock.Position.X) >= -10)
            {
                totalTime += 15;
                clock.Position = new Vector2(random.Next(0, 500), random.Next(0, 250));
            }

            animatedChar.Update();
            distanceBar.Update();
            timeBar.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            animatedChar.Draw(_spriteBatch);

            timeBar.Draw(_spriteBatch);
            distanceBar.Draw(_spriteBatch);
            clock.Draw(_spriteBatch);

            _spriteBatch.DrawString(font, "Time Remaining: " + (int)(totalTime - givenTime), new Vector2(40, 30), Color.Red);
            _spriteBatch.DrawString(font, "Distance Traveled: " + (int)distanceTraveled, new Vector2(340, 30), Color.LightGreen);
            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}