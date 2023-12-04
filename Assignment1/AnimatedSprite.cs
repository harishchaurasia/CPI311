using Microsoft.VisualBasic.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; //this is automatically added in, but library found within xna
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Windows.Forms;

namespace CPI311.GameEngine
{
    public class AnimatedSprite : Sprite
    {

        public static int Frames { get; set; }
        private static int Rows { get; set; }
        public static float Frame { get; set; }
        public static float Speed { get; set; }
        public List<float> spriteRow { get; set; }

        private int Height;
        public int currentY { get; set; }
        public AnimatedSprite(Texture2D texture, int frames = 8, int rows = 5) : base(texture)
        {
            Origin = new Vector2(texture.Width / frames / 2, texture.Height / rows / 2);
            spriteRow = new List<float>();
            Frames = frames;
            Frame = 0;
            Speed = frames;
            Rows = rows;
            currentY = 0;
        }
        public override void Update()
        {
            Frame += Speed * Time.ElapsedGameTime; //frame time
            Frame %= Frames;
            Source = new Rectangle((int)Frame * Texture.Width / Frames, currentY, Texture.Width / Frames, Texture.Height / Rows);
        }
    }
}