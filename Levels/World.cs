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

        public World(ContentManager Content)   
        {
            _worldArray = new List<SceneObject>();
            level = new Levels(Content, 4);
            enemies = new List<Enemy>();
        }

        public void Draw(EffectHandler effectHandler, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix, float playerX)
        {
            _worldArray = level.returnSceneObjects(playerX);
            foreach (SceneObject sceneObject in _worldArray)
            {
                sceneObject.Draw(effectHandler, worldMatrix, viewMatrix, projectionMatrix, sceneObject.color);
            }
            //enemies = level.returnEnemiesList(playerX);
        }

        public List<Enemy> returnEnemiesList(float playerX)
        {
            enemies = level.returnEnemiesList(playerX);
            return enemies;
        }

        public List<SceneObject> GetWorldList()
        {
            return _worldArray;
        }
        
        
    }
}
