using System;
using System.Collections.Generic;
using System.Text;
using CPI311.GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CPI311.GameEngine
{
    public class Ship : GameObject
    {
        public Model Model;
        public Matrix[] Transforms;

        //Position of the model in world space
        public Vector3 Position = Vector3.Zero;

        //Velocity of the model, applied each frame to the model's position
        public Vector3 Velocity = Vector3.Zero;
        private const float VelocityScale = 5.0f; //amplifies controller speed input

        public bool isActive = true;

        public Matrix RotationMatrix = Matrix.CreateRotationX(MathHelper.PiOver2);
        private float rotation = 0.0f;
        public float Rotation
        {
            get { return rotation; }
            set
            {
                float newVal = value;
                while (newVal >= MathHelper.TwoPi)
                {
                    newVal -= MathHelper.TwoPi;
                }
                while (newVal < 0)
                {
                    newVal += MathHelper.TwoPi;
                }

                if (rotation != value)
                {
                    rotation = value;
                    RotationMatrix = Matrix.CreateRotationX(MathHelper.PiOver2) *
                        Matrix.CreateRotationZ(rotation);
                }
            }
        }

        public Ship(ContentManager Content, Camera camera, GraphicsDevice graphicsDevice, Light light) : base()
        {
            // *** Add Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1;
            Add<Rigidbody>(rigidbody);
            // *** Add Renderer
            Texture2D texture = Content.Load<Texture2D>("fire");
            Renderer renderer = new Renderer(Content.Load<Model>("p1_wedge"),
            Transform, camera, Content, graphicsDevice, light, 1, null, 20f, texture);
            Add<Renderer>(renderer);
            // *** Add collider
            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = renderer.ObjectModel.Meshes[0].BoundingSphere.Radius * 0.0f;
            sphereCollider.Transform = Transform;
            Add<Collider>(sphereCollider);
            //*** Additional Property
            isActive = true;
        }

        public void ShipUpdate()
        {
            // Rotate the model using the A and D
            if (InputManager.IsKeyDown(Keys.A))
            {
                Rotation += 0.05f;
            }
            if (InputManager.IsKeyDown(Keys.D))
            {
                Rotation -= 0.05f;
            }

            /*
            // Create some velocity
            modelVelocityAdd = Vector3.Zero;

            // Find out what direction we should be thrusting, using rotation.
            modelVelocityAdd.X = -(float)Math.Sin(modelRotation);
            modelVelocityAdd.Z = -(float)Math.Cos(modelRotation);
            */

            //add velocity with W
            if (InputManager.IsKeyDown(Keys.W))
            {
                Velocity += RotationMatrix.Forward * VelocityScale;
            }
        }
    }
}
