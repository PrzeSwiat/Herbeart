using Assimp.Unmanaged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using TheGame.Core;

namespace TheGame
{
    internal class HUD
    {
        private SpriteFont ScoreFont, ScoreMultiplier;
        private SpriteFont ItemFont;
        private Texture2D multiplierTexture;

        private SpriteFont MenuFont, MenuFont2, MenuFont3;
        private SpriteFont Menu, Menu2;
        private Texture2D menu;
        private Texture2D difficulty;
        private Texture2D dot;
        private Texture2D healtColor, healthBar, healthBackdrop;
        private Texture2D strenghtTex, speedTex, immortalTex, heal1Tex, heal2Tex;
        private Texture2D defaultItemFrame, offensiveItemFrame, teaItemFrame;
        private Texture2D appleTex, meliseTex, mintTex, nettleTex;
        private Texture2D appleTexCrafting, meliseTexCrafting, mintTexCrafting, nettleTexCrafting;
        private Texture2D attack_tutorial, brew_tutorial, festival_tutorial, special_tutorial;
        private Texture2D[] craftingTextures = new Texture2D[4];
        private Texture2D EnemyHealth, HalfEnemyHealth;
        private Texture2D ReceptureBar;
        private Texture2D Arrow;
        private Texture2D effectMelise, effectMint, effectNettle;
        private Texture2D appleBuff1, appleBuff2, attackSpeed1, attackSpeed2;
        private Texture2D mintBuff1, mintBuff2, nettleBuff1, nettleBuff2;
        private Texture2D speedBuff1, speedBuff2, strenght1, strenght2;
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

            appleBuff1 = models.getTexture("HUD/PlayerUpgrades/appleBuff1");
            appleBuff2 = models.getTexture("HUD/PlayerUpgrades/appleBuff2");
            attackSpeed1 = models.getTexture("HUD/PlayerUpgrades/atackSpeed1");
            attackSpeed2 = models.getTexture("HUD/PlayerUpgrades/atackSpeed2");
            mintBuff1 = models.getTexture("HUD/PlayerUpgrades/mintBuff1");
            mintBuff2 = models.getTexture("HUD/PlayerUpgrades/mintBuff2");
            nettleBuff1 = models.getTexture("HUD/PlayerUpgrades/nettleBuff1");
            nettleBuff2 = models.getTexture("HUD/PlayerUpgrades/nettleBuff2");
            speedBuff1 = models.getTexture("HUD/PlayerUpgrades/speedBuff1");
            speedBuff2 = models.getTexture("HUD/PlayerUpgrades/speedBuff2");
            strenght1 = models.getTexture("HUD/PlayerUpgrades/strenght1");
            strenght2 = models.getTexture("HUD/PlayerUpgrades/strenght2");

            dot = models.getTexture("Textures/EnemyHealth");
            menu = models.getTexture("HUD/tlo_listki_menu2");
            attack_tutorial = models.getTexture("Textures/attackdash_tutorial");
            brew_tutorial = models.getTexture("Textures/brewTEA_tutorial");
            festival_tutorial = models.getTexture("Textures/festival_tutorial");
            special_tutorial = models.getTexture("Textures/SPECIALS_tutorial");
            difficulty = models.getTexture("HUD/tlo_difficulty3");
            healtColor = models.getTexture("HUD/magenta");
            healthBar = models.getTexture("HUD/pasekzycia");
            healthBackdrop = models.getTexture("HUD/pasekzycia_tlo");
            strenghtTex = models.getTexture("HUD/PlayerEffects/strenght");
            speedTex = models.getTexture("HUD/PlayerEffects/speed");
            immortalTex = models.getTexture("HUD/PlayerEffects/immortal"); 
            heal1Tex = models.getTexture("HUD/PlayerEffects/heal1");
            heal2Tex = models.getTexture("HUD/PlayerEffects/heal2");
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
            multiplierTexture = models.getTexture("HUD/multiplier");
            effectMelise = models.getTexture("HUD/efffectMelise");
            effectMint = models.getTexture("HUD/effectMint");
            effectNettle = models.getTexture("HUD/effectNettle");



            MenuFont = Globals.content.Load<SpriteFont>("menuFont");
            MenuFont2 = Globals.content.Load<SpriteFont>("menuFont2");
            MenuFont3 = Globals.content.Load<SpriteFont>("menuFont3");
            ItemFont = Globals.content.Load<SpriteFont>("ItemFont");
            ScoreFont = Globals.content.Load<SpriteFont>("ScoreFont");
            ScoreMultiplier = Globals.content.Load<SpriteFont>("ScoreMultiplier");
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

        public void DrawFrontground(float hp, List<Enemy> enemies)
        {
            Globals.spriteBatch.Begin();
            DrawInventory();
            DrawHealthBar(hp);
            DrawPlayerEffects();
            DrawEnemyHealthBar(enemies);
            DrawScore();
            Globals.spriteBatch.End();
        }

        private void DrawPlayerUpgrades()
        {
            int size = 70;
            int distance = 60;
            int rectX = 1120, rectY = 15;
            Rectangle attackSpeedRect = new Rectangle(rectX, rectY, size, size);
            Rectangle strenghtRect = new Rectangle(rectX + distance, rectY, size, size);
            Rectangle speedRect = new Rectangle(rectX + distance * 2, rectY, size, size);
            Rectangle mintRect = new Rectangle(rectX + distance * 3, rectY, size, size);
            Rectangle nettleSpeedRect = new Rectangle(rectX + distance * 4, rectY, size, size);
            Rectangle appleRect = new Rectangle(rectX + distance * 5, rectY, size, size);

            if (Globals.attackspeedupgrade == 1)
            {
                Globals.spriteBatch.Draw(attackSpeed1, attackSpeedRect, Color.White);
            } else if (Globals.attackspeedupgrade == 2)
            {
                Globals.spriteBatch.Draw(attackSpeed2, attackSpeedRect, Color.White);
            }

            if (Globals.dmgupgrade == 1)
            {
                Globals.spriteBatch.Draw(strenght1, strenghtRect, Color.White);
            }
            else if (Globals.dmgupgrade == 2)
            {
                Globals.spriteBatch.Draw(strenght2, strenghtRect, Color.White);
            }

            if (Globals.mintupgrade == 1)
            {
                Globals.spriteBatch.Draw(mintBuff1, mintRect, Color.White);
            }
            else if (Globals.mintupgrade == 2)
            {
                Globals.spriteBatch.Draw(mintBuff2, mintRect, Color.White);
            }

            if (Globals.appleupgrade == 1)
            {
                Globals.spriteBatch.Draw(appleBuff1, appleRect, Color.White);
            }
            else if (Globals.appleupgrade == 2)
            {
                Globals.spriteBatch.Draw(appleBuff2, appleRect, Color.White);
            }

            if (Globals.nettleupgrade == 1)
            {
                Globals.spriteBatch.Draw(nettleBuff1, nettleSpeedRect, Color.White);
            }
            else if (Globals.nettleupgrade == 2)
            {
                Globals.spriteBatch.Draw(nettleBuff2, nettleSpeedRect, Color.White);
            }

            if (Globals.speedupgrade == 1)
            {
                Globals.spriteBatch.Draw(speedBuff1, speedRect, Color.White);
            }
            else if (Globals.speedupgrade == 2)
            {
                Globals.spriteBatch.Draw(speedBuff2, speedRect, Color.White);
            }

        }

        private void DrawPlayerEffects()
        {
            DrawPlayerUpgrades();
            int posX = WindowWidth / 2 - this.healthBar.Width / 8;
            int rectX = posX - 100;
            Rectangle rect = new Rectangle(rectX, 0, 100, 100);
            for (int i = 0; i < Globals.playerActiveEffects.Count; i++)
            {
                rect.X = rectX - 100 * i;
                if (Globals.playerActiveEffects[i] == "immortal" || Globals.playerActiveEffects[i] == "immortal2")
                {
                    Globals.spriteBatch.Draw(immortalTex, rect, Color.White);
                }
                if (Globals.playerActiveEffects[i] == "haste")
                {
                    Globals.spriteBatch.Draw(speedTex, rect, Color.White);
                }
                if (Globals.playerActiveEffects[i] == "strenght")
                {
                    Globals.spriteBatch.Draw(strenghtTex, rect, Color.White);
                }
                if (Globals.playerActiveEffects[i] == "stun")
                {
                    Globals.spriteBatch.Draw(effectMelise, rect, Color.White);
                }

            }
        }

        private void DrawHealthBar(float hp)
        {
            int posX = WindowWidth / 2 - this.healthBar.Width / 8;
            int calculatedHP = (int)(hp * 300);
            Rectangle health = new Rectangle(posX + 10, 40, calculatedHP, 30);
            Rectangle healthBar = new Rectangle(posX, 0, this.healthBar.Width / 4, this.healthBar.Height / 4);
            Rectangle healthBackdrop = new Rectangle(posX, 0, this.healthBackdrop.Width / 4, this.healthBackdrop.Height / 4);

            Globals.spriteBatch.Draw(this.healthBackdrop, healthBackdrop, Color.White); 
            Globals.spriteBatch.Draw(this.healtColor, health, Color.Magenta);
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

            Rectangle effectRect = new Rectangle(receptureRect.X + 350, receptureRect.Y + 25, 90, 90);
            if (Globals.learnedRecepture[Globals.numberOfRecepture] == "AAA")
            {
                Globals.spriteBatch.Draw(heal1Tex, effectRect, Color.White);
            } 
            else if (Globals.learnedRecepture[Globals.numberOfRecepture] == "AAB")
            {
                Globals.spriteBatch.Draw(heal2Tex, effectRect, Color.White);
            }
            else if (Globals.learnedRecepture[Globals.numberOfRecepture] == "AXY")
            {
                Globals.spriteBatch.Draw(speedTex, effectRect, Color.White);
            }
            else if (Globals.learnedRecepture[Globals.numberOfRecepture] == "XBY")
            {
                Globals.spriteBatch.Draw(strenghtTex, effectRect, Color.White);
            }
            else if (Globals.learnedRecepture[Globals.numberOfRecepture] == "ABX")
            {
                Globals.spriteBatch.Draw(immortalTex, effectRect, Color.White);
            }

        }

        public void DrawScore()
        {
            int scoreX = WindowWidth - (int)ScoreFont.MeasureString(Globals.Score.ToString()).X - 20;
            int scoreY = 20;
            int grayDist = 3, whiteDist = 7;

            string scoreMul = "x" + Globals.ScoreMultiplier.ToString();
            int pointsX = scoreX - (int)ScoreMultiplier.MeasureString(scoreMul.ToString()).X - 26;
            int pointsY = scoreY + 20;

            Globals.spriteBatch.DrawString(ScoreFont, Globals.Score.ToString(), new Vector2(scoreX, scoreY), Color.Black);
            Globals.spriteBatch.DrawString(ScoreFont, Globals.Score.ToString(), new Vector2(scoreX, scoreY - grayDist), Color.Gray);
            Globals.spriteBatch.DrawString(ScoreFont, Globals.Score.ToString(), new Vector2(scoreX, scoreY - whiteDist), Color.White);
            
            Rectangle rect = new Rectangle(pointsX - 13, pointsY - 16, 60, 60);
            Globals.spriteBatch.Draw(multiplierTexture, rect, Color.White);

            Globals.spriteBatch.DrawString(ScoreMultiplier, scoreMul, new Vector2(pointsX, pointsY), Color.Black);
            Globals.spriteBatch.DrawString(ScoreMultiplier, scoreMul, new Vector2(pointsX, pointsY - 2), Color.Gray);
            Globals.spriteBatch.DrawString(ScoreMultiplier, scoreMul, new Vector2(pointsX, pointsY - 4), Color.White);
        }

        public void DrawInventory()
        {
            int invLenght = WindowHeight / 3;
            Rectangle inv = new Rectangle(20, WindowHeight - invLenght - 20, invLenght, invLenght);
            Rectangle rect_crafting = new Rectangle(WindowWidth / 2 - 100, WindowHeight / 2 - 240, 200, 70);
            int craftingIndex = 0;

            Vector2 mintVec;
            Vector2 nettleVec;
            Vector2 meliseVec;
            Vector2 appleVec;
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
                mintVec = new Vector2(invLenght / 2 + 53 - ItemFont.MeasureString(leafs.ElementAt(0).Value.ToString()).X, WindowHeight - 74);
                nettleVec = new Vector2(invLenght + 2 - ItemFont.MeasureString(leafs.ElementAt(1).Value.ToString()).X, WindowHeight - invLenght / 2 - 20);
                meliseVec = new Vector2(125 - ItemFont.MeasureString(leafs.ElementAt(2).Value.ToString()).X, WindowHeight - invLenght / 2 - 22);
                appleVec = new Vector2(invLenght / 2 + 57 - ItemFont.MeasureString(leafs.ElementAt(3).Value.ToString()).X, WindowHeight - invLenght + 55);
            } 
            else if (isThrowing) 
            {
                Globals.spriteBatch.Draw(offensiveItemFrame, inv, Color.White);
                DrawArrow();
                mintVec = new Vector2(invLenght / 2 + 53 - ItemFont.MeasureString(leafs.ElementAt(0).Value.ToString()).X, WindowHeight - 74);
                nettleVec = new Vector2(invLenght + 2 - ItemFont.MeasureString(leafs.ElementAt(1).Value.ToString()).X, WindowHeight - invLenght / 2 - 20);
                meliseVec = new Vector2(125 - ItemFont.MeasureString(leafs.ElementAt(2).Value.ToString()).X, WindowHeight - invLenght / 2 - 22);
                appleVec = new Vector2(invLenght / 2 + 57 - ItemFont.MeasureString(leafs.ElementAt(3).Value.ToString()).X, WindowHeight - invLenght + 55);
            } else
            {
                Globals.spriteBatch.Draw(defaultItemFrame, inv, Color.White);

                mintVec = new Vector2(invLenght / 2 + 50 - ItemFont.MeasureString(leafs.ElementAt(0).Value.ToString()).X, WindowHeight - 70);
                nettleVec = new Vector2(invLenght + 4 - ItemFont.MeasureString(leafs.ElementAt(1).Value.ToString()).X, WindowHeight - invLenght / 2 - 22);
                meliseVec = new Vector2(116 - ItemFont.MeasureString(leafs.ElementAt(2).Value.ToString()).X, WindowHeight - invLenght / 2 - 22);
                appleVec = new Vector2(invLenght / 2 + 50 - ItemFont.MeasureString(leafs.ElementAt(3).Value.ToString()).X, WindowHeight - invLenght + 37);
            }

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
            Rectangle rect = new Rectangle(0, 0, WindowWidth, WindowHeight);
            Color one = Color.Black;
            Color two = Color.Black;
            Color three = Color.Black;
            Vector2 dotpos = new Vector2(0, 0);

            if (MenuOption == 1)
            {
                one = Color.Orange;
                dotpos = new Vector2(WindowWidth * 13 / 20 + 20, WindowHeight * 12 / 20 + 30);
            }
            if (MenuOption == 2)
            {
                two = Color.Orange;
                dotpos = new Vector2(WindowWidth * 13 / 20 + 20, WindowHeight * 14 / 20 + 30);
            }
            if (MenuOption == 3)
            {
                three = Color.Orange;
                dotpos = new Vector2(WindowWidth * 13 / 20 + 20, WindowHeight * 16 / 20 + 30);
            }

            Globals.spriteBatch.Begin();
            Globals.spriteBatch.Draw(difficulty, rect, Color.White);
            Globals.spriteBatch.Draw(dot, dotpos, null, Color.White, 0, Vector2.Zero, 0.4f, SpriteEffects.None, 0f);
            Globals.spriteBatch.DrawString(MenuFont2, "game paused", new Vector2(WindowWidth * 4.5f / 20, WindowHeight * 1 / 20 + 13), new Color(92, 112, 142));
            Globals.spriteBatch.DrawString(MenuFont2, "game paused", new Vector2(WindowWidth * 4.5f / 20, WindowHeight * 1 / 20 + 8), new Color(36, 36, 36));
            Globals.spriteBatch.DrawString(MenuFont2, "game paused", new Vector2(WindowWidth * 4.5f / 20, WindowHeight * 1 / 20), Color.White);

            Globals.spriteBatch.DrawString(MenuFont, "resume", new Vector2(WindowWidth * 14 / 20, WindowHeight * 12 / 20 + 13), one);
            Globals.spriteBatch.DrawString(MenuFont, "resume", new Vector2(WindowWidth * 14 / 20, WindowHeight * 12 / 20 + 5), Color.Gray);
            Globals.spriteBatch.DrawString(MenuFont, "resume", new Vector2(WindowWidth * 14 / 20, WindowHeight * 12 / 20), Color.White);

            Globals.spriteBatch.DrawString(MenuFont, "main menu", new Vector2(WindowWidth * 14 / 20, WindowHeight * 14 / 20 + 13), two);
            Globals.spriteBatch.DrawString(MenuFont, "main menu", new Vector2(WindowWidth * 14 / 20, WindowHeight * 14 / 20 + 5), Color.Gray);
            Globals.spriteBatch.DrawString(MenuFont, "main menu", new Vector2(WindowWidth * 14 / 20, WindowHeight * 14 / 20), Color.White);

            Globals.spriteBatch.DrawString(MenuFont, "exit", new Vector2(WindowWidth * 14 / 20, WindowHeight * 16 / 20 + 13), three);
            Globals.spriteBatch.DrawString(MenuFont, "exit", new Vector2(WindowWidth * 14 / 20, WindowHeight * 16 / 20 + 5), Color.Gray);
            Globals.spriteBatch.DrawString(MenuFont, "exit", new Vector2(WindowWidth * 14 / 20, WindowHeight * 16 / 20), Color.White);
            Globals.spriteBatch.End();
        }

        public void DrawTutorial(int number)
        {
            if(number==1)    //wiktor 2 modul
            {
                Rectangle rect = new Rectangle(WindowWidth * 13 / 20, WindowHeight * 7 / 20, 375, 300);
                Globals.spriteBatch.Begin();
                Globals.spriteBatch.Draw(attack_tutorial, rect, Color.White);
                Globals.spriteBatch.End();
            }
            if(number == 2)
            {
                Rectangle rect = new Rectangle(WindowWidth * 13 / 20, WindowHeight * 3 / 20, 475, 550); //1900 2200
                Globals.spriteBatch.Begin();
                Globals.spriteBatch.Draw(brew_tutorial, rect, Color.White);
                Globals.spriteBatch.End();
            }
            if (number == 4) //efekty miotane pierwsze, co robią efekty miotane (ziel,czer)
            {
                Rectangle rect = new Rectangle(WindowWidth * 13 / 20, WindowHeight * 3 / 20, 500, 450); //2000 1800
                Globals.spriteBatch.Begin();
                Globals.spriteBatch.Draw(special_tutorial, rect, Color.White);
                Globals.spriteBatch.End();
            }

            if (number == 5) //wesel
            {
                Rectangle rect = new Rectangle(WindowWidth * 13 / 20, WindowHeight * 3 / 20, 400, 575); //1600 2300
                Globals.spriteBatch.Begin();
                Globals.spriteBatch.Draw(festival_tutorial, rect, Color.White);
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
            Color one = Color.Black;
            Color two = Color.Black;
            Color three = Color.Black;
            Color four = Color.Black;
            Vector2 dotpos = new Vector2(0,0);

            if (MenuOption == 1)
            {
                one = Color.Orange;
                 dotpos = new Vector2(WindowWidth * 12 / 20 + 20, WindowHeight * 10 / 20 + 30);
            }
            if(MenuOption == 2)
            {
                two = Color.Orange;
                 dotpos = new Vector2(WindowWidth * 12 / 20 + 20, WindowHeight * 12 / 20 + 30);
            }
            if (MenuOption == 3)
            {
                three = Color.Orange;
                dotpos = new Vector2(WindowWidth * 12 / 20 + 20, WindowHeight * 14 / 20 + 30);
            }
            if (MenuOption==4)
            {
                four = Color.Orange;
                 dotpos = new Vector2(WindowWidth * 12 / 20 + 20, WindowHeight * 16 / 20 + 30);
            }

            Rectangle rect = new Rectangle(0, 0, WindowWidth, WindowHeight);
            Globals.spriteBatch.Begin();
            Globals.spriteBatch.Draw(menu, rect, Color.White);
            Globals.spriteBatch.Draw(dot, dotpos, null, Color.White, 0, Vector2.Zero, 0.4f, SpriteEffects.None, 0f);
            Globals.spriteBatch.DrawString(MenuFont, "start game", new Vector2(WindowWidth * 13/ 20 , WindowHeight * 10 / 20+13), one);
            Globals.spriteBatch.DrawString(MenuFont, "start game", new Vector2(WindowWidth * 13 / 20, WindowHeight * 10 / 20+5), Color.Gray);
            Globals.spriteBatch.DrawString(MenuFont, "start game", new Vector2(WindowWidth * 13 / 20, WindowHeight * 10 / 20), Color.White);

            Globals.spriteBatch.DrawString(MenuFont, "credits", new Vector2(WindowWidth * 13 / 20, WindowHeight * 12 / 20 + 13), two);
            Globals.spriteBatch.DrawString(MenuFont, "credits", new Vector2(WindowWidth * 13 / 20, WindowHeight * 12 / 20 + 5), Color.Gray);
            Globals.spriteBatch.DrawString(MenuFont, "credits", new Vector2(WindowWidth * 13 / 20, WindowHeight * 12 / 20), Color.White);

            Globals.spriteBatch.DrawString(MenuFont, "leader board", new Vector2(WindowWidth * 13 / 20, WindowHeight * 14 / 20 + 13), three);
            Globals.spriteBatch.DrawString(MenuFont, "leader board", new Vector2(WindowWidth * 13 / 20, WindowHeight * 14 / 20 + 5), Color.Gray);
            Globals.spriteBatch.DrawString(MenuFont, "leader board", new Vector2(WindowWidth * 13 / 20, WindowHeight * 14 / 20), Color.White);

            Globals.spriteBatch.DrawString(MenuFont, "exit", new Vector2(WindowWidth * 13 / 20, WindowHeight * 16 / 20 + 13), four);
            Globals.spriteBatch.DrawString(MenuFont, "exit", new Vector2(WindowWidth * 13 / 20, WindowHeight * 16 / 20 + 5), Color.Gray);
            Globals.spriteBatch.DrawString(MenuFont, "exit", new Vector2(WindowWidth * 13 / 20, WindowHeight * 16 / 20), Color.White);
            Globals.spriteBatch.End();

        }

        public void DrawTutorialMenu()
        {
            Color one = Color.Black;
            Color two = Color.Black;
            Vector2 dotpos = new Vector2(0, 0);
            Rectangle rect = new Rectangle(0, 0, WindowWidth, WindowHeight);
            Globals.spriteBatch.Begin();
            Globals.spriteBatch.Draw(difficulty, rect, Color.White);

            if (MenuOption == 1)
            {
                one = Color.Orange;
                dotpos = new Vector2(WindowWidth * 15 / 20 + 20, WindowHeight * 10 / 20 + 30);
                Globals.spriteBatch.DrawString(MenuFont3, "if you are a new player, choose this! \n(hints enabled)", new Vector2(WindowWidth * 12 / 20, WindowHeight * 17 / 20 + 5), Color.Black);
                Globals.spriteBatch.DrawString(MenuFont3, "if you are a new player, choose this! \n(hints enabled)", new Vector2(WindowWidth * 12 / 20, WindowHeight * 17 / 20), Color.White);

            }
            if (MenuOption == 2)
            {
                two = Color.Orange;
                dotpos = new Vector2(WindowWidth * 15 / 20 + 20, WindowHeight * 12 / 20 + 30);
                Globals.spriteBatch.DrawString(MenuFont3, "experts only! \n(hints disabled)", new Vector2(WindowWidth * 12 / 20, WindowHeight * 17 / 20 + 5), Color.Black);
                Globals.spriteBatch.DrawString(MenuFont3, "experts only! \n(hints disabled)", new Vector2(WindowWidth * 12 / 20, WindowHeight * 17 / 20), Color.White);

            }
            Globals.spriteBatch.Draw(dot, dotpos, null, Color.White, 0, Vector2.Zero, 0.4f, SpriteEffects.None, 0f);
            Globals.spriteBatch.DrawString(MenuFont2, "choose difficulty", new Vector2(WindowWidth * 3 / 20, WindowHeight * 1 / 20 + 13), new Color(92, 112, 142));
            Globals.spriteBatch.DrawString(MenuFont2, "choose difficulty", new Vector2(WindowWidth * 3 / 20, WindowHeight * 1 / 20 + 8), new Color(36, 36, 36));
            Globals.spriteBatch.DrawString(MenuFont2, "choose difficulty", new Vector2(WindowWidth * 3 / 20, WindowHeight * 1 / 20), Color.White);

            Globals.spriteBatch.DrawString(MenuFont, "EASY", new Vector2(WindowWidth * 16 / 20, WindowHeight * 10 / 20 + 13), one);
            Globals.spriteBatch.DrawString(MenuFont, "EASY", new Vector2(WindowWidth * 16 / 20, WindowHeight * 10 / 20 + 5), Color.Gray);
            Globals.spriteBatch.DrawString(MenuFont, "EASY", new Vector2(WindowWidth * 16 / 20, WindowHeight * 10 / 20), Color.White);

            Globals.spriteBatch.DrawString(MenuFont, "HARD", new Vector2(WindowWidth * 16 / 20, WindowHeight * 12 / 20 + 13), two);
            Globals.spriteBatch.DrawString(MenuFont, "HARD", new Vector2(WindowWidth * 16 / 20, WindowHeight * 12 / 20 + 5), Color.Gray);
            Globals.spriteBatch.DrawString(MenuFont, "HARD", new Vector2(WindowWidth * 16 / 20, WindowHeight * 12 / 20), Color.White);

            Globals.spriteBatch.End();

        }

        public void DrawLeaderBoard()
        {
            Rectangle rect = new Rectangle(0, 0, WindowWidth, WindowHeight);
            Globals.spriteBatch.Begin();
            Globals.spriteBatch.Draw(menu, rect, Color.Black);
            Globals.spriteBatch.DrawString(Menu2, "Type your 'name (company name)' ", new Vector2(WindowWidth / 5, WindowHeight * 2 / 20), Color.Gray);
            Globals.spriteBatch.DrawString(Menu2, name, new Vector2(WindowWidth / 5, WindowHeight * 7 / 20), Color.White);
            Globals.spriteBatch.DrawString(Menu2, "Press 'A / ENTER' to accept", new Vector2(WindowWidth / 5, WindowHeight * 16 / 20), Color.Gray);
            Globals.spriteBatch.End();

        }
        public void DrawCredits()
        {
            Rectangle rect = new Rectangle(0, 0, WindowWidth, WindowHeight);
            Globals.spriteBatch.Begin();
            Globals.spriteBatch.Draw(menu, rect, Color.Black);
            Globals.spriteBatch.DrawString(Menu2, "DEJ NO JESZCZE GRAFIKE", new Vector2(WindowWidth / 5, WindowHeight * 16 / 20), Color.Gray);
            Globals.spriteBatch.End();
        }

        public void DrawLeaderBoardSumup(string leaderboard)
        {
            Rectangle rect = new Rectangle(0, 0, WindowWidth, WindowHeight);
            int leng = 8;
            Globals.spriteBatch.Begin();
            Globals.spriteBatch.Draw(menu, rect, Color.Black);
            Globals.spriteBatch.DrawString(Menu2, "DEJ NO JESZCZE GRAFIKE", new Vector2(WindowWidth / 5, WindowHeight * 16 / 20), Color.Gray);
            string[] lines = leaderboard.Split(new[] { "\n", Environment.NewLine }, StringSplitOptions.None);
            string modifiedInput = "";
            if (lines.Length>= leng)                //trim na linie 
            {
                lines = lines.Take(leng).ToArray(); 
                modifiedInput = string.Join(Environment.NewLine, lines);
            }

            if(lines.Length>=3)         // podium
            {
                string[] podium = lines.Take(3).ToArray();
                Globals.spriteBatch.DrawString(ItemFont, podium[0], new Vector2(WindowWidth * 10 / 20, WindowHeight * 3 / 20 + 8), new Color(92, 112, 142));
                Globals.spriteBatch.DrawString(ItemFont, podium[0], new Vector2(WindowWidth * 10 / 20, WindowHeight * 3 / 20 + 5), new Color(36, 36, 36));
                Globals.spriteBatch.DrawString(ItemFont, podium[0], new Vector2(WindowWidth * 10 / 20, WindowHeight * 3 / 20), Color.White);

                Globals.spriteBatch.DrawString(ItemFont, podium[1], new Vector2(WindowWidth * 3 / 20, WindowHeight * 5 / 20 + 8), new Color(92, 112, 142));
                Globals.spriteBatch.DrawString(ItemFont, podium[1], new Vector2(WindowWidth * 3 / 20, WindowHeight * 5 / 20 + 5), new Color(36, 36, 36));
                Globals.spriteBatch.DrawString(ItemFont, podium[1], new Vector2(WindowWidth * 3 / 20, WindowHeight * 5 / 20), Color.White);

                Globals.spriteBatch.DrawString(ItemFont, podium[2], new Vector2(WindowWidth * 15 / 20, WindowHeight * 7 / 20 + 8), new Color(92, 112, 142));
                Globals.spriteBatch.DrawString(ItemFont, podium[2], new Vector2(WindowWidth * 15 / 20, WindowHeight * 7 / 20 + 5), new Color(36, 36, 36));
                Globals.spriteBatch.DrawString(ItemFont, podium[2], new Vector2(WindowWidth * 15 / 20, WindowHeight * 7 / 20), Color.White);

                lines = lines.Skip(3).ToArray(); 
                modifiedInput = string.Join(Environment.NewLine, lines);
            }

                Globals.spriteBatch.DrawString(ItemFont, modifiedInput, new Vector2(WindowWidth * 10 / 20, WindowHeight * 9 / 20 + 8), new Color(92, 112, 142));
                Globals.spriteBatch.DrawString(ItemFont, modifiedInput, new Vector2(WindowWidth * 10 / 20, WindowHeight * 9 / 20 + 5), new Color(36, 36, 36));
                Globals.spriteBatch.DrawString(ItemFont, modifiedInput, new Vector2(WindowWidth * 10 / 20, WindowHeight * 9 / 20), Color.White);
            
            
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

        private void DrawEnemyHealthBar(List<Enemy> enemies)
        {
            foreach (Enemy e in enemies)
            {
                Vector3 projectedPosition = Globals.viewport.Project(e.GetPosition(),
                Globals.projectionMatrix, Globals.viewMatrix, Matrix.Identity);
                int fakeHealth = e.Health / 2;
                Rectangle rect = new Rectangle(0, 0, 0, 0);


                int effectPosX = 0;
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
                    if ( i == 0 )
                    {
                        effectPosX = rect.X;
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
                    
                    if (fakeHealth == 0)
                    {
                        effectPosX = rect.X;
                    }
                    Globals.spriteBatch.Draw(HalfEnemyHealth, rect, Color.White);
                }

                for (int i = 0; i < e.effectList.Count; i++)
                {
                    Rectangle effectRect = new Rectangle(effectPosX - 30 - i * 30, rect.Y, 30, 30);
                    if (e.effectList[i] == "stun")
                    {
                        Globals.spriteBatch.Draw(effectMelise, effectRect, Color.White);
                    }
                    if (e.effectList[i] == "slow")
                    {
                        Globals.spriteBatch.Draw(effectMint, effectRect, Color.White);
                    }
                    if (e.effectList[i] == "nettle")
                    {
                        Globals.spriteBatch.Draw(effectNettle, effectRect, Color.White);
                    }
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