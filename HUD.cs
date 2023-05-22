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
using TheGame.Core;

namespace TheGame
{
    internal class HUD
    {
        private string _texFilename;
        private SpriteFont Font;
        private SpriteFont Menu;
        private SpriteFont Menu2;
        private Texture2D skyTex, red, itemFrame;
        private Rectangle skyRec;
        private int BackgroundWidth, BackgroundHeight, WindowWidth, WindowHeight;
        private int BackgroundX, BackgroundY;
        private Dictionary<string, int> leafs;
        //private string[] buttons;
        GamePadState prevState;

        private DateTime lastChangeTime, actualTime;

        public HUD(string texFilename, int windowWidth, int windowHeight)
        {
            _texFilename = texFilename;
            lastChangeTime = DateTime.Now;
            actualTime = lastChangeTime;
            BackgroundWidth = 200;
            BackgroundHeight = 200;
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
            skyRec = new Rectangle(0, 0, BackgroundWidth, BackgroundHeight);

            //buttons = new string[] { "A", "B", "X", "Y" };
        }

        public void LoadContent()
        {
            LoadedModels models = LoadedModels.Instance;
            skyTex = models.getTexture(_texFilename);
            red = models.getTexture("Textures/Red");
            itemFrame = models.getTexture("Textures/itemFrame");
            Font = Globals.content.Load<SpriteFont>("Fonts");
            Menu = Globals.content.Load<SpriteFont>("Menu");
            Menu2 = Globals.content.Load<SpriteFont>("Menu2");
        }

        public void Update(Vector3 camPos, Dictionary<string, int> leafs)
        {
            BackgroundX = (int)camPos.X % BackgroundWidth - BackgroundWidth;
            BackgroundY = (int)camPos.Z % BackgroundHeight - BackgroundHeight;

            this.leafs = leafs;
        }

        public void DrawBackground(SpriteBatch spriteBatch)
        {
            int numberOfWidthBG = WindowWidth / BackgroundWidth;
            int numberOfHeightBG = WindowHeight / BackgroundHeight;

            spriteBatch.Begin();
            
            for(int i = 0; i <= numberOfWidthBG + 2; i++)
            {
                skyRec.X = BackgroundX + i * BackgroundWidth;
                for (int j = 0; j <= numberOfHeightBG + 1; j++)
                {   
                    
                    skyRec.Y = BackgroundY + j * BackgroundHeight;
                    spriteBatch.Draw(skyTex, skyRec, Color.Gray);
                }
            }

            spriteBatch.End();
        }

        public void DrawFrontground(SpriteBatch spriteBatch, int hp)
        {
            spriteBatch.Begin();

            DrawInventory(spriteBatch);
            DrawHealthBar(spriteBatch, hp);

            spriteBatch.End();
        }

        private void DrawHealthBar(SpriteBatch spriteBatch, int health)
        {
            Rectangle rect = new Rectangle(10, 10, health, 20);
            spriteBatch.Draw(red, rect, Color.Red);
        }

        public void DrawInventory(SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle(0, WindowHeight - 100, 50, 50);
            actualTime = DateTime.Now;

            for (int i = 0; i < 4; i++)
            {
                rect.X = 20 + i * 50;
                spriteBatch.Draw(itemFrame, rect, Color.Gray);
                spriteBatch.DrawString(Font, leafs.ElementAt(i).Key, new Vector2(50 + i * 50, WindowHeight - 90), Color.Black);
                spriteBatch.DrawString(Font, leafs.ElementAt(i).Value.ToString(), new Vector2(50 + i * 50, WindowHeight - 50), Color.Black);
            }
   
            
        }

        public void MainMenuCheck()
        {

            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            if (gamePadState.Buttons.Start == ButtonState.Pressed && prevState.Buttons.Start == ButtonState.Released)
            {
                Globals.Pause = !Globals.Pause;
            }
            prevState = gamePadState;

        }

        public void DrawMainMenu(SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle(-WindowWidth,-WindowHeight,2*WindowWidth,2*WindowHeight);
            spriteBatch.Begin();
            spriteBatch.Draw(red,rect, Color.Black);
            spriteBatch.DrawString(Menu,"Herbeart", new Vector2(WindowWidth/2 - WindowWidth/5,WindowHeight*1/10), Color.White);
            spriteBatch.DrawString(Menu2, "Press 'Start' to start the game", new Vector2(WindowWidth / 15, WindowHeight * 4 / 10), Color.White);
            spriteBatch.End();
        }


    }
}
