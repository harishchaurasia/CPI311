using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
//using SharpDX.Direct2D1;


namespace Assignment4
{
    public class Assignment4 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;

        // cam info
        Camera camera;
        Light light;
        Vector3 cameraPosition = new Vector3(0.0f, 0.0f, GameConstants.CameraHeight);
        Matrix projectionMatrix;
        Matrix viewMatrix;


        //Audio components
        SoundEffect audioEngine;
        bool soundEnginePlaying = false;
        SoundEffectInstance soundEngineInstance;
        SoundEffect soundHyperspaceActivation;
        SoundEffect soundExplosion2;
        SoundEffect soundExplosion3;
        SoundEffect soundWeaponsFire;


        //ship size stuff
        private float shipScale = 5f; // Default scale is 1.0 (original size)



        //Visual components
        Ship ship;
        Model asteroidM;
        Model bulletM;
        SpriteFont font;

        Matrix[] asteroidTransforms;
        Matrix[] bulletTransforms;

        /*        Asteroid[] asteroidList = new Asteroid[GameConstants.NumAsteroids];
                Bullet[] bulletList = new Bullet[GameConstants.NumBullets];*/

        List<Asteroid> asteroidList;
        List<Bullet> bulletList;
        int sceneNum;



        //Score & background
        int score;
        Random random = new Random();
        Texture2D stars;
        Vector2 scorePosition = new Vector2(100, 50);

        // Particles
        ParticleManager particleManager;
        Texture2D particleTex;
        Effect particleEffect;

        public Assignment4()
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


            score = 0;
            sceneNum = 1;

            camera = new Camera();
            light = new Light();
            ship = new Ship(Content, camera, GraphicsDevice, light);

            asteroidList = new List<Asteroid>();
            bulletList = new List<Bullet>();


            ResetAsteroids();


            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), _graphics.GraphicsDevice.DisplayMode.AspectRatio, GameConstants.CameraHeight - 1000.0f, GameConstants.CameraHeight + 1000.0f);
            viewMatrix = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ship = new Ship(Content, camera, GraphicsDevice, light);

            /*            for (int i = 0; i < GameConstants.NumBullets; i++)
                            bulletList[i] = new Bullet(Content, camera, GraphicsDevice, light);*/
            //ResetAsteroids(); // look at the below private method


            // TODO: use this.Content to load your game content here


            audioEngine = Content.Load<SoundEffect>("engine_2");
            soundHyperspaceActivation = Content.Load<SoundEffect>("hyperspace_activate");
            soundExplosion2 = Content.Load<SoundEffect>("explosion2");
            soundExplosion3 = Content.Load<SoundEffect>("explosion3");
            soundWeaponsFire = Content.Load<SoundEffect>("tx0_fire1");

            ship.Model = Content.Load<Model>("p1_wedge");
            ship.Transforms = SetupEffectDefaults(ship.Model);

            asteroidM = Content.Load<Model>("asteroid4");
            asteroidTransforms = SetupEffectDefaults(asteroidM);

            bulletM = Content.Load<Model>("bullet");
            bulletTransforms = SetupEffectDefaults(bulletM);

            stars = Content.Load<Texture2D>("B1_stars");
            font = Content.Load<SpriteFont>("Font");

            // *** Particle
            particleManager = new ParticleManager(GraphicsDevice, 100);
            particleEffect = Content.Load<Effect>("ParticleShader-complete");
            particleTex = Content.Load<Texture2D>("fire");
        }

        protected override void Update(GameTime gameTime)
        {
            if (sceneNum == 1)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    sceneNum = 0;
            }
            else if (sceneNum == 0)
            {
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;


                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();


                InputManager.Update();
                Time.Update(gameTime);

                // Get some input.
                UpdateInput();

                // Add velocity to the current position.
                ship.Position += ship.Velocity;

                // Bleed off velocity over time.
                ship.Velocity *= 0.95f;


                /*ship.Update();*/
                /*
                                // Add velocity to the current position.
                                ship.Position += ship.Velocity;

                                // Bleed off velocity over time.
                                ship.Velocity *= 0.95f;*/


                //update asteroids
                for (int i = 0; i < asteroidList.Count; i++)
                {
                    if (asteroidList[i].isActive)
                    {
                        asteroidList[i].Update(timeDelta);
                    }
                }

                //update bullets
                for (int i = 0; i < bulletList.Count; i++)
                {
                    if (bulletList[i].isActive)
                    {
                        bulletList[i].Update(timeDelta);
                    }
                }

                //ship-asteroid collision check
                if (ship.isActive)
                {
                    BoundingSphere shipSphere = new BoundingSphere(ship.Position, ship.Model.Meshes[0].BoundingSphere.Radius * GameConstants.ShipBoundingSphereScale);
                    for (int i = 0; i < asteroidList.Count; i++)
                    {
                        if (asteroidList[i].isActive)
                        {
                            BoundingSphere b = new BoundingSphere(asteroidList[i].position, asteroidM.Meshes[0].BoundingSphere.Radius * GameConstants.AsteroidBoundingSphereScale);
                            if (b.Intersects(shipSphere))
                            {
                                // Particles
                                Particle particle = particleManager.getNext();
                                particle.Position = asteroidList[i].position;
                                particle.Velocity = new Vector3(random.Next(-5, 5), 2, random.Next(-50, 50));
                                particle.Acceleration = new Vector3(0, 3, 0);
                                particle.MaxAge = random.Next(1, 6);
                                particle.Init();

                                soundExplosion3.Play();
                                ship.isActive = false;
                                asteroidList[i].isActive = false;
                                asteroidList.Remove(asteroidList[i]);
                                score -= GameConstants.DeathPenalty;
                                //exiting the loop
                                break; 
                            }
                        }
                    }
                }

                //collision check for the bullet and asteroid 
                for (int i = 0; i < asteroidList.Count; i++)
                {
                    if (asteroidList[i].isActive)
                    {
                        BoundingSphere asteroidSphere = new BoundingSphere(asteroidList[i].position, asteroidM.Meshes[0].BoundingSphere.Radius * GameConstants.AsteroidBoundingSphereScale);
                        for (int j = 0; j < bulletList.Count; j++)
                        {
                            if (bulletList[j].isActive)
                            {
                                BoundingSphere bulletSphere = new BoundingSphere(bulletList[j].position, bulletM.Meshes[0].BoundingSphere.Radius);
                                if (asteroidSphere.Intersects(bulletSphere))
                                {
                                    // Particles
                                    Particle particle = particleManager.getNext();
                                    particle.Position = asteroidList[i].position;
                                    particle.Velocity = new Vector3(random.Next(-5, 5), 2, random.Next(-50, 50));
                                    particle.Acceleration = new Vector3(0, 3, 0);
                                    particle.MaxAge = random.Next(1, 6);
                                    particle.Init();

                                    soundExplosion2.Play();
                                    asteroidList[i].isActive = false;
                                    bulletList[j].isActive = false;
                                    asteroidList.Remove(asteroidList[i]);
                                    score += GameConstants.KillBonus;
                                    break; 
                                }
                            }
                        }
                    }
                }

                particleManager.Update();

                if (asteroidList.Count < 1)
                {
                    sceneNum = 2;
                }

                base.Update(gameTime);
            }
            else
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();
            }

        }
        protected override void Draw(GameTime gameTime)
        {
            if (sceneNum == 1)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(stars, new Rectangle(0, 0, 800, 600), Color.White);
                spriteBatch.DrawString(font, "Welcome To StarShooter!", new Vector2(GraphicsDevice.Viewport.Width / 2 - 380, GraphicsDevice.Viewport.Height / 2 - 200), Color.AliceBlue);
                spriteBatch.DrawString(font, "To Start the Game, Press [SPACE]", new Vector2(GraphicsDevice.Viewport.Width / 2 - 380, GraphicsDevice.Viewport.Height / 2 - 170), Color.AliceBlue);
                spriteBatch.End();
            }
            else if (sceneNum == 0)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);

                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                spriteBatch.Begin();
                spriteBatch.Draw(stars, new Rectangle(0, 0, 800, 600), Color.White);
                spriteBatch.DrawString(font, "Score: " + score, new Vector2(30, 410), Color.AliceBlue);
                spriteBatch.DrawString(font, "Asteroids Left: " + asteroidList.Count, new Vector2(30, 430), Color.AliceBlue);
                spriteBatch.DrawString(font, "Press LMB to Shoot", new Vector2(600, 370), Color.AliceBlue);
                spriteBatch.DrawString(font, "Press W to Move", new Vector2(600, 390), Color.AliceBlue);
                spriteBatch.DrawString(font, "Press A and D to Rotate", new Vector2(600, 410), Color.AliceBlue);
                spriteBatch.DrawString(font, "Press ENTER to Respawn", new Vector2(600, 430), Color.AliceBlue);
                spriteBatch.End();
                GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;




                Matrix shipTransformMatrix = ship.RotationMatrix * Matrix.CreateTranslation(ship.Position);



                 //Drawing the model
                if (ship.isActive)
                {
                    DrawModel(ship.Model, shipTransformMatrix, ship.Transforms);
                }
                /*
                                if (ship.isActive)
                                {
                                    Matrix shipTransformMatrix = ship.RotationMatrix * Matrix.CreateTranslation(ship.Position);
                                    DrawModel(ship.Model, shipTransformMatrix, ship.Transforms, shipScale);
                                }
                */

                //drawing the asteroids
                for (int i = 0; i < asteroidList.Count; i++)
                {
                    Matrix asteroidTransform = Matrix.CreateTranslation(asteroidList[i].position);
                    if (asteroidList[i].isActive)
                    {
                        DrawModel(asteroidM, asteroidTransform, asteroidTransforms);
                    }
                }

                //drawing the bullets
                for (int i = 0; i < bulletList.Count; i++)
                {
                    Matrix bulletTransform = Matrix.CreateTranslation(bulletList[i].position);
                    if (bulletList[i].isActive)
                    {
                        DrawModel(bulletM, bulletTransform, bulletTransforms);
                    }
                }

                //for (int i = 0; i < GameConstants.NumAsteroids; i++) asteroidList[i].Draw();

                //particle draw
                GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
                particleEffect.CurrentTechnique = particleEffect.Techniques["particle"];
                particleEffect.CurrentTechnique.Passes[0].Apply();
                particleEffect.Parameters["ViewProj"].SetValue(viewMatrix * projectionMatrix);
                particleEffect.Parameters["World"].SetValue(Matrix.Identity);
                Vector3 scale, translation;
                Quaternion rotation;
                viewMatrix.Decompose(out scale, out rotation, out translation);
                particleEffect.Parameters["CamIRot"].SetValue(Matrix.Invert(Matrix.CreateFromQuaternion(rotation)));
                particleEffect.Parameters["Texture"].SetValue(particleTex);
                particleManager.Draw(GraphicsDevice);
                GraphicsDevice.RasterizerState = RasterizerState.CullNone;


                base.Draw(gameTime);
            }
            else
            {
                spriteBatch.Begin();
                spriteBatch.Draw(stars, new Rectangle(0, 0, 800, 600), Color.White);
                spriteBatch.DrawString(font, "Ahh Damn it, Game Over", new Vector2(GraphicsDevice.Viewport.Width / 2 - 380, GraphicsDevice.Viewport.Height / 2 - 200), Color.AliceBlue);
                spriteBatch.DrawString(font, "Your Score: " +  score, new Vector2(GraphicsDevice.Viewport.Width / 2 - 380, GraphicsDevice.Viewport.Height / 2 - 170), Color.AliceBlue);
                spriteBatch.End();
            }

        }

        /*        private void ResetAsteroids()
                {
                    float xStart;
                    float yStart;
                    for (int i = 0; i < GameConstants.NumAsteroids; i++)
                    {
                        if (random.Next(2) == 0)
                            xStart = (float)-GameConstants.PlayfieldSizeX;
                        else
                            xStart = (float)GameConstants.PlayfieldSizeX;
                        yStart = (float)random.NextDouble() * GameConstants.PlayfieldSizeY;
                        asteroidList[i] = new Asteroid(Content, camera, GraphicsDevice, light);
                        asteroidList[i].Transform.Position = new Vector3(0, 0.0f, 0);
                        double angle = random.NextDouble() * 2 * Math.PI;
                        asteroidList[i].Rigidbody.Velocity = new Vector3(
                           -(float)Math.Sin(angle), 0, (float)Math.Cos(angle)) *
                    (GameConstants.AsteroidMinSpeed + (float)random.NextDouble() *
                    GameConstants.AsteroidMaxSpeed);
                        asteroidList[i].isActive = true;
                    }
                }*/


        private void ResetAsteroids()
        {
            float xStart;
            float yStart;
            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {

                if (random.Next(1, 3) == 1)
                {
                    xStart = random.Next((int)-GameConstants.PlayfieldSizeX + 100, -100);
                }
                else
                {
                    xStart = random.Next(+100, (int)GameConstants.PlayfieldSizeX - 100);
                }
                if (random.Next(1, 3) == 1)
                {
                    yStart = random.Next((int)-GameConstants.PlayfieldSizeY + 100, -100);
                }
                else
                {
                    yStart = random.Next(+100, (int)GameConstants.PlayfieldSizeY - 100);
                }

                Asteroid asteroid = new Asteroid(Content, camera, GraphicsDevice, light);

                asteroid.position = new Vector3(xStart, yStart, 0.0f);
                double angle = random.NextDouble() * 2 * Math.PI;
                asteroid.direction.X = -(float)Math.Sin(angle);
                asteroid.direction.Y = (float)Math.Cos(angle);
                asteroid.speed = GameConstants.AsteroidMinSpeed + (float)random.NextDouble() * GameConstants.AsteroidMaxSpeed;
                /*
                asteroid.direction.X = 0;
                asteroid.direction.Y = 1;
                asteroid.speed = 2000;
                */
                asteroid.isActive = true;

                asteroidList.Add(asteroid);
            }
        }
        private Matrix[] SetupEffectDefaults(Model myModel)
        {
            Matrix[] absoluteTransforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(absoluteTransforms);

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Projection = projectionMatrix;
                    effect.View = viewMatrix;
                }
            }
            return absoluteTransforms;
        }
        public void DrawModel(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms)
        {
            //Drawing the model, we use loop for multiple meshes
            foreach (ModelMesh mesh in model.Meshes)
            {
                //This is where the mesh orientation is set
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * modelTransform;
                }
                //Drawing the mesh, will use the effects set above.
                mesh.Draw();
            }
        }


        /*        public void DrawModel(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms, float scale)
                {
                    // Apply scaling here
                    Matrix scaleMatrix = Matrix.CreateScale(scale);
                    modelTransform *= scaleMatrix;

                    //Drawing the model, loop for multiple meshes
                    foreach (ModelMesh mesh in model.Meshes)
                    {
                        //This is where the mesh orientation is set, including the new scale
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * modelTransform;
                        }
                        // Drawing the mesh, will use the effects set above.
                        mesh.Draw();
                    }
                }*/

        protected void UpdateInput()
        {
            ship.ShipUpdate();

            //Play engine sound only when the engine is on.
            if (InputManager.IsKeyDown(Keys.W) && !soundEnginePlaying)
            {
                if (soundEngineInstance == null)
                {
                    soundEngineInstance = audioEngine.CreateInstance();
                    soundEngineInstance.Play();
                }
                else
                {
                    soundEngineInstance.Resume();
                }
                soundEnginePlaying = true;
            }
            else if (!InputManager.IsKeyDown(Keys.W) && soundEnginePlaying)
            {
                soundEngineInstance.Pause();
                soundEnginePlaying = false;
            }

            // press Enter to warp back to the center.
            if (InputManager.IsKeyDown(Keys.Enter))
            {
                ship.isActive = true;
                ship.Position = Vector3.Zero;
                ship.Velocity = Vector3.Zero;
                ship.Rotation = 0.0f;
                soundHyperspaceActivation.Play();
            }

            //shoot
            if (InputManager.GetMouseCurrent().LeftButton == ButtonState.Pressed && InputManager.GetMousePrevious().LeftButton != ButtonState.Pressed)
            {
                if (ship.isActive)
                {
                    Bullet bullet = new Bullet(Content, camera, GraphicsDevice, light);
                    bullet.direction = ship.RotationMatrix.Forward;
                    bullet.speed = GameConstants.BulletSpeedAdjustment;
                    bullet.position = ship.Position + (200 * bullet.direction);
                    bullet.isActive = true;
                    soundWeaponsFire.Play();
                    bulletList.Add(bullet);
                    score -= GameConstants.ShotPenalty;
                }
            }
        }
    }
}