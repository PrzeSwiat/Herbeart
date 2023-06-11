﻿using Assimp.Configs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class Crafting
    {
        private string recepture;
        private string[] recepture2 = new string[3];
        private PlayerEffectHandler playerEffects;
        private Inventory inventory;
        public Texture2D Mint_Icon, Nettle_Icon, Apple_Icon, Melissa_Icon;
        public List<Animation2D> animationList = new List<Animation2D>();
        public Crafting(Inventory inv, PlayerEffectHandler playerEffects) 
        { 
            recepture = string.Empty;
            this.inventory = inv;
            this.playerEffects = playerEffects;
            
        }
        public void LoadContent()
        {
            Apple_Icon = Globals.content.Load<Texture2D>("Animation2D/ikona_japco");
            Melissa_Icon = Globals.content.Load<Texture2D>("Animation2D/ikona_melisa");
            Mint_Icon = Globals.content.Load<Texture2D>("Animation2D/ikona_mieta");
            Nettle_Icon = Globals.content.Load<Texture2D>("Animation2D/ikona_pokrzywa");
        }

        public void addIngredient(char button)
        {
            switch (button)
            {
                case 'A':
                    if (inventory.checkMintLeafNumber())
                    {
                        recepture2[recepture.Length] = "mint";
                        recepture += button;
                        animationList.Add(new Animation2D(Mint_Icon, new Microsoft.Xna.Framework.Vector2(150,900), new Microsoft.Xna.Framework.Vector2(850 + recepture.Length * 45, 350), 1, Globals.viewport));
                        inventory.removeMintLeaf();
                    }
                    break;
                case 'B':
                    if (inventory.checkNettleLeafNumber())
                    {
                        recepture2[recepture.Length] = "nettle";
                        recepture += button;
                        animationList.Add(new Animation2D(Nettle_Icon, new Vector2(250, 815), new Microsoft.Xna.Framework.Vector2(850 + recepture.Length * 45, 350), 1, Globals.viewport));
                        inventory.removeNettleLeaf();
                    }
                    break;
                case 'X':
                    if (inventory.checkMeliseLeafNumber())
                    {
                        recepture2[recepture.Length] = "melise";
                        recepture += button;
                        animationList.Add(new Animation2D(Melissa_Icon, new Vector2(80, 815), new Microsoft.Xna.Framework.Vector2(850 + recepture.Length * 45, 350), 1, Globals.viewport));
                        inventory.removeMeliseLeaf();
                    }
                    break;
                case 'Y': 
                    if (inventory.checkAppleLeafNumber())
                    {
                        recepture2[recepture.Length] = "apple";
                        recepture += button;
                        animationList.Add(new Animation2D(Apple_Icon, new Vector2(150, 700), new Microsoft.Xna.Framework.Vector2(850 + recepture.Length * 45, 350), 1, Globals.viewport));
                        inventory.removeAppleLeaf();
                    }
                    
                    break;
                default:
                    break;
            }
            if (howLong() == 3)
            {
                makeTea();
            }
        }

        public string[] returnRecepture()
        {
            return this.recepture2;
        }

        public void restartRecepture() { recepture = string.Empty; recepture2 = new string[3]; }

        public void cleanRecepture()
        {
            foreach (char c in recepture)
            {
                if (c == 'A') inventory.addMintLeaf();
                else if (c == 'B') inventory.addNettleLeaf();
                else if (c == 'Y') inventory.addAppleLeaf();
                else if (c == 'X') inventory.addMeliseLeaf();
            }

            restartRecepture();
        }
        
        private void makeTea()
        {
            switch (recepture)
            {
                case "AAB":
                    playerEffects.RegenarateHP(200);
                    break;

                case "AXY":
                    playerEffects.RegenarateHP(150);
                    playerEffects.Haste(10, 10);
                    break;
                case "XBY":
                    playerEffects.RegenarateHP(100);
                    playerEffects.BuffStrenght(3, 10);
                    break;
                case "ABX":
                    playerEffects.RegenarateHP(100);
                    playerEffects.MakeImmortal(10);
                    break;

                default:
                    foreach (char c in recepture)
                    {
                        if (c == 'A') inventory.addMintLeaf();
                        else if (c == 'B') inventory.addNettleLeaf();
                        else if (c == 'Y') inventory.addAppleLeaf();
                        else if (c == 'X') inventory.addMeliseLeaf();
                    }
                    break;
                
            }

            restartRecepture();
        }

        public string getRecepture()
        {
            return this.recepture;
        }

        public int howLong()
        {
            return recepture.Length;
        }
        public void Update(GameTime gametime)
        {
            foreach (Animation2D anim in animationList.ToList())
            {
                anim.Update(gametime,true);
                anim.OnDestroy += DestroyAnimation;
            }
        }
        public void DrawAnimation(SpriteBatch spriteBatch)
        {
            foreach(Animation2D anim in animationList)
            {
                anim.Draw(spriteBatch);
            }
        }
        private void DestroyAnimation(object obj, EventArgs e)
        {
            animationList.Remove((Animation2D)obj);
        }
    }
}
