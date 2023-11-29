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
        Camera camera2;
        Light light;

        Player player;
        //Agent agent;

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

            effect.Parameters["AmbientColor"].SetValue(new Vector3(0.1f, 0.1f, 0.1f));
            effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.3f, 0.1f, 0.1f));
            effect.Parameters["SpecularColor"].SetValue(new Vector3(0f, 0f, 0.2f));
            effect.Parameters["Shininess"].SetValue(20f);


            //Camera 1
            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.Up * 60;
            camera.Transform.Rotate(Vector3.Left, MathHelper.PiOver2-0.2f);

            light = new Light();
            light.Transform = new Transform();
            light.Transform.LocalPosition = Vector3.Backward * 5 + Vector3.Right * 5 + Vector3.Up * 5;

            player = new Player(terrain, Content, camera, GraphicsDevice,light);




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


            /*if ((InputManager.IsKeyDown(Keys.W)) && !((terrain.GetAltitude(camera.Transform.LocalPosition))>0) && !((terrain.GetAltitude(camera.Transform.LocalPosition)) > 0))
                camera.Transform.LocalPosition += camera.Transform.Forward * Time.ElapsedGameTime * 8;

            if (InputManager.IsKeyDown(Keys.S))
                camera.Transform.LocalPosition += camera.Transform.Backward * Time.ElapsedGameTime * 8;


            if (InputManager.IsKeyDown(Keys.A))
                *//* camera.Transform.LocalPosition += camera.Transform.Left * Time.ElapsedGameTime * 10 *//*
                camera.Transform.Rotate(Vector3.Up, Time.ElapsedGameTime*2);


            if (InputManager.IsKeyDown(Keys.D))
                *//*camera.Transform.LocalPosition += camera.Transform.Right * Time.ElapsedGameTime * 10 *//*
                camera.Transform.Rotate(Vector3.Down, Time.ElapsedGameTime*2);*/

            //for the camera to not climb 

            /*camera.Transform.LocalPosition = new Vector3(camera.Transform.LocalPosition.X, *//*terrain.GetAltitude(camera.Transform.LocalPosition)*//*0, camera.Transform.LocalPosition.Z) + Vector3.Up;  // not climbing up the wall*/

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["World"].SetValue(terrain.Transform.World);
            effect.Parameters["CameraPosition"].SetValue(camera.Transform.Position);
            effect.Parameters["LightPosition"].SetValue(camera.Transform.Position);   // +Vector3.Up * 10
            effect.Parameters["NormalMap"].SetValue(terrain.NormalMap);


            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                terrain.Draw();
                player.Draw();
            }


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}