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
        private List<MapType> maps;
        private MapType currentMap;
        private MapType choosedMap;

        #region Module parametrs and rectangles
        private int numberOfModules;
        private int moduleSeparatorX = 156;
        private int moduleSeparatorZ = 156;
        private int moduleSeparatorZCount = 0;
        private int moduleHeightChange = 0;
        List<Rectangle> modulesList;
        Rectangle module;
        HashSet<Rectangle> visited;
        List<int> modulesWithParty;
        List<Vector3> shopsOnPartyModules;
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
            shopsOnPartyModules = new List<Vector3>();
            MapType.SetInitialProbabilities();
            
        }

        public void LoadContent()
        {
            prepareFirstLevels();
        }

        public void prepareFirstLevels()
        {
            //Moduł 1 - z salą weselną
            prepareModule(MapType.Map_spawn, 0, 0);
            currentMap = MapType.Map_straight_2;
            numberOfModules = 0;
            

        }

        public (int, int) setEnemiesCount()
        {
            int enemyCount;
            int difficultyLevel;        //od 4 są normalne, do 4 są tutorialowe ze spacjalnymi ograniczeniami
            if (numberOfModules == 1)
            {
                enemyCount = 2; 
                difficultyLevel = 8;
            }
            else if (numberOfModules == 2)
            {
                enemyCount = 4;
                difficultyLevel = 8;
            }
            else if (numberOfModules == 3)
            {
                enemyCount = 6;
                difficultyLevel = 1;
            }
            else if (numberOfModules <= 7)
            {
                enemyCount = 7;
                difficultyLevel = 2;
            }
            else if (numberOfModules <= 10)
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

        public bool partyFrequency(int numberOfModules)
        {
            if (numberOfModules == 4)
            {
                return true;
            } 
            else if (numberOfModules < 20 && numberOfModules > 4 && (numberOfModules - 4) % 6 == 0) 
            {
                return true;
            }
            else if(numberOfModules >= 20 && (numberOfModules - 4) % 8 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void prepareMap()
        {
            numberOfModules++;
            
            maps = new List<MapType>();

            //wybieranie ilości przeciwników w levelu
            var result = setEnemiesCount();
            int enemyCount = result.Item1;
            int difficultyLevel = result.Item2;

            bool ifParty = partyFrequency(numberOfModules);

            switch (currentMap.type)
            {
                case "straight":        //Przypadek prostych map - wylot z lewej i prawej
                    if (ifParty)
                    {
                        modulesWithParty.Add(numberOfModules);
                        maps.Add(MapType.Map_party_left_up);
                        maps.Add(MapType.Map_party_left_down);
                        MapType map = MapType.GenerateRandomMap(maps);
                        prepareModule(map, 0, 0);
                        maps.Clear();
                    }
                    else
                    {
                        maps.AddRange(MapType.Maps_straight);   //dalej prosto
                        maps.AddRange(MapType.Maps_left_up);    //idziemy do gory
                        maps.AddRange(MapType.Maps_left_down);  //idziemy do dołu
                        maps.Add(MapType.Map_loot_forest_straight_1);
                        maps.Add(MapType.Map_loot_forest_straight_2);
                        prepareRandomModule(enemyCount, difficultyLevel);
                    }
                    break;
                case "left_up":         //Zakręt do góry - wylot z lewej i góry
                    moduleSeparatorZCount--;
                    moduleHeightChange++;
                    if (ifParty)
                    {
                        modulesWithParty.Add(numberOfModules);
                        maps.Add(MapType.Map_party_down_up);
                        maps.Add(MapType.Map_party_down_right);
                        MapType map = MapType.GenerateRandomMap(maps);
                        prepareModule(map, 0, 0);
                        maps.Clear();
                    }
                    else
                    {
                        maps.Add(MapType.Map_down_right_1);     //idziemy od dołu w prawo
                        maps.Add(MapType.Map_down_up_1);
                        prepareRandomModule(enemyCount, difficultyLevel);
                    }
                    break;
                case "down_right":          //Zakręt od dołu w prawo - wylot z dołu i po prawo
                    if (ifParty)
                    {
                        modulesWithParty.Add(numberOfModules);
                        maps.Add(MapType.Map_party_left_up);
                        MapType map = MapType.GenerateRandomMap(maps);
                        prepareModule(map, 0, 0);
                        maps.Clear();
                    }
                    else
                    {
                        maps.AddRange(MapType.Maps_straight);
                        maps.AddRange(MapType.Maps_left_up);
                        prepareRandomModule(enemyCount, difficultyLevel);
                    }
                    break;
                case "left_down":           //Zakręt w dół - wylot z lewej i z dołu
                    moduleSeparatorZCount++;
                    moduleHeightChange++;
                    if (ifParty)
                    {
                        modulesWithParty.Add(numberOfModules);
                        maps.Add(MapType.Map_party_up_right);
                        maps.Add(MapType.Map_party_up_down);
                        MapType map = MapType.GenerateRandomMap(maps);
                        prepareModule(map, 0, 0);
                        maps.Clear();
                    }
                    else
                    {
                        maps.Add(MapType.Map_up_right_1);
                        maps.Add(MapType.Map_up_down_1);
                        prepareRandomModule(enemyCount, difficultyLevel);
                    }
                    break;
                case "up_right":            //Wylot z góry i z prawej
                    if (ifParty)
                    {
                        modulesWithParty.Add(numberOfModules);
                        maps.Add(MapType.Map_party_left_up);
                        maps.Add(MapType.Map_party_left_down);
                        MapType map = MapType.GenerateRandomMap(maps);
                        prepareModule(map, 0, 0);
                        maps.Clear();
                    }
                    else
                    {
                        maps.AddRange(MapType.Maps_straight);   //dalej prosto
                        maps.AddRange(MapType.Maps_left_up);    //idziemy do gory
                        maps.AddRange(MapType.Maps_left_down);  //idziemy do dołu
                        prepareRandomModule(enemyCount, difficultyLevel);
                    }
                    break;
                case "up_down":     //Prosta od góry do dołu 
                    moduleSeparatorZCount++;
                    moduleHeightChange++;
                    if (ifParty)
                    {
                        modulesWithParty.Add(numberOfModules);
                        prepareModule(MapType.Map_party_up_right, 0, 0);
                    }
                    else
                    {
                        maps.Add(MapType.Map_up_right_1);
                        maps.Add(MapType.Map_loot_up_down_1);
                        prepareRandomModule(enemyCount, difficultyLevel);
                    }
                    break;
                case "down_up":     //Prosta od dołu do góry
                    moduleSeparatorZCount--;
                    moduleHeightChange++;
                    if (ifParty)
                    {
                        modulesWithParty.Add(numberOfModules);
                        prepareModule(MapType.Map_party_down_right, 0, 0);
                    }
                    else
                    {
                        maps.Add(MapType.Map_down_right_1);
                        maps.Add(MapType.Map_loot_down_up_1);
                        prepareRandomModule(enemyCount, difficultyLevel);
                    }
                    break;
            }
            currentMap = choosedMap;
        }

        public void prepareRandomModule(int enemyCount, int difficultyLevel)
        {
            MapType map = MapType.GenerateRandomMap(maps);
            prepareModule(map, enemyCount, difficultyLevel);
            maps.Clear();
        }

        public void prepareModule(MapType map, int enemyCount, int difficultyLevel)
        {
            choosedMap = map;
            module = new Rectangle((_levels.Count - moduleHeightChange) * moduleSeparatorX, moduleSeparatorZCount * moduleSeparatorZ, moduleSeparatorX, moduleSeparatorZ);
            Level level = new Level(map.name, (_levels.Count - moduleHeightChange) * moduleSeparatorX, moduleSeparatorZCount * moduleSeparatorZ, enemyCount, difficultyLevel);
            level.LoadContent();
            _levels.Add(level);
            modulesList.Add(module);
            if(!level.shopOnPartyModule.Equals(Vector3.Zero))
            {
                shopsOnPartyModules.Add(level.shopOnPartyModule);
            }
        }

        public List<Vector2> returnEnemiesColliders(float playerX, float playerY)
        {
            List<Vector2> result = new List<Vector2>();

            int numberOfModule = returnModuleNumber(playerX, playerY);

            for (int i = numberOfModule - 1; i <= numberOfModule + 1; i++)
            {
                if (i >= 0 && i < _levels.Count)
                {
                    foreach (Vector2 vector in _levels[i].returnEnemiesColliders())
                    {
                        result.Add(vector);
                    }
                }

            }
            return result;
        }

        public bool ifPlayerIsCloseToShop(Vector3 playerPosition)
        {
            if (shopsOnPartyModules.Count > 0)
            {
                foreach (Vector3 shopVector in shopsOnPartyModules)
                {
                    float distance = Vector3.Distance(playerPosition, shopVector);
                    if (distance < 20.0f)
                    {
                        shopsOnPartyModules.Remove(shopVector);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        public bool ifPlayerOnPartyModule(Vector3 playerPosition)
        {
            int numberOfModule = returnModuleNumber(playerPosition.X, playerPosition.Z);

            //&& !visited.Contains(modulesList[numberOfModule])
            if (modulesWithParty.Contains(numberOfModule)) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<SceneObject> returnSceneObjects(float playerX, float playerZ)
        {
            List<SceneObject> _sceneObjects = new List<SceneObject>();

            int numberOfModule = returnModuleNumber(playerX, playerZ);

            for (int i = numberOfModule - 1; i <= numberOfModule + 1; i++)
            {
                if (i >= 0 && i < _levels.Count)
                {
                    foreach (SceneObject obj in _levels[i].returnSceneObjects())
                    {
                        _sceneObjects.Add(obj);
                    }
                }

            }


            return _sceneObjects;
        }

        public void prepareRandomMap(float playerX, float playerZ)
        {
            int numberOfModule = returnModuleNumber(playerX, playerZ);

            if (numberOfModule == numberOfModules)
            {
                prepareMap();

            }
        }

        public List<SceneObject> returnNonCollideSceneObjects(float playerX, float playerZ)
        {
            List<SceneObject> _sceneObjects = new List<SceneObject>();

            int numberOfModule = returnModuleNumber(playerX, playerZ);
            

            for (int i = numberOfModule - 1; i <= numberOfModule + 1; i++)
            {
                if (i >= 0 && i < _levels.Count)
                {
                    foreach (SceneObject obj in _levels[i].returnNonCollideSceneObjects())
                    {
                        _sceneObjects.Add(obj);
                    }


                }

            }


            return _sceneObjects;
        }

        public void setTransparentTrees(Vector3 playerPosition)
        {

            int numberOfModule = returnModuleNumber(playerPosition.X, playerPosition.Z);


            foreach (SceneObject obj in _levels[numberOfModule].returnTransparentTrees())
            {
                float distance = Microsoft.Xna.Framework.Vector3.Distance(playerPosition, obj.GetPosition());
                if (distance < 10.0f)
                {
                    obj.transparency = true;
                }
                else
                {
                    obj.transparency = false;   
                }
            }

        }
        

        public List<Enemy> returnEnemiesList(float playerX, float playerZ)
        {
            List<Enemy> enemiesList = new List<Enemy>();

            int numberOfModule = returnModuleNumber(playerX, playerZ);

            if (!visited.Contains(modulesList[numberOfModule]) && numberOfModule != modulesList.Count - 1)
            {
                foreach (Enemy enemy in _levels[numberOfModule + 1].returnEnemies())
                {
                    enemiesList.Add(enemy);
                }
                visited.Add(modulesList[numberOfModule]);
            }


            return enemiesList;
        }

        public int returnModuleNumber(float playerX, float playerZ)
        {
            int numberOfModule = 0;

            for (int i = 0; i < modulesList.Count; i++)
            {
                if (modulesList[i].Contains((int)playerX, (int)playerZ))
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
