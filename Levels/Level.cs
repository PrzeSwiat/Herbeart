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
    internal class Level
    {

        #region Models
        private string[] treeModels = { "Objects/drzewo1", "Objects/drzewo2", "Objects/drzewo3" };
        private string[] grassModels = { "Objects/big_grass", "Objects/grass1", "Objects/grass2", "Objects/grass3", "Objects/three_grass" };
        private string[] otherSmallModels = { "Objects/big_stone", "Objects/small_stone", "Objects/two_stones", "Objects/bush", "Objects/small_bush", "Objects/flower", "Objects/flower", "Objects/flower" };
        #endregion
        #region Textures
        private string[] bushTextures = { "Textures/bush1", "Textures/bush2" };
        private string[] flowerTextures = { "Textures/flower1", "Textures/flower2" };
        private string[] bigGrassTextures = { "Textures/big_grass1", "Textures/big_grass2" };
        private string[] grassTextures = { "Textures/grass1", "Textures/grass2", "Textures/grass3", "Textures/grass4", "Textures/grass5", "Textures/grass6" };
        #endregion
        #region TileLists
        private List<Tile> groundTiles;
        private List<Tile> forestTiles;
        private List<Tile> spawnTiles;
        private List<Tile> borderTiles;
        #endregion
        private List<SceneObject> _sceneObjects;
        private List<SceneObject> nonColideObjects;
        private int tileSize = 6;
        private int moduleWidth = 26;
        private int moduleHeight = 26;
        #region Enemies
        private List<Enemy> enemies;
        private List<Vector2> enemiesColliders;       //lista drzew z ktorymi mają kolidować przeciwnicy
        private int enemyCount;
        private EnemiesGenerator enemiesGenerator;
        private int difficultyLevel;
        #endregion
        private string fileName;
        private float separatorX, separatorZ;




        public Level(string fileName, float separatorX, float separatorZ, int enemyCount, int difficultyLevel)
        {
            _sceneObjects = new List<SceneObject>();
            nonColideObjects = new List<SceneObject>();
            enemies = new List<Enemy>();
            enemiesGenerator = new EnemiesGenerator();
            enemiesColliders = new List<Vector2>();       
            groundTiles = new List<Tile>();
            forestTiles = new List<Tile>();
            spawnTiles = new List<Tile>();
            borderTiles = new List<Tile>();

            this.fileName = fileName;
            this.separatorX = separatorX;
            this.separatorZ = separatorZ;
            this.enemyCount = enemyCount;
            this.difficultyLevel = difficultyLevel;

        }

        public void LoadContent()
        {
            LoadScene();
        }

        public List<Enemy> returnEnemies()
        {
            return enemies;
        }

        public List<Vector2> returnEnemiesColliders()
        {
            return enemiesColliders;
        }

        public struct Tile
        {
            public string groundType;
            public float height;
            public Vector3 position;
            public double probability;

            public Tile(string groundType, float height, Vector3 position, double probability)
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
                        enemiesColliders.Add(new Vector2(newVector1.X, newVector1.Z));
                        x++;
                        break;
                    case 50:
                        groundType = "forest_border_without_obj";
                        height = 0.0f;
                        Vector3 wektor5 = new Vector3(x * tileSize + separatorX, height, z * tileSize + separatorZ);
                        Tile tile5 = new Tile(groundType, height, wektor5, 0);
                        forestTiles.Add(tile5);
                        Vector3 newVector5 = ChangeTileVector(tile5, -2, 2, true, true, -1.5, 1.5);
                        GenerateForest(newVector5, 150, 220);
                        enemiesColliders.Add(new Vector2(newVector5.X, newVector5.Z));
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
                    case 99:
                        groundType = "party";
                        height = 0.0f;
                        Vector3 wektor4 = new Vector3(x * tileSize + separatorX, height, z * tileSize + separatorZ);
                        SceneObject plotek = new SceneObject(wektor4, "Objects/festiwal", "Textures/festiwal");
                        nonColideObjects.Add(plotek);
                        //spawnTiles.Add(new Tile(groundType, height, wektor4, 0));
                        x++;
                        break;
                }

            }
            _sceneObjects.Add(new SceneObject(groundPos, "Objects/ground1", "Textures/tekstura_wielkiego_tila2"));
            Random random = new Random();
            int grassCount = random.Next(35, 40);
            GenerateGrassAndStones(grassCount, 4);
            GenerateRandomEnemies();
            ObjectInitializer();
        }

        public void GenerateRandomEnemies()
        {
            if (enemyCount != 0)
            {
                List<Vector3> enemiesPositions = new List<Vector3>();
                for (int i = 0; i < enemyCount; i++)
                {
                    Vector3 enemyPosition = ChoosedTileVector();
                    enemiesPositions.Add(enemyPosition);
                }
                enemiesGenerator.GenerateRandomEnemies(enemiesPositions, difficultyLevel);
                enemies.AddRange(enemiesGenerator.returnEnemies());
            }
        }

        public void GenerateEnemy(string enemyType, Vector3 enemyPosition)
        {
            enemiesGenerator.GenerateEnemy(enemyType, enemyPosition);
            enemies.AddRange(enemiesGenerator.returnEnemies());
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
                    big_grass.SetScale(scale / 2);
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
                    big_stone.SetScale(scale / 2);
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
                    bush.SetScale(scale / 2);
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
