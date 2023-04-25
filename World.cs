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
        private List<SceneObject> _worldArray;
        private Levels level;

        /*private int _worldWidth;
        private int _minusWidth;

        private int _worldLenght;
        private int _minusLenght;*/


        public World(ContentManager Content, float blockSeparation, float worldWidth, float worldLenght, float separator)   //simple creator (develop it later)
        {
            _worldArray = new List<SceneObject>();
            level = new Levels("../../../map1.txt");
            
            /*_worldWidth = (int)worldWidth / 2;
            _minusWidth = -_worldWidth;

            _worldLenght = (int)worldLenght / 2;
            _minusLenght = -_worldLenght;*/

            for (int i = 0; i <= worldLenght; i++)
            {
                for (int j = 0; j <= worldWidth; j++)
                {
                    int index = i * 21 + j;
                    float height = level.returnTileHeight(index);
                    String model = level.returnTileModel(index);
                    String texture = level.returnTileTexture(index);
                    Vector3 wektor = new Vector3(j * blockSeparation - separator, height, i * blockSeparation);
                    _worldArray.Add(new SceneObject(wektor, model, texture));
                }
            }

            foreach (SceneObject obj in _worldArray)
            {
                obj.LoadContent(Content);
            }


        }

        /*private AddSceneObjectsFromModule()
        {
            for (int i = _minusLenght, a = 0; i <= _worldLenght; i++, a++)
            {
                for (int j = _minusWidth, b = 0; j <= _worldWidth; j++, b++)
                {
                    int index = a * 21 + b;
                    float height = level.returnTileHeight(index);
                    String model = level.returnTileModel(index);
                    String texture = level.returnTileTexture(index);
                    Vector3 wektor = new Vector3(j * blockSeparation - separator, height, i * blockSeparation);
                    _worldArray.Add(new SceneObject(wektor, model, texture));
                }
            }
        }*/

        public void Draw(EffectHandler effectHandler, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix)
        {
            foreach (SceneObject sceneObject in _worldArray)
            {
                sceneObject.Draw(effectHandler, worldMatrix, viewMatrix, projectionMatrix, sceneObject.color);
            }
        }

        public void ObjectInitializer(ContentManager Content)
        {
            foreach (SceneObject obj in _worldArray)
            {
                obj.LoadContent(Content);
            }
        }

        public List<SceneObject> GetWorldList()
        {
            return _worldArray;
        }
        
        
    }
}
