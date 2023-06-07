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
           
                player.Inventory.addMeliseLeaf();
                //Debug.Write("Dodalem Melise" + player.Inventory.getMellisaLeafNumber() + "\n");
                this.RemoveFromWorld();
                ispossible = false;
            
        }

        public override bool UpdateInventory(Player player,List<Animation2D> AnimationsList)
        {
            if (this.chceckCollison(player))
            {
                AnimationsList.Add(new Animation2D(Melissa_Icon, this.GetPosition(), new Vector2(80, 815), 1, Globals.viewport));
                this.AddToInventory(player);
                return true;
            }
            return false;
        }
    }
}
