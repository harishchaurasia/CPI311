using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;
//using SharpDX.Direct3D9;
//sing SharpDX.Direct3D9;

namespace FinalProject
{
    public class FinalProject : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;



        TerrainRenderer terrain;
        Effect effect;
        Light light;

        Camera camera;
        Camera thirdCam;


        bool camToggle = true;



        Player player;
        Agent agent1;
        Agent agent2;
        Agent agent3;
        Agent agent4;
        Agent agent5;
        Enemies enemies;


        SpriteFont font;
        string finalTime;
        int bountyCollected = 0;
        int screenNum = 0;
        float height = 0;


        //Audio components
        SoundEffectInstance audiInstance;
        SoundEffect audi;
        SoundEffect collect_ring;
        SoundEffect negative_beep;



        public FinalProject()
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



            audi = Content.Load<SoundEffect>("audi");
            collect_ring = Content.Load<SoundEffect>("collect-ring");
            negative_beep = Content.Load<SoundEffect>("negative_beep");
            audiInstance = audi.CreateInstance();



            terrain = new TerrainRenderer(Content.Load<Texture2D>("mazeH2"), Vector2.One * 100, Vector2.One * 200); // (100,100) size and (200,200) resolution
            terrain.NormalMap = Content.Load<Texture2D>("mazeN2");
            terrain.Transform = new Transform();
            terrain.Transform.LocalScale *= new Vector3(1, 5, 1);

            effect = Content.Load<Effect>("TerrainShader");

            effect.Parameters["AmbientColor"].SetValue(new Vector3(0.1f, 0.1f, 0.1f));
            effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.4f, 0.3f, 0.2f));
            effect.Parameters["SpecularColor"].SetValue(new Vector3(0.1f, 0.1f, 0.2f));
            effect.Parameters["Shininess"].SetValue(20f);

            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.Backward * 5 + Vector3.Right * 3 + Vector3.Up * 5;


            thirdCam = new Camera();
            thirdCam.Transform = new Transform();
            thirdCam.Transform.LocalPosition = Vector3.Up * 50;
            thirdCam.Transform.Rotate(Vector3.Left, MathHelper.PiOver2 - 0.2f);
            //camera.Transform = player.Transform;



            light = new Light();
            light.Transform = new Transform();
            light.Transform.LocalPosition = Vector3.Backward * -0.5f + Vector3.Up * 3;
            light.Transform.Parent = camera.Transform;


            agent1 = new Agent(terrain, Content, camera, GraphicsDevice, light);
            agent2 = new Agent(terrain, Content, camera, GraphicsDevice, light);
            agent3 = new Agent(terrain, Content, camera, GraphicsDevice, light);
            agent4 = new Agent(terrain, Content, camera, GraphicsDevice, light);
            agent5 = new Agent(terrain, Content, camera, GraphicsDevice, light);
            player = new Player(terrain, Content, camera, GraphicsDevice, light);

            //player.Transform.Parent = camera.Transform;

            enemies = new Enemies(terrain, Content, camera, GraphicsDevice, light);
            enemies.player = player;


            font = Content.Load<SpriteFont>("Font");


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            bool isWPressed = InputManager.IsKeyDown(Keys.W);
            bool isSPressed = InputManager.IsKeyDown(Keys.S);


            if ((isWPressed || isSPressed) && audiInstance.State != SoundState.Playing)
            {
                audiInstance.Play();
            }

            // If neither W nor S is pressed, and the sound is playing, stop the sound
            if (!isWPressed && !isSPressed && audiInstance.State == SoundState.Playing)
            {
                audiInstance.Stop();
            }




            InputManager.Update();

            if (screenNum == 0)
            {
                //Debug.WriteLine("Scene 1");
                if (InputManager.IsKeyDown(Keys.Enter))
                {
                    screenNum = 1;
                }
            }
            else if (screenNum == 1)
            {
                Time.Update(gameTime);

                agent1.Update();
                agent2.Update();
                agent3.Update();
                agent4.Update();
                agent5.Update();
                player.Update();
                enemies.Update();

                Vector3 normal;
                if (player.Collider.Collides(agent1.Collider, out normal))
                {
                    Debug.WriteLine("agent1 collided");
                    bountyCollected++;
                    agent1.path = null;
                    collect_ring.Play();
                }
                if (player.Collider.Collides(agent2.Collider, out normal))
                {
     
                    bountyCollected++;
                    Debug.WriteLine("agent2 collided");
                    agent2.path = null;
                    collect_ring.Play();
                }
                if (player.Collider.Collides(agent3.Collider, out normal))
                {
                    
                    bountyCollected++;
                    Debug.WriteLine("agent3 collided");
                    agent3.path = null;
                    collect_ring.Play();
                }
                if (player.Collider.Collides(agent4.Collider, out normal))
                {
                    bountyCollected++;
                    Debug.WriteLine("agent4 collided");
                    agent4.path = null;
                    collect_ring.Play();

                }
                if (player.Collider.Collides(agent5.Collider, out normal))
                {
                    bountyCollected++;
                    Debug.WriteLine("agent5 collided");
                    agent5.path = null;
                    collect_ring.Play();

                }
                if (player.Collider.Collides(enemies.Collider, out normal))
                {
                    enemies.path = null;
                    negative_beep.Play();
                    screenNum = 3;
                }

                if (bountyCollected >= 3)
                {
                    screenNum = 2;
                    finalTime = Time.TotalGameTime.TotalSeconds.ToString();
                }


                if (InputManager.IsKeyDown(Keys.W) && !(terrain.GetAltitude(camera.Transform.LocalPosition + camera.Transform.Forward * Time.ElapsedGameTime * 10f) > 0.01f))
                {
                    camera.Transform.LocalPosition += camera.Transform.Forward * Time.ElapsedGameTime * 7;
                    //audi.Play();
                    //player.Transform.LocalPosition += player.Transform.Forward * Time.ElapsedGameTime * 7;
                }
               

                if (InputManager.IsKeyDown(Keys.S) && !(terrain.GetAltitude(camera.Transform.LocalPosition + camera.Transform.Backward * Time.ElapsedGameTime * 10f) > 0.01))
                {
                    camera.Transform.LocalPosition += camera.Transform.Backward * Time.ElapsedGameTime * 7;
                    //audi.Play();
                }

                if (InputManager.IsKeyDown(Keys.A))
                    camera.Transform.Rotate(Vector3.Up, Time.ElapsedGameTime * 5);
                //camera.Transform.LocalPosition += camera.Transform.Left * Time.ElapsedGameTime * 10;

                if (InputManager.IsKeyDown(Keys.D))
                    camera.Transform.Rotate(Vector3.Down, Time.ElapsedGameTime * 5);
                //camera.Transform.LocalPosition += camera.Transform.Right * Time.ElapsedGameTime * 10;



                if (InputManager.IsKeyPressed(Keys.Tab))
                {
                    camToggle = !camToggle;
                }



                if (InputManager.IsKeyDown(Keys.Up) && !((player.Transform.Position.Y) > 7))
                {
                    height = height + 0.25f;
                }
                if (InputManager.IsKeyDown(Keys.Down) && !((player.Transform.Position.Y) < 2))
                {
                    height = height - 0.25f;
                }


                //camera.Transform.LocalPosition = new Vector3(camera.Transform.LocalPosition.X,terrain.GetAltitude(camera.Transform.LocalPosition),camera.Transform.LocalPosition.Z) + Vector3.Up;
                //camera.Transform.LocalPosition = new Vector3(camera.Transform.LocalPosition.X, 0, camera.Transform.LocalPosition.Z) + Vector3.Up;
                camera.Transform.LocalPosition = new Vector3(camera.Transform.LocalPosition.X, height, camera.Transform.LocalPosition.Z) + Vector3.Up;
            }

                base.Update(gameTime);
        }

        public float altitude()
        {
            return terrain.GetAltitude(camera.Transform.LocalPosition);
        }

        protected override void Draw(GameTime gameTime)
        {

            //GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            if (screenNum == 0)
            {
                GraphicsDevice.Clear(Color.Black);
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                _spriteBatch.Begin();
                _spriteBatch.DrawString(font, "Game Task: Collect  10  Golden Balls to win the Game.", new Vector2(205, 50), Color.Red);
                _spriteBatch.DrawString(font, "Press [Enter] to start", new Vector2(320, 220), Color.Red);
                _spriteBatch.End();
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
            else if (screenNum == 1)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                /*effect.Parameters["View"].SetValue(camera.View);
                effect.Parameters["Projection"].SetValue(camera.Projection);
                effect.Parameters["World"].SetValue(terrain.Transform.World);
                effect.Parameters["LightPosition"].SetValue(light.Transform.Position);
                effect.Parameters["CameraPosition"].SetValue(camera.Transform.Position);
                effect.Parameters["NormalMap"].SetValue(terrain.NormalMap);*/


                if (camToggle)
                {
                    effect.Parameters["View"].SetValue(camera.View);
                    effect.Parameters["Projection"].SetValue(camera.Projection);
                    effect.Parameters["World"].SetValue(terrain.Transform.World);
                    effect.Parameters["CameraPosition"].SetValue(camera.Transform.Position);
                    effect.Parameters["LightPosition"].SetValue(light.Transform.Position);
                    effect.Parameters["NormalMap"].SetValue(terrain.NormalMap);
                }
                else
                {
                    effect.Parameters["View"].SetValue(thirdCam.View);
                    effect.Parameters["Projection"].SetValue(thirdCam.Projection);
                    effect.Parameters["World"].SetValue(terrain.Transform.World);
                    effect.Parameters["CameraPosition"].SetValue(camera.Transform.Position);
                    effect.Parameters["LightPosition"].SetValue(light.Transform.Position);
                    effect.Parameters["NormalMap"].SetValue(terrain.NormalMap);
                }


                _spriteBatch.Begin();
                _spriteBatch.DrawString(font, "Bounty Collected: " + bountyCollected + "/10", new Vector2(75, 10), Color.Red);
                _spriteBatch.DrawString(font, "Time Spent: " + Time.TotalGameTime.TotalSeconds, new Vector2(75, 30), Color.Red);
                _spriteBatch.End();
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    terrain.Draw();
                    player.Draw();
                    agent1.Draw();
                    agent2.Draw();
                    agent3.Draw();
                    agent4.Draw();
                    agent5.Draw();
                    enemies.Draw();
                }
                
            }
            else if (screenNum == 2)
            {
                GraphicsDevice.Clear(Color.Black);
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                _spriteBatch.Begin();
                _spriteBatch.DrawString(font, "You collected all 10 golden bounties in: " + finalTime, new Vector2(225, 50), Color.Red);
                _spriteBatch.DrawString(font, "Press [ESCAPE] To Exit", new Vector2(275, 150), Color.Red);
                _spriteBatch.End();
            }
            else
            {
                GraphicsDevice.Clear(Color.Black);
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                _spriteBatch.Begin();
                _spriteBatch.DrawString(font, "A  Bomb...Exploded on you...Boom...You died!", new Vector2(225, 100), Color.Red);
                _spriteBatch.DrawString(font, "Press [ESCAPE] To Exit", new Vector2(290, 200), Color.Red);
                _spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}