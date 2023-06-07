using Assimp.Configs;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

        public Crafting(Inventory inv, PlayerEffectHandler playerEffects) 
        { 
            recepture = string.Empty;
            this.inventory = inv;
            this.playerEffects = playerEffects;
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
                        inventory.removeMintLeaf();
                    }
                    break;
                case 'B':
                    if (inventory.checkNettleLeafNumber())
                    {
                        recepture2[recepture.Length] = "nettle";
                        recepture += button;
                        inventory.removeNettleLeaf();
                    }
                    break;
                case 'X':
                    if (inventory.checkMeliseLeafNumber())
                    {
                        recepture2[recepture.Length] = "melise";
                        recepture += button;
                        inventory.removeMeliseLeaf();
                    }
                    break;
                case 'Y': 
                    if (inventory.checkAppleLeafNumber())
                    {
                        recepture2[recepture.Length] = "apple";
                        recepture += button;
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
    }
}
