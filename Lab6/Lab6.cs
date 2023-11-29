using CPI311.GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Lab6
{
    public class Lab6 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        int numberCollision;



        BoxCollider boxCollider;
        SphereCollider sphere1, sphere2;

        List<Transform> transforms;
        List<Collider> colliders;
        List<Rigidbody> rigidbodies;


        Random random;
        Model model;
        Camera camera;
        Transform cameraTransform;



        public Lab6()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Time.Initialize();
            InputManager.Initialize();
            random = new Random();
            colliders = new List<Collider>();
            rigidbodies = new List<Rigidbody>();
            transforms = new List<Transform>();
            boxCollider = new BoxCollider();
            boxCollider.Size = 10;
            numberCollision = 0;


            for (int i = 0; i < 2; i++)
            {
                Transform transform = new Transform();
                transform.LocalPosition += Vector3.Right * 3 * i;


                Rigidbody rigidbody = new Rigidbody();
                rigidbody.Transform = transform;
                rigidbody.Mass = 1;

                Vector3 direction = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
                direction.Normalize();
                rigidbody.Velocity = direction * ((float)random.NextDouble() * 5 + 5);

                transforms.Add(transform);
                rigidbodies.Add(rigidbody);
                SphereCollider var = new SphereCollider();
                var.Radius = 1;

                var.Transform = transform;
                colliders.Add(var);

            }
            colliders.Add(boxCollider);

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


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Time.Update(gameTime);
            InputManager.Update();

            foreach (Rigidbody rigidbody in rigidbodies) 
                rigidbody.Update();
            //...


            Vector3 normal; // it is updated if a collision happens
            for (int i = 0; i < transforms.Count; i++)
            {
                if (boxCollider.Collides(colliders[i], out normal))
                {
                    numberCollision++;

                    if (Vector3.Dot(normal, rigidbodies[i].Velocity) < 0)
                        rigidbodies[i].Impulse +=
                        Vector3.Dot(normal, rigidbodies[i].Velocity) * -2 * normal;
                }
                for (int j = i + 1; j < transforms.Count; j++)
                {
                    if (colliders[i].Collides(colliders[j], out normal))
                        numberCollision++;


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
            //model.Draw(Matrix.Identity, camera.View, camera.Projection);


        // TODO: Add your drawing code here

        base.Draw(gameTime);
        }
    }
}