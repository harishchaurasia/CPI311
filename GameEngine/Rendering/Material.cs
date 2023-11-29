using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace CPI311.GameEngine
{
    public class Material
    {
        public Effect effect;
        public Texture2D DiffuseTexture;
        Texture2D Texture;
        //public float Shininess;
        //public Effect effect;
        public int Passes { get { return effect.CurrentTechnique.Passes.Count; } }
        public int CurrentTechnique { get; set; }

        public Vector3 Diffuse { get; set; }
        public Vector3 Ambient { get; set; }
        public Vector3 Specular { get; set; }   
        public float Shininess { get; set; }
        public Matrix World {  get ; set; } 
        public Camera Camera { get; set; }


        public Material(Matrix world, Camera camera, ContentManager content, string filename, int currentTechnique, float shininess, Texture2D texture)
        {
            effect = content.Load<Effect>(filename);
            World = world; 
            Camera = camera;
            Shininess = shininess;
            Texture = texture;
            Diffuse = Color.Gray.ToVector3();
            Ambient = Color.Gray.ToVector3();
            Specular = Color.Gray.ToVector3();
        }
        public virtual void Apply(int currentPass)
        {
            //effect.CurrentTechnique.Passes[currentPass].Apply();




            //effect.CurrentTechnique = effect.Techniques[CurrentTechnique]; //"0" is the first technique

            effect.Parameters["World"].SetValue(World);
            effect.Parameters["View"].SetValue(Camera.View);
            effect.Parameters["Projection"].SetValue(Camera.Projection);
            effect.Parameters["LightPosition"].SetValue(Vector3.Backward * 10 +
            Vector3.Right * 5);
            effect.Parameters["CameraPosition"].SetValue(Camera.Transform.Position);
            effect.Parameters["Shininess"].SetValue(Shininess);
            effect.Parameters["AmbientColor"].SetValue(Ambient);
            effect.Parameters["DiffuseColor"].SetValue(Diffuse);
            effect.Parameters["SpecularColor"].SetValue(Specular);
            effect.Parameters["DiffuseTexture"].SetValue(Texture);
        }
    }
}
