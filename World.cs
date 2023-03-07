using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class World
    {
        private List<SceneObject> _sceneObjectsArray;

        public World(ContentManager Content,float blockSeparation, float worldWidth, float worldLenght, string modelFileName, string textureFileName, string effectFileName)   //simple creator (develop it later)
        {
            _sceneObjectsArray = new List<SceneObject>();
            float _blockSeparation = blockSeparation;
            int _worldWidth = (int)worldWidth / 2;
            int _minusWidth = -_worldWidth;

            int _worldLenght = (int)worldLenght / 2;
            int _minusLenght = -_worldLenght;
            float level = -4.0f;

            for (int i = _minusLenght; i <= _worldLenght; i++)
            {
                for (int j = _minusLenght; j <= _worldWidth; j++)
                {
                    _sceneObjectsArray.Add(new SceneObject(Content, new Vector3(j * blockSeparation, level, i * blockSeparation), modelFileName, textureFileName, effectFileName));
                }
            }
        }

        public void ObjectInitializer(ContentManager Content)
        {
            //_sceneObjectsArray.Add(new SceneObject(Content,new Vector3(0, 0, 2), "test", "StarSparrow_Green", "ShaderOne"));
            //_sceneObjectsArray.Add(new SceneObject(Content,new Vector3(2.15f, 0, 0), "maja", "StarSparrow_Green", "ShaderOne"));
        }


        public Matrix WorldMove(Matrix worldMatrix,Vector3 moveVec)
        {
            return worldMatrix * Matrix.CreateTranslation(moveVec);
        }

        public List<SceneObject> GetSceneObjectList()
        {
            return _sceneObjectsArray;
        }
        
    }
}
