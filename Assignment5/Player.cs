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

namespace Assignment5
{
    public class Player : GameObject
    {
        public TerrainRenderer Terrain { get; set; }
        public Player(TerrainRenderer terrain, ContentManager Content, Camera camera,
        GraphicsDevice graphicsDevice, Light light) : base()
        {
            Terrain = terrain;

            //Rigidbody

            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1.0f;
            Add<Rigidbody>(rigidbody);

            //SphereCollider
            SphereCollider collider = new SphereCollider(); 
            collider.Radius = 1.0f;
            collider.Transform = Transform;
            Add<Collider>(collider);

            //Renderer
            Texture2D texture = Content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(Content.Load<Model>("Sphere"),Transform, camera, Content, graphicsDevice, light, 1,"SimpleShading", 20f, texture);
            Add<Renderer>(renderer);




        }
        public override void Update()
        {
            // Control the player
            if (InputManager.IsKeyDown(Keys.W)) // move forward
                this.Transform.LocalPosition += this.Transform.Forward * Time.ElapsedGameTime * 10;

            if (InputManager.IsKeyDown(Keys.S)) // move backwars
                this.Transform.LocalPosition += this.Transform.Backward * Time.ElapsedGameTime * 10;

            if (InputManager.IsKeyDown(Keys.W)) // move left
                this.Transform.LocalPosition += this.Transform.Left * Time.ElapsedGameTime * 10;

            if (InputManager.IsKeyDown(Keys.S)) // move right
                this.Transform.LocalPosition += this.Transform.Right * Time.ElapsedGameTime * 10;

            // change the Y position corresponding to the terrain (maze)
            this.Transform.LocalPosition = new Vector3(this.Transform.LocalPosition.X,Terrain.GetAltitude(this.Transform.LocalPosition),this.Transform.LocalPosition.Z) + Vector3.Up;

            base.Update();
        }
    }

}
