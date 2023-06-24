using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Numerics;
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
        private List<SceneObject> nonCollideArray;
        private List<Vector2> enemiesCollidersArray;
        private Levels level;
        private List<Enemy> enemies;
        private bool ifPlayerCloseToShop;
        private bool ifPlayerPartyModule;
        public World()
        {
            _worldArray = new List<SceneObject>();
            nonCollideArray = new List<SceneObject>();
            enemiesCollidersArray = new List<Vector2>();
            level = new Levels();
            enemies = new List<Enemy>();
        }

        public void LoadContent()
        {
            level.LoadContent();
        }

        public void Draw(Vector3 player)
        {
            _worldArray = level.returnSceneObjects(player.X, player.Z);
            nonCollideArray = level.returnNonCollideSceneObjects(player.X, player.Z);
            level.setTransparentTrees(player);
            foreach (SceneObject sceneObject in _worldArray)
            {
                if (sceneObject.transparency)
                {
                    sceneObject.DrawTransparency(player);
                }
                else
                {
                    sceneObject.Draw(player);
                }
            }
            foreach (SceneObject sceneObject in nonCollideArray)
            {
                sceneObject.Draw(player);
            }


        }

        public void PrepareRandomMap(float playerX, float playerZ)
        {
            level.prepareRandomMap(playerX, playerZ);
        }

        public List<Enemy> returnEnemiesList(float playerX, float playerZ)
        {
            enemies = level.returnEnemiesList(playerX, playerZ);
            return enemies;
        }

        public List<SceneObject> GetWorldList()
        {
            return _worldArray;
        }
        
        public List<Vector2> GetEnemiesColliders(float playerX, float playerZ)
        {
            enemiesCollidersArray = level.returnEnemiesColliders(playerX, playerZ);
            return enemiesCollidersArray;
        }

        public bool ifPlayerIsCloseToShop(Microsoft.Xna.Framework.Vector3 playerPosition)
        {
            Vector3 newPlayerPosition = new Vector3(playerPosition.X, playerPosition.Y, playerPosition.Z);
            ifPlayerCloseToShop = level.ifPlayerIsCloseToShop(newPlayerPosition);
            return ifPlayerCloseToShop;
        }

        private bool ifPlayerOnPartyModule(Microsoft.Xna.Framework.Vector3 playerPosition)
        {
            Vector3 newPlayerPosition = new Vector3(playerPosition.X, playerPosition.Y, playerPosition.Z);
            ifPlayerPartyModule = level.ifPlayerOnPartyModule(newPlayerPosition);
            return ifPlayerPartyModule;
        }

        private Vector2 ConvertToXnaVector2(System.Numerics.Vector2 systemVector2)
        {
            return new Vector2(systemVector2.X, systemVector2.Y);
        }

    }
}
