using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame.Leafs
{
    internal class MintLeaf : Leaf
    {
        public MintLeaf(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {

        }

        public override void AddToInventory(Player player)
        {         
                player.Inventory.addMintLeaf();
                Debug.Write("Dodalem miete" + player.Inventory.getMintLeafNumber() + "\n");
                this.RemoveFromWorld(); 
                ispossible = false;
        }

        public override bool UpdateInventory(Player player)
        {
            if (this.chceckCollison(player))
            {
                this.AddToInventory(player);
                return true;
            }
            return false;
        }
    }
}
