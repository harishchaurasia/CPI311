using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CPI311.GameEngine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace FinalProject
{
    public class Player : GameObject
    {

        Camera _camera;
        public TerrainRenderer Terrain { get; set; }

        public Player(TerrainRenderer terrain, ContentManager Content, Camera camera, GraphicsDevice graphicsDevice, Light light) : base()
        {
            Terrain = terrain;
            // *** Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1;
            Add<Rigidbody>(rigidbody);

            // *** SphereCollider
            SphereCollider collider = new SphereCollider();
            collider.Radius = 1;
            collider.Transform = Transform;
            Add<Collider>(collider);

            // *** Renderer
            Texture2D texture = Content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(Content.Load<Model>("Sphere"), Transform, camera, Content, graphicsDevice, light, 1, "SimpleShading", 20f, texture);
            renderer.Material.Diffuse = Color.Indigo.ToVector3();
            renderer.Material.Ambient = Color.Indigo.ToVector3();
            renderer.Material.Specular = Color.Indigo.ToVector3();
            Add<Renderer>(renderer);

            _camera = camera;

            //this.Transform.Parent = _camera.Transform;

        }

        public override void Update()
        {
            // Control the player
            /*if (InputManager.IsKeyDown(Keys.W)) // move forward
            {
                if (Terrain.GetAltitude(this.Transform.LocalPosition + this.Transform.Forward) <= 4)
                {
                    this.Transform.LocalPosition += this.Transform.Forward * Time.ElapsedGameTime * 10f;
                }
            }
            if (InputManager.IsKeyDown(Keys.S)) // move backwards
            {
                if (Terrain.GetAltitude(this.Transform.LocalPosition + this.Transform.Backward) <= 4)
                {
                    this.Transform.LocalPosition += this.Transform.Backward * Time.ElapsedGameTime * 10f;
                }
            }*/

            /*if (InputManager.IsKeyDown(Keys.A)) // move left
            {
                if (Terrain.GetAltitude(this.Transform.LocalPosition + this.Transform.Left) <= 4)
                {
                    this.Transform.LocalPosition += this.Transform.Left * Time.ElapsedGameTime * 10f;

                    //this.Transform.Rotate(Vector3.Down, Time.ElapsedGameTime * 3);
                }
            }
            if (InputManager.IsKeyDown(Keys.D)) // move right
            {
                if (Terrain.GetAltitude(this.Transform.LocalPosition + this.Transform.Right) <= 4)
                {
                    this.Transform.LocalPosition += this.Transform.Right * Time.ElapsedGameTime * 10f;
                }
            }
            */



            //this.Transform.LocalPosition = new Vector3(this.Transform.LocalPosition.X,Terrain.GetAltitude(this.Transform.LocalPosition),this.Transform.LocalPosition.Z) + Vector3.Up;

            this.Transform.LocalPosition = _camera.Transform.LocalPosition;

            base.Update();
        }
    }
}
