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
        private List<Enemy> enemies;

        public World()   
        {
            _worldArray = new List<SceneObject>();
            level = new Levels(100);
            enemies = new List<Enemy>();
        }

        public void LoadContent()
        {
            level.LoadContent();
        }

        public void Draw(EffectHandler effectHandler, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix, float playerX, float playerZ)
        {
            _worldArray = level.returnSceneObjects(playerX, playerZ);
            foreach (SceneObject sceneObject in _worldArray)
            {
                sceneObject.Draw(effectHandler, worldMatrix, viewMatrix, projectionMatrix, sceneObject.color);
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
        
        
    }
}
