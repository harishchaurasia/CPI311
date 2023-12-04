using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; //this is automatically added in, but library found within xna
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Assignment 1
namespace CPI311.GameEngine
{
    public class ProgressBar : Sprite
    {

        public Color FillColor { get; set; }
        public float Value { get; set; }
        public float Speed { get; set; }

        public ProgressBar(Texture2D texture, Color color, float value, float speed) : base(texture)
        {
            FillColor = color;
            Value = value;
            Speed = speed;
        }
        public void Update()
        {
            Value = Math.Clamp(Value, 0f, 1f);
        }

        //spriteBatch.Draw(Texture, Position, new Rectangle(,?,?,?), FillColor, Rotation, Origin, Scale, Effects, Layer);
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch); // let the sprite do its work
            spriteBatch.Draw(Texture, Position, new Rectangle(0, 0, (int)(32 * Value), 32), FillColor, Rotation, Origin, Scale, Effects, Layer);
        }
    }
}
