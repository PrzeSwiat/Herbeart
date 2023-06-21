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
        List<int> modulesWithParty;
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
            modulesWithParty = new List<int>();

        }

        public void LoadContent()
        {
            prepareFirstLevels();
        }

        public void prepareFirstLevels()
        {
            //Moduł 1 - z salą weselną
            prepareModule("Maps/map1.txt", 0, 0);
            

            //Moduł 2
            prepareModule("Maps/map2.txt", 0, 0);
            //_levels[1].GenerateEnemy(mintEnemy, new Vector3(moduleSeparatorX + 30, 0, 80));
            //_levels[1].GenerateEnemy(mintEnemy, new Vector3(moduleSeparatorX + 30, 0, 50));
            _levels[1].GenerateEnemy(nettleEnemy, new Vector3(moduleSeparatorX + 30, 0, 50));

            //Moduł 3
            prepareModule("Maps/map3.txt", 0, 0);
            _levels[2].GenerateEnemy(mintEnemy, new Vector3(2 * moduleSeparatorX + 30, 0, 50));
            _levels[2].GenerateEnemy(mintEnemy, new Vector3(2 * moduleSeparatorX + 30, 0, 55));
            _levels[2].GenerateEnemy(nettleEnemy, new Vector3(2 * moduleSeparatorX + 30, 0, 60));

            currentMap = "Maps/map3.txt";
            numberOfModules = 3;

        }

        public (int, int) setEnemiesCount()
        {
            int enemyCount;
            int difficultyLevel;        //od 4 są normalne, do 4 są tutorialowe ze spacjalnymi ograniczeniami
            if (numberOfModules == 4)
            {
                enemyCount = 6;
                difficultyLevel = 1;
            }
            else if (numberOfModules <= 8)
            {
                enemyCount = 7;
                difficultyLevel = 2;
            }
            else if (numberOfModules <= 11)
            {
                enemyCount = 8;
                difficultyLevel = 3;    //bez zmian prawdopodobienstwa
            }
            else if (numberOfModules <= 15)
            {
                enemyCount = 10;
                difficultyLevel = 4;
            }
            else
            {
                int moduleIncrements = (numberOfModules - 15) / 5;
                enemyCount = 9 + (moduleIncrements * 8);
                difficultyLevel = 4 + moduleIncrements;
                if (difficultyLevel > 7)
                {
                    difficultyLevel = 7;
                }
            }
            return (enemyCount, difficultyLevel);
        }

        public void prepareMap()
        {
            numberOfModules++;

            
            maps = new List<string>();

            //wybieranie ilości przeciwników w levelu
            var result = setEnemiesCount();
            int enemyCount = result.Item1;
            int difficultyLevel = result.Item2;


            //Przypadek prostych map - wylot z lewej i prawej
            if (currentMap == "Maps/map1.txt" || maps_straight.Contains(currentMap))
            {

                if (numberOfModules == 5 || numberOfModules % 15 == 0)
                {
                    modulesWithParty.Add(numberOfModules - 1);
                    //maps.Add("Maps/map_party_straight.txt");
                    maps.Add("Maps/map_party_left_up.txt");
                    maps.Add("Maps/map_party_left_down.txt");
                    string map = generateRandomStringFromList(maps);
                    prepareModule(map, 0, 0);
                    switch (map)
                    {
/*                        case "Maps/map_party_straight.txt":
                            choosedMap = "Maps/map_2.txt";
                            break;*/
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
                    prepareRandomModule(enemyCount, difficultyLevel);
                }
            }

            //Zakręt do góry - wylot z lewej i góry
            if (maps_left_up.Contains(currentMap))
            {
                moduleSeparatorZCount--;
                moduleHeightChange++;
                if (numberOfModules == 5 || numberOfModules % 15 == 0)
                {
                    modulesWithParty.Add(numberOfModules - 1);
                    maps.Add("Maps/map_party_down_up.txt");
                    maps.Add("Maps/map_party_down_right.txt");
                    string map = generateRandomStringFromList(maps);
                    prepareModule(map, 0, 0);
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
                    prepareRandomModule(enemyCount, difficultyLevel);
                }

            }

            //Zakręt od dołu w prawo - wylot z dołu i po prawo
            if (maps_down_right.Contains(currentMap))
            {
                if(numberOfModules == 5 || numberOfModules % 15 == 0)
                {
                    modulesWithParty.Add(numberOfModules);
                    //maps.Add("Maps/map_party_straight.txt");
                    maps.Add("Maps/map_party_left_up.txt");
                    string map = generateRandomStringFromList(maps);
                    prepareModule(map, 0, 0);
                    switch (map)
                    {
/*                        case "Maps/map_party_straight.txt":
                            choosedMap = "Maps/map_2.txt";
                            break;*/
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
                    prepareRandomModule(enemyCount, difficultyLevel);
                }
            }

            //Zakręt w dół - wylot z lewej i z dołu
            if (maps_left_down.Contains(currentMap))
            {
                moduleSeparatorZCount++;
                moduleHeightChange++;
                if (numberOfModules == 5 || numberOfModules % 15 == 0)
                {
                    modulesWithParty.Add(numberOfModules - 1);
                    maps.Add("Maps/map_party_up_right.txt");
                    maps.Add("Maps/map_party_up_down.txt");
                    string map = generateRandomStringFromList(maps);
                    prepareModule(map, 0, 0);
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
                    prepareRandomModule(enemyCount, difficultyLevel);
                }
            }

            //Wylot z góry i z prawej
            if (maps_up_right.Contains(currentMap))
            {
                if (numberOfModules == 5 || numberOfModules % 15 == 0)
                {
                    modulesWithParty.Add(numberOfModules - 1);
                    //maps.Add("Maps/map_party_straight.txt");
                    maps.Add("Maps/map_party_left_up.txt");
                    maps.Add("Maps/map_party_left_down.txt");
                    string map = generateRandomStringFromList(maps);
                    prepareModule(map, 0, 0);
                    switch (map)
                    {
/*                        case "Maps/map_party_straight.txt":
                            choosedMap = "Maps/map_2.txt";
                            break;*/
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
                    prepareRandomModule(enemyCount, difficultyLevel);
                }
            }
            //Prosta od góry do dołu              //work in progress
            if (maps_up_down.Contains(currentMap))
            {
                moduleSeparatorZCount++;
                moduleHeightChange++;
                if (numberOfModules == 5 || numberOfModules % 15 == 0)
                {
                    modulesWithParty.Add(numberOfModules - 1);
                    prepareModule("Maps/map_party_up_right.txt", 0, 0);
                    choosedMap = "Maps/map_up_right.txt";
                }
                else
                {
                    maps.AddRange(maps_up_right);
                    prepareRandomModule(enemyCount, difficultyLevel);
                }

            }

            //Prosta od dołu do góry
            if (maps_down_up.Contains(currentMap))
            {
                moduleSeparatorZCount--;
                moduleHeightChange++;
                if (numberOfModules == 5 || numberOfModules % 15 == 0)
                {
                    modulesWithParty.Add(numberOfModules - 1);
                    prepareModule("Maps/map_party_down_right.txt", 0, 0);
                    choosedMap = "Maps/map_down_right.txt";
                }
                else
                {
                    maps.AddRange(maps_down_right);
                    prepareRandomModule(enemyCount, difficultyLevel);
                }

            }

            currentMap = choosedMap;
        }

        public void prepareRandomModule(int enemyCount, int difficultyLevel)
        {
            choosedMap = generateRandomStringFromList(maps);

            prepareModule(choosedMap, enemyCount, difficultyLevel);

            maps.Clear();

        }

        public void prepareModule(string map, int enemyCount, int difficultyLevel)
        {
            module = new Rectangle((_levels.Count - moduleHeightChange) * moduleSeparatorX, moduleSeparatorZCount * moduleSeparatorZ, moduleSeparatorX, moduleSeparatorZ);
            Level level = new Level(map, (_levels.Count - moduleHeightChange) * moduleSeparatorX, moduleSeparatorZCount * moduleSeparatorZ, enemyCount, difficultyLevel);
            level.LoadContent();
            _levels.Add(level);
            modulesList.Add(module);
        }

        public List<Vector2> returnEnemiesColliders(float playerX, float playerY)
        {
            List<Vector2> result = new List<Vector2>();

            int numberOfModule = returnModuleNumber(playerX, playerY);

            for (int i = numberOfModule - 1; i <= numberOfModule + 1; i++)
            {
                if (i >= 0 && i < _levels.Count - 1)
                {
                    foreach (Vector2 vector in _levels[i].returnEnemiesColliders())
                    {
                        result.Add(vector);
                    }
                }

            }
            return result;
        }

        public bool ifPlayerOnPartyModule(float playerX, float playerY)
        {
            int numberOfModule = returnModuleNumber(playerX, playerY);
            if (modulesWithParty.Contains(numberOfModule) && !visited.Contains(modulesList[numberOfModule]))
            {
                return true;
            }
            else
            {
                return false;
            }
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

        public void prepareRandomMap(float playerX, float playerY)
        {
            int numberOfModule = returnModuleNumber(playerX, playerY);


            if (numberOfModule == numberOfModules - 2)
            {
                prepareMap();
            }
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

            if (numberOfModule == 1)
                Globals.Module2 = true;
            if (numberOfModule == 2)
                Globals.Module3 = true;
            if (numberOfModule == 3)
                Globals.Module4 = true;
            if (numberOfModule == 4)
                Globals.Module5 = true;
            if (numberOfModule == 5)
                Globals.Module6 = true;
            if (numberOfModule == 8)
                Globals.Module9 = true;

            return numberOfModule;
        }

        public string generateRandomStringFromList(List<string> list)
        {
            Random random = new Random();
            string randomString = list[random.Next(list.Count)];
            return randomString;
        }




    }
}
