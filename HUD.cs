using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class HUD
    {
        string _texFilename;
        public Texture2D skyTex;
        public Rectangle skyRec;
        public int MoveSpeed = 2;

        public HUD(string texFilename, int WindowWidth, int WindowHeight)
        {
            _texFilename = texFilename;
            skyRec = new Rectangle(0,0, WindowWidth * 3, WindowHeight * 3);
        }

        public void LoadContent(ContentManager content)
        {
            skyTex = (content.Load<Texture2D>(_texFilename));
        }

        public void Move()
        {
            GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);
            if (capabilities.IsConnected)
            {
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                if (capabilities.HasLeftXThumbStick)
                {
                    if (gamePadState.ThumbSticks.Left.X < -0.5f)
                    {
                        skyRec.X += MoveSpeed;  
                    }
                    if (gamePadState.ThumbSticks.Left.X > 0.5f)
                    {
                        skyRec.X -= MoveSpeed;
                    }
                    if (gamePadState.ThumbSticks.Left.Y > 0.5f)
                    {
                        skyRec.Y += MoveSpeed;
                    }
                    if (gamePadState.ThumbSticks.Left.Y < -0.5f)
                    {
                        skyRec.Y -= MoveSpeed;
                    }
                }
            }
        }

        public void DrawBackgroud(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(skyTex, skyRec, Color.Gray);
            spriteBatch.End();
        }

        public void DrawInventory(SpriteBatch spriteBatch)
        {

        }



    }
}
