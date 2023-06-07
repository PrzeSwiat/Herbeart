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
        private SpriteFont ScoreFont;
        private SpriteFont ItemFont;
        private SpriteFont Menu;
        private SpriteFont Menu2;
        private Texture2D healtColor, healthBar;
        private Texture2D defaultItemFrame, offensiveItemFrame, teaItemFrame;
        private Texture2D craftingTex, appleTex, meliseTex, mintTex, nettleTex;
        private Texture2D EnemyHealth, HalfEnemyHealth;
        private int WindowWidth, WindowHeight;
        
        public int MenuOption = 1; 

        private Dictionary<string, int> leafs;
        private bool isThrowing = false;
        private bool isCrafting = false;
        private string[] actualRecepture = new string[3];

        public HUD(int windowWidth, int windowHeight)
        {
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;

            //buttons = new string[] { "A", "B", "X", "Y" };
        }

        public void LoadContent()
        {
            LoadedModels models = LoadedModels.Instance;

            healtColor = models.getTexture("HUD/magenta");
            healthBar = models.getTexture("HUD/pasekzycia");
            defaultItemFrame = models.getTexture("HUD/default");
            offensiveItemFrame = models.getTexture("HUD/atak");
            teaItemFrame = models.getTexture("HUD/defens");
            EnemyHealth = models.getTexture("Textures/EnemyHealth");
            HalfEnemyHealth= models.getTexture("Textures/HalfEnemyHealth");

            craftingTex = models.getTexture("HUD/robienieherbatki_preview");
            appleTex = models.getTexture("HUD/ikona_japco_menu");
            mintTex = models.getTexture("HUD/ikona_mieta_menu");
            nettleTex = models.getTexture("HUD/ikona_pokrzywa_menu");
            meliseTex = models.getTexture("HUD/ikona_melisa_menu");


            ItemFont = Globals.content.Load<SpriteFont>("ItemFont");
            ScoreFont = Globals.content.Load<SpriteFont>("ScoreFont");
            Menu = Globals.content.Load<SpriteFont>("Menu");
            Menu2 = Globals.content.Load<SpriteFont>("Menu2");
        }

        public void Update(Dictionary<string, int> leafs, bool crafting, bool throwing, string[] recepture)
        {
            this.leafs = leafs;
            this.isThrowing = throwing;
            this.isCrafting = crafting;
            actualRecepture = recepture;
        }

        public void DrawFrontground(SpriteBatch spriteBatch, int hp, List<Enemy> enemies)
        {
            spriteBatch.Begin();
            DrawInventory(spriteBatch);
            DrawHealthBar(spriteBatch, hp);
            DrawEnemyHealthBar(spriteBatch, enemies);
            DrawScore(spriteBatch);
            spriteBatch.End();
        }

        private void DrawHealthBar(SpriteBatch spriteBatch, int hp)
        {
            Rectangle health = new Rectangle(WindowWidth / 2 - 325/2 + 10, 17, hp, 30);
            Rectangle healthBar = new Rectangle(WindowWidth / 2 - 325/2, 10, 320, 48);
            
            spriteBatch.Draw(healtColor, health, Color.Magenta);
            spriteBatch.Draw(this.healthBar, healthBar, Color.White); 
        }
        private void DrawEnemyHealthBar(SpriteBatch spriteBatch, List<Enemy> enemies)
        {
            /*
            foreach (Enemy e in enemies)
            {
                Vector3 projectedPosition = Globals.viewport.Project(e.GetPosition(),
                    Globals.projectionMatrix, Globals.viewMatrix, Matrix.Identity);
                Rectangle rect = new Rectangle((int)projectedPosition.X - 40, (int)(projectedPosition.Y - 100), e.Health, 10);
                spriteBatch.Draw(healtColor, rect, Color.Red);

            }
            */
            
            foreach (Enemy e in enemies)
            {
                Vector3 projectedPosition =Globals.viewport.Project(e.GetPosition(),
                    Globals.projectionMatrix, Globals.viewMatrix, Matrix.Identity);

                int fakeHealth = e.Health / 2;
                Rectangle rect = new Rectangle(0,0,1,1);
                for (int i = 0; i < fakeHealth; i++)
                {
                    
                    if (e.GetType() == typeof(Mint))
                    {
                         rect = new Rectangle((int)projectedPosition.X + (-20 + (i * 30)), (int)(projectedPosition.Y - 180), 30, 30);
                    }
                    else if(e.GetType() == typeof(Nettle))
                    {
                        rect = new Rectangle((int)projectedPosition.X + (-50 + (i * 30)), (int)(projectedPosition.Y - 155), 30, 30);
                    }
                    else if (e.GetType() == typeof(Melissa))
                    {
                        rect = new Rectangle((int)projectedPosition.X + (-60 + (i * 30)), (int)(projectedPosition.Y - 240), 30, 30);
                    }
                    else if (e.GetType() == typeof(AppleTree))
                    {
                        rect = new Rectangle((int)projectedPosition.X + (-30 + (i * 30)), (int)(projectedPosition.Y - 240), 30, 30);
                    }
                    spriteBatch.Draw(EnemyHealth, rect, Color.White);
                }
                if (e.Health % 2 == 1)
                {
                    if (e.GetType() == typeof(Mint))
                    {
                        rect = new Rectangle((int)projectedPosition.X + (-20 + (fakeHealth * 30)), (int)(projectedPosition.Y - 180), 30, 30);
                    }
                    else if (e.GetType() == typeof(Nettle))
                    {
                        rect = new Rectangle((int)projectedPosition.X + (-50 + (fakeHealth * 30)), (int)(projectedPosition.Y - 155), 30, 30);
                    }
                    else if (e.GetType() == typeof(Melissa))
                    {
                        rect = new Rectangle((int)projectedPosition.X + (-60 + (fakeHealth * 30)), (int)(projectedPosition.Y - 240), 30, 30);
                    }
                    else if (e.GetType() == typeof(AppleTree))
                    {
                        rect = new Rectangle((int)projectedPosition.X + (-30 + (fakeHealth * 30)), (int)(projectedPosition.Y - 240), 30, 30);
                    }
                    // Rectangle rect = new Rectangle((int)projectedPosition.X + (0 + ((fakeHealth) * 20)), (int)(projectedPosition.Y - 100), 30, 30);
                    spriteBatch.Draw(HalfEnemyHealth, rect, Color.White);
                }
            
                }
            }

        public void DrawInventory(SpriteBatch spriteBatch)
        {
            int invLenght = WindowHeight / 3;
            Rectangle inv = new Rectangle(20, WindowHeight - invLenght - 20, invLenght, invLenght);
            Rectangle rect_crafting = new Rectangle(WindowWidth / 2 - 70, WindowHeight / 2 - 170, 150, 50);


            if (isCrafting) 
            { 
                spriteBatch.Draw(teaItemFrame, inv, Color.White);
                spriteBatch.Draw(craftingTex, rect_crafting, Color.White);
                Debug.Write(actualRecepture[0] + " " + actualRecepture[1] +  "\n");

                int leafPosition = WindowWidth / 2 - 66;
                for (int i = 0; i < 3; i++)
                {
                    if (actualRecepture[i] != "")
                    {
                        Rectangle rect = new Rectangle(leafPosition + i * 50, WindowHeight / 2 - 163, 37, 37);
                        switch (actualRecepture[i])
                        {
                            case "mint":
                                spriteBatch.Draw(mintTex, rect, Color.White);
                                break;
                            case "melise":
                                spriteBatch.Draw(meliseTex, rect, Color.White);
                                break;
                            case "apple":
                                spriteBatch.Draw(appleTex, rect, Color.White);
                                break;
                            case "nettle":
                                spriteBatch.Draw(nettleTex, rect, Color.White);
                                break;
                            default:
                                break;
                        }
                    }
                }
            } 
            else if (isThrowing) 
            { 
                spriteBatch.Draw(offensiveItemFrame, inv, Color.White); 
            } else
            {
                spriteBatch.Draw(defaultItemFrame, inv, Color.White);
            }
            spriteBatch.DrawString(ItemFont, leafs.ElementAt(0).Value.ToString(), new Vector2(invLenght / 2 + 32, WindowHeight - 55), Color.White);    // mint
            spriteBatch.DrawString(ItemFont, leafs.ElementAt(1).Value.ToString(), new Vector2(invLenght - 12, WindowHeight - invLenght / 2 - 12), Color.White);    // nettle
            spriteBatch.DrawString(ItemFont, leafs.ElementAt(2).Value.ToString(), new Vector2(35, WindowHeight - invLenght / 2 - 12), Color.White);    // melise
            spriteBatch.DrawString(ItemFont, leafs.ElementAt(3).Value.ToString(), new Vector2(invLenght / 2 + 32, WindowHeight - invLenght + 44), Color.White);    // apple
        }

       

        public void DrawPause(SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle(-WindowWidth, -WindowHeight, 2 * WindowWidth, 2 * WindowHeight);
            spriteBatch.Begin();
            spriteBatch.Draw(healtColor, rect, Color.Black);
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
            spriteBatch.Draw(healtColor, rect, Color.Black);
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
            spriteBatch.Draw(healtColor, rect, Color.Black);
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