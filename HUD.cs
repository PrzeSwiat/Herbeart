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
        private Texture2D ReceptureBar;
        private int WindowWidth, WindowHeight;
        public string name = "";
        
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
            HalfEnemyHealth = models.getTexture("Textures/HalfEnemyHealth");
            ReceptureBar = models.getTexture("HUD/przepisy_preview");

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

        public void DrawFrontground(int hp, List<Enemy> enemies)
        {
            Globals.spriteBatch.Begin();
            DrawInventory();
            DrawHealthBar(hp);
            DrawEnemyHealthBar(enemies);
            DrawScore();
            Globals.spriteBatch.End();
        }

        private void DrawHealthBar(int hp)
        {
            Rectangle health = new Rectangle(WindowWidth / 2 - 325/2 + 10, 17, hp, 30);
            Rectangle healthBar = new Rectangle(WindowWidth / 2 - 325/2, 10, 320, 48);

            Globals.spriteBatch.Draw(healtColor, health, Color.Magenta);
            Globals.spriteBatch.Draw(this.healthBar, healthBar, Color.White); 
        }
        

        private void DrawReceptures()
        {
            string[] recepture = { "AAB", "AXY", "XBY", "ABX" };
            Rectangle receptureRect = new Rectangle(0, 200, 300, 100);
            Globals.spriteBatch.Draw(ReceptureBar, receptureRect, Color.White);

            for (int i = 0; i < 3; i++)
            {
                Rectangle rect = new Rectangle(receptureRect.X + 30 + i * 80, receptureRect.Y + 10, 80, 80);
                switch (recepture[Globals.numberOfRecepture][i])
                {
                    case 'A':
                        Globals.spriteBatch.Draw(mintTex, rect, Color.White);
                        break;
                    case 'X':
                        Globals.spriteBatch.Draw(meliseTex, rect, Color.White);
                        break;
                    case 'Y':
                        Globals.spriteBatch.Draw(appleTex, rect, Color.White);
                        break;
                    case 'B':
                        Globals.spriteBatch.Draw(nettleTex, rect, Color.White);
                        break;
                    default:
                        break;
                }

            }
        }

        public void DrawInventory()
        {
            int invLenght = WindowHeight / 3;
            Rectangle inv = new Rectangle(20, WindowHeight - invLenght - 20, invLenght, invLenght);
            Rectangle rect_crafting = new Rectangle(WindowWidth / 2 - 70, WindowHeight / 2 - 220, 150, 50);
            DrawReceptures();

            if (isCrafting) 
            {
                Globals.spriteBatch.Draw(teaItemFrame, inv, Color.White);
                Globals.spriteBatch.Draw(craftingTex, rect_crafting, Color.White);

                int leafPosition = WindowWidth / 2 - 66;
                for (int i = 0; i < 3; i++)
                {
                    if (actualRecepture[i] != "")
                    {
                        Rectangle rect = new Rectangle(rect_crafting.X + i * 50 + 5, rect_crafting.Y + 6, 37, 37);
                        switch (actualRecepture[i])
                        {
                            case "mint":
                                Globals.spriteBatch.Draw(mintTex, rect, Color.White);
                                break;
                            case "melise":
                                Globals.spriteBatch.Draw(meliseTex, rect, Color.White);
                                break;
                            case "apple":
                                Globals.spriteBatch.Draw(appleTex, rect, Color.White);
                                break;
                            case "nettle":
                                Globals.spriteBatch.Draw(nettleTex, rect, Color.White);
                                break;
                            default:
                                break;
                        }
                    }
                }
            } 
            else if (isThrowing) 
            {
                Globals.spriteBatch.Draw(offensiveItemFrame, inv, Color.White); 
            } else
            {
                Globals.spriteBatch.Draw(defaultItemFrame, inv, Color.White);
            }
            Globals.spriteBatch.DrawString(ItemFont, leafs.ElementAt(0).Value.ToString(), new Vector2(invLenght / 2 + 32, WindowHeight - 55), Color.White);    // mint
            Globals.spriteBatch.DrawString(ItemFont, leafs.ElementAt(1).Value.ToString(), new Vector2(invLenght - 12, WindowHeight - invLenght / 2 - 12), Color.White);    // nettle
            Globals.spriteBatch.DrawString(ItemFont, leafs.ElementAt(2).Value.ToString(), new Vector2(35, WindowHeight - invLenght / 2 - 12), Color.White);    // melise
            Globals.spriteBatch.DrawString(ItemFont, leafs.ElementAt(3).Value.ToString(), new Vector2(invLenght / 2 + 32, WindowHeight - invLenght + 44), Color.White);    // apple
        }

        public void DrawPause()
        {
            Rectangle rect = new Rectangle(-WindowWidth, -WindowHeight, 2 * WindowWidth, 2 * WindowHeight);
            Color one = Color.Gray;
            Color two = Color.Gray;
            Color three = Color.Gray;

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

            Globals.spriteBatch.Begin();
            Globals.spriteBatch.Draw(healtColor, rect, Color.Black);
            Globals.spriteBatch.DrawString(Menu, "Game Paused", new Vector2(WindowWidth / 4 , WindowHeight * 1 / 10), Color.White);
            Globals.spriteBatch.DrawString(Menu2, "Resume", new Vector2(WindowWidth / 10, WindowHeight * 9 / 20), one);
            Globals.spriteBatch.DrawString(Menu2, "Main menu", new Vector2(WindowWidth / 10, WindowHeight * 12 / 20), two);
            Globals.spriteBatch.DrawString(Menu2, "Exit", new Vector2(WindowWidth / 10, WindowHeight * 16 / 20), three);
            Globals.spriteBatch.End();
        }

        public void DrawTutorial()
        {
            Rectangle rect = new Rectangle(WindowWidth * 15 / 20, WindowHeight * 13 / 20, 400, 200);
            Globals.spriteBatch.Begin();
            Globals.spriteBatch.Draw(healtColor, rect, Color.Black);
            Globals.spriteBatch.End();
        }

        public void DrawDeathMenu()
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
            Globals.spriteBatch.Begin();
            Globals.spriteBatch.Draw(healtColor, rect, Color.Black);
            Globals.spriteBatch.DrawString(Menu, "You fell asleep", new Vector2(WindowWidth / 3.5f, WindowHeight * 1 / 20), Color.OrangeRed);
            Globals.spriteBatch.DrawString(Menu2, "Save your score !", new Vector2(WindowWidth / 10, WindowHeight * 6 / 20), one);
            Globals.spriteBatch.DrawString(Menu2, "Try again", new Vector2(WindowWidth / 10, WindowHeight * 9 / 20), two);
            Globals.spriteBatch.DrawString(Menu2, "Main menu", new Vector2(WindowWidth / 10, WindowHeight * 12 / 20), three);
            Globals.spriteBatch.DrawString(Menu2, "Exit", new Vector2(WindowWidth / 10, WindowHeight * 16 / 20), four);
            Globals.spriteBatch.End();
        }

        public void DrawMainMenu()
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
            Globals.spriteBatch.Begin();
            Globals.spriteBatch.Draw(healtColor, rect, Color.Black);
            Globals.spriteBatch.DrawString(Menu, "Herbeart", new Vector2(WindowWidth / 3, WindowHeight * 1 / 20), Color.Gray);
            Globals.spriteBatch.DrawString(Menu2, "Start new game", new Vector2(WindowWidth / 10, WindowHeight * 7 / 20), one);
            Globals.spriteBatch.DrawString(Menu2, "Not implemented option", new Vector2(WindowWidth / 10, WindowHeight * 10 / 20), two);
            Globals.spriteBatch.DrawString(Menu2, "Exit", new Vector2(WindowWidth / 10, WindowHeight * 15 / 20), three);
            Globals.spriteBatch.End();

        }

        public void DrawTutorialMenu()
        {
            Color one = Color.Gray;
            Color two = Color.Gray;
            Rectangle rect = new Rectangle(-WindowWidth, -WindowHeight, 2 * WindowWidth, 2 * WindowHeight);
            Globals.spriteBatch.Begin();
            Globals.spriteBatch.Draw(healtColor, rect, Color.Black);
            if (MenuOption == 1)
            {
                one = Color.White;
                Globals.spriteBatch.DrawString(Menu2, "New player mode (hints enabled)", new Vector2(WindowWidth / 10, WindowHeight * 16 / 20), Color.Gray);
                
            }
            if (MenuOption == 2)
            {
                two = Color.White;
                Globals.spriteBatch.DrawString(Menu2, "Expert player mode (hints disabled)", new Vector2(WindowWidth / 10, WindowHeight * 16 / 20), Color.Gray);
                
            }

            
            
            Globals.spriteBatch.DrawString(Menu2, "Choose difficulty", new Vector2(WindowWidth / 5, WindowHeight * 2 / 20), Color.Gray);
            Globals.spriteBatch.DrawString(Menu2, "Easy", new Vector2(WindowWidth / 10, WindowHeight * 7 / 20), one);
            Globals.spriteBatch.DrawString(Menu2, "Hard", new Vector2(WindowWidth / 10, WindowHeight * 10 / 20), two);
            Globals.spriteBatch.End();

        }

        public void DrawLeaderBoard()
        {
            Rectangle rect = new Rectangle(-WindowWidth, -WindowHeight, 2 * WindowWidth, 2 * WindowHeight);
            Globals.spriteBatch.Begin();
            Globals.spriteBatch.Draw(healtColor, rect, Color.Black);
            Globals.spriteBatch.DrawString(Menu2, "Type your 'name (company name)' ", new Vector2(WindowWidth / 5, WindowHeight * 2 / 20), Color.Gray);
            Globals.spriteBatch.DrawString(Menu2, name, new Vector2(WindowWidth / 5, WindowHeight * 7 / 20), Color.White);
            Globals.spriteBatch.DrawString(Menu2, "Press 'A / ENTER' to accept", new Vector2(WindowWidth / 5, WindowHeight * 16 / 20), Color.Gray);
            Globals.spriteBatch.End();


        }

        public void TextInputHandler(object sender, TextInputEventArgs args)
        {

            var character = args.Character;
            if ((char.IsLetter(character) || character == ' ' || character == '(' || character == ')') && name.Length < 28)
            {
                name += character;
            }
            if (character == '\b')
            {
                if (name.Length > 0)
                {
                    name = name.Substring(0, name.Length - 1);
                }
            }

            //Debug.WriteLine(name);
        }

        public void DrawScore()
        {
            Globals.spriteBatch.DrawString(ScoreFont, Globals.Score.ToString(), new Vector2(WindowWidth - 40 * Globals.Score.ToString().Length, 5), Color.Black);
            Globals.spriteBatch.DrawString(ScoreFont, Globals.Score.ToString(), new Vector2(WindowWidth - 39 * Globals.Score.ToString().Length, 4), Color.Gray);
            Globals.spriteBatch.DrawString(ScoreFont, Globals.Score.ToString(), new Vector2(WindowWidth - 38 * Globals.Score.ToString().Length, 2), Color.White);
        }

        private void DrawEnemyHealthBar(List<Enemy> enemies)
        {
            foreach (Enemy e in enemies)
            {
                Vector3 projectedPosition = Globals.viewport.Project(e.GetPosition(),
                Globals.projectionMatrix, Globals.viewMatrix, Matrix.Identity);
                int fakeHealth = e.Health / 2;
                Rectangle rect = new Rectangle(0, 0, 1, 1);

                for (int i = 0; i < fakeHealth; i++)
                {

                    if (e.GetType() == typeof(Mint))
                    {
                        rect = new Rectangle((int)projectedPosition.X + (-20 + (i * 30)), (int)(projectedPosition.Y - 180), 30, 30);
                    }
                    else if (e.GetType() == typeof(Nettle))
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
                    Globals.spriteBatch.Draw(EnemyHealth, rect, Color.White);
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
                    Globals.spriteBatch.Draw(HalfEnemyHealth, rect, Color.White);
                }
            }
        }
    }
}