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
    internal class Player : SceneObject
    {
        private float movementSpeedUpDown;
        private float movementSpeedRightLeft = 0.15f;

        public Player(Vector3 Position, string modelFileName, string textureFileName) : base(Position,modelFileName,textureFileName)
        {
            movementSpeedUpDown = movementSpeedRightLeft * 2;
            SetScale(0.5f);
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
            if(state.GetPressedKeyCount() > 0)
            {
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
}
