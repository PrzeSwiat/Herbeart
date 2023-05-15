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
                    if (inventory.checknettleLeafNumber())
                    {
                        recepture += button;
                        inventory.removeNettleLeaf();
                    }
                    break;
                case 'B':
                    if (inventory.checkmintLeafNumber())
                    {
                        recepture += button;
                        inventory.removeMintLeaf();
                    }
                    break;
                case 'X':
                    if (inventory.checkMelissaLeafNumber())
                    {
                        recepture += button;
                        inventory.removeMelissaLeaf();
                    }
                    break;
                case 'Y': 
                    if (inventory.checkappleLeafNumber())
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
        
        public void makeTea(Inventory inventory)
        {
            switch (recepture)
            {
                case "ABY":
                    playerEffects.RegenarateHP(100);
                    break;

                case "ABX":
                    playerEffects.RegenarateHP(150);
                    playerEffects.Haste(10, 10);
                    break;

                default:
                    foreach (char c in recepture)
                    {
                        if (c == 'A') inventory.addNettleLeaf();
                        else if (c == 'B') inventory.addMintLeaf();
                        else if (c == 'Y') inventory.addAppleLeaf();
                        else if (c == 'X') inventory.addMelissaLeaf();

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
