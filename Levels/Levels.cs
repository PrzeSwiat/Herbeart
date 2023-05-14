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
        #region Maps
        private string[] maps_straight = { "Maps/map2.txt", "Maps/map3.txt", "Maps/map4.txt" };
/*        private string[] maps_down_straight = { "Maps/map_Up_Straight_1.txt" };
        private string[] maps_up_straight = { "Maps/map_Down_Straight_1.txt" };*/
        private string[] maps_up_down = { "Maps/map_up_down_1.txt"};
        private string[] maps_down_up = { "Maps/map_down_up_1.txt" };
        private string[] maps_up_right = { "Maps/map_up_right.txt" };
        private string[] maps_left_down = { "Maps/map_left_down.txt" };
        private string[] maps_left_up = { "Maps/map_Up_1.txt" };
        private string[] maps_down_right = { "Maps/map_down_right_1.txt" };
        #endregion
        private List<string> maps = new List<string>();
        private string currentMap;
        private string choosedMap;
        #region Module parametrs and rectangles
        private int moduleSeparatorX = 80;
        private int moduleSeparatorZCount = 0;
        private int moduleHeightChange = 0;
        private int moduleSeparatorZ = 80;
        List<Rectangle> modulesList;
        Rectangle module;
        HashSet<Rectangle> visited;
        #endregion



        public Levels(ContentManager Content, int numberOfModules)
        {
            _levels = new List<Level>();
            modulesList = new List<Rectangle> ();
            prepareMap(Content, numberOfModules);
            visited = new HashSet<Rectangle>();
        }


        public void prepareMap (ContentManager Content, int numberOfModules)
        {
            int enemyCount;
            _levels.Add(new Level(Content, "Maps/map1.txt", _levels.Count * moduleSeparatorX, 0, 0));
            module = new Rectangle(0, 0, moduleSeparatorX, moduleSeparatorZ);
            modulesList.Add(module);
            currentMap = "Maps/map1.txt";


            for (int i = 0; i < numberOfModules + 1; i++)
            {
                maps = new List<string>();

                //wybieranie ilości przeciwników w levelu
                if (numberOfModules < 4)
                    enemyCount = 2;
                else
                    enemyCount = 4;

                Debug.Write(currentMap);
                Debug.Write("\n");

                //Przypadek prostych map - wylot z lewej i prawej
                if (currentMap == "Maps/map1.txt" || maps_straight.Contains(currentMap))
                {
                    maps.AddRange(maps_straight);   //dalej prosto
                    maps.AddRange(maps_left_up);    //idziemy do gory
                    maps.AddRange(maps_left_down);  //idziemy do dołu

                    prepareModule(Content, enemyCount);
                }

                //Zakręt do góry - wylot z lewej i góry
                if (maps_left_up.Contains(currentMap))
                {
                    maps.AddRange(maps_down_right);     //idziemy od dołu w prawo
                    maps.AddRange(maps_down_up);

                    moduleSeparatorZCount--;
                    moduleHeightChange++;

                    prepareModule(Content, enemyCount);
                }

                //Zakręt od dołu w prawo - wylot z dołu i po prawo
                if (maps_down_right.Contains(currentMap))
                {
                    maps.AddRange(maps_straight);
                    maps.AddRange(maps_left_up);

                    prepareModule(Content, enemyCount);
                }

                //Zakręt w dół - wylot z lewej i z dołu
                if (maps_left_down.Contains(currentMap))
                {
                    maps.AddRange(maps_up_right);
                    maps.AddRange(maps_up_down);

                    moduleSeparatorZCount++;
                    moduleHeightChange++;

                    prepareModule(Content, enemyCount);
                }

                //Wylot z góry i z prawej
                if (maps_up_right.Contains(currentMap))
                {
                    maps.AddRange(maps_straight);   //dalej prosto
                    maps.AddRange(maps_left_up);    //idziemy do gory
                    maps.AddRange(maps_left_down);  //idziemy do dołu

                    prepareModule(Content, enemyCount);
                }
                //Prosta od góry do dołu              //work in progress
                if (maps_up_down.Contains(currentMap))
                {
                    maps.AddRange(maps_up_right);

                    moduleSeparatorZCount++;
                    moduleHeightChange++;

                    prepareModule(Content, enemyCount);

                }

                //Prosta od dołu do góry
                if (maps_down_up.Contains(currentMap))
                {
                    maps.AddRange(maps_down_right);

                    moduleSeparatorZCount--;
                    moduleHeightChange++;

                    prepareModule(Content, enemyCount);

                }



                /*                //Wylot z dołu, lewej i prawej
                                if(maps_down_straight.Contains(currentMap))                             //Work in progres -  nie ruszac
                                {
                                    maps.AddRange(maps_up_straight);
                                    maps.AddRange(maps_straight);
                                    choosed = generateRandomStringFromList(maps);
                                    _levels.Add(new Level(Content, choosed, _levels.Count * moduleSeparator, enemyCount));
                                    maps.Clear();
                                }

                                //Wylot z góry, lewej, prawej
                                if (maps_up_straight.Contains(currentMap))
                                {
                                    maps.AddRange(maps_up_straight);
                                    maps.AddRange(maps_straight);
                                    choosed = generateRandomStringFromList(maps);
                                    _levels.Add(new Level(Content, choosed, _levels.Count * moduleSeparator, enemyCount));
                                    maps.Clear();
                                }*/

                currentMap = choosedMap;

            }
        }

        public void prepareModule(ContentManager Content, int enemyCount)
        {
            choosedMap = generateRandomStringFromList(maps);

            module = new Rectangle((_levels.Count - moduleHeightChange) * moduleSeparatorX, moduleSeparatorZCount * moduleSeparatorZ, moduleSeparatorX, moduleSeparatorZ);

            _levels.Add(new Level(Content, choosedMap, (_levels.Count - moduleHeightChange) * moduleSeparatorX, enemyCount, moduleSeparatorZCount * moduleSeparatorZ));

            modulesList.Add(module);

            maps.Clear();
            
        }

        public List<SceneObject> returnSceneObjects(float playerX, float playerY)
        {
            List<SceneObject> _sceneObjects = new List<SceneObject>();

            int numberOfModule = returnModuleNumber(playerX, playerY);

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

        public List<Enemy> returnEnemiesList(float playerX, float playerY)
        {
            List<Enemy> enemiesList = new List<Enemy>();

            int numberOfModule = returnModuleNumber(playerX, playerY);

            if (!visited.Contains(modulesList[numberOfModule]) && numberOfModule != modulesList.Count - 2)
            {
                foreach (Enemy enemy in _levels[numberOfModule+1].returnEnemies())
                {
                    enemiesList.Add(enemy);
                }
                visited.Add(modulesList[numberOfModule]);    
            }
                
            
            return enemiesList;
        }

        public int returnModuleNumber(float playerX, float playerY)
        {
            int numberOfModule = 0;

            for (int i = 0; i < modulesList.Count; i++)
            {
                if (modulesList[i].Contains((int)playerX, (int)playerY))
                {
                    numberOfModule = i;
                }
            }
            return numberOfModule;
        }
        
        public string generateRandomStringFromList(List<string> list)
        {
            Random random = new Random();
            string randomString = list[random.Next(list.Count)];
            return randomString;
        }

        class Level
        {
            private string[] treeModels = { "tree1", "tree2", "tree3" };
            private string[] enemyTypes = { "apple", "mint", "nettle", "melissa" };
            //private string[] grassTextures = {"trawa1", "trawa2", "trawa3"};
            private List<Tile> _tiles;
            private List<SceneObject> _sceneObjects;
            private int tileSize = 4;
            private int moduleWidth = 20;
            private int moduleHeight = 20;
            private string ground = "test";
            private ContentManager content;
            private List<Enemy> enemies;
            
            
            public Level(ContentManager Content, string fileName, float separator, int enemyCount, float separatorZ) 
            {
                _sceneObjects = new List<SceneObject>();
                enemies = new List<Enemy>();
                content = Content;
                LoadSceneObjects(Content, fileName, separator, enemyCount, separatorZ);
                
            }

            public List<Enemy> returnEnemies()
            {
                return enemies;
            }

            public struct Tile
            {
                public string model;
                public string texture;
                public float height;

                public Tile(String model, String texture, float height)
                {
                    this.model = model;
                    this.texture = texture;
                    this.height = height;
                }
            }

            public List<SceneObject> returnSceneObjects() { return _sceneObjects; }


            private void LoadSceneObjects(ContentManager Content, string fileName, float separatorX, int enemyCount, float separatorZ)
            {
                LoadScene(fileName, enemyCount);
                List<Vector3> groundPositions = new List<Vector3>();
                int groundListSize = 0;

                for (int i = 0; i < moduleHeight; i++)
                {
                    for (int j = 0; j < moduleWidth; j++)
                    {
                        int index = i * moduleWidth + j;
                        float height = this._tiles[index].height;
                        string model = this._tiles[index].model;
                        string texture = this._tiles[index].texture;
                        Vector3 wektor = new Vector3(j * tileSize + separatorX, height, i * tileSize + separatorZ);
                        if (model == ground)
                        {
                            Vector3 enemyWektor = wektor;
                            enemyWektor.Y = 2;
                            groundListSize++;
                            groundPositions.Add(enemyWektor);
                        }
                        if (treeModels.Contains(model))
                        {
                            _sceneObjects.Add(new SceneObject(wektor, ground, "trawa1"));       //dodanie trawy pod drzewka
                            SceneObject tree = new SceneObject(wektor, model, texture);
                            Random rand = new Random();
                            float rflot =( float)rand.NextDouble()*2*(float)Math.PI;            //zmiana obrotu drzewa losowo
                            float size = (float)rand.Next(200, 300)/100;                                //zmiana wielkosci drzewa losowo
                            tree.SetScale(size);
                            tree.SetRotation(new Vector3(0,rflot,0));
                            _sceneObjects.Add(tree);
                        }
                        else
                        {
                            _sceneObjects.Add(new SceneObject(wektor, model, texture));
                        }
                        
                    }
                }
                GenerateEnemies(enemyCount, groundPositions, groundListSize);
                ObjectInitializer(Content);
            }

            public void ObjectInitializer(ContentManager Content)
            {
                foreach (SceneObject obj in _sceneObjects)
                {
                    obj.LoadContent(Content);
                }
            }

            public void LoadScene(string fileName, int enemyCount)
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
                            _tiles.Add(new Tile(ground, "trawa1", -2.0f));
                            break;
                        case 98:
                            _tiles.Add(new Tile(ground, "trawa2", -2.0f));
                            break;
                        case 99:
                            _tiles.Add(new Tile(ground, "trawa3", -2.0f));
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

            public static int GenerateRandomInt(int[] list)
            {
                Random random = new Random();
                int index = random.Next(list.Length);
                return index;
            }

            public void GenerateEnemies(int enemyCount, List<Vector3> groundPositions, int groundListSize)
            {

                int[] groundList = new int[groundListSize];
                Vector3[] enemiesPositions = new Vector3[enemyCount];
                for (int i = 0; i < enemyCount; i++)
                {
                    int index = GenerateRandomInt(groundList);
                    Vector3 groundPosition = groundPositions[index];
                    if(!enemiesPositions.Contains(groundPosition))
                    {
                        enemiesPositions[i] = groundPosition;
                    }
                }
                if (enemyCount != 0)
                {
                    for (int i = 0; i < enemyCount; i++)
                    {
                        string enemyType = GenerateRandomString(enemyTypes);
                        switch (enemyType)
                        {
                            case "apple":
                                AppleTree apple = new AppleTree(enemiesPositions[i], "player", "StarSparrow_Green");
                                apple.LoadContent(content);
                                enemies.Add(apple);
                                break;
                            case "melissa":
                                Melissa melissa = new Melissa(enemiesPositions[i], "player", "StarSparrow_Green");
                                melissa.LoadContent(content);
                                enemies.Add(melissa);
                                break;
                            case "nettle":
                                Nettle nettle = new Nettle(enemiesPositions[i], "player", "StarSparrow_Green");
                                nettle.LoadContent(content);
                                enemies.Add(nettle);
                                break;
                            case "mint":
                                Mint mint = new Mint(enemiesPositions[i], "player", "StarSparrow_Green");
                                mint.LoadContent(content);
                                enemies.Add(mint);
                                break;
                        }
                    }
                }
            }

        }

            
       
    }
}
