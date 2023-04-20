using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class AppleTree : Enemy
    {
        float PossibledistanceToPlayer = 20f;
        bool canCorrectPosition = true;
        float timecounter = 0;
        public AppleTree(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {

        }

        public override void Update(float deltaTime, Player player)
        {
            Update();
            
            Vector3 difference = this.GetPosition() - player.GetPosition();
            float distance = difference.LengthSquared();
            timecounter -= deltaTime;
            //Debug.Write(timecounter.ToString() + "\n");

            if (timecounter > 0)
            {
                canCorrectPosition = false;
            }
            else if (timecounter <= 0)
            {
                canCorrectPosition = true;
            }

            if (canCorrectPosition)
            { 
                if (PossibledistanceToPlayer * PossibledistanceToPlayer < distance )
                {
                    this.FollowPlayer(player.GetPosition(), deltaTime, 0);
                    difference = this.GetPosition() - player.GetPosition();
                    distance = difference.LengthSquared();

                    if (PossibledistanceToPlayer * PossibledistanceToPlayer >= distance) {
                            timecounter = 10f;
                        }

                }
            else if(PossibledistanceToPlayer * PossibledistanceToPlayer >= distance ) {


                    this.FollowPlayer(player.GetPosition(), deltaTime,1);
                    difference = this.GetPosition() - player.GetPosition();
                    distance = difference.LengthSquared();
                    if (PossibledistanceToPlayer * PossibledistanceToPlayer <= distance)
                    {
                        timecounter = 10f;
                    }
                }
            }
                
            
            

        }
    }
}
