using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Threading;
using System.Linq;

namespace Assignment3
{
    public class Assignment3 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        BoxCollider boxCollider;
        Random random;
        
        Model model;
        Camera camera;
        Transform cameraTransfrom;

        bool haveThreadRunning = false;
        int lastSecondCollisions = 0;
        SpriteFont font;

        Light light;
        List<GameObject> gameObjects;

        int numberCollisions;
        int numberOfBalls;

        private double elapsedTotalMilliseconds = 0;
        private int frameCount = 0;
        private double fps = 0;

        bool showDiagnostics;
        bool showColor;

        public Assignment3()
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

            haveThreadRunning = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(CollisionReset));

            random = new Random();
            boxCollider = new BoxCollider();
            gameObjects = new List<GameObject>();
            boxCollider.Size = 10;
            numberOfBalls = 0;
            showDiagnostics = true;
            showColor = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            model = Content.Load<Model>("Sphere");
            cameraTransfrom = new Transform();
            cameraTransfrom.LocalPosition = Vector3.Backward * 20;
            camera = new Camera();
            camera.Transform = cameraTransfrom;
            font = Content.Load<SpriteFont>("Font");
            light = new Light();

            AddGameObject();
            AddGameObject();
            AddGameObject();
            AddGameObject();
            AddGameObject();

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            InputManager.Update();
            Time.Update(gameTime);

            if (InputManager.IsKeyPressed(Keys.Up))
            {
                AddGameObject();
            }
            if (InputManager.IsKeyPressed(Keys.Down))
            {
                RemoveGameObject();
            }
            if (InputManager.IsKeyDown(Keys.Left))
            {
                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.Rigidbody.Velocity /= 1.05f;
                }
            }
            if (InputManager.IsKeyDown(Keys.Right))
            {
                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.Rigidbody.Velocity *= 1.05f;
                }
            }
            if (InputManager.IsKeyPressed(Keys.LeftShift))
            {
                showDiagnostics = !showDiagnostics;
            }
            if (InputManager.IsKeyPressed(Keys.Space))
            {
                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.Remove<Renderer>();
                }
                showColor = true;
            }
            if (InputManager.IsKeyPressed(Keys.LeftAlt))
            {
                foreach (GameObject gameObject in gameObjects)
                {
                    Texture2D texture = Content.Load<Texture2D>("Square");
                    Renderer renderer = new Renderer(model, gameObject.Transform, camera, Content, GraphicsDevice,light, 2, "SimpleShading", 20f, texture);
                    gameObject.Add<Renderer>(renderer);
                }
                showColor = false;
            }

            // TODO: Add your update logic here
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update();
            }

            Vector3 normal; // it is updated if a collision happens
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (boxCollider.Collides(gameObjects[i].Collider, out normal))
                {
                    numberCollisions++;
                    if (Vector3.Dot(normal, gameObjects[i].Rigidbody.Velocity) < 0)
                    {
                        gameObjects[i].Rigidbody.Impulse += (Vector3.Dot(normal, gameObjects[i].Rigidbody.Velocity) * -2 * normal) / 8;
                    }
                }
                for (int j = i + 1; j < gameObjects.Count; j++)
                {
                    SphereCollider sc = gameObjects[i].Get<Collider>() as SphereCollider;
                    if (sc.SweptCollides(gameObjects[j].Collider, gameObjects[j].Rigidbody.LastPosition, gameObjects[j].Rigidbody.LastPosition, out normal))
                    //if (gameObjects[i].Collider.Collides(gameObjects[j].Collider, out normal))
                    {
                        numberCollisions++;
                    }
                    Vector3 velocityNormal = Vector3.Dot(normal, gameObjects[i].Rigidbody.Velocity - gameObjects[j].Rigidbody.Velocity) * -2 * normal * gameObjects[i].Rigidbody.Mass * gameObjects[j].Rigidbody.Mass;
                    

                    gameObjects[i].Rigidbody.Impulse += velocityNormal / 5;
                    gameObjects[j].Rigidbody.Impulse += -velocityNormal / 5;
                }
            }

            //Calculating the FPS
            // Calculate the time passed since the last frame
            double elapsedMilliseconds = gameTime.ElapsedGameTime.TotalMilliseconds;

            // Update FPS calculation
            elapsedTotalMilliseconds += elapsedMilliseconds;
            frameCount++;

            if (elapsedTotalMilliseconds >= 1000) // Update FPS every second
            {
                fps = frameCount;
                frameCount = 0;
                elapsedTotalMilliseconds = 0;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            for (int i = 0; i < gameObjects.Count; i++)
            {
                float speed = gameObjects[i].Rigidbody.Velocity.Length();
                float speedValue = MathHelper.Clamp(speed / 50f, 0, 1);
                (model.Meshes[0].Effects[0] as BasicEffect).DiffuseColor = new Vector3(speedValue, speedValue, 1);
                model.Draw(gameObjects[i].Transform.World, camera.View, camera.Projection);
            }

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw();
            }


            _spriteBatch.Begin();

            if (showDiagnostics)
            {
                _spriteBatch.DrawString(font, "Collisons: " + lastSecondCollisions, new Vector2(40, 10), Color.Black);
                _spriteBatch.DrawString(font, "Balls: " + numberOfBalls, new Vector2(40, 30), Color.Black);
                _spriteBatch.DrawString(font, "FPS: " + fps, new Vector2(40, 50), Color.Black);

                if (showColor)
                {
                    _spriteBatch.DrawString(font, "Sphere Visual: Speed Color", new Vector2(40, 70), Color.Black);
                }
                else
                {
                    _spriteBatch.DrawString(font, "Sphere Visual: Material Texture", new Vector2(40, 70), Color.Black);
                }
            }

            _spriteBatch.DrawString(font, "Left Arrow to Decrease Speed", new Vector2(40, 310), Color.Black);
            _spriteBatch.DrawString(font, "Right Arrow to Increase Speed", new Vector2(40, 330), Color.Black);
            _spriteBatch.DrawString(font, "Up Arrow to Add Sphere", new Vector2(40, 350), Color.Black);
            _spriteBatch.DrawString(font, "Down Arrow to Remove Sphere", new Vector2(40, 370), Color.Black);
            _spriteBatch.DrawString(font, "Left Shift to Hide/Show Diagnostics", new Vector2(40, 390), Color.Black);
            _spriteBatch.DrawString(font, "Space to Show Speed Colors", new Vector2(40, 410), Color.Black);
            _spriteBatch.DrawString(font, "Left Alt to Show Textures", new Vector2(40, 430), Color.Black);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void CollisionReset(Object obj)
        {
            while (haveThreadRunning)
            {
                lastSecondCollisions = numberCollisions;
                numberCollisions = 0;
                System.Threading.Thread.Sleep(1000);
            }
        }

        private void AddGameObject()
        {
            GameObject gameObject = new GameObject();
            Transform transform = gameObject.Transform;
            transform.LocalPosition += Vector3.Right * ((float)random.NextDouble() * 5);
            //transform.LocalPosition += new Vector3((float)random.NextDouble() * 10, (float)random.NextDouble() * 8, (float)random.NextDouble() * 8);

            //rigidbody
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = transform;
            rigidbody.Mass = (float)random.NextDouble();
            Vector3 direction = new Vector3(
                (float)random.NextDouble(),
                (float)random.NextDouble(),
                (float)random.NextDouble());
            direction.Normalize();
            rigidbody.Velocity = direction * ((float)random.NextDouble() * 5 + 5); //rand value from 5-10
            
            //collider
            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = 1.0f * transform.LocalScale.Y;
            sphereCollider.Transform = transform;

            //renderer
            if (!showColor)
            {
                Texture2D texture = Content.Load<Texture2D>("Square");
                Renderer renderer = new Renderer(model, transform, camera, Content, GraphicsDevice,light, 2, "SimpleShading", 20f, texture) ;
                gameObject.Add<Renderer>(renderer);
            }
            

            gameObject.Add<Rigidbody>(rigidbody);
            gameObject.Add<Collider>(sphereCollider);
            
            
            gameObjects.Add(gameObject);
            numberOfBalls++;

        }

        private void RemoveGameObject()
        {
            if (gameObjects.Count > 0)
            {
                gameObjects.Remove(gameObjects.Last());
            }
            if (numberOfBalls > 0)
            {
                numberOfBalls--;
            }
        }
    }
}