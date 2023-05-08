using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame.Leafs
{
    internal class Leaf : SceneObject
    {
        public event EventHandler OnDestroy;
        public event EventHandler OnCreate;
        public bool ispossible = true;
        public Leaf(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {

        }
        public virtual void AddToInventory(Player player)
        {

        }

        public void RemoveFromWorld()
        {
            OnDestroy?.Invoke(this, EventArgs.Empty);
        }
        public void AddToWorld()
        {
            OnCreate?.Invoke(this, EventArgs.Empty);
        }
        public bool chceckCollison(Player player)
        {
            if (this.boundingBox.Intersects(player.boundingBox))
            {
                return true;
            }

            return false;
        }
        public virtual void UpdateInventory(Player player)
        {
            //Update();

            if (this.chceckCollison(player))
            {
                //Debug.Write("przecina\n");
                this.AddToInventory(player);
            }

        }
    }
}
