using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGame.Core;
using Assimp.Unmanaged;

namespace TheGame
{
    

    internal class Shadow:SceneObject
    {    

        public Shadow(Vector3 position) : base(position, "Objects/test", "Textures/Shadow")
        {
            this.SetPosition(position);
            SetPositionY(-1f);
        }
        
        public void UpdatingPlayer(Vector3 pos)
        {
            this.SetPosition(pos+new Vector3 (1.8f,0f,-1.9f));
            this.SetPositionY(-1f);
        }
        public void UpdatingEnemy(Vector3 pos, Vector3 correctPos)
        {
            this.SetPosition(pos + correctPos);
            this.SetPositionY(-1f);
        }
        public void Draw()
        {
            this.DrawPlayer(Vector3.Zero);
        }
    }
}
