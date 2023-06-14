using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame.Leafs
{
    internal class AppleLeaf : Leaf
    {

        public AppleLeaf(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {

        }
        public override void AddToInventory(Player player)
        {
            player.Inventory.addAppleLeaf();
            this.RemoveFromWorld();
            ispossible = false;
        }

        public override bool UpdateInventory(Player player,List<Animation2D> AnimationsList)
        {
            Vector3 rotation = GetRotation();
            rotation.Y += 0.05f;
            SetRotation(rotation);
            if (this.chceckCollison(player))
            {
                AnimationsList.Add(new Animation2D(Apple_Icon, this.GetPosition(), new Vector2(150, 700), 1, Globals.viewport));
                this.AddToInventory(player);
                return true;
            }
        return false;
        }
    }
}
