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
        private PlayerEffectHandler playerEffects;

        public Crafting(PlayerEffectHandler playerEffects) 
        { 
            recepture = string.Empty;
            this.playerEffects = playerEffects;
        }

        public void addIngredient(char button, Inventory inventory)
        {
            switch (button)
            {
                case 'A':
                    if (inventory.checkMintLeafNumber())
                    {
                        recepture += button;
                        inventory.removeMintLeaf();
                    }
                    break;
                case 'B':
                    if (inventory.checkNettleLeafNumber())
                    {
                        recepture += button;
                        inventory.removeNettleLeaf();
                    }
                    break;
                case 'X':
                    if (inventory.checkMeliseLeafNumber())
                    {
                        recepture += button;
                        inventory.removeMeliseLeaf();
                    }
                    break;
                case 'Y': 
                    if (inventory.checkAppleLeafNumber())
                    {
                        recepture += button;
                        inventory.removeAppleLeaf();
                    }
                    
                    break;
                default:
                    break;
            }
            if (howLong() == 3)
            {
                makeTea(inventory);
            }
        }

        public void restartRecepture() { recepture = string.Empty; }

        public void cleanRecepture(Inventory inventory)
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
        
        private void makeTea(Inventory inventory)
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
                    playerEffects.BuffStrenght(20, 20);
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
