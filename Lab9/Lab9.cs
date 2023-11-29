using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
//sing System.Windows.Forms;
using CPI311.GameEngine;
using System.Reflection;

namespace Lab9
{
    public class Lab9 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Model cube;
        Model sphere;
        AStarSearch search;
        List<Vector3> path;
        int size = 10;
        Random random = new Random();


        

        Camera camera;
        Transform cameraTransform;


        public Lab9()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        protected override void Initialize()
        {

            Time.Initialize();  
            InputManager.Initialize();
            ScreenManager.Initialize(_graphics);



            //Lab9 Initialize****************

            search = new AStarSearch(size, size); // size of grid 

            foreach (AStarNode node in search.Nodes)
                if (random.NextDouble() < 0.2)
                    search.Nodes[random.Next(size), random.Next(size)].Passable = false;

            search.Start = search.Nodes[0, 0];
            search.Start.Passable = true;
            search.End = search.Nodes[size - 1, size - 1];
            search.End.Passable = true;

            //Main Search Process

            search.Search(); // A search is made here.

            path = new List<Vector3>();
            AStarNode current = search.End;
            while (current != null)
            {
                path.Insert(0, current.Position);
                current = current.Parent;
            }


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            sphere = Content.Load<Model>("Sphere");
            cube = Content.Load<Model>("cube");

            camera = new Camera();
            cameraTransform = new Transform();
            camera.Transform = cameraTransform;
            camera.Transform.LocalPosition = Vector3.Backward * 5 + Vector3.Right * 5 + Vector3.Up * 8 ;// + Vector3.Up * 50;
            camera.Transform.Rotate(Vector3.Right, -MathHelper.PiOver2); // Looks Down


        }

        protected override void Update(GameTime gameTime)
        {


            Time.Update(gameTime);
            InputManager.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if(InputManager.IsKeyPressed(Keys.Space))
            {


                AStarNode randSnode;
                AStarNode randEnode;
                do
                {
                    int rX = random.Next(size);
                    int rY = random.Next(size);

                    randSnode = search.Nodes[rX, rY];
                } while (!randSnode.Passable);

                search.Start = randSnode;

                do
                {
                    int rX = random.Next(size);
                    int rY = random.Next(size);

                    randEnode = search.Nodes[rX, rY];
                } while (!randEnode.Passable);

                search.End = randEnode;

                search.Start = randSnode; // assign a random start node (passable)
                search.End = randEnode; // assign a random end node (passable)

                search.Search();
                path.Clear();

                AStarNode current = search.End;
                while(current != null)
                {
                    path.Insert(0, current.Position); 
                    current = current.Parent;
                }

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (AStarNode node in search.Nodes)
                if (!node.Passable)
                    cube.Draw(Matrix.CreateScale(0.5f, 0.05f, 0.5f) * Matrix.CreateTranslation(node.Position), camera.View, camera.Projection);

            foreach (Vector3 position in path)
                sphere.Draw(Matrix.CreateScale(0.1f, 0.1f, 0.1f) * Matrix.CreateTranslation(position), camera.View, camera.Projection);


            base.Draw(gameTime);
        }
    }
}