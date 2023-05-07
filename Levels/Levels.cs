using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Numerics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using System.Xml.XPath;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Emit;

namespace TheGame
{
    internal class Levels
    {
        private List<Level> _levels;
        private string[] maps = { "map2.txt", "map3.txt", "map4.txt" };
        private int moduleSeparator = 80;


        public Levels(ContentManager Content, int numberOfModules)
        {
            _levels = new List<Level>();
            _levels.Add(new Level(Content, "map1.txt", _levels.Count * moduleSeparator));
            for (int i = 0; i < numberOfModules; i++)
            {
                _levels.Add(new Level(Content, Level.GenerateRandomString(maps), _levels.Count * moduleSeparator));
            }
            
        }

        public List<SceneObject> returnSceneObjects(float playerX)
        {
            List<SceneObject> _sceneObjects = new List<SceneObject>();

            int numberOfModule = (int)playerX / moduleSeparator;

            for (int i = numberOfModule - 1; i <= numberOfModule + 1; i++)
            {
                if (i >= 0 && i < _levels.Count - 1)
                {
                    foreach (SceneObject obj in _levels[i].returnSceneObjects())
                    {
                        _sceneObjects.Add(obj);
                    }
                }
                
            }


            return _sceneObjects;
        }

        


        class Level
        {
            private string[] treeModels = { "tree1", "tree2", "tree3" };
            //private string[] grassTextures = {"trawa1", "trawa2", "trawa3"};
            private List<Tile> _tiles;
            private List<SceneObject> _sceneObjects;
            private int tileSize = 4;
            private int moduleWidth = 20;
            private int moduleHeight = 20;

            public Level(ContentManager Content, string fileName, float separator) 
            {
                _sceneObjects = new List<SceneObject>();
                LoadSceneObjects(Content, fileName, separator);
                
            }


            public struct Tile
            {
                public String model;
                public String texture;
                public float height;

                public Tile(String model, String texture, float height)
                {
                    this.model = model;
                    this.texture = texture;
                    this.height = height;
                }
            }

            public List<SceneObject> returnSceneObjects() { return _sceneObjects; }


            private void LoadSceneObjects(ContentManager Content, string fileName, float separator)
            {
                LoadScene(fileName);

                for (int i = 0; i < moduleHeight; i++)
                {
                    for (int j = 0; j < moduleWidth; j++)
                    {
                        int index = i * moduleWidth + j;
                        float height = this._tiles[index].height;
                        String model = this._tiles[index].model;
                        String texture = this._tiles[index].texture;
                        Vector3 wektor = new Vector3(j * tileSize + separator, height, i * tileSize);
                        if (this._tiles[index].texture == "tree1_color")
                        {
                            _sceneObjects.Add(new SceneObject(wektor, "test", "trawa1"));
                        }
                        _sceneObjects.Add(new SceneObject(wektor, model, texture));
                    }
                }

                ObjectInitializer(Content);
            }

            public void ObjectInitializer(ContentManager Content)
            {
                foreach (SceneObject obj in _sceneObjects)
                {
                    obj.LoadContent(Content);
                }
            }

            public void LoadScene(string fileName)
            {
                List<int> tileList = ReadFile(fileName);
               _tiles = new List<Tile>();

                for (int i = 0; i < tileList.Count; i++)
                {
                    switch (tileList[i])
                    {
                        case 48: //0
                            string treeModel = GenerateRandomString(treeModels);
                            _tiles.Add(new Tile(treeModel, "tree1_color", -2.0f));
                            break;
                        case 97: //a
                            _tiles.Add(new Tile("test", "trawa1", -2.0f));
                            break;
                        case 98:
                            _tiles.Add(new Tile("test", "trawa2", -2.0f));
                            break;
                        case 99:
                            _tiles.Add(new Tile("test", "trawa3", -2.0f));
                            break;
                    }
                }
            }

            public List<int> ReadFile(string fileName)
            {
                List<int> tileList = new List<int>();
                if (File.Exists(fileName))
                {
                    Stream s = new FileStream(fileName, FileMode.Open);
                    while (true)
                    {
                        int val = s.ReadByte();
                        if (val < 0)
                            break;
                        if (val != 32)
                        {
                            tileList.Add(val);
                        }
                    }
                    s.Close();
                    return tileList;

                }
                else return null;

            }

            public static string GenerateRandomString(string[] list)
            {
                Random random = new Random();
                int index = random.Next(list.Length);
                string generatedRandom = list[index];
                return generatedRandom;
            }


        }

       
    }
}
