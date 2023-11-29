using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using CPI311.GameEngine;
using SharpDX.Direct3D9;

namespace Lab3
{
    public class Lab3 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        SpriteFont font;

        

        //step1
        Model model;
        Matrix world, view, projection;

        //step2
        Vector3 cameraPos = new Vector3(0, 0, 5);
        Vector2 cameraCen = new Vector2(0, 0);
        Vector3 modelPos = new Vector3(0, 0, 0);
        float modelScale = 1.0f;
        float yaw = 0, pitch = 0, roll = 0;
        bool wtoggle = true;
        bool ptoggle = true;

        float left = -0.5f;
        float right = 0.5f;
        float bottom = -0.5f;
        float top = 0.5f;

        float cenSpeed = 0.1f;
        float sizSpeed = 0.1f;


        public Lab3()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // **CPI311 Manager Init **
            InputManager.Initialize();
            Time.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Font");

            model = Content.Load<Model>("Torus");
            foreach(ModelMesh MESH in model.Meshes) 
            {
                foreach (BasicEffect effect in MESH.Effects)
                {

                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true ;
                
                }
            }

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            //cpi311 manager update
            InputManager.Update();
            Time.Update(gameTime);
            //**************************



            //camera controls

            if (InputManager.IsKeyDown(Keys.S))
            {
                cameraPos += Vector3.Up * Time.ElapsedGameTime * 5;
            }
            if (InputManager.IsKeyDown(Keys.W))
            {
                cameraPos += Vector3.Down * Time.ElapsedGameTime * 5;
            }
            if (InputManager.IsKeyDown(Keys.A))
            {
                cameraPos += Vector3.Right * Time.ElapsedGameTime * 5;
            }
            if (InputManager.IsKeyDown(Keys.D))
            {
                cameraPos += Vector3.Left * Time.ElapsedGameTime * 5;
            }

            //model change controls
            if (InputManager.IsKeyDown(Keys.Up))
            {
                modelPos += Vector3.Up * Time.ElapsedGameTime * 5;

            }
            if (InputManager.IsKeyDown(Keys.Down))
            {
                modelPos += Vector3.Down * Time.ElapsedGameTime * 5;
            }
            if (InputManager.IsKeyDown(Keys.Right))
            {
                modelPos += Vector3.Right * Time.ElapsedGameTime * 5;
            }
            if (InputManager.IsKeyDown(Keys.Left))
            {
                modelPos += Vector3.Left * Time.ElapsedGameTime * 5;
            }

            // yaw, pitch and roll
            if (InputManager.IsKeyDown(Keys.Insert))
            {
                yaw += Time.ElapsedGameTime * 5;
            }

            if (InputManager.IsKeyDown(Keys.Delete))
            {
                yaw -= Time.ElapsedGameTime * 5;
            }

            if (InputManager.IsKeyDown(Keys.Home))
            {
                pitch += Time.ElapsedGameTime * 5;
            }
            if (InputManager.IsKeyDown(Keys.End))
            {
                pitch += Time.ElapsedGameTime * 5;
            }

            if (InputManager.IsKeyDown(Keys.PageUp))
            {
                roll += Time.ElapsedGameTime * 5;
            }
            if (InputManager.IsKeyDown(Keys.PageDown))
            {
                roll -= Time.ElapsedGameTime * 5;
            }

            //increase-decrease scale of the model
            if ((InputManager.IsKeyDown(Keys.LeftShift)) && (InputManager.IsKeyDown(Keys.Up)))
            {
                modelScale += Time.ElapsedGameTime * 5.0f;
            }
            if ((InputManager.IsKeyDown(Keys.LeftShift)) && (InputManager.IsKeyDown(Keys.Down)))
            {
                modelScale -= Time.ElapsedGameTime * 5.0f;
            }


            //Space Toggle
            if(InputManager.IsKeyPressed(Keys.Space))
            {
                {
                    wtoggle = !wtoggle;
                }
            }

            if (wtoggle == true)
            {
                world = Matrix.CreateScale(modelScale) * Matrix.CreateFromYawPitchRoll(yaw, pitch, roll) * Matrix.CreateTranslation(modelPos);
            }
            else if (wtoggle == false)
            {
                world = Matrix.CreateTranslation(modelPos) * Matrix.CreateFromYawPitchRoll(yaw, pitch, roll) * Matrix.CreateScale(modelScale);
            }







            view = Matrix.CreateLookAt(cameraPos, cameraPos + Vector3.Forward, Vector3.Up);
            
            
            /* projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver2,                           //90 degree angle
                1.33f,                                        //width/height proportion
                0.1f,                                          //nearest
                1000f);                                         //farthest */

            // perspective toggle
            if (InputManager.IsKeyPressed(Keys.Tab))
            {
                {
                    ptoggle = !ptoggle;
                }
            }

            if (ptoggle == true)
            {
                projection = Matrix.CreatePerspectiveOffCenter(left, right, bottom, top, 0.1f, 1000f);                                      //farthest

            }
            else if (ptoggle == false)
            {

                projection = Matrix.CreateOrthographicOffCenter(left, right, bottom, top, 0.1f, 1000f);
                          /*  1,                                               
                           -1,                                        
                            0.1f,                                         
                            1000f);  */                                     

            }

            //move center of camera

            if (InputManager.IsKeyDown(Keys.LeftShift))
            {
                if (InputManager.IsKeyDown(Keys.W))
                {
                    top += cenSpeed;
                    bottom += sizSpeed;
                }
                if (InputManager.IsKeyDown(Keys.S))
                {
                    top -= cenSpeed;
                    bottom -= sizSpeed;
                }
                if (InputManager.IsKeyDown(Keys.A))
                {
                    right -= cenSpeed;
                    left -= sizSpeed;
                }
                if (InputManager.IsKeyDown(Keys.D))
                {
                    right += cenSpeed;
                    left += sizSpeed;
                }
            }

            //camera width/height update
            if (InputManager.IsKeyDown(Keys.LeftControl))
            {
                if (InputManager.IsKeyDown(Keys.W))
                {
                    top += cenSpeed;
                    bottom -= sizSpeed;
                }
                if (InputManager.IsKeyDown(Keys.S))
                {
                    top -= cenSpeed;
                    bottom += sizSpeed;
                }
                if (InputManager.IsKeyDown(Keys.A))
                {
                    right += cenSpeed;
                    left -= sizSpeed;
                }
                if (InputManager.IsKeyDown(Keys.D))
                {
                    right -= cenSpeed;
                    left += sizSpeed;
                }
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //fixing the depth
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = new DepthStencilState();


            model.Draw(world, view, projection);   // TODO: Add your drawing code here


            _spriteBatch.Begin();

            _spriteBatch.DrawString(font, "WSAD Buttons for Camera Controls", new Vector2(50, 50), Color.Red);
            _spriteBatch.DrawString(font, "CameraX: " + cameraPos.X.ToString("0.00"), new Vector2(50,80), Color.Red);
            _spriteBatch.DrawString(font, "CameraY: " + cameraPos.Y.ToString("0.00"), new Vector2(50, 110), Color.Red);
            _spriteBatch.DrawString(font, "CameraZ: " + cameraPos.Z.ToString("0.00"), new Vector2(50, 140), Color.Red);
            _spriteBatch.DrawString(font, "ModelX: " + modelPos.X.ToString("0.00"), new Vector2(50, 170), Color.Red);
            _spriteBatch.DrawString(font, "ModelY: " + modelPos.Y.ToString("0.00"), new Vector2(50, 200), Color.Red);
            _spriteBatch.DrawString(font, "ModelZ: " + modelPos.Z.ToString("0.00"), new Vector2(50, 230), Color.Red);
            _spriteBatch.DrawString(font, "World Toggle: " + wtoggle, new Vector2(50, 260), Color.Red);
            _spriteBatch.DrawString(font, "Perspective toggle: " + ptoggle, new Vector2(50, 290), Color.Red);

            _spriteBatch.End(); 

            base.Draw(gameTime);
        }
    }
}