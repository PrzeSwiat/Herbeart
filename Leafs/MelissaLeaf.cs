using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame.Leafs
{
    internal class MelissaLeaf : Leaf
    {
        public MelissaLeaf(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {
        }

        public override void AddToInventory(Player player)
        {
           
                player.Inventory.addMelissaLeaf();
                Debug.Write("Dodalem Melise" + player.Inventory.getMellisaLeafNumber() + "\n");
                this.RemoveFromWorld();
                ispossible = false;
            
        }

        public override bool UpdateInventory(Player player)
        {
            if (this.chceckCollison(player))
            {
                //Debug.Write("przecina\n");
                this.AddToInventory(player);
                return true;
            }
            return false;
        }
    }
}
