using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
//using SharpDX.Direct3D9;
//sing SharpDX.Direct3D9;

namespace FinalProject
{
    public class FinalProject : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        
        TerrainRenderer terrain;
        Camera camera;
        Camera topCam;
        Camera activatedCam;
        bool camToggle = true;
        Effect effect;
        Light light;

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
            //camera.Transform = player.Transform;


            /*topCam = new Camera();
            topCam.Transform = new Transform();
            topCam.Transform.LocalPosition = Vector3.Up * 50;
            topCam.Transform.Rotate(Vector3.Left, MathHelper.PiOver2 - 0.2f);*/


            light = new Light();
            light.Transform = new Transform();
            light.Transform.LocalPosition = Vector3.Backward * 5 + Vector3.Right * 5 + Vector3.Up * 5;


            agent1 = new Agent(terrain, Content, camera, GraphicsDevice, light);
            agent2 = new Agent(terrain, Content, camera, GraphicsDevice, light);
            agent3 = new Agent(terrain, Content, camera, GraphicsDevice, light);
            agent4 = new Agent(terrain, Content, camera, GraphicsDevice, light);
            agent5 = new Agent(terrain, Content, camera, GraphicsDevice, light);
            player = new Player(terrain, Content, camera, GraphicsDevice, light);


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();




            Time.Update(gameTime);
            InputManager.Update();

            


            if (InputManager.IsKeyDown(Keys.W))
                camera.Transform.LocalPosition += camera.Transform.Forward * Time.ElapsedGameTime * 10;

            if (InputManager.IsKeyDown(Keys.S))
                camera.Transform.LocalPosition += camera.Transform.Backward * Time.ElapsedGameTime * 10;

            if (InputManager.IsKeyDown(Keys.A))
                camera.Transform.Rotate(Vector3.Up, Time.ElapsedGameTime * 3);
            //camera.Transform.LocalPosition += camera.Transform.Left * Time.ElapsedGameTime * 10;

            if (InputManager.IsKeyDown(Keys.D))
                camera.Transform.Rotate(Vector3.Down, Time.ElapsedGameTime * 3);
            //camera.Transform.LocalPosition += camera.Transform.Right * Time.ElapsedGameTime * 10;

            Vector3 normal;
            if (player.Collider.Collides(agent1.Collider, out normal))
            {
                agent1.path = null;
                bountyCollected++;
            }
            if (player.Collider.Collides(agent2.Collider, out normal))
            {
                agent2.path = null;
                bountyCollected++;
            }
            if (player.Collider.Collides(agent3.Collider, out normal))
            {
                agent3.path = null;
                bountyCollected++;
            }
            if (player.Collider.Collides(enemies.Collider, out normal))
            {
                enemies.path = null;
                screenNum = 3;
            }

            if (bountyCollected >= 3)
            {
                screenNum = 2;
                finalTime = Time.TotalGameTime.TotalSeconds.ToString();
            }
        }



        /*if (InputManager.IsKeyDown(Keys.Tab))
        {
            camToggle = !camToggle;
        }


        if (InputManager.IsKeyDown(Keys.Up))
        {
            topCam.Transform.Rotate(Vector3.Right, Time.ElapsedGameTime);
        }
        if (InputManager.IsKeyDown(Keys.Down))
        {
            topCam.Transform.Rotate(Vector3.Left, Time.ElapsedGameTime);
        }*/



        //camera.Transform.LocalPosition = new Vector3(camera.Transform.LocalPosition.X,terrain.GetAltitude(camera.Transform.LocalPosition),camera.Transform.LocalPosition.Z) + Vector3.Up;
        //camera.Transform.LocalPosition = new Vector3(camera.Transform.LocalPosition.X,0, camera.Transform.LocalPosition.Z) + Vector3.Up;
        camera.Transform.LocalPosition = new Vector3(camera.Transform.LocalPosition.X, 0, camera.Transform.LocalPosition.Z) + Vector3.Up;


            agent1.Update();
            agent2.Update();
            agent3.Update();
            agent4.Update();
            agent5.Update();
            player.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            activatedCam = camToggle ? camera : topCam;

            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["World"].SetValue(terrain.Transform.World);
            effect.Parameters["CameraPosition"].SetValue(camera.Transform.Position);
            effect.Parameters["LightPosition"].SetValue(light.Transform.Position);
            effect.Parameters["NormalMap"].SetValue(terrain.NormalMap);




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
            }


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}