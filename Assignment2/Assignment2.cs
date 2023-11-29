using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using System.Reflection;
using System.Collections.Generic;
using System;


namespace Assignment2
{
    public class Assignment2 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        bool cameraMode = true;
        float speed = 1.0f;
        float zoom = 1.0f;

        float mercuryOrbitRadius = 20.0f;
        float mercuryOrbitSpeed = 2.0f;
        float mercuryOrbitAngle = 1.5708f;


        Model sun;
        //Model sun02;
        Model earth;
        Model luna;
        Model mercury;
        Model ground;
        Model person;

        Transform sunTransform;
        //Transform sun2;

        float sunRotationSpeed = 0.01f;
        Transform earthTransform;
        Transform lunaTransform;
        Transform mercuryTransform;
        Transform groundTransform;
        Transform personTransform;

        SpriteFont font;
        Effect effect;

        //Matrix Sun = Matrix.CreateTranslation(new Vector3(1,0,0));



        //Camera for player
        Camera camera_player;
        Transform playerTransform;

        //Camera for the world
        Camera camera_world;
        Transform worldTransform;



        public Assignment2()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        protected override void Initialize()
        {
            //camera = new Camera(GraphicsDevice.Viewport); // TODO: Add your initialization logic here
            Time.Initialize();
            InputManager.Initialize();






            /*          sun2 = new Transform();
                        sun2.LocalPosition = sunTransform.LocalPosition;
                        sun2.LocalPosition = new Vector3(20, 0, 20);
                        sun2.Scale = sunTransform.Scale;
                        
                        sun2.LocalRotation = sunTransform.LocalRotation;    
            */







            //ground
            //groundTransform = new Transform();
            //groundTransform.Scale = new Vector3(1, 1, 1);
            //groundTransform.Rotate(Vector3.Right, 1.0f);
            //groundTransform.LocalPosition = new Vector3(0, 0, 10);

            //person
            //personTransform = new Transform();
            //personTransform.Scale = new Vector3(1, 1, 1);
            //personTransform.LocalPosition = new Vector3(5, 5, 5);






            playerTransform = new Transform();
            playerTransform.LocalPosition = new Vector3(1, 1, 1);
            //playerTransform.Rotate(Vector3.Left, 1f);
            camera_player = new Camera();
            camera_player.Transform = playerTransform;



            worldTransform = new Transform();
            worldTransform.LocalPosition = new Vector3(0, 0, 0);
            //worldTransform.Rotate(Vector3.UnitX, -1.55f);
            camera_world = new Camera();
            camera_world.Transform = worldTransform;



            //worldTransform.Rotate(Vector3.Left, 1f);




            base.Initialize();
        }

        protected override void LoadContent()
        {

            effect = Content.Load<Effect>("SimpleShading");

            _spriteBatch = new SpriteBatch(GraphicsDevice);


            sun = Content.Load<Model>("SunModel");
            sunTransform = new Transform();
            sunTransform.LocalScale = new Vector3(1, 1, 1);
            sunTransform.LocalPosition = new Vector3(0, 0, 0);
            //sunTransform.Rotate(Vector3.Up, sunRotationSpeed * Time.ElapsedGameTime);


            //sun02 = Content.Load<Model>("SunModel");
            //sunTransform = new Transform();

            earth = Content.Load<Model>("Earth");
            //earthTransform = new Transform();
            earthTransform = new Transform();
            //earthTransform.Parent = sunTransform;
            earthTransform.Scale = new Vector3(1, 1, 1);
            earthTransform.LocalPosition = new Vector3(-10, 0, 0);

            luna = Content.Load<Model>("Luna");
            //lunaTransform = new Transform();
            lunaTransform = new Transform();
            lunaTransform.Parent = earthTransform;
            lunaTransform.Scale = new Vector3(1, 1, 1);
            lunaTransform.LocalPosition = new Vector3(10, 0, 0);

            mercury = Content.Load<Model>("Merc");
            mercuryTransform = new Transform();
            //mercuryTransform.Parent = sunTransform;
            mercuryTransform.Scale = new Vector3(1, 1, 1);
            //mercuryTransform.LocalPosition = new Vector3(-50, 50, 50);
            mercuryTransform.LocalPosition = new Vector3(-50, 0, 0);

            ground = Content.Load<Model>("Ground");
            //groundTransform = new Transform();

            person = Content.Load<Model>("PLayer");
            //personTransform = new Transform();

            font = Content.Load<SpriteFont>("Font");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();



            InputManager.Update();
            Time.Update(gameTime);



            sunTransform.Rotate(Vector3.Down, Time.ElapsedGameTime);
            sunTransform.UpdateWorld();
            //sun2.Rotate(Vector3.Down,Time.ElapsedGameTime * speed/2);
            earthTransform.Rotate(Vector3.Up, Time.ElapsedGameTime * speed);
            earthTransform.UpdateWorld();
            //lunaTransform.Rotate(Vector3.Up,Time.ElapsedGameTime * speed);
            //lunaTransform.UpdateWorld();
            //mercuryTransform.Rotate(Vector3.Up, Time.ElapsedGameTime);
            //mercuryTransform.UpdateWorld();


            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                sunTransform.LocalPosition += new Vector3(1f, 0, 0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                sunTransform.LocalPosition -= new Vector3(1f, 0, 0);
            }

            /* if (Keyboard.GetState().IsKeyDown(Keys.A))
                 
             if (Keyboard.GetState().IsKeyDown(Keys.D))*/












            //sunTransform.Rotate(Vector3.Up, sunRotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            /*mercuryTransform.LocalPosition = new Vector3(-70, 0, 0);
            mercuryOrbitAngle += mercuryOrbitSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector3 mercuryrelPosition = new Vector3( (float)Math.Sin(mercuryOrbitAngle) * mercuryOrbitRadius, -5f, (float)Math.Cos(mercuryOrbitAngle) * mercuryOrbitRadius);
            mercuryTransform.LocalPosition = mercuryrelPosition;*/






            /*            float playerMoveSpeed = 5.0f;
                        if (Keyboard.GetState().IsKeyDown(Keys.W))
                            playerTransform.LocalPosition += playerTransform.Forward * playerMoveSpeed * Time.ElapsedGameTime;
                        if (Keyboard.GetState().IsKeyDown(Keys.S))
                            playerTransform.LocalPosition -= playerTransform.Forward * playerMoveSpeed * Time.ElapsedGameTime;
                        if (Keyboard.GetState().IsKeyDown(Keys.A))
                            playerTransform.LocalPosition -= playerTransform.Right * playerMoveSpeed * Time.ElapsedGameTime;
                        if (Keyboard.GetState().IsKeyDown(Keys.D))
                            playerTransform.LocalPosition += playerTransform.Right * playerMoveSpeed * Time.ElapsedGameTime;*/


            float playerRotationSpeed = 0.05f;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                playerTransform.Rotate(Vector3.Up, -playerRotationSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                playerTransform.Rotate(Vector3.Up, playerRotationSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                playerTransform.Rotate(Vector3.Right, -playerRotationSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                playerTransform.Rotate(Vector3.Right, playerRotationSpeed);


            if (Keyboard.GetState().IsKeyDown(Keys.PageDown))
                camera_world.FieldOfView += 0.01f;
            if (Keyboard.GetState().IsKeyDown(Keys.PageUp))
                camera_world.FieldOfView -= 0.01f;






            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.Tab))
            {
                cameraMode = !cameraMode;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.U))
            {
                //worldTransform.Rotate(Vector3.Right, -0.01f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.O))
            {

            }
            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {

            }
            if (Keyboard.GetState().IsKeyDown(Keys.I))
            {

            }
            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {

            }
            if (Keyboard.GetState().IsKeyDown(Keys.K))
            {

            }
            if (Keyboard.GetState().IsKeyDown(Keys.L))
            {

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = new DepthStencilState();


            foreach (ModelMesh mesh in sun.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }
            foreach (ModelMesh mesh in mercury.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }
            foreach (ModelMesh mesh in earth.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }
            foreach (ModelMesh mesh in luna.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }


          (sun.Meshes[0].Effects[0] as BasicEffect).DiffuseColor = Color.Gold.ToVector3();
            sun.Draw(sunTransform.World, camera_world.View, camera_world.Projection);
            //(sun02.Meshes[0].Effects[0] as BasicEffect).DiffuseColor = Color.Gold.ToVector3();
            //sun02.Draw(sunTransform.World, camera_world.View, camera_world.Projection);

            (mercury.Meshes[0].Effects[0] as BasicEffect).DiffuseColor = Color.AliceBlue.ToVector3();
            mercury.Draw(mercuryTransform.World, camera_world.View, camera_world.Projection);

            (earth.Meshes[0].Effects[0] as BasicEffect).DiffuseColor = Color.Green.ToVector3();
            earth.Draw(earthTransform.World, camera_world.View, camera_world.Projection);

            (luna.Meshes[0].Effects[0] as BasicEffect).DiffuseColor = Color.Red.ToVector3();
            luna.Draw(lunaTransform.World, camera_world.View, camera_world.Projection);



            //(ground.Meshes[0].Effects[0] as BasicEffect).DiffuseColor = Color.Black.ToVector3();
            //ground.Draw(groundTransform.World, camera_world.View, camera_world.Projection);

            //(sun.Meshes[0].Effects[0] as BasicEffect).DiffuseColor = Color.Gold.ToVector3();
            //player.Draw(sunTransform.World, camera_world.View, camera_world.Projection);



            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "Move Player using WASD\n" +
              "Change Player View: Arrow Keys\n" +
              "Change Camera: Tab\n" +
              "Zoom Page: Up/Down\n" +
              "Change Speed: Home/End \n" +
              "Change Sol Rotation Axis: U\n" +
              "Change Mercury Rotation Axis: I\n" +
              "Change Earth Rotation Axis: O\n" +
              "Change Luna Rotation Axis: P\n" +
              "Change Sol Revolution Tilt: H\n" +
              "Change Mercury Revolution Tilt: I\n" +
              "Change Earth Revolution Tilt: J\n" +
              "Change Luna Revolution Tilt: L\n" +
              "NOTE: I and H will have no effect", new Vector2(10, 10), Color.Black);


            _spriteBatch.End();



            base.Draw(gameTime);
        }

    }
}