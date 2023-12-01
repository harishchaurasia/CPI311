using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;

namespace Assignment5
{
    public class Assignmnet5 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        TerrainRenderer terrain;
        Effect effect;

        Camera camera;
        Camera miniMap;

        Light light;

        Player player;
        Agent agent;
        Bomb bomb;

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
            /*            effect.Parameters["AmbientColor"].SetValue(new Vector3(0.3f, 0.3f, 0.3f));
                        effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.5f, 0.5f, 0.5f));
                        effect.Parameters["SpecularColor"].SetValue(new Vector3(0.1f, 0.1f, 0.1f));*/
            effect.Parameters["Shininess"].SetValue(20f);


            //Camera 1
            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.Up * 50;
            camera.Transform.Rotate(Vector3.Left, MathHelper.PiOver2 - 0.2f);

            /*//MiniMap
            miniMap = new Camera();
            miniMap.Position = new Vector2(1f,1f);
            miniMap.Size = new Vector2(.5f, .5f);
            miniMap.Transform = new Transform();
            miniMap.Transform.LocalPosition = new Vector3(0,100,0);
            miniMap.Transform.LookAt(Vector3.Zero);

            miniMap.Projection = Matrix.CreateOrthographic(100, 100, 0.1f, 1000f);
            miniMap.Transform = new Transform();
            miniMap.Transform.LocalPosition = new Vector3(0, 50, 0); // Adjust as needed
            miniMap.Transform.Rotate(Vector3.Left, MathHelper.PiOver2); */

            light = new Light();
            light.Transform = new Transform();
            light.Transform.LocalPosition = Vector3.Backward * 5 + Vector3.Right * 5 + Vector3.Up * 5;

            player = new Player(terrain, Content, camera, GraphicsDevice, light);
            agent = new Agent(terrain, Content, camera, GraphicsDevice, light);
            bomb = new Bomb(terrain, Content, camera, GraphicsDevice, light);
            bomb.player = player;



            /*//Camera 2
            camera2 = new Camera();
            camera2.Transform = new Transform();
            camera2.Transform.LocalPosition = Vector3.Backward * 5 + Vector3.Right * 3 + Vector3.Up * 50;*/
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            Time.Update(gameTime);
            InputManager.Update();



            if (InputManager.IsKeyDown(Keys.Up))
                camera.Transform.Rotate(Vector3.Right, Time.ElapsedGameTime);

            if (InputManager.IsKeyDown(Keys.Down))
                camera.Transform.Rotate(Vector3.Left, Time.ElapsedGameTime);


            player.Update();
            agent.Update();
            bomb.Update();
            
            base.Update(gameTime);
        }

        public float altitude()
        {
            return terrain.GetAltitude(camera.Transform.LocalPosition);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
/*
            GraphicsDevice.Viewport = miniMap.Viewport;
            effect.Parameters["View"].SetValue(miniMap.View);
            effect.Parameters["Projection"].SetValue(miniMap.Projection);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                terrain.Draw();
                player.Draw();
                agent.Draw();
            }*/



            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["World"].SetValue(terrain.Transform.World);
            effect.Parameters["CameraPosition"].SetValue(camera.Transform.Position);
            effect.Parameters["LightPosition"].SetValue(light.Transform.Position);   // +Vector3.Up * 10
            effect.Parameters["NormalMap"].SetValue(terrain.NormalMap);



            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                terrain.Draw();
                player.Draw();
                agent.Draw();
                bomb.Draw();    
            }

            



            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}