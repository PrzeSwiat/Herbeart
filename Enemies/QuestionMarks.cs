using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class Bush : Enemy
    {
        bool isAttacked = false;
        public Bush(Vector3 worldPosition) : base(worldPosition, "Objects/test", "Textures/Shadow")
        {
            AssignParameters(1, 0, 0, 0);
            this.leaf = new Leafs.MelissaLeaf(worldPosition, "Objects/melise_pickup", "Textures/melise_pickup");
        }
        public override void Update(float deltaTime, Player player)
        {
                base.Update(deltaTime, player);
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public void dropLeafs(object obj, EventArgs e)
        {

        }
    }
    internal class Chest : Enemy
    {
        public Chest(Vector3 worldPosition) : base(worldPosition, "Objects/test", "Textures/Shadow")
        {

            AssignParameters(1, 0, 0, 0);
            this.leaf = new Leafs.MelissaLeaf(worldPosition, "Objects/melise_pickup", "Textures/melise_pickup");
        }

        public override void Update(float deltaTime, Player player)
        {
            base.Update(deltaTime, player);
        }

    }
}
