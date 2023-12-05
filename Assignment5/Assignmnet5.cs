using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using System.Diagnostics;

namespace Assignment5
{
    public class Assignmnet5 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        TerrainRenderer terrain;
        Effect effect;

        Camera camera;
        Light light;

        Player player;
        Agent agent1;
        Agent agent2;
        Agent agent3;
        Bomb bomb;

        SpriteFont font;

        int agentsCaught = 0;
        int screenNum = 0;

        string finalTime;

        //************************

        public Assignmnet5()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here


            Time.Initialize();
            InputManager.Initialize();
            ScreenManager.Initialize(_graphics);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            terrain = new TerrainRenderer(Content.Load<Texture2D>("mazeH2"), Vector2.One * 100, Vector2.One * 200); // (100,100) size and (200,200) resolution
            terrain.NormalMap = Content.Load<Texture2D>("mazeN2");
            terrain.Transform = new Transform();
            terrain.Transform.LocalScale *= new Vector3(1, 5, 1);

            effect = Content.Load<Effect>("TerrainShader");

            effect.Parameters["AmbientColor"].SetValue(new Vector3(0.2f, 0.2f, 0.2f));
            effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.1f, 0.1f, 0.1f));
            effect.Parameters["SpecularColor"].SetValue(new Vector3(0.2f, 0.2f, 0.2f));
            effect.Parameters["Shininess"].SetValue(20f);


            //Camera 1
            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.Up * 50;
            camera.Transform.Rotate(Vector3.Left, MathHelper.PiOver2 - 0.2f);

            light = new Light();
            light.Transform = new Transform();
            light.Transform.LocalPosition = Vector3.Backward * 5 + Vector3.Right * 5 + Vector3.Up * 5;

            player = new Player(terrain, Content, camera, GraphicsDevice, light);
            agent1 = new Agent(terrain, Content, camera, GraphicsDevice, light);
            agent2 = new Agent(terrain, Content, camera, GraphicsDevice, light);
            agent3 = new Agent(terrain, Content, camera, GraphicsDevice, light);
            bomb = new Bomb(terrain, Content, camera, GraphicsDevice, light);
            bomb.player = player;


            font = Content.Load<SpriteFont>("Font");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            InputManager.Update();

            // TODO: Add your update logic here
            if (screenNum == 0)
            {
                Debug.WriteLine("Here");
                if (InputManager.IsKeyDown(Keys.Enter))
                {

                    screenNum = 1;
                }
            }
            else if (screenNum == 1)
            {
                Time.Update(gameTime);


                if (InputManager.IsKeyDown(Keys.Up))
                {
                    camera.Transform.Rotate(Vector3.Right, Time.ElapsedGameTime);
                }
                if (InputManager.IsKeyDown(Keys.Down))
                {
                    camera.Transform.Rotate(Vector3.Left, Time.ElapsedGameTime);
                }

                player.Update();
                agent1.Update();
                agent2.Update();
                agent3.Update();
                bomb.Update();

                Vector3 normal;
                if (player.Collider.Collides(agent1.Collider, out normal))
                {
                    agent1.path = null;
                    agentsCaught++;
                }
                if (player.Collider.Collides(agent2.Collider, out normal))
                {
                    agent2.path = null;
                    agentsCaught++;
                }
                if (player.Collider.Collides(agent3.Collider, out normal))
                {
                    agent3.path = null;
                    agentsCaught++;
                }
                if (player.Collider.Collides(bomb.Collider, out normal))
                {
                    bomb.path = null;
                    screenNum = 3;
                }

                if (agentsCaught >= 3)
                {
                    screenNum = 2;
                    finalTime = Time.TotalGameTime.TotalSeconds.ToString();
                }
            }


            base.Update(gameTime);
        }

        public float altitude()
        {
            return terrain.GetAltitude(camera.Transform.LocalPosition);
        }

        protected override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here
            if (screenNum == 0)
            {
                GraphicsDevice.Clear(Color.Black);

                _spriteBatch.Begin();
                _spriteBatch.DrawString(font, " Game Task: Catch the 3 Yelllow Agents While Avoiding The Black Bomb", new Vector2(150, 50), Color.Red);
                _spriteBatch.DrawString(font, "Press [Enter] to start", new Vector2(320, 220), Color.Red);
                _spriteBatch.End();
            }
            else if (screenNum == 1)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);

                effect.Parameters["View"].SetValue(camera.View);
                effect.Parameters["Projection"].SetValue(camera.Projection);
                effect.Parameters["World"].SetValue(terrain.Transform.World);
                effect.Parameters["LightPosition"].SetValue(light.Transform.Position);
                effect.Parameters["CameraPosition"].SetValue(camera.Transform.Position);
                effect.Parameters["NormalMap"].SetValue(terrain.NormalMap);

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    terrain.Draw();
                    player.Draw();
                    agent1.Draw();
                    agent2.Draw();
                    agent3.Draw();
                    bomb.Draw();
                }

                _spriteBatch.Begin();
                _spriteBatch.DrawString(font, "Bounty Collected: " + agentsCaught, new Vector2(75, 10), Color.Red);
                _spriteBatch.DrawString(font, "Time Spent: " + Time.TotalGameTime.TotalSeconds, new Vector2(75, 30), Color.Red);
                _spriteBatch.End();

            }
            else if (screenNum == 2)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);

                _spriteBatch.Begin();
                _spriteBatch.DrawString(font, "You caught 3 agents in: " + finalTime, new Vector2(225, 50), Color.Red);
                _spriteBatch.DrawString(font, "Press [ESCAPE] To Exit", new Vector2(275, 150), Color.Black);
                _spriteBatch.End();
            }
            else
            {
                GraphicsDevice.Clear(Color.Black);

                _spriteBatch.Begin();
                _spriteBatch.DrawString(font, "A Bomb...Exploded on you...Booom.....You died!", new Vector2(200, 50), Color.Red);
                _spriteBatch.DrawString(font, "Press [ESCAPE] To Exit", new Vector2(250, 100), Color.Red);
                _spriteBatch.End();
            }


            base.Draw(gameTime);
        }
    }
}