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
        private string[] maps_up_down = { "Maps/map_up_down_1.txt" };
        private string[] maps_down_up = { "Maps/map_down_up_1.txt" };
        private string[] maps_up_right = { "Maps/map_up_right.txt" };
        private string[] maps_left_down = { "Maps/map_left_down.txt" };
        private string[] maps_left_up = { "Maps/map_Up_1.txt" };
        private string[] maps_down_right = { "Maps/map_down_right_1.txt" };
        #endregion
        private List<string> maps = new List<string>();
        private string currentMap;
        private string choosedMap;
        private int numberOfModules;
        #region Module parametrs and rectangles
        private int moduleSeparatorX = 120;
        private int moduleSeparatorZCount = 0;
        private int moduleHeightChange = 0;
        private int moduleSeparatorZ = 120;
        List<Rectangle> modulesList;
        Rectangle module;
        HashSet<Rectangle> visited;
        #endregion
        #region Enemies Types
        private string mintEnemy = "Objects/mint";
        private string nettleEnemy = "Objects/nettle";
        private string appleEnemy = "Objects/apple";
        private string melissaEnemy = "Objects/melissa";
        #endregion



        public Levels(int numberOfModules)
        {
            _levels = new List<Level>();
            modulesList = new List<Rectangle>();
            visited = new HashSet<Rectangle>();
            this.numberOfModules = numberOfModules;
        }

        public void LoadContent()
        {
            prepareMap();
        }

        public void prepareFirstLevels()
        {
            //Moduł 1 - z salą weselną
            prepareModule("Maps/map1.txt", 0);

            //Moduł 2
            prepareModule("Maps/map2.txt", 0);
            _levels[1].GenerateEnemy(mintEnemy, new Vector3(moduleSeparatorX + 30, 0, 50));
            _levels[1].GenerateEnemy(mintEnemy, new Vector3(moduleSeparatorX + 30, 0, 60));

            //Moduł 3
            prepareModule("Maps/map3.txt", 0);
            _levels[2].GenerateEnemy(mintEnemy, new Vector3(2 * moduleSeparatorX + 30, 0, 50));
            _levels[2].GenerateEnemy(nettleEnemy, new Vector3(2 * moduleSeparatorX + 30, 0, 60));

            currentMap = "Maps/map3.txt";

        }

        public void prepareMap()
        {
            int enemyCount;
            int partyNumber = 0;
            prepareFirstLevels();


            for (int i = 0; i < numberOfModules + 1; i++)
            {
                maps = new List<string>();

                //wybieranie ilości przeciwników w levelu
                if (numberOfModules < 10)
                    enemyCount = 2;
                else
                    enemyCount = 4;

/*                if (numberOfModules + partyNumber >= 10 && maps_straight.Contains(currentMap))          //wesele!
                {
                    prepareModule("Maps/map6.txt", 0);
                    currentMap = "Maps/map6.txt";
                    partyNumber += 10;
                }*/

                //Przypadek prostych map - wylot z lewej i prawej
                if (currentMap == "Maps/map1.txt" || maps_straight.Contains(currentMap))
                {
                    maps.AddRange(maps_straight);   //dalej prosto
                    maps.AddRange(maps_left_up);    //idziemy do gory
                    maps.AddRange(maps_left_down);  //idziemy do dołu

                    prepareRandomModule(enemyCount);
                }

                //Zakręt do góry - wylot z lewej i góry
                if (maps_left_up.Contains(currentMap))
                {
                    maps.AddRange(maps_down_right);     //idziemy od dołu w prawo
                    maps.AddRange(maps_down_up);

                    moduleSeparatorZCount--;
                    moduleHeightChange++;

                    prepareRandomModule(enemyCount);
                }

                //Zakręt od dołu w prawo - wylot z dołu i po prawo
                if (maps_down_right.Contains(currentMap))
                {
                    maps.AddRange(maps_straight);
                    maps.AddRange(maps_left_up);

                    prepareRandomModule(enemyCount);
                }

                //Zakręt w dół - wylot z lewej i z dołu
                if (maps_left_down.Contains(currentMap))
                {
                    maps.AddRange(maps_up_right);
                    maps.AddRange(maps_up_down);

                    moduleSeparatorZCount++;
                    moduleHeightChange++;

                    prepareRandomModule(enemyCount);
                }

                //Wylot z góry i z prawej
                if (maps_up_right.Contains(currentMap))
                {
                    maps.AddRange(maps_straight);   //dalej prosto
                    maps.AddRange(maps_left_up);    //idziemy do gory
                    maps.AddRange(maps_left_down);  //idziemy do dołu

                    prepareRandomModule(enemyCount);
                }
                //Prosta od góry do dołu              //work in progress
                if (maps_up_down.Contains(currentMap))
                {
                    maps.AddRange(maps_up_right);

                    moduleSeparatorZCount++;
                    moduleHeightChange++;

                    prepareRandomModule(enemyCount);

                }

                //Prosta od dołu do góry
                if (maps_down_up.Contains(currentMap))
                {
                    maps.AddRange(maps_down_right);

                    moduleSeparatorZCount--;
                    moduleHeightChange++;

                    prepareRandomModule(enemyCount);

                }

                currentMap = choosedMap;


            }
        }

        public void prepareRandomModule(int enemyCount)
        {
            choosedMap = generateRandomStringFromList(maps);

            prepareModule(choosedMap, enemyCount);

            maps.Clear();

        }

        public void prepareModule(string map, int enemyCount)
        {
            module = new Rectangle((_levels.Count - moduleHeightChange) * moduleSeparatorX, moduleSeparatorZCount * moduleSeparatorZ, moduleSeparatorX, moduleSeparatorZ);
            Level level = new Level(map, (_levels.Count - moduleHeightChange) * moduleSeparatorX, enemyCount, moduleSeparatorZCount * moduleSeparatorZ);
            level.LoadContent();
            _levels.Add(level);
            modulesList.Add(module);
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
                foreach (Enemy enemy in _levels[numberOfModule + 1].returnEnemies())
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
            private string[] treeModels = { "Objects/tree1", "Objects/tree2", "Objects/tree3" };
            private string[] enemyTypes = { "Objects/apple", "Objects/mint", "Objects/nettle", "Objects/melissa" };
            private string[] otherModels = { "Objects/rock2", "Objects/rock18", "Objects/tree1" };
            private List<Tile> _tiles;
            private List<SceneObject> _sceneObjects;
            private int tileSize = 6;
            private int moduleWidth = 20;
            private int moduleHeight = 20;
            private string ground = "Objects/test";
            private List<Enemy> enemies;
            private List<Vector3> groundPositions;

            private string fileName;
            private float separatorX, separatorZ;
            private int enemyCount;



            public Level(string fileName, float separatorX, int enemyCount, float separatorZ)
            {
                _sceneObjects = new List<SceneObject>();
                enemies = new List<Enemy>();
                groundPositions = new List<Vector3>();

                this.fileName = fileName; 
                this.separatorX = separatorX;
                this.separatorZ = separatorZ;
                this.enemyCount = enemyCount;
                
            }

            public void LoadContent()
            {
                LoadSceneObjects();
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

            //public List<Vector3> lista = new List<Vector3>();
            private void LoadSceneObjects()
            {
                LoadScene(fileName, enemyCount);
                Vector3 groundPos = new Vector3(0.5f * moduleWidth * tileSize + separatorX, -0.7f, 0.5f * moduleHeight * tileSize + separatorZ);
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
                            enemyWektor.Y = 0;
                            groundPositions.Add(enemyWektor);
                        }
                        if (treeModels.Contains(model))
                        {
                            SceneObject tree = new SceneObject(wektor, model, texture);
                            Random rand = new Random();
                            float rflot = (float)rand.NextDouble() * 2 * (float)Math.PI;            //zmiana obrotu drzewa losowo
                            float size = (float)rand.Next(200, 300) / 100;                                //zmiana wielkosci drzewa losowo
                            tree.SetScale(size);
                            tree.SetRotation(new Vector3(0, rflot, 0));
                            _sceneObjects.Add(tree);
                        }

                    }
                }
                _sceneObjects.Add(new SceneObject(groundPos, "Objects/ground", "Textures/floor"));

                Random random = new Random();
                int objectsCount = random.Next(4, 7);
                GenerateOtherObjects(objectsCount);
                if (enemyCount != 0) 
                { 
                    GenerateRandomEnemies(enemyCount); 
                }
                ObjectInitializer();
            }

            public void ObjectInitializer()
            {
                foreach (SceneObject obj in _sceneObjects)
                {
                    obj.LoadContent();
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

                            _tiles.Add(new Tile(treeModel, "Textures/tree1_color", -2.0f));
                            break;
                        case 97: //a
                            _tiles.Add(new Tile(ground, "Textures/trawa1", -2.0f));
                            break;
/*                        case 98:
                            _tiles.Add(new Tile(ground, "Textures/trawa2", -3.0f));
                            break;
                        case 99:
                            _tiles.Add(new Tile(ground, "Textures/trawa3", -3.0f));
                            break;*/
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


            public void GenerateRandomEnemies(int enemyCount)
            {
                if (this.enemyCount != 0)
                {
                    for (int i = 0; i < this.enemyCount; i++)
                    {
                        string enemyType = GenerateRandomString(enemyTypes);
                        int[] groundList = new int[groundPositions.Count];
                        int index = GenerateRandomInt(groundList);
                        Vector3 groundPosition = groundPositions[index];
                        groundPositions.RemoveAt(index);
                        GenerateEnemy(enemyType, groundPosition);
                    }
                }
            }

            public void GenerateEnemy(string enemyType, Vector3 groundPosition)
            {
                switch (enemyType)
                {
                    case "Objects/apple":
                        AppleTree apple = new AppleTree(groundPosition, "Objects/jablon", "Textures/drzewotekstur");
                        apple.LoadContent();
                        enemies.Add(apple);
                        break;
                    case "Objects/melissa":
                        Melissa melissa = new Melissa(groundPosition, "Objects/melisa", "Textures/melisa");
                        melissa.LoadContent();
                        enemies.Add(melissa);
                        break;
                    case "Objects/nettle":
                        Nettle nettle = new Nettle(groundPosition, "Objects/pokrzywa", "Textures/pokrzyw");
                        nettle.LoadContent();
                        enemies.Add(nettle);
                        break;
                    case "Objects/mint":
                        Mint mint = new Mint(groundPosition, "Objects/mieta", "Textures/mieta");
                        mint.LoadContent();
                        enemies.Add(mint);
                        break;
                }
            }

            public void GenerateOtherObjects(int count)
            {
                if (count != 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        int[] groundList = new int[groundPositions.Count];
                        int index = GenerateRandomInt(groundList);
                        Vector3 groundPosition = groundPositions[index];
                        groundPositions.RemoveAt(index);

                        string objectType = GenerateRandomString(otherModels);

                        Random rand = new Random();
                        float rflot = (float)rand.NextDouble() * 2 * (float)Math.PI; //zmiana obrotu losowo
                        switch (objectType)
                        {
                            case "Objects/rock2":
                                SceneObject stone = new SceneObject(groundPosition, "Objects/rock2", "Textures/black");
                                stone.SetScale((float)rand.Next(80, 100) / 100);
                                stone.SetRotation(new Vector3(0, rflot, 0));
                                _sceneObjects.Add(stone);
                                break;
                            case "Objects/rock18":
                                SceneObject stone1 = new SceneObject(groundPosition, "Objects/rock18", "Textures/black");
                                stone1.SetScale((float)rand.Next(80, 100) / 100);
                                stone1.SetRotation(new Vector3(0, rflot, 0));
                                _sceneObjects.Add(stone1);
                                break;
                            case "Objects/tree1":
                                SceneObject tree = new SceneObject(groundPosition, "Objects/tree1", "Textures/tree1_color");           
                                tree.SetScale((float)rand.Next(75, 150) / 100);
                                tree.SetRotation(new Vector3(0, rflot, 0));
                                _sceneObjects.Add(tree);
                                break;

                        }
                    }
                }
            }
        }



    }
}
