using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Assimp.Metadata;

namespace TheGame.Leafs
{
    internal class Leaf : SceneObject
    {
        public Texture2D Mint_Icon, Nettle_Icon, Apple_Icon, Melissa_Icon;
        public event EventHandler OnDestroy;
        public event EventHandler OnCreate;
        public bool ispossible = true;
        public bool canStartAnimation = false;
        Vector3 velocity = Vector3.Zero;
        public Leaf(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {

        }

        public override void LoadContent()
        {
            base.LoadContent();
            Apple_Icon = Globals.content.Load<Texture2D>("Animation2D/ikona_japco");
            Melissa_Icon = Globals.content.Load<Texture2D>("Animation2D/ikona_melisa");
            Mint_Icon = Globals.content.Load<Texture2D>("Animation2D/ikona_mieta");
            Nettle_Icon = Globals.content.Load<Texture2D>("Animation2D/ikona_pokrzywa");
        }

        public virtual void AddToInventory(Player player)
        {

        }
        public void updatePosition(Player player)
        {
            Vector3 direction =  this.GetPosition()- player.GetPosition();
            Vector3.Normalize(direction);   
            velocity += direction * 0.002f ;
            if (velocity.X > 30)
            {
                velocity.X = 30;
            }
             if (velocity.Y > 30)
            {
                velocity.Y = 30;
            }
             if(velocity.Z > 30)
            {
                velocity.Z = 30;
            }
            this.SetPosition(this.GetPosition()-velocity);
            this.boundingBox.Min -= velocity;
            this.boundingBox.Max -= velocity;
            
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
        public virtual bool UpdateInventory(Player player,List<Animation2D> AnimationsList)
        {
            if (this.chceckCollison(player))
            {
                this.AddToInventory(player);
                canStartAnimation = true;
                return true;
            }
            return false;

        }

        public static explicit operator Leaf(Type v)
        {
            throw new NotImplementedException();
        }
    }
}
