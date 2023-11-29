﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using CPI311.GameEngine;

namespace Lab05
{
    public class Lab05 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Effect effect;
        Model model;
        Texture2D texture;

        Transform modelTransform;
        Transform cameraTransform;
        Camera camera;
        int shaderNum;

        private SpriteFont font;

        public Lab05()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.GraphicsProfile = GraphicsProfile.HiDef;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            InputManager.Initialize();
            Time.Initialize(); 
            base.Initialize();
            shaderNum = 0;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Font");

            // TODO: use this.Content to load your game content here

            effect = Content.Load<Effect>("SimpleShading");
            model = Content.Load<Model>("Torus");
            texture = Content.Load<Texture2D>("Square");

            modelTransform = new Transform();
            cameraTransform = new Transform();
            cameraTransform.LocalPosition = Vector3.Backward * 5;
            camera = new Camera();
            camera.Transform = cameraTransform;

        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            Time.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (InputManager.IsKeyDown(Keys.W))
                cameraTransform.LocalPosition += cameraTransform.Forward * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.S))
                cameraTransform.LocalPosition += cameraTransform.Backward * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.A))
                cameraTransform.Rotate(Vector3.Up, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.D))
                cameraTransform.Rotate(Vector3.Down, Time.ElapsedGameTime);

            if (InputManager.IsKeyPressed(Keys.Tab))
            {
                shaderNum++;
                if (shaderNum > 3)
                {
                    shaderNum = 0; 
                }
            }

                base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            Matrix view = camera.View;
            Matrix projection = camera.Projection;

            effect.CurrentTechnique = effect.Techniques[shaderNum];
            effect.Parameters["World"].SetValue(modelTransform.World);
            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);
            effect.Parameters["LightPosition"].SetValue(Vector3.Backward * 10 + Vector3.Right * 5);
            effect.Parameters["CameraPosition"].SetValue(cameraTransform.Position);
            effect.Parameters["Shininess"].SetValue(20f);
            effect.Parameters["AmbientColor"].SetValue(new Vector3(0.2f, 0.2f, 0.2f));
            effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.5f, 0, 0));
            effect.Parameters["SpecularColor"].SetValue(new Vector3(1, 1, 1));
            effect.Parameters["DiffuseTexture"].SetValue(texture);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                foreach (ModelMesh mesh in model.Meshes)
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        GraphicsDevice.SetVertexBuffer(part.VertexBuffer);
                        GraphicsDevice.Indices = part.IndexBuffer;
                        GraphicsDevice.DrawIndexedPrimitives(
                            PrimitiveType.TriangleList,
                            part.VertexOffset,
                            0,
                            part.PrimitiveCount);
                    }
            }
             

            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "Shader Num: " + shaderNum, new Vector2(10, 20), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}