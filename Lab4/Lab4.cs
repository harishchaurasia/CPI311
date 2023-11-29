using CPI311.GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lab4
{
    public class Lab4 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Model model;

        Transform modelTransfrm;
        Transform cameraTransfrm;
        Camera camera;

        Model parent;
        Transform parentTransfrm;


        

        public Lab4()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            InputManager.Initialize();
            Time.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);



            


            

            // TODO: use this.Content to load your game content here
            model = Content.Load<Model>("Sphere");
            modelTransfrm = new Transform();
            cameraTransfrm = new Transform();
            cameraTransfrm.LocalPosition = Vector3.Backward * 3;
            camera = new Camera();
            camera.Transform = cameraTransfrm;

            // LAB 4-C ****************************
            parent = Content.Load<Model>("Torus");
            parentTransfrm = new Transform();
            parentTransfrm.LocalPosition = Vector3.Right * 5;
            modelTransfrm.Parent = parentTransfrm;

            //attach the Torus as a child of the sphere




            _spriteBatch = new SpriteBatch(GraphicsDevice);


            //lighting effect

            foreach (ModelMesh mesh in model.Meshes)
            { 
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }

            foreach (ModelMesh mesh in parent.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {

            InputManager.Update();
            Time.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if (InputManager.IsKeyDown(Keys.W))
            {
                cameraTransfrm.LocalPosition += cameraTransfrm.Forward * Time.ElapsedGameTime;
            }
            if (InputManager.IsKeyDown(Keys.S))
            {
                cameraTransfrm.LocalPosition += cameraTransfrm.Backward * Time.ElapsedGameTime;
            }
            if (InputManager.IsKeyDown(Keys.A))
            {
                cameraTransfrm.Rotate(Vector3.Up, Time.ElapsedGameTime);
                cameraTransfrm.UpdateWorld();
            }
            if (InputManager.IsKeyDown(Keys.D))
            {
                cameraTransfrm.Rotate(Vector3.Down , Time.ElapsedGameTime);
                cameraTransfrm.UpdateWorld();
            }



            if (InputManager.IsKeyDown(Keys.Up))
            {
                parentTransfrm.LocalPosition += parentTransfrm.Up * Time.ElapsedGameTime;
            }
            if (InputManager.IsKeyDown(Keys.Down))
            {
                parentTransfrm.LocalPosition += parentTransfrm.Down * Time.ElapsedGameTime;
            }
            if (InputManager.IsKeyDown(Keys.Left))
            {
                parentTransfrm.Rotate(Vector3.Up, Time.ElapsedGameTime);
                parentTransfrm.UpdateWorld();
            }
            if (InputManager.IsKeyDown(Keys.Right))
            {
                parentTransfrm.Rotate(Vector3.Down, Time.ElapsedGameTime);
                parentTransfrm.UpdateWorld();
            }




            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            model.Draw(modelTransfrm.World, camera.View, camera.Projection);
            parent.Draw(parentTransfrm.World, camera.View, camera.Projection);

           /* Matrix view = Matrix.CreateLookAt(
                new Vector3(0, 0, 5),
                Vector3.Zero,
                Vector3.Up);

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver2,
                GraphicsDevice.Viewport.AspectRatio,
                0.1f,
                100f);

            model.Draw(Matrix.Identity, view, projection);  

            */

            base.Draw(gameTime);
        }
    }
}