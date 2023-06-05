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

        private DateTime lastAttackTime, actualTime;
        public event EventHandler OnAttack;

        public Mint(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {
            AssignParameters(75, 12, 2, 0.5f);
            this.shadow.SetScale(0.7f);
            this.leaf = new Leafs.MintLeaf(worldPosition, "Objects/mis4", "Textures/StarSparrow_Orange");
        }
        public override void Update(float deltaTime, Player player)
        {
            base.Update(deltaTime, player);
            this.shadow.UpdatingEnemy(this.GetPosition(), new Vector3(1.2f,0,-1));
            this.shadow.SetPositionY(-0.5f);

        }

        protected override void Attack(Player player)
        {
            actualTime = DateTime.Now;
            TimeSpan time = actualTime - lastAttackTime;
            if (time.TotalSeconds > 1)
            {
                OnAttack?.Invoke(this, EventArgs.Empty);
                lastAttackTime = actualTime;
                player.Hit(this.Strength);
            }
        }
    }
}
