using CPI311.GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Threading;


namespace Lab7
{
    public class Lab7 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        BoxCollider boxCollider;
        //SphereCollider sphere1, sphere2;

        List<Transform> transforms;
        List<Collider> colliders;
        List<Rigidbody> rigidbodies;
        List<Renderer> renderers; 



        Random random;
        Model model;
        Camera camera;
        Transform cameraTransform;


        //Lab7

        int numberCollisions = 0;
        bool haveThreadRunning = false;
        int lastSecondCollisions = 0;
        SpriteFont font;

        //**************
        public Lab7()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.GraphicsProfile = GraphicsProfile.HiDef;

        }

        protected override void Initialize()
        {
            Time.Initialize();
            InputManager.Initialize();

            //lab7
            haveThreadRunning = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(CollisionReset));
            //****************


            random = new Random();
            colliders = new List<Collider>();
            rigidbodies = new List<Rigidbody>();
            transforms = new List<Transform>();
            boxCollider = new BoxCollider();
            renderers = new List<Renderer>();

            boxCollider.Size = 10;





            //for (int i = 0; i < 2; i++)
            //{
            //    Transform transform = new Transform();
            //    transform.LocalPosition += Vector3.Right * 3 * i;



            //    Rigidbody rigidbody = new Rigidbody();
            //    rigidbody.Transform = transform;
            //    rigidbody.Mass = 1.0f;

            //    Vector3 direction = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            //    direction.Normalize();
            //    rigidbody.Velocity = direction * ((float)random.NextDouble() * 5 + 5);

            //    transforms.Add(transform);
            //    rigidbodies.Add(rigidbody);


            //    SphereCollider var = new SphereCollider();
            //    var.Radius = 1;

            //    var.Transform = transform;
            //    colliders.Add(var);

            //}
            //colliders.Add(boxCollider);

            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            model = Content.Load<Model>("Sphere");
            cameraTransform = new Transform();
            cameraTransform.LocalPosition = Vector3.Backward * 20;
            camera = new Camera();
            camera.Transform = cameraTransform;


            font = Content.Load<SpriteFont>("Font");


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Time.Update(gameTime);
            InputManager.Update();

            if(InputManager.IsKeyPressed(Keys.Space))
            {
                AddSphere();
            }

            foreach (Rigidbody rigidbody in rigidbodies)
                rigidbody.Update();
            //...


            Vector3 normal; // it is updated if a collision happens
            for (int i = 0; i < transforms.Count; i++)
            {
                if (boxCollider.Collides(colliders[i], out normal))
                {
                    numberCollisions++;

                    if (Vector3.Dot(normal, rigidbodies[i].Velocity) < 0)
                        rigidbodies[i].Impulse +=
                        Vector3.Dot(normal, rigidbodies[i].Velocity) * -2 * normal;
                }
                for (int j = i + 1; j < transforms.Count; j++)
                {
                    if (colliders[i].Collides(colliders[j], out normal))
                        numberCollisions++;

                    Vector3 velocityNormal = Vector3.Dot(normal,
                    rigidbodies[i].Velocity - rigidbodies[j].Velocity) * -2
                    * normal * rigidbodies[i].Mass * rigidbodies[j].Mass;
                    rigidbodies[i].Impulse += velocityNormal / 2;
                    rigidbodies[j].Impulse += -velocityNormal / 2;


                    // TODO: Add your update logic here

                    base.Update(gameTime);
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (Transform transform in transforms)
                model.Draw(transform.World, camera.View, camera.Projection);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "Colision: " + lastSecondCollisions, Vector2.Zero, Color.Black);
            _spriteBatch.End();



            for (int i = 0; i < transforms.Count; i++)
            {
                Transform transform = transforms[i];
                float speed = rigidbodies[i].Velocity.Length();
                float speedValue = MathHelper.Clamp(speed / 20f, 0, 1);
                (model.Meshes[0].Effects[0] as BasicEffect).DiffuseColor =
                new Vector3(speedValue, speedValue, 1);
                model.Draw(transform.World, camera.View, camera.Projection);
            }
            for (int i = 0; i < renderers.Count; i++) renderers[i].Draw();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        //lab 07 part

        private void CollisionReset(Object obj)
        {
            while (haveThreadRunning)
            {
                lastSecondCollisions = numberCollisions;
                numberCollisions = 0;
                System.Threading.Thread.Sleep(1000);
            }
        }
        private void AddSphere()
        {
                Transform transform = new Transform();
                transform.LocalPosition += Vector3.Right * 3;



                Rigidbody rigidbody = new Rigidbody();
                rigidbody.Transform = transform;
                rigidbody.Mass = 1.0f;

                Vector3 direction = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
                direction.Normalize();
                rigidbody.Velocity = direction * ((float)random.NextDouble() * 5 + 5);

                //renderer
                Texture2D texture = Content.Load<Texture2D>("Square");
                Renderer renderer = new Renderer(model, transform, camera, Content, GraphicsDevice, 2, "SimpleShading", 20f, texture);
                renderers.Add(renderer);


                transforms.Add(transform);
                rigidbodies.Add(rigidbody);


                SphereCollider var = new SphereCollider();
                var.Radius = 1;

                var.Transform = transform;
                colliders.Add(var);
        }
    }
}