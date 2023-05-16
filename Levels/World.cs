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
            level = new Levels(Content, 100);
            enemies = new List<Enemy>();
        }

        public void MainDraw(EffectHandler effectHandler, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix, float playerX, float playerZ,Vector3 Lightpos)
        {
            _worldArray = level.returnSceneObjects(playerX, playerZ);
            foreach (SceneObject sceneObject in _worldArray)
            {
                sceneObject.MainDraw(effectHandler, worldMatrix, viewMatrix, projectionMatrix, sceneObject.color,Lightpos);
            }
            
        }

        public List<Enemy> returnEnemiesList(float playerX, float playerZ)
        {
            enemies = level.returnEnemiesList(playerX, playerZ);
            //Debug.Write(enemies.Count + "\n");
            return enemies;
        }

        public List<SceneObject> GetWorldList()
        {
            return _worldArray;
        }
        
        
    }
}
