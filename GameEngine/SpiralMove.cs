using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;

namespace CPI311.GameEngine
{
    public class SpiralMove
    {
        public Sprite Sprite { get; set; }
        public Vector2 Position { get; set; }
        public float Radius { get; set; }
        public float Phase { get; set; }
        public SpiralMove(Texture2D texture, Vector2 position, float radius = 150) 
        {
            Sprite = new Sprite(texture);
            Position = position;
            Radius = radius;
            Sprite.Position = Position + new Vector2(Radius, 0);
        }

        //Methods

        public void Draw(SpriteBatch spriteBatch) 
        {
            Sprite.Draw(spriteBatch);
        }

        public void Update() 
        {
            Position = new Vector2(300, 300);
            Phase += Time.ElapsedGameTime;


            if (InputManager.IsKeyDown(Keys.Up)) Radius += 1;
            if (InputManager.IsKeyDown(Keys.Down)) Radius -= 1;


            Sprite.Position = Position + new Vector2((float)((Radius + 20 * Math.Cos(10 * Phase)) * Math.Cos(Phase)), (float)((Radius + 20 * Math.Cos(10 * Phase)) * Math.Sin(Phase)));
        }
    }
}
