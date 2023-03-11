using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class World
    {
        private List<SceneObject> _sceneObjectsArray;
        int w;
        int h;
        
        public World(int WindowWidth,int WindowHeight,ContentManager Content,float blockSeparation, float worldWidth, float worldLenght, string modelFileName, string textureFileName, string effectFileName)   //simple creator (develop it later)
        {
            w = WindowWidth;
            h = WindowHeight;
            _sceneObjectsArray = new List<SceneObject>();
            float _blockSeparation = blockSeparation;
            int _worldWidth = (int)worldWidth / 2;
            int _minusWidth = -_worldWidth;

            int _worldLenght = (int)worldLenght / 2;
            int _minusLenght = -_worldLenght;
            float level = 0.0f;

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
            //_sceneObjectsArray.Add(new SceneObject(Content,new Vector3(0, 0, 0), "test", "StarSparrow_Orange", "ShaderOne"));
            //_sceneObjectsArray.Add(new SceneObject(Content,new Vector3(0, 1, 0), "test", "StarSparrow_Orange", "ShaderOne"));
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
