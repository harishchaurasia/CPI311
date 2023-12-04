using CPI311.GameEngine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using FinalProject;

namespace FinalProject
{
    public class Enemies : GameObject
    {
        public AStarSearch search;
        public List<Vector3> path;

        public Player player;

        private float speed = 5f; //moving speed
        private int gridSize = 20; //grid size
        private TerrainRenderer Terrain;

        public Enemies(TerrainRenderer terrain, ContentManager Content, Camera camera, GraphicsDevice graphicsDevice, Light light) : base()
        {
            Terrain = terrain;
            path = null;

            //Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1.0f;
            Add<Rigidbody>(rigidbody);

            //SphereCollider
            SphereCollider collider = new SphereCollider();
            collider.Radius = 1;
            collider.Transform = Transform;
            Add<Collider>(collider);

            //Renderer
            Texture2D texture = Content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(Content.Load<Model>("Sphere"), Transform, camera, Content, graphicsDevice, light, 1, "SimpleShading", 20f, texture);
            renderer.Material.Diffuse = Color.Black.ToVector3();
            renderer.Material.Ambient = Color.Black.ToVector3();
            renderer.Material.Specular = Color.Black.ToVector3();
            Add<Renderer>(renderer);

            //Make a path
            search = new AStarSearch(gridSize, gridSize); // AStar Search has the same grid size
            float gridW = Terrain.size.X / gridSize;
            float gridH = Terrain.size.Y / gridSize;

            for (int i = 0; i < gridSize; i++)
                for (int j = 0; j < gridSize; j++)
                {
                    Vector3 pos = new Vector3(gridW * i + gridW / 2 - Terrain.size.X / 2,
                        0,
                        gridH * j + gridH / 2 - Terrain.size.Y / 2);

                    if (Terrain.GetAltitude(pos) > 1.0)
                        search.Nodes[j, i].Passable = false;
                }
        }



        public override void Update()
        {
            if (path != null && path.Count > 0)
            {
                // Move to the destination along the path
                Vector3 currP = Transform.Position;
                Vector3 destP = GetGridPosition(path[0]);
                currP.Y = 0;
                destP.Y = 0;

                Vector3 direction = Vector3.Distance(currP, destP) == 0 ? Vector3.Zero : Vector3.Normalize(destP - currP);

                this.Rigidbody.Velocity = new Vector3(direction.X, 0, direction.Z) * speed;

                //Debug.WriteLine("P", currP);

                if (Vector3.Distance(currP, destP) < 1.0f) // if it reaches to a point, go to the next in path
                {
                    path.RemoveAt(0); // path[0] is removed
                    if (path.Count == 0) // if it reached to the goal
                    {
                        //path = null;
                        RandomPathFinding();
                        return;
                    }
                }
            }
            else
            {
                // Search again to make a new path.
                RandomPathFinding();
                Transform.LocalPosition = GetGridPosition(path[0]); //move to the start position

            }

            this.Transform.LocalPosition = new Vector3(this.Transform.LocalPosition.X, Terrain.GetAltitude(this.Transform.LocalPosition), this.Transform.LocalPosition.Z) + Vector3.Up;

            Transform.Update();
            base.Update();
        }
        private Vector3 GetGridPosition(Vector3 gridPos)
        {
            float gridW = Terrain.size.X / search.Cols;
            float gridH = Terrain.size.Y / search.Rows;
            return new Vector3(gridW * gridPos.X + gridW / 2 - Terrain.size.X / 2,
                0,
                gridH * gridPos.Z + gridH / 2 - Terrain.size.Y / 2);
        }

        //public void RandomPathFinding()
        public void RandomPathFinding()
        {
            Random random = new Random();
            if (path == null)
            {
                while (!(search.Start = search.Nodes[random.Next(search.Rows), random.Next(search.Cols)]).Passable) ;
            }
            else
            {
                search.Start = search.End;
            }

            /*search.End = search.Nodes[search.Rows / 2, search.Cols / 2]; // center position*/
            search.End = search.Nodes[(int)((player.Transform.Position.X + 45) / 5) % search.Rows + 1, (int)((player.Transform.Position.Z + 45) / 5) % search.Cols + 1];
            search.Search();
            path = new List<Vector3>();
            AStarNode current = search.End;
            var count = 0;
            while (current != null) // List class should have reverse function
            {
                count++;
                path.Insert(0, current.Position);
                current = current.Parent;
            }
        }
    }
}