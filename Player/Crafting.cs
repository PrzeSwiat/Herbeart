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
        public string recepture;
        public Crafting() { 
        recepture = string.Empty;
        }
        public void addA()
        {
            recepture += "A";
        }
        public void addB() {  recepture += "B";}
        public void addY() {  recepture += "Y";}
        public void addX() { recepture += "X"; }
        public void restartRecepture() { recepture = string.Empty; }
        public void makeTea(Inventory inventory)
        {
            if(recepture == "ABY") {
                Debug.Write("Na uspokojenie" + "\n");
            }else if(recepture == "ABX")
            {
                Debug.Write("Na stres" + "\n");
            }
            else
            {
                foreach(char c in recepture) {
                if(c == 'A') inventory.addNettleLeaf();  
                else if (c == 'B') inventory.addMintLeaf();
                else if (c == 'Y') inventory.addAppleLeaf();
                else if (c == 'X') inventory.addMelissaLeaf();
                    
                }
                
            }

            restartRecepture();
        }
        public int howLong()
        {
            return recepture.Length;
        }
    }
}
