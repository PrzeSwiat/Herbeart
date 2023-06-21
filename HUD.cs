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
        private Texture2D appleTex, meliseTex, mintTex, nettleTex;
        private Texture2D appleTexCrafting, meliseTexCrafting, mintTexCrafting, nettleTexCrafting;
        private Texture2D[] craftingTextures = new Texture2D[4];
        private Texture2D EnemyHealth, HalfEnemyHealth;
        private Texture2D ReceptureBar;
        private Texture2D Arrow;
        private int WindowWidth, WindowHeight;
        public string name = "";
        
        public int MenuOption = 1;

        private Dictionary<string, int> leafs;
        private bool isThrowing = false;
        private bool isCrafting = false;
        private string[] actualRecepture = new string[3];
        float playerRotation = 0;

        public HUD()
        {
            WindowWidth = Globals.WindowWidth;
            WindowHeight = Globals.WindowHeight;

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
            Arrow = models.getTexture("HUD/celownik");

            craftingTextures[0] = models.getTexture("HUD/robienieherbatki_preview0");
            craftingTextures[1] = models.getTexture("HUD/robienieherbatki_preview1");
            craftingTextures[2] = models.getTexture("HUD/robienieherbatki_preview2");
            craftingTextures[3] = models.getTexture("HUD/robienieherbatki_preview3");
            appleTex = models.getTexture("HUD/ikona_japco_menu");
            mintTex = models.getTexture("HUD/ikona_mieta_menu");
            nettleTex = models.getTexture("HUD/ikona_pokrzywa_menu");
            meliseTex = models.getTexture("HUD/ikona_melisa_menu");
            appleTexCrafting = models.getTexture("HUD/ikona_japco_crafting");
            mintTexCrafting = models.getTexture("HUD/ikona_mieta_crafting");
            nettleTexCrafting = models.getTexture("HUD/ikona_pokrzywa_crafting");
            meliseTexCrafting = models.getTexture("HUD/ikona_melisa_crafting");


            ItemFont = Globals.content.Load<SpriteFont>("ItemFont");
            ScoreFont = Globals.content.Load<SpriteFont>("ScoreFont");
            Menu = Globals.content.Load<SpriteFont>("Menu");
            Menu2 = Globals.content.Load<SpriteFont>("Menu2");
        }

        public void Update(Dictionary<string, int> leafs, bool crafting, bool throwing, string[] recepture, float playerRotation)
        {
            this.leafs = leafs;
            this.isThrowing = throwing;
            this.isCrafting = crafting;
            actualRecepture = recepture;
            this.playerRotation = playerRotation;
            Globals.maxReceptures = Globals.learnedRecepture.Count();
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
        
        private void DrawArrow()
        {
            Vector2 midPos = new Vector2(Arrow.Width / 2, Arrow.Height / 2);
            Vector2 arrowPosition = new Vector2(WindowWidth / 2, 200);
            float rotation = (float)(2 * Math.PI - playerRotation);
            Point point = new Point(WindowWidth / 2, WindowHeight / 2);
            Point rotatedPoint = RotatePoint(new Point(WindowWidth / 2 + 110, WindowHeight / 2),point, rotation + Math.PI / 2);

            
            arrowPosition.X = rotatedPoint.X;
            arrowPosition.Y = rotatedPoint.Y - 140;
            Globals.spriteBatch.Draw(Arrow, arrowPosition, null, Color.White, rotation, midPos, 0.25f, SpriteEffects.None, 0f);
        }

        private void DrawReceptures()
        {
            //string[] recepture = { "AAB", "AXY", "XBY", "ABX" };
            float scale = 4.5f;
            Rectangle receptureRect = new Rectangle(WindowWidth / 2 - (int)(2208 / scale) / 2, WindowHeight - (int)(622 / scale) - 40, (int)(2208 / scale), (int)(622 / scale));
            Globals.spriteBatch.Draw(ReceptureBar, receptureRect, Color.White);

            for (int i = 0; i < 3; i++)
            {
                Rectangle rect = new Rectangle(receptureRect.X + 30 + i * 104, receptureRect.Y + 17, 100, 100);
                switch (Globals.learnedRecepture[Globals.numberOfRecepture][i])
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
            Rectangle rect_crafting = new Rectangle(WindowWidth / 2 - 100, WindowHeight / 2 - 240, 200, 70);
            int craftingIndex = 0;
            for (int i = 0; i < 3; i++)
            {
                if (actualRecepture[i] != "")
                {
                    craftingIndex = i + 1;
                }
                
            }

            if (isCrafting) 
            {
                Globals.spriteBatch.Draw(teaItemFrame, inv, Color.White);
                Globals.spriteBatch.Draw(craftingTextures[craftingIndex], rect_crafting, Color.White);
                DrawReceptures();

                int leafPosition = WindowWidth / 2 - 66;
                for (int i = 0; i < 3; i++)
                {
                    if (actualRecepture[i] != "")
                    {
                        Rectangle rect = new Rectangle(rect_crafting.X + i * 58 + 20, rect_crafting.Y + 14, 40, 40);
                        switch (actualRecepture[i])
                        {
                            case "mint":
                                Globals.spriteBatch.Draw(mintTexCrafting, rect, Color.White);
                                break;
                            case "melise":
                                Globals.spriteBatch.Draw(meliseTexCrafting, rect, Color.White);
                                break;
                            case "apple":
                                Globals.spriteBatch.Draw(appleTexCrafting, rect, Color.White);
                                break;
                            case "nettle":
                                Globals.spriteBatch.Draw(nettleTexCrafting, rect, Color.White);
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
                DrawArrow();
            } else
            {
                Globals.spriteBatch.Draw(defaultItemFrame, inv, Color.White);
            }


            Vector2 mintVec = new Vector2(invLenght / 2 + 50 - ItemFont.MeasureString(leafs.ElementAt(0).Value.ToString()).X, WindowHeight - 70);
            Vector2 nettleVec = new Vector2(invLenght + 4 - ItemFont.MeasureString(leafs.ElementAt(1).Value.ToString()).X, WindowHeight - invLenght / 2 - 22);
            Vector2 meliseVec = new Vector2(116 - ItemFont.MeasureString(leafs.ElementAt(2).Value.ToString()).X, WindowHeight - invLenght / 2 - 22);
            Vector2 appleVec = new Vector2(invLenght / 2 + 50 - ItemFont.MeasureString(leafs.ElementAt(3).Value.ToString()).X, WindowHeight - invLenght + 37);

            Globals.spriteBatch.DrawString(ItemFont, leafs.ElementAt(0).Value.ToString(), new Vector2(mintVec.X - 2, mintVec.Y + 2), Color.Black);    // mint
            Globals.spriteBatch.DrawString(ItemFont, leafs.ElementAt(1).Value.ToString(), new Vector2(nettleVec.X - 2, nettleVec.Y + 2), Color.Black);    // nettle
            Globals.spriteBatch.DrawString(ItemFont, leafs.ElementAt(2).Value.ToString(), new Vector2(meliseVec.X - 2, meliseVec.Y + 2), Color.Black);    // melise
            Globals.spriteBatch.DrawString(ItemFont, leafs.ElementAt(3).Value.ToString(), new Vector2(appleVec.X - 2, appleVec.Y + 2), Color.Black);    // apple
            Globals.spriteBatch.DrawString(ItemFont, leafs.ElementAt(0).Value.ToString(), mintVec, Color.White);    // mint
            Globals.spriteBatch.DrawString(ItemFont, leafs.ElementAt(1).Value.ToString(), nettleVec, Color.White);    // nettle
            Globals.spriteBatch.DrawString(ItemFont, leafs.ElementAt(2).Value.ToString(), meliseVec, Color.White);    // melise
            Globals.spriteBatch.DrawString(ItemFont, leafs.ElementAt(3).Value.ToString(), appleVec, Color.White);    // apple
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

        public void DrawTutorial(int number)
        {
            if(number==1)    //wiktor 2 modul
            {
                Rectangle rect = new Rectangle(WindowWidth * 15 / 20, WindowHeight * 13 / 20, 400, 200);
                Globals.spriteBatch.Begin();
                Globals.spriteBatch.Draw(healtColor, rect, Color.Black);
                Globals.spriteBatch.DrawString(Menu2, "Attack -> A", new Vector2(WindowWidth * 10 / 20, WindowHeight * 14 / 20), Color.Yellow);
                Globals.spriteBatch.End();
            }
            if(number == 2)
            {
                Rectangle rect = new Rectangle(WindowWidth * 12 / 20, WindowHeight * 0 / 20, 400, 100);
                Globals.spriteBatch.Begin();
                Globals.spriteBatch.Draw(healtColor, rect, Color.Black);
                Globals.spriteBatch.DrawString(Menu2, "< hold LT to heal yourself ", new Vector2(WindowWidth * 10 / 20, WindowHeight * 0 / 20), Color.Yellow);
                Globals.spriteBatch.End();
            }
            if (number == 3) //pokrzywa przeciwnik
            {
                Rectangle rect = new Rectangle(WindowWidth * 5 / 20, WindowHeight * 5 / 20, 500, 450);
                Globals.spriteBatch.Begin();
                Globals.spriteBatch.Draw(healtColor, rect, Color.Black);
                Globals.spriteBatch.DrawString(Menu2, "pokrzywa", new Vector2(WindowWidth * 6 / 20, WindowHeight * 6 / 20), Color.Yellow);
                Globals.spriteBatch.End();
            }
            if (number == 4) //efekty miotane pierwsze, co robią efekty miotane (ziel,czer)
            {
                Rectangle rect = new Rectangle(WindowWidth * 5 / 20, WindowHeight * 0 / 20, 500, 450);
                Globals.spriteBatch.Begin();
                Globals.spriteBatch.Draw(healtColor, rect, Color.Black);
                Globals.spriteBatch.DrawString(Menu2, "hold RT for special abilities", new Vector2(WindowWidth * 6 / 20, WindowHeight * 9 / 20), Color.Yellow);
                Globals.spriteBatch.End();
            }

            if (number == 6) //jablon przeciwnik 
            {
                Rectangle rect = new Rectangle(WindowWidth * 5 / 20, WindowHeight * 0 / 20, 500, 450);
                Globals.spriteBatch.Begin();
                Globals.spriteBatch.Draw(healtColor, rect, Color.Black);
                Globals.spriteBatch.DrawString(Menu2, "jablon", new Vector2(WindowWidth * 6 / 20, WindowHeight * 9 / 20), Color.Yellow);
                Globals.spriteBatch.End();
            }
            if (number == 9) //melisa przeciwnik 
            {
                Rectangle rect = new Rectangle(WindowWidth * 5 / 20, WindowHeight * 0 / 20, 500, 450);
                Globals.spriteBatch.Begin();
                Globals.spriteBatch.Draw(healtColor, rect, Color.Black);
                Globals.spriteBatch.DrawString(Menu2, "melisa", new Vector2(WindowWidth * 6 / 20, WindowHeight * 9 / 20), Color.Yellow);
                Globals.spriteBatch.End();
            }

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
            Globals.spriteBatch.DrawString(Menu2, "Score: " + Globals.Score, new Vector2(WindowWidth * 12/20, WindowHeight * 10 / 20), Color.White);
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

        public Point RotatePoint(Point p1, Point p2, double angle)
        {

            double radians = angle;
            double sin = Math.Sin(radians);
            double cos = Math.Cos(radians);

            // Translate point back to origin
            p1.X -= p2.X;
            p1.Y -= p2.Y;

            // Rotate point
            double xnew = p1.X * cos - p1.Y * sin;
            double ynew = p1.X * sin + p1.Y * cos;

            // Translate point back
            Point newPoint = new Point((int)xnew + p2.X, (int)ynew + p2.Y);
            return newPoint;
        }

    }
}