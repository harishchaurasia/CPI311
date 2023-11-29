﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;

using Microsoft.Xna.Framework.Audio;
using System.Reflection;
using System.Collections.Generic;
/*using System.Drawing;*/

namespace Lab8
{
    public class Lab8 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        SoundEffect gunSound;
        SoundEffectInstance soundInstance;
        Model model;
        Camera camera, topDownCamera;
        List<Transform> transforms;
        List<Collider> colliders;
        List<Camera> cameras;
        Effect effect;
        Texture2D texture;


        public Lab8()
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
            ScreenManager.Initialize(_graphics);


            cameras = new List<Camera>();
            colliders = new List<Collider>();   
            transforms = new List<Transform>(); 

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            gunSound = Content.Load<SoundEffect>("Gun");
            model = Content.Load<Model>("Sphere");
            texture = Content.Load<Texture2D>("Square");
            effect = Content.Load<Effect>("SimpleShading");

            Transform transform = new Transform();
            SphereCollider collider = new SphereCollider();
            collider.Radius = 1.2f;
            collider.Transform = transform;
            transforms.Add(transform);
            colliders.Add(collider);

            // *** Lab 8 Item ***********************
            ScreenManager.Setup(false, 1920, 1080);
            //***************************************



            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.Backward * 5;
            camera.Position = new Vector2(0f, 0f);
            camera.Size = new Vector2(0.5f, 1f);
            camera.AspectRatio = camera.Viewport.AspectRatio;

            topDownCamera = new Camera();
            topDownCamera.Transform = new Transform();
            topDownCamera.Transform.LocalPosition = Vector3.Up * 10;
            topDownCamera.Transform.Rotate(Vector3.Right, -MathHelper.PiOver2);
            topDownCamera.Position = new Vector2(0.5f, 0f);
            topDownCamera.Size = new Vector2(0.5f, 1f);
            topDownCamera.AspectRatio = topDownCamera.Viewport.AspectRatio;

            cameras.Add(topDownCamera);
            cameras.Add(camera);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            Ray ray = camera.ScreenPointToWorldRay(InputManager.GetMousePosition());

            foreach (Collider collider in colliders)
            {
                if (collider.Intersects(ray) != null)
                {
                    effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
                    //cube.Meshes[0].Effects[0] as BasicEffect).DiffuseColor = Color.Blue.ToVector3();

                    soundInstance = gunSound.CreateInstance();
                    soundInstance.Play();
                }
                else
                {
                    effect.Parameters["DiffuseColor"].SetValue(Color.Blue.ToVector3());
                    //(cube.Meshes[0].Effects[0] as BasicEffect).DiffuseColor =Color.Red.ToVector3();

                    base.Update(gameTime);
                }
            }

            Time.Update(gameTime);
            InputManager.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            foreach (Camera camera in cameras)
            {
                GraphicsDevice.DepthStencilState = new DepthStencilState();
                GraphicsDevice.Viewport = camera.Viewport;

                Matrix view = camera.View;
                Matrix projection = camera.Projection;

                effect.CurrentTechnique = effect.Techniques[1];
                effect.Parameters["View"].SetValue(view);
                effect.Parameters["Projection"].SetValue(projection);
                effect.Parameters["LightPosition"].SetValue(Vector3.Backward * 10 + Vector3.Right * 5);
                effect.Parameters["Shininess"].SetValue(20f);
                effect.Parameters["AmbientColor"].SetValue(new Vector3(0.2f, 0.2f, 0.2f));
                effect.Parameters["SpecularColor"].SetValue(new Vector3(1, 1, 1));
                effect.Parameters["DiffuseTexture"].SetValue(texture);

                foreach (Transform transform in transforms)
                {
                    effect.Parameters["World"].SetValue(transform.World);
                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                        pass.Apply();
                    foreach (ModelMesh mesh in model.Meshes)
                        foreach (ModelMeshPart part in mesh.MeshParts)
                        {
                            GraphicsDevice.SetVertexBuffer(part.VertexBuffer);
                            GraphicsDevice.Indices = part.IndexBuffer;
                            GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.VertexOffset, 0, part.PrimitiveCount);
                        }
                }
            }

            base.Draw(gameTime);
        }
    }
}