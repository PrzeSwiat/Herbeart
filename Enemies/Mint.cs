using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class Mint : Enemy
    {
        public Mint(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {
            AssignParameters(100, 10, 2);
            this.leaf = new Leafs.MintLeaf(worldPosition, "mis4", "StarSparrow_Orange");
        }
    }
}
