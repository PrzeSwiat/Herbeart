using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class SceneObject
    {
        private Vector3 position;
        private Vector3 rotation = new Vector3(0.0f,0.0f,0.0f);
        private float scale = 1;
        private Model model;
        private Texture2D texture2D;
        private Effect effect;
        private string _modelFileName;
        private string _textureFileName;
        private string _effectFileName;

        public SceneObject(ContentManager content,Vector3 worldPosition, string modelFileName , string textureFileName, string effectFileName)
        {
            position = worldPosition;
            _modelFileName = modelFileName;
            _textureFileName = textureFileName;
            _effectFileName = effectFileName;

            SetModel(content.Load<Model>(modelFileName));
            SetTexture(content.Load<Texture2D>(textureFileName));
            SetEffect(content.Load<Effect>(effectFileName));
        }


        //GET'ERS
        public Vector3 GetPosition()
        {
            return position;
        }
        public Vector3 GetRotation()
        {
            return rotation;
        }
        public float GetScale()
        {
            return scale;
        }
        public Model GetModel()
        {
            return model;
        }
        public Texture2D GetTexture2D()
        {
            return texture2D;
        }
        public string GetModelFileName()
        {
            return _modelFileName;
        }
        public string GetTextureFileName()
        {
            return _textureFileName;
        }
        //.................

        //SET'ERS
        public void SetPosition(float x, float y, float z)
        {
            position = new Vector3(x,y,z);
        }
        public void SetPosition(Vector3 pos)
        {
            position = pos;
        }
        public void SetRotation(Vector3 rot)
        {
            rotation = rot;
        }
        public void SetRotation(float x, float y, float z)
        {
            rotation = new Vector3(x, y, z);
        }
        public void SetScale(float scl)
        {
            scale = scl;
        }
        public void SetModel(Model mod)
        {
            model = mod;
        }
        public void SetTexture(Texture2D tex)
        {
            texture2D = tex;
        }
        public void SetEffect(Effect eff)
        {
            effect = eff;
        }
        //.................

        public void DrawModelWithEffect(Model model, Matrix world, Matrix view, Matrix projection, Texture2D texture2D)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * world));
                    effect.Parameters["WorldInverseTransposeMatrix"].SetValue(worldInverseTransposeMatrix);
                    //effect.Parameters["AmbienceColor"].SetValue(Color.Gray.ToVector4());      //uncomment if ambient needed
                    effect.Parameters["DiffuseColor"].SetValue(Color.White.ToVector4());
                    effect.Parameters["DiffuseLightPower"].SetValue(2);              // diffuse light power 
                    effect.Parameters["DiffuseLightDirection"].SetValue(new Vector3(2.0f, 4.0f, 1.0f));
                    effect.Parameters["ModelTexture"].SetValue(texture2D);
                    effect.Parameters["WorldMatrix"].SetValue(world * mesh.ParentBone.Transform);
                    effect.Parameters["ViewMatrix"].SetValue(view);
                    effect.Parameters["ProjectionMatrix"].SetValue(projection);

                }

                mesh.Draw();
            }
        }
    }
}
