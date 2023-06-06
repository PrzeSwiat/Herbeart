﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheGame
{
    public class Globals
    {
        public static ContentManager content;
        public static GraphicsDeviceManager _graphics;

        public static EffectHandler effectHandler;
        public static EffectHandler effectHandler1;
        public static Matrix projectionMatrix;
        public static Matrix viewMatrix;
        public static Matrix worldMatrix;
        
        public static bool Start;
        public static bool Death;
        public static bool Tutorial;
        public static bool Pause;
        public static float time;
        public static int Score;
        public static GamePadState prevState;
        public static KeyboardState prevKeyBoardState;
    }
}
