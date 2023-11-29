using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

using CPI311.GameEngine;

namespace Game1
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Model asteroidModel;
        Matrix[] asteroidTransforms;
        Asteroid[] asteroidList = new Asteroid[GameConstants.NumAsteroids];
        Random random = new Random();


        Ship ship = new Ship();

        SoundEffect soundEngine;
        SoundEffect soundHyperspaceActivation;
        SoundEffectInstance soundEngineInstance;


        Matrix projectionMatrix;

        // Camera position
        Vector3 cameraPosition = new Vector3(0.0f, 0.0f, GameConstants.CameraHeight);



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), GraphicsDevice.DisplayMode.AspectRatio, GameConstants.CameraHeight - 1000.0f, GameConstants.CameraHeight + 1000.0f);

            ResetAsteroids(); // Initialize and position the asteroids

            base.Initialize();
        }
        private Matrix[] SetupEffectDefaults(Model myModel)

        {

            Matrix[] absoluteTransforms = new Matrix[myModel.Bones.Count]; myModel.CopyAbsoluteBoneTransformsTo(absoluteTransforms);

            foreach (ModelMesh mesh in myModel.Meshes)

            {

                foreach (BasicEffect effect in mesh.Effects)

                {

                    effect.EnableDefaultLighting();

                    effect.Projection = projectionMatrix;

                    effect.View = viewMatrix;

                }

            }

            return absoluteTransforms;

        }



        protected override void LoadContent()
        {
            {
                spriteBatch = new SpriteBatch(GraphicsDevice);

                // Load the ship and asteroid models and setup their transforms
                ship.Model = Content.Load<Model>("p1_wedge");
                ship.Transforms = SetupEffectDefaults(ship.Model);
                asteroidModel = Content.Load<Model>("asteroid1");
                asteroidTransforms = SetupEffectDefaults(asteroidModel);

                // Load sound effects
                soundEngine = Content.Load<SoundEffect>("engine_2");
                soundEngineInstance = soundEngine.CreateInstance();
                soundHyperspaceActivation = Content.Load<SoundEffect>("hyperspace_activate");
            }
        }

        protected override void UnloadContent()
        {
            // Unload content (if any).
        }

        protected override void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Your existing code
            UpdateInput();
            ship.Position += ship.Velocity;
            ship.Velocity *= 0.95f;

            // Update asteroids
            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
                asteroidList[i].Update(timeDelta);
            }

            // Handle user input and update the ship's position
            ship.Update(timeDelta);
            base.Update(gameTime);
        }




        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            // Render the ship
            Matrix shipTransformMatrix = ship.RotationMatrix * Matrix.CreateTranslation(ship.Position);
            DrawModel(ship.Model, shipTransformMatrix, ship.Transforms);

            // Loop through asteroids and render them
            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
                Matrix asteroidTransform = Matrix.CreateTranslation(asteroidList[i].position);
                DrawModel(asteroidModel, asteroidTransform, asteroidTransforms);
            }

            base.Draw(gameTime);
        }

        public static void DrawModel(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms)

        {

            //Draw the model, a model can have multiple meshes, so loop

            foreach (ModelMesh mesh in model.Meshes)
            {
                //This is where the mesh orientation is set
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World =
                    absoluteBoneTransforms[mesh.ParentBone.Index] *
                    modelTransform;
                }
                //Draw the mesh, will use the effects set above.
                mesh.Draw();
            }
        }
        private void ResetAsteroids()
        {
            float xStart;
            float yStart;
            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {

                if (random.Next(2) == 0)

                {
                    xStart = (float)-GameConstants.PlayfieldSizeX;
                }

                else

                {

                    xStart = (float)GameConstants.PlayfieldSizeX;

                }

                yStart = (float)random.NextDouble() * GameConstants.PlayfieldSizeY; asteroidList[i].position = new Vector3(xStart, yStart, 0.0f); double angle = random.NextDouble() * 2 * Math.PI; asteroidList[i].direction.X = -(float)Math.Sin(angle); asteroidList[i].direction.Y = (float)Math.Cos(angle); asteroidList[i].speed = GameConstants.AsteroidMinSpeed +

                (float)random.NextDouble() * GameConstants.AsteroidMaxSpeed;
            }
        }
    }
}
