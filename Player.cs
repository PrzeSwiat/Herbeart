using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;

namespace TheGame
{
    [Serializable]
    internal class Player
    {
        public Vector3 position;
        public Vector3 rotation = new Vector3(0.0f, 0.0f, 0.0f);
        public float scale = 0.5f;
        private Model model;
        private Texture2D texture2D;
        public string _modelFileName;
        public string _textureFileName;
        private float movementSpeedUpDown;
        private float movementSpeedRightLeft = 0.15f;

        public Player( Vector3 Position, string modelFileName, string textureFileName)
        {
            position = Position;
            _modelFileName = modelFileName;
            _textureFileName = textureFileName;
            movementSpeedUpDown = 2 * movementSpeedRightLeft;
        }
        public void LoadContent(ContentManager content)
        {
            model = (content.Load<Model>(_modelFileName));
            texture2D = (content.Load<Texture2D>(_textureFileName));
        }
        //GET'ERS
        public float GetmovementSpeedUpDown()
        {
            return movementSpeedUpDown;
        }
        public float GetmovementSpeedRightLeft()
        {
            return movementSpeedRightLeft;
        }
        public Vector3 GetPosition()
        {
            return position;
        }
        public Vector3 GetRotation()
        {
            return rotation;
        }
        public float GetScale()
        {
            return scale;
        }
        public Model GetModel()
        {
            return model;
        }
        public Texture2D GetTexture2D()
        {
            return texture2D;
        }
        public string GetModelFileName()
        {
            return _modelFileName;
        }
        public string GetTextureFileName()
        {
            return _textureFileName;
        }
        //.................

        //SET'ERS
        public void SetmovementSpeedUpDown(float speed)
        {
            movementSpeedUpDown = speed;
        }
        public void SetmovementSpeedRightLeft(float speed)
        {
            movementSpeedRightLeft = speed;
        }
        public void SetPosition(float x, float y, float z)
        {
            position = new Vector3(x, y, z);
        }
        public void SetPosition(Vector3 pos)
        {
            position = pos;
        }
        public void SetRotation(Vector3 rot)
        {
            rotation = rot;
        }
        public void SetRotation(float x, float y, float z)
        {
            rotation = new Vector3(x, y, z);
        }
        public void SetScale(float scl)
        {
            scale = scl;
        }
        public void SetModel(Model mod)
        {
            model = mod;
        }
        public void SetTexture(Texture2D tex)
        {
            texture2D = tex;
        }

        //.................


        public void PlayerMovement(World world, float cosAngle,float sinAngle,float tanAngle)
        {
            //GameControler 
            GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);
            if(capabilities.IsConnected)
            {
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                if (capabilities.HasLeftXThumbStick)
                {
                    
                    if (gamePadState.ThumbSticks.Left.X < -0.5f)
                    {
                        
                       SetPosition(position - new Vector3(movementSpeedRightLeft, 0, -movementSpeedRightLeft * cosAngle));
                    }
                    if (gamePadState.ThumbSticks.Left.X > 0.5f)
                    {
                        SetPosition(position - new Vector3(-movementSpeedRightLeft, 0, movementSpeedRightLeft * cosAngle));
                    }
                    if (gamePadState.ThumbSticks.Left.Y > 0.5f)
                    {
                        SetPosition(position - new Vector3(movementSpeedUpDown / tanAngle, 0, movementSpeedUpDown));
                    }
                    if (gamePadState.ThumbSticks.Left.Y < -0.5f)
                    {
                        SetPosition(position - new Vector3(-movementSpeedUpDown / tanAngle, 0, -movementSpeedUpDown));
                    }

                    // UPOŚLEDZONY RUCH KAMERĄ LEFT FUKIN THUMBSTICK
                    
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == 1 && Math.Round(gamePadState.ThumbSticks.Left.Y) == -1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 1 / 4, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == 1 && Math.Round(gamePadState.ThumbSticks.Left.Y) == 0)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 1 / 2, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == 1 && Math.Round(gamePadState.ThumbSticks.Left.Y) == 1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 3 / 4, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == 0 && Math.Round(gamePadState.ThumbSticks.Left.Y) == 1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == -1 && Math.Round(gamePadState.ThumbSticks.Left.Y) == 1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 5 / 4, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == -1 && Math.Round(gamePadState.ThumbSticks.Left.Y) == 0)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 3 / 2, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == -1 && Math.Round(gamePadState.ThumbSticks.Left.Y) == -1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI*7/4, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == 0 && Math.Round(gamePadState.ThumbSticks.Left.Y) == -1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 0, 0);
                    }
                    //////////////////????////////////////////////////////////////
                    ///LEFT THUMBSTICK
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == 1 && Math.Round(gamePadState.ThumbSticks.Right.Y) == -1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 1 / 4, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == 1 && Math.Round(gamePadState.ThumbSticks.Right.Y) == 0)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 1 / 2, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == 1 && Math.Round(gamePadState.ThumbSticks.Right.Y) == 1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 3 / 4, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == 0 && Math.Round(gamePadState.ThumbSticks.Right.Y) == 1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == -1 && Math.Round(gamePadState.ThumbSticks.Right.Y) == 1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 5 / 4, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == -1 && Math.Round(gamePadState.ThumbSticks.Right.Y) == 0)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 3 / 2, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == -1 && Math.Round(gamePadState.ThumbSticks.Right.Y) == -1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 7 / 4, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == 0 && Math.Round(gamePadState.ThumbSticks.Right.Y) == -1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 0, 0);
                    }
                    /////////////
                }
            }


            //Keyboard
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.A))
            {
                SetPosition(position - new Vector3(movementSpeedRightLeft, 0, -movementSpeedRightLeft * cosAngle));
               SetRotation(0, -cosAngle, 0);
                //Camera movement acccording to player A move 

            }
            if (state.IsKeyDown(Keys.D))
            {

                SetPosition(position - new Vector3(-movementSpeedRightLeft, 0, movementSpeedRightLeft * cosAngle));
                SetRotation(0, cosAngle*3, 0);
                //Camera movement acccording to player D move 
            }
            if (state.IsKeyDown(Keys.W))
            {

                SetPosition(position - new Vector3(movementSpeedUpDown / tanAngle, 0, movementSpeedUpDown));
                SetRotation(0, cosAngle * 5.5f, 0);
                //Camera movement acccording to player W move 
            }
            if (state.IsKeyDown(Keys.S))
            {

                SetPosition(position - new Vector3(-movementSpeedUpDown / tanAngle, 0, -movementSpeedUpDown));
                SetRotation(0, cosAngle, 0);
                //Camera movement acccording to player S move 
            }

        }

        

    }
}
