using CPI311.GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPI311.GameEngine
{
    public class Asteroid : GameObject
    {
        public Vector3 position;
        public Vector3 direction;
        public float speed;
        public bool isActive;

        public Asteroid(ContentManager Content, Camera camera, GraphicsDevice graphicsDevice, Light light) : base()
        {
            // *** Add Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1;
            Add<Rigidbody>(rigidbody);
            // *** Add Renderer
            Texture2D texture = Content.Load<Texture2D>("fire");
            Renderer renderer = new Renderer(Content.Load<Model>("asteroid4"),
            Transform, camera, Content, graphicsDevice, light, 1, null, 20f, texture);
            Add<Renderer>(renderer);
            // *** Add collider
            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = renderer.ObjectModel.Meshes[0].BoundingSphere.Radius * 0.95f;
            sphereCollider.Transform = Transform;
            Add<Collider>(sphereCollider);
            //*** Additional Property
            isActive = true;
        }

        public void Update(float delta)
        {
            position += direction * speed * GameConstants.AsteroidSpeedAdjustment * delta;

            if (position.X > GameConstants.PlayfieldSizeX)
            {
                position.X = -GameConstants.PlayfieldSizeX;
            }
            if (position.X < -GameConstants.PlayfieldSizeX)
            {
                position.X = GameConstants.PlayfieldSizeX;
            }
            if (position.Y > GameConstants.PlayfieldSizeY)
            {
                position.Y = -GameConstants.PlayfieldSizeY;
            }
            if (position.Y < -GameConstants.PlayfieldSizeY)
            {
                position.Y = GameConstants.PlayfieldSizeY;
            }

        }
    }
}
