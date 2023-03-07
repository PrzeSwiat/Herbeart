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
        private List<SceneObject> _sceneObjectsArray;
        int w;
        int h;
        
        public World(int WindowWidth,int WindowHeight,ContentManager Content,float blockSeparation, float worldWidth, float worldLenght, string modelFileName, string textureFileName, string effectFileName)   //simple creator (develop it later)
        {
            w = WindowWidth;
            h = WindowHeight;
            _sceneObjectsArray = new List<SceneObject>();
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
                    _sceneObjectsArray.Add(new SceneObject(Content, new Vector3(j * blockSeparation, level, i * blockSeparation), modelFileName, textureFileName, effectFileName));
                }
            }
        }

        public void ObjectInitializer(ContentManager Content)
        {
            //_sceneObjectsArray.Add(new SceneObject(Content,new Vector3(0, 0, 2), "test", "StarSparrow_Green", "ShaderOne"));
            //_sceneObjectsArray.Add(new SceneObject(Content,new Vector3(2.15f, 0, 0), "test", "StarSparrow_Green", "ShaderOne"));
        }


        public Matrix WorldMove(Matrix worldMatrix,Vector3 moveVec)
        {
            return worldMatrix * Matrix.CreateTranslation(moveVec);
        }

        public List<SceneObject> GetSceneObjectList()
        {
            return _sceneObjectsArray;
        }
        
        public Matrix MouseMovement(Matrix worldMatrix)
        {
            MouseState state = Mouse.GetState();

            

            if(state.RightButton == ButtonState.Pressed)
            {
                int moveX = 0;
                int moveY = 0;

                if (state.X < (w / 2) && state.Y < (h / 2))
                {
                    moveX = (w/2 - state.X)*2;
                    moveY = (h/2 - state.Y)*2;
                }
                if(state.X < (w / 2) && state.Y > (h / 2))
                {
                    moveX = w / 2 + state.X;
                    moveY = h / 2 - state.Y;
                }
                if (state.X > (w / 2) && state.Y < (h / 2))
                {
                    moveX = w / 2 - state.X;
                    moveY = h / 2 + state.Y;
                }
                if (state.X > (w / 2) && state.Y > (h / 2))
                {
                    moveX = -(w / 2 + state.X)/2;
                    moveY = -(h / 2 + state.Y)/2;
                }

                worldMatrix = WorldMove(worldMatrix, new Vector3((float)moveX/1000, 0, (float)moveY/1000));

            }
            if(state.RightButton == ButtonState.Released)
            {

            }

            return worldMatrix;
        }
    }
}
