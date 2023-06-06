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
        private SpriteFont ScoreFont;
        private SpriteFont ItemFont;
        private SpriteFont Menu;
        private SpriteFont Menu2;
        private Texture2D skyTex, red, healthBar, defaultItemFrame;
        private Rectangle skyRec;
        private int BackgroundWidth, BackgroundHeight, WindowWidth, WindowHeight;
        private int BackgroundX, BackgroundY;
        private Dictionary<string, int> leafs;
        public int MenuOption = 1; 
        //private string[] buttons;
        

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
            healthBar = models.getTexture("HUD/pasekzycia");
            defaultItemFrame = models.getTexture("HUD/default");
            ItemFont = Globals.content.Load<SpriteFont>("ItemFont");
            ScoreFont = Globals.content.Load<SpriteFont>("ScoreFont");
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

            for (int i = 0; i <= numberOfWidthBG + 2; i++)
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

        public void DrawFrontground(SpriteBatch spriteBatch, int hp, List<Enemy> enemies, Viewport viewport)
        {
            spriteBatch.Begin();
            DrawInventory(spriteBatch);
            DrawHealthBar(spriteBatch, hp);
            DrawEnemyHealthBar(spriteBatch, enemies, viewport);
            DrawScore(spriteBatch);
            spriteBatch.End();
        }

        private void DrawHealthBar(SpriteBatch spriteBatch, int hp)
        {
            
            Rectangle health = new Rectangle(WindowWidth / 2 - 200, 15, hp * 2, 30);
            Rectangle healthBar = new Rectangle(WindowWidth / 2 - 215, 10, 430, 40);
            
            spriteBatch.Draw(red, health, Color.Red);
            spriteBatch.Draw(this.healthBar, healthBar, Color.White); 
        }
        private void DrawEnemyHealthBar(SpriteBatch spriteBatch, List<Enemy> enemies, Viewport viewport)
        {
            foreach (Enemy e in enemies)
            {
                Vector3 projectedPosition = viewport.Project(e.GetPosition(),
                    Globals.projectionMatrix, Globals.viewMatrix, Matrix.Identity);
                Rectangle rect = new Rectangle((int)projectedPosition.X - 40, (int)(projectedPosition.Y - 100), e.Health, 10);
                spriteBatch.Draw(red, rect, Color.Red);

            }
            /*
            foreach (Enemy e in enemies)
            {
                Vector3 projectedPosition = viewport.Project(e.GetPosition(),
                    Globals.projectionMatrix, Globals.viewMatrix, Matrix.Identity);

                int fakeHealth = e.Health / 2;
                for (int i = 0; i < fakeHealth; i++)
                {
                    Rectangle rect = new Rectangle((int)projectedPosition.X + (0 + (i * 20)), (int)(projectedPosition.Y - 100), 20, 20);
                    spriteBatch.Draw(enemyhealth, rect, Color.White);
                }
                if (e.Health % 2 == 1)
                {

                    Rectangle rect = new Rectangle((int)projectedPosition.X + (0 + ((fakeHealth) * 20)), (int)(projectedPosition.Y - 100), 20, 20);
                    spriteBatch.Draw(halfenemyhealth, rect, Color.Gray);
                }
            */

            }

        public void DrawInventory(SpriteBatch spriteBatch)
        {
            int invLenght = WindowHeight / 3;
            Rectangle rect = new Rectangle(0, WindowHeight - invLenght, invLenght, invLenght);

            spriteBatch.Draw(defaultItemFrame, rect, Color.Gray);
            spriteBatch.DrawString(ItemFont, leafs.ElementAt(0).Value.ToString(), new Vector2(invLenght / 2 + 13, WindowHeight - 40), Color.Black);    // mint
            spriteBatch.DrawString(ItemFont, leafs.ElementAt(1).Value.ToString(), new Vector2(3 * invLenght / 4, WindowHeight - invLenght / 2), Color.Black);    // melise
            spriteBatch.DrawString(ItemFont, leafs.ElementAt(2).Value.ToString(), new Vector2(17, WindowHeight - invLenght / 2), Color.Black);    // nettle
            spriteBatch.DrawString(ItemFont, leafs.ElementAt(3).Value.ToString(), new Vector2(invLenght / 2 + 13, WindowHeight - invLenght + 20), Color.Black);    // apple
        }

       

        public void DrawPause(SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle(-WindowWidth, -WindowHeight, 2 * WindowWidth, 2 * WindowHeight);
            spriteBatch.Begin();
            spriteBatch.Draw(red, rect, Color.Black);
            spriteBatch.DrawString(Menu, "Game Paused", new Vector2(WindowWidth / 4 , WindowHeight * 1 / 10), Color.White);
            spriteBatch.DrawString(Menu2, "Press 'Start/ESC' to reasume", new Vector2(WindowWidth / 5, WindowHeight * 4 / 10), Color.White);
            spriteBatch.End();
        }

        public void DrawDeathMenu(SpriteBatch spriteBatch)
        {
            Color one = Color.Gray;
            Color two = Color.Gray;
            Color three = Color.Gray;
            Color four = Color.Gray;

            if (MenuOption == 1)
            {
                one = Color.White;
            }
            if (MenuOption == 2)
            {
                two = Color.White;
            }
            if (MenuOption == 3)
            {
                three = Color.White;
            }
            if(MenuOption == 4)
            {
                four = Color.White;
            }

            Rectangle rect = new Rectangle(-WindowWidth, -WindowHeight, 2 * WindowWidth, 2 * WindowHeight);
            spriteBatch.Begin();
            spriteBatch.Draw(red, rect, Color.Black);
            spriteBatch.DrawString(Menu, "You fell asleep", new Vector2(WindowWidth / 3.5f, WindowHeight * 1 / 20), Color.OrangeRed);
            spriteBatch.DrawString(Menu2, "Leader board", new Vector2(WindowWidth / 10, WindowHeight * 6 / 20), one);
            spriteBatch.DrawString(Menu2, "Try again", new Vector2(WindowWidth / 10, WindowHeight * 9 / 20), two);
            spriteBatch.DrawString(Menu2, "Main menu", new Vector2(WindowWidth / 10, WindowHeight * 12 / 20), three);
            spriteBatch.DrawString(Menu2, "Exit", new Vector2(WindowWidth / 10, WindowHeight * 16 / 20), four);
            spriteBatch.End();
        }

        public void DrawMainMenu(SpriteBatch spriteBatch)
        {
            Color one = Color.Gray;
            Color two = Color.Gray;
            Color three = Color.Gray;

            if (MenuOption == 1)
            {
                one = Color.White;
            }
            if(MenuOption == 2)
            {
                two = Color.White;
            }
            if(MenuOption==3)
            {
                three = Color.White;
            }

            Rectangle rect = new Rectangle(-WindowWidth, -WindowHeight, 2 * WindowWidth, 2 * WindowHeight);
            spriteBatch.Begin();
            spriteBatch.Draw(red, rect, Color.Black);
            spriteBatch.DrawString(Menu, "Herbeart", new Vector2(WindowWidth / 3, WindowHeight * 1 / 20), Color.Gray);
            spriteBatch.DrawString(Menu2, "Start new game", new Vector2(WindowWidth / 10, WindowHeight * 7 / 20), one);
            spriteBatch.DrawString(Menu2, "Not implemented option", new Vector2(WindowWidth / 10, WindowHeight * 10 / 20), two);
            spriteBatch.DrawString(Menu2, "Exit", new Vector2(WindowWidth / 10, WindowHeight * 15 / 20), three);
            spriteBatch.End();

        }

        public void DrawScore(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(ScoreFont, Globals.Score.ToString(), new Vector2(WindowWidth - 40 * Globals.Score.ToString().Length, 5), Color.Black);
            spriteBatch.DrawString(ScoreFont, Globals.Score.ToString(), new Vector2(WindowWidth - 39 * Globals.Score.ToString().Length, 4), Color.Gray);
            spriteBatch.DrawString(ScoreFont, Globals.Score.ToString(), new Vector2(WindowWidth - 38 * Globals.Score.ToString().Length, 2), Color.White);
        }
    }
}