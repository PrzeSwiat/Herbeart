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
        int w;
        int h;
        
        public World(int WindowWidth,int WindowHeight,ContentManager Content,float blockSeparation, float worldWidth, float worldLenght, string modelFileName, string textureFileName)   //simple creator (develop it later)
        {
            w = WindowWidth;
            h = WindowHeight;
            _worldArray = new List<SceneObject>();
            float _blockSeparation = blockSeparation;
            int _worldWidth = (int)worldWidth / 2;
            int _minusWidth = -_worldWidth;

            int _worldLenght = (int)worldLenght / 2;
            int _minusLenght = -_worldLenght;
            float level = 0.0f;

            for (int i = _minusLenght; i <= _worldLenght; i++)
            {
                for (int j = _minusLenght; j <= _worldWidth; j++)
                {
                    _worldArray.Add(new SceneObject(new Vector3(j * blockSeparation, level, i * blockSeparation), modelFileName, textureFileName));
                    
                }
            }

            foreach (SceneObject obj in _worldArray)
            {
                obj.LoadContent(Content);
            }

            
        }

        public World(ContentManager Content, float blockSeparation, float worldWidth, float worldLenght, List<string> modelFileName, List<string> textureFileName, List<float> level, float separator)   //simple creator (develop it later)
        {
            _worldArray = new List<SceneObject>();
            //float _blockSeparation = blockSeparation;
            int _worldWidth = (int)worldWidth / 2;
            int _minusWidth = -_worldWidth;

            int _worldLenght = (int)worldLenght / 2;
            int _minusLenght = -_worldLenght;
            //float levels = -2.0f;

            for (int i = _minusLenght, a = 0; i <= _worldLenght; i++, a++)
            {
                for (int j = _minusWidth, b = 0; j <= _worldWidth; j++, b++)
                {
/*                    Debug.Write(j * blockSeparation);
                    Debug.Write("\n");
                    Debug.Write(i * blockSeparation);
                    Debug.Write("\n");
                    Debug.Write("\n");*/
                    _worldArray.Add(new SceneObject(new Vector3(j * blockSeparation - separator, level[a * 21 + b], i * blockSeparation), (string)modelFileName[a* 21 + b], (string)textureFileName[a * 21 + b]));

                }
            }

            foreach (SceneObject obj in _worldArray)
            {
                obj.LoadContent(Content);
            }


        }

        public void ObjectInitializer(ContentManager Content)
        {
           // _worldArray.Add(new SceneObject(Content,new Vector3(12, 0, 0), "test", "StarSparrow_Orange"));
            //_worldArray.Add(new SceneObject(new Vector3(10, 1, 0), "test", "StarSparrow_Orange"));

            foreach (SceneObject obj in _worldArray)
            {
                obj.LoadContent(Content);
            }
        }

        public List<SceneObject> GetWorldList()
        {
            return _worldArray;
        }
        
        
    }
}
