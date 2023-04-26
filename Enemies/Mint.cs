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
        private int attaccounter = 0;
        public Mint(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {
        }
        public override void Update(float deltaTime, Player player)
        {
            Update();
            if (!(this.boundingSphere.Intersects(player.boundingBox)))
            { FollowPlayer(player.GetPosition(), deltaTime, 0); }
            else
            {


                {
                    actualTime = DateTime.Now;
                    TimeSpan time = actualTime - lastAttackTime;
                    if (time.TotalSeconds > 1)
                    {
                        if(attaccounter == 5)
                        {
                            attaccounter = 0;
                            player.setStun(false);
                        }
                        OnAttack?.Invoke(this, EventArgs.Empty);
                        lastAttackTime = actualTime;
                        attaccounter++;
                        player.Hit(5);
                    }

                }
            }

        }
    }
}
