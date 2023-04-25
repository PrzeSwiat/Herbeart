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

        private int worldWidth = 20;
        private int worldLenght = 20;
        private float blockSeparation = 4f;
        private int moduleSeparator = 80;
        private string[] maps = { "../../../map2.txt", "../../../map3.txt", "../../../map3.txt"};


        public World(ContentManager Content)   
        {
            _worldArray = new List<SceneObject>();
            AddSceneObjectsFromModule(Content, "../../../map1.txt", 0);
            for (int i = 1; i < 4; i++)
            {
                Random random = new Random();
                int index = random.Next(maps.Length);
                string mapName = maps[index];
                AddSceneObjectsFromModule(Content, mapName, moduleSeparator * i);
            }
            //AddSceneObjectsFromModule(Content, "../../../map2.txt", 80);

        }

        private void AddSceneObjectsFromModule(ContentManager Content, String fileName, float separator)
        {
            level = new Levels(fileName);

            for (int i = 0; i < worldLenght; i++)
            {
                for (int j = 0; j < worldWidth; j++)
                {
                    int index = i * 20 + j;
                    float height = level.returnTileHeight(index);
                    String model = level.returnTileModel(index);
                    String texture = level.returnTileTexture(index);
                    Vector3 wektor = new Vector3(j * blockSeparation + separator, height, i * blockSeparation);
                    _worldArray.Add(new SceneObject(wektor, model, texture));
                }
            }

            foreach (SceneObject obj in _worldArray)
            {
                obj.LoadContent(Content);
            }
        }

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
