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
        private List<SceneObject> nonCollideArray;
        private List<Vector2> enemiesCollidersArray;
        private Levels level;
        private List<Enemy> enemies;

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
            foreach (SceneObject sceneObject in _worldArray)
            {
                sceneObject.Draw(player);
            }
            foreach (SceneObject sceneObject in nonCollideArray)
            {
                sceneObject.Draw(player);
            }

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
        
        public List<Vector2> GetEnemiesColliders()
        {
            return enemiesCollidersArray;
        }
        
    }
}
