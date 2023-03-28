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
    [Serializable]
    internal class SceneObject
    {
        public Vector3 position;
        public Vector3 rotation = new Vector3(0.0f,0.0f,0.0f);
        public float scale = 1;
        protected Model model;
        protected Texture2D texture2D;
        public string _modelFileName;
        public string _textureFileName;

        public SceneObject(Vector3 worldPosition, string modelFileName , string textureFileName)
        {
            position = worldPosition;
            _modelFileName = modelFileName;
            _textureFileName = textureFileName;

        }

        public void LoadContent(ContentManager content)
        {
            model = (content.Load<Model>(_modelFileName));
            texture2D = (content.Load<Texture2D>(_textureFileName));
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
        //.................

    }
}
