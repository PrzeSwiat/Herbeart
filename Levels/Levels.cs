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
        private string[] maps_straight = { "Maps/map2.txt", "Maps/map3.txt", "Maps/map4.txt", "Maps/map5.txt" };
        private string[] maps_up_down = { "Maps/map_up_down_1.txt" };
        private string[] maps_down_up = { "Maps/map_down_up_1.txt" };
        private string[] maps_up_right = { "Maps/map_up_right.txt" };
        private string[] maps_left_down = { "Maps/map_left_down_1.txt", "Maps/map_left_down_2.txt", "Maps/map_left_down_3.txt" };
        private string[] maps_left_up = { "Maps/map_left_up_1.txt", "Maps/map_left_up_2.txt", "Maps/map_left_up_3.txt" };
        private string[] maps_down_right = { "Maps/map_down_right_1.txt" };
        #endregion
        private List<string> maps = new List<string>();
        private string currentMap;
        private string choosedMap;

        #region Module parametrs and rectangles
        private int numberOfModules;
        private int moduleSeparatorX = 156;
        private int moduleSeparatorZCount = 0;
        private int moduleHeightChange = 0;
        private int moduleSeparatorZ = 156;
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



        public Levels()
        {
            _levels = new List<Level>();
            modulesList = new List<Rectangle>();
            visited = new HashSet<Rectangle>();
        }

        public void LoadContent()
        {
            prepareFirstLevels();
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
            numberOfModules = 3;

        }

        public void prepareMap()
        {
            numberOfModules++;

            int enemyCount;
            maps = new List<string>();

            //wybieranie ilości przeciwników w levelu
            if (numberOfModules > 10)
                enemyCount = 9;
            else if (numberOfModules > 15)
                enemyCount = 15;
            else 
                enemyCount = 4;
            
            //Przypadek prostych map - wylot z lewej i prawej
            if (currentMap == "Maps/map1.txt" || maps_straight.Contains(currentMap))
            {

                if (numberOfModules % 10 == 0)
                {
                    maps.Add("Maps/map_party_straight.txt");
                    maps.Add("Maps/map_party_left_up.txt");
                    maps.Add("Maps/map_party_left_down.txt");
                    string map = generateRandomStringFromList(maps);
                    prepareModule(map, 0);
                    switch (map)
                    {
                        case "Maps/map_party_straight.txt":
                            choosedMap = "Maps/map_2.txt";
                            break;
                        case "Maps/map_party_left_up.txt":
                            choosedMap = "Maps/map_left_up_1.txt";
                            break;
                        case "Maps/map_party_left_down.txt":
                            choosedMap = "Maps/map_left_down_1.txt";
                            break;
                    }
                    maps.Clear();
                }
                else
                {
                    maps.AddRange(maps_straight);   //dalej prosto
                    maps.AddRange(maps_left_up);    //idziemy do gory
                    maps.AddRange(maps_left_down);  //idziemy do dołu
                    prepareRandomModule(enemyCount);
                }
            }

            //Zakręt do góry - wylot z lewej i góry
            if (maps_left_up.Contains(currentMap))
            {
                moduleSeparatorZCount--;
                moduleHeightChange++;
                if (numberOfModules % 10 == 0)
                {
                    maps.Add("Maps/map_party_down_up.txt");
                    maps.Add("Maps/map_party_down_right.txt");
                    string map = generateRandomStringFromList(maps);
                    prepareModule(map, 0);
                    switch (map)
                    {
                        case "Maps/map_party_down_up.txt":
                            choosedMap = "Maps/map_down_up_1.txt";
                            break;
                        case "Maps/map_party_down_right.txt":
                            choosedMap = "Maps/map_down_right_1.txt";
                            break;
                    }
                    maps.Clear();
                }
                else
                {
                    maps.AddRange(maps_down_right);     //idziemy od dołu w prawo
                    maps.AddRange(maps_down_up);
                    prepareRandomModule(enemyCount);
                }

            }

            //Zakręt od dołu w prawo - wylot z dołu i po prawo
            if (maps_down_right.Contains(currentMap))
            {
                if(numberOfModules % 10 == 0)
                {
                    maps.Add("Maps/map_party_straight.txt");
                    maps.Add("Maps/map_party_left_up.txt");
                    string map = generateRandomStringFromList(maps);
                    prepareModule(map, 0);
                    switch (map)
                    {
                        case "Maps/map_party_straight.txt":
                            choosedMap = "Maps/map_2.txt";
                            break;
                        case "Maps/map_party_left_up.txt":
                            choosedMap = "Maps/map_left_up_1.txt";
                            break;
                    }
                    maps.Clear();
                }
                else
                {
                    maps.AddRange(maps_straight);
                    maps.AddRange(maps_left_up);
                    prepareRandomModule(enemyCount);
                }
            }

            //Zakręt w dół - wylot z lewej i z dołu
            if (maps_left_down.Contains(currentMap))
            {
                moduleSeparatorZCount++;
                moduleHeightChange++;
                if (numberOfModules % 10 == 0)
                {
                    maps.Add("Maps/map_party_up_right.txt");
                    maps.Add("Maps/map_party_up_down.txt");
                    string map = generateRandomStringFromList(maps);
                    prepareModule(map, 0);
                    switch (map)
                    {
                        case "Maps/map_party_up_right.txt":
                            choosedMap = "Maps/map_up_right.txt";
                            break;
                        case "Maps/map_party_up_down.txt":
                            choosedMap = "Maps/map_up_down_1.txt";
                            break;
                    }
                    maps.Clear();
                }
                else
                {
                    maps.AddRange(maps_up_right);
                    maps.AddRange(maps_up_down);
                    prepareRandomModule(enemyCount);
                }
            }

            //Wylot z góry i z prawej
            if (maps_up_right.Contains(currentMap))
            {
                if (numberOfModules % 10 == 0)
                {
                    maps.Add("Maps/map_party_straight.txt");
                    maps.Add("Maps/map_party_left_up.txt");
                    maps.Add("Maps/map_party_left_down.txt");
                    string map = generateRandomStringFromList(maps);
                    prepareModule(map, 0);
                    switch (map)
                    {
                        case "Maps/map_party_straight.txt":
                            choosedMap = "Maps/map_2.txt";
                            break;
                        case "Maps/map_party_left_up.txt":
                            choosedMap = "Maps/map_left_up_1.txt";
                            break;
                        case "Maps/map_party_left_down.txt":
                            choosedMap = "Maps/map_left_down_1.txt";
                            break;
                    }
                    maps.Clear();
                }
                else
                {
                    maps.AddRange(maps_straight);   //dalej prosto
                    maps.AddRange(maps_left_up);    //idziemy do gory
                    maps.AddRange(maps_left_down);  //idziemy do dołu
                    prepareRandomModule(enemyCount);
                }
            }
            //Prosta od góry do dołu              //work in progress
            if (maps_up_down.Contains(currentMap))
            {
                moduleSeparatorZCount++;
                moduleHeightChange++;
                if (numberOfModules % 10 == 0)
                {
                    prepareModule("Maps/map_party_up_right.txt", 0);
                    choosedMap = "Maps/map_up_right.txt";
                }
                else
                {
                    maps.AddRange(maps_up_right);
                    prepareRandomModule(enemyCount);
                }

            }

            //Prosta od dołu do góry
            if (maps_down_up.Contains(currentMap))
            {
                moduleSeparatorZCount--;
                moduleHeightChange++;
                if (numberOfModules % 10 == 0)
                {
                    prepareModule("Maps/map_party_down_right.txt", 0);
                    choosedMap = "Maps/map_down_right.txt";
                }
                else
                {
                    maps.AddRange(maps_down_right);
                    prepareRandomModule(enemyCount);
                }

            }

            currentMap = choosedMap;
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

            if (numberOfModule == numberOfModules-2)
            {
                prepareMap();
            }
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

        public List<SceneObject> returnNonCollideSceneObjects(float playerX, float playerY)
        {
            List<SceneObject> _sceneObjects = new List<SceneObject>();

            int numberOfModule = returnModuleNumber(playerX, playerY);


            for (int i = numberOfModule - 1; i <= numberOfModule + 1; i++)
            {
                if (i >= 0 && i < _levels.Count - 1)
                {
                    foreach (SceneObject obj in _levels[i].returnNonCollideSceneObjects())
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

            #region Models
            private string[] treeModels = { "Objects/drzewo1", "Objects/drzewo2", "Objects/drzewo3" };
            private string[] enemyTypes = { "Objects/apple", "Objects/mint", "Objects/nettle", "Objects/melissa" };
            private string[] grassModels = { "Objects/big_grass", "Objects/grass1", "Objects/grass2", "Objects/grass3", "Objects/three_grass" };
            private string[] otherSmallModels = { "Objects/big_stone", "Objects/small_stone", "Objects/two_stones", "Objects/bush", "Objects/small_bush", "Objects/flower", "Objects/flower", "Objects/flower" };
            #endregion
            #region Textures
            private string[] bushTextures = { "Textures/bush1", "Textures/bush2" };
            private string[] flowerTextures = { "Textures/flower1", "Textures/flower2" };
            private string[] bigGrassTextures = { "Textures/big_grass1", "Textures/big_grass2" };
            private string[] grassTextures = { "Textures/grass1", "Textures/grass2", "Textures/grass3", "Textures/grass4", "Textures/grass5", "Textures/grass6"};
            #endregion
            private List<Tile> groundTiles;
            private List<Tile> forestTiles;
            private List<Tile> spawnTiles;
            private List<Tile> borderTiles;
            private List<SceneObject> _sceneObjects;
            private List<SceneObject> nonColideObjects;
            private int tileSize = 6;
            private int moduleWidth = 26;
            private int moduleHeight = 26;
            private List<Enemy> enemies;

            private string fileName;
            private float separatorX, separatorZ;
            public int enemyCount;



            public Level(string fileName, float separatorX, int enemyCount, float separatorZ)
            {
                _sceneObjects = new List<SceneObject>();
                nonColideObjects = new List<SceneObject>();
                enemies = new List<Enemy>();
                groundTiles = new List<Tile>();
                forestTiles = new List<Tile>();
                spawnTiles = new List<Tile>();
                borderTiles = new List<Tile>();

                this.fileName = fileName; 
                this.separatorX = separatorX;
                this.separatorZ = separatorZ;
                this.enemyCount = enemyCount;
                
            }

            public void LoadContent()
            {
                LoadScene();
            }

            public List<Enemy> returnEnemies()
            {
                return enemies;
            }

            public struct Tile
            {
                public string groundType;
                public float height;
                public Vector3 position;
                public double probability; 

                public Tile(String groundType, float height, Vector3 position, double probability)
                {
                    this.groundType = groundType;
                    this.height = height;
                    this.position = position;
                    this.probability = probability;
                }
            }


            public List<SceneObject> returnSceneObjects() { return _sceneObjects; }

            public List<SceneObject> returnNonCollideSceneObjects() { return nonColideObjects; }

            public void ObjectInitializer()
            {
                foreach (SceneObject obj in _sceneObjects)
                {
                    obj.LoadContent();
                }
                foreach (SceneObject obj in nonColideObjects)
                {
                    obj.LoadContent();
                }
            }


            public void GenerateForest(Vector3 wektor, float minSize, float maxSize)
            {
                string treeModel = GenerateRandomString(treeModels);
                string texture = "null";
                switch (treeModel)
                {
                    case "Objects/drzewo1":
                        texture = "Textures/drzewo1";
                        break;
                    case "Objects/drzewo2":
                        texture = "Textures/drzewo2";
                        break;
                    case "Objects/drzewo3":
                        texture = "Textures/drzewo3";
                        break;
                }
                SceneObject tree = new SceneObject(wektor, treeModel, texture);
                Random rand = new Random();
                float rflot = (float)rand.NextDouble() * 2 * (float)Math.PI;            //zmiana obrotu drzewa losowo
                float size = (float)rand.Next(100, 220) / 100;                                //zmiana wielkosci drzewa losowo
                tree.SetScale(size);
                tree.SetRotation(new Vector3(0, rflot, 0));
                _sceneObjects.Add(tree);
            }

            public void LoadScene()
            {
                string groundType;
                float height;

                int x = 0;  //do poprawnego wyswietlania tile'ow
                int z = 0;

                List<int> tileList = ReadFile(fileName);
                Vector3 groundPos = new Vector3(0.5f * moduleWidth * tileSize + separatorX, -0.7f, 0.5f * moduleHeight * tileSize + separatorZ);
                for (int i = 0; i < tileList.Count; i++)
                {
                    if (x == moduleWidth)
                    {
                        z++;
                        x = 0;
                    }
                    switch (tileList[i])
                    {
                        case 48: //0
                            groundType = "forest";
                            height = 0.0f;
                            Vector3 wektor = new Vector3(x * tileSize + separatorX, height, z * tileSize + separatorZ);
                            Tile tile = new Tile(groundType, height, wektor, 0);
                            forestTiles.Add(tile);
                            Vector3 newVector = ChangeTileVector(tile, -2.5, 2.5, true, false, 0, 0);
                            GenerateForest(newVector, 150, 220);
                            x++;
                            break;
                        case 49: //1
                            groundType = "forest_border";
                            height = 0.0f;
                            Vector3 wektor3 = new Vector3(x * tileSize + separatorX, height, z * tileSize + separatorZ);
                            Tile tile1 = new Tile(groundType, height, wektor3, 0);
                            forestTiles.Add(tile1);
                            borderTiles.Add(tile1);
                            Vector3 newVector1 = ChangeTileVector(tile1, -2, 2, true, true, -1.5, 1.5);
                            GenerateForest(newVector1, 130, 150);
                            GenerateGreenObjectsNearTrees(tile1);
                            x++;
                            break;
                        case 97: //a
                            groundType = "grass";
                            height = 0.0f;
                            Vector3 wektor1 = new Vector3(x * tileSize + separatorX, height, z * tileSize + separatorZ);
                            groundTiles.Add(new Tile(groundType, height, wektor1, 0));
                            x++;
                            break;
                        case 98: //b
                            groundType = "spawn";
                            height = -2.0f;
                            Vector3 wektor2 = new Vector3(x * tileSize + separatorX, height, z * tileSize + separatorZ);
                            spawnTiles.Add(new Tile(groundType, height, wektor2, 0));
                            x++;
                            break;
                    }
                    
                }
                _sceneObjects.Add(new SceneObject(groundPos, "Objects/ground1", "Textures/tekstura_wielkiego_tila2"));
                Random random = new Random();
                int grassCount = random.Next(35, 40);
                GenerateGrassAndStones(grassCount, 4);
                if (enemyCount != 0)
                {
                    GenerateRandomEnemies(enemyCount);
                }
                ObjectInitializer();
            }

            public void GenerateGreenObjectsNearTrees(Tile tile)
            {
                ChangeTileVectorAdvanced(tile, -6, -5, 5, 6, true, false);
                string objectType = GenerateRandomString(grassModels);
                GenerateGrass(objectType, tile.position);
            }

            public void SetInitialProbabilities(List<Tile> tileList)
            {
                double initialProbability = 1.0 / tileList.Count;
                for (int i = 0; i < tileList.Count; i++)
                {
                    Tile tile = tileList[i];
                    tile.probability = initialProbability;
                    tileList[i] = tile;
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

            public static Tile GenerateRandomTileWithProbability(List<Tile> tiles)
            {
                Random random = new Random();

                double totalProbability = tiles.Sum(tile => tile.probability);

                double randomNumber = random.NextDouble() * totalProbability;

                // Znalezienie elementu, którego prawdopodobieństwo przekracza wylosowaną liczbę
                double cumulativeProbability = 0;

                for (int i = 0; i < tiles.Count; i++)
                {
                    Tile tile = tiles[i];
                    cumulativeProbability += tile.probability;
                    if (randomNumber < cumulativeProbability)
                    {
                        tile.probability = 0;
                        tiles[i] = tile;
                        return tiles[i];
                    }
                }

                // Powinno zwrócić ostatni element, jeśli poprzednie nie spełniły warunku
                return tiles[tiles.Count - 1];
            }

            public void GenerateRandomEnemies(int enemyCount)
            {
                if (this.enemyCount != 0)
                {
                    for (int i = 0; i < this.enemyCount; i++)
                    {
                        string enemyType = GenerateRandomString(enemyTypes);
                        Vector3 groundPosition = ChoosedTileVector();
                        GenerateEnemy(enemyType, groundPosition);
                    }
                }
            }

            public Vector3 ChangeTileVector(Tile tile, double min, double max, bool x, bool z, double min1, double max1)
            {
                if (x)
                {
                    Random random = new Random();
                    float randomNumber = (float)(random.NextDouble() * max + min);
                    tile.position.X += randomNumber;
                }
                if (z)
                {
                    Random random1 = new Random();
                    float randomNumber1 = (float)(random1.NextDouble() * max1 + min1);
                    tile.position.Z += randomNumber1;
                }
                return tile.position;
            }

            public Vector3 ChangeTileVectorAdvanced(Tile tile, double lowest1, double highest1, double lowest2, double highest2, bool x, bool z)
            {
                Random random = new Random();

                // Wylosuj liczbę typu double z zakresu od lowest1 do highest1 lub od lowest2 do highest2
                float number;

                if (random.Next(0, 2) == 0)
                {
                    number = (float)(random.NextDouble() * (Math.Abs(highest1 - (lowest1))) + (lowest1));
                }
                else
                {
                    number = (float)(random.NextDouble() * (Math.Abs(highest2 - lowest2)) + lowest2);
                }

                if (x)
                {
                    tile.position.X += number;
                }
                if (z)
                {
                    tile.position.Z += number;
                }
                return tile.position;
            }
            public Vector3 ChoosedTileVector()
            {
                Tile tile = GenerateRandomTileWithProbability(groundTiles);
                UpdateProbability(groundTiles, tile);
                Vector3 position = ChangeTileVector(tile, -6, 6, true, true, -5, 5);
                return position;
            }

            public static void UpdateProbability(List<Tile> tiles, Tile selectedTile)
            {
                double changeFactor = 0.7;

                for (int i = 0; i < tiles.Count; i++)
                {
                    Tile tile = tiles[i];

                    if (Distance(tile.position, selectedTile.position, 6))
                    {
                        tile.probability *= changeFactor;
                    }
                    else if (Distance(tile.position, selectedTile.position, 12))
                    {
                        tile.probability *= 0.3;
                    } 
                    else
                    {
                        tile.probability *= (1 + changeFactor);
                    }

                    tiles[i] = tile; // Zapisz zmieniony element z powrotem do listy
                }

                double totalProbability = tiles.Sum(tile => tile.probability);
                // Znormalizowanie prawdopodobieństw, aby suma wynosiła 1
                for (int i = 0; i < tiles.Count; i++)
                {
                    Tile tile = tiles[i];
                    tile.probability /= totalProbability;
                    tiles[i] = tile; // Zapisz zmieniony element z powrotem do listy
                }
            }

            public static bool Distance(Vector3 vector1, Vector3 vector2, float distance)
            {
                float deltaX = Math.Abs(vector1.X - vector2.X);
                float deltaZ = Math.Abs(vector1.Z - vector2.Z);

                if (deltaX <= distance && deltaZ <= distance)
                {
                    return true;
                }
                else
                    return false;
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

            public void GenerateGrass(string objectType, Vector3 groundPosition)
            {
                Random rand = new Random();
                float scale = (float)rand.Next(200, 250) / 100;
                float rflot = (float)rand.NextDouble() * 2 * (float)Math.PI; //zmiana obrotu losowo
                string texture;
                switch (objectType)
                {
                    case "Objects/big_grass":
                        texture = GenerateRandomString(bigGrassTextures);
                        SceneObject big_grass = new SceneObject(groundPosition, "Objects/big_grass", texture);
                        big_grass.SetScale(scale/2);
                        big_grass.SetRotation(new Vector3(0, rflot, 0));
                        nonColideObjects.Add(big_grass);
                        break;
                    case "Objects/grass1":
                        texture = GenerateRandomString(grassTextures);
                        SceneObject grass1 = new SceneObject(groundPosition, "Objects/grass1", texture);
                        grass1.SetScale(scale);
                        grass1.SetRotation(new Vector3(0, rflot, 0));
                        nonColideObjects.Add(grass1);
                        break;
                    case "Objects/grass2":
                        texture = GenerateRandomString(grassTextures);
                        SceneObject grass2 = new SceneObject(groundPosition, "Objects/grass2", texture);
                        grass2.SetScale(scale);
                        grass2.SetRotation(new Vector3(0, rflot, 0));
                        nonColideObjects.Add(grass2);
                        break;
                    case "Objects/grass3":
                        texture = GenerateRandomString(grassTextures);
                        SceneObject grass3 = new SceneObject(groundPosition, "Objects/grass3", texture);
                        grass3.SetScale(scale);
                        grass3.SetRotation(new Vector3(0, rflot, 0));
                        nonColideObjects.Add(grass3);
                        break;
                    case "Objects/three_grass":
                        texture = GenerateRandomString(grassTextures);
                        SceneObject grass4 = new SceneObject(groundPosition, "Objects/three_grass", texture);
                        grass4.SetScale(scale);
                        grass4.SetRotation(new Vector3(0, rflot, 0));
                        nonColideObjects.Add(grass4);
                        break;
                }
            }

            public void GenerateOtherObjects(string objectType, Vector3 groundPosition)
            {
                Random rand = new Random();
                float scale = (float)rand.Next(200, 250) / 100;
                float rflot = (float)rand.NextDouble() * 2 * (float)Math.PI; //zmiana obrotu losowo
                string texture;
                switch (objectType)
                {
                    case "Objects/big_stone":
                        texture = "Textures/stone";
                        SceneObject big_stone = new SceneObject(groundPosition, "Objects/big_stone", texture);
                        big_stone.SetScale(scale/2);
                        big_stone.SetRotation(new Vector3(0, rflot, 0));
                        _sceneObjects.Add(big_stone);
                        break;
                    case "Objects/small_stone":
                        texture = "Textures/stone";
                        SceneObject small_stone = new SceneObject(groundPosition, "Objects/small_stone", texture);
                        small_stone.SetScale(scale);
                        small_stone.SetRotation(new Vector3(0, rflot, 0));
                        nonColideObjects.Add(small_stone);
                        break;
                    case "Objects/two_stones":
                        texture = "Textures/stone";
                        SceneObject two_stones = new SceneObject(groundPosition, "Objects/two_stones", texture);
                        two_stones.SetScale(scale);
                        two_stones.SetRotation(new Vector3(0, rflot, 0));
                        _sceneObjects.Add(two_stones);
                        break;
                    case "Objects/bush":
                        texture = GenerateRandomString(bushTextures);
                        SceneObject bush = new SceneObject(groundPosition, "Objects/bush", texture);
                        bush.SetScale(scale/2);
                        _sceneObjects.Add(bush);
                        break;
                    case "Objects/small_bush":
                        texture = GenerateRandomString(bushTextures);
                        SceneObject small_bush = new SceneObject(groundPosition, "Objects/small_bush", texture);
                        small_bush.SetScale(scale);
                        _sceneObjects.Add(small_bush);
                        break;
                    case "Objects/flower":
                        texture = GenerateRandomString(flowerTextures);
                        SceneObject flower = new SceneObject(groundPosition, "Objects/flower", texture);
                        flower.SetScale(scale);
                        nonColideObjects.Add(flower);
                        break;
                }
            }
            public void GenerateGrassAndStones(int grassCount, int otherCount)
            {
                SetInitialProbabilities(groundTiles);
                if (grassCount != 0)
                {
                    for (int i = 0; i < grassCount; i++)
                    {
                        Vector3 groundPosition = ChoosedTileVector();

                        string objectType = GenerateRandomString(grassModels);
                        GenerateGrass(objectType, groundPosition);
                    }
                }

                if (otherCount != 0)
                {
                    for (int i = 0; i < otherCount; i++)
                    {

                        Vector3 groundPosition = ChoosedTileVector();

                        string objectType = GenerateRandomString(otherSmallModels);
                        GenerateOtherObjects(objectType, groundPosition);
                    }
                }
            }
        }



    }
}
