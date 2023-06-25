using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGame.Leafs;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame
{
    internal class LeafList
    {
        
        private List<Leaf> LeafsList;
        public List<Animation2D> AnimationsList;
        public LeafList()
        {
            LeafsList = new List<Leaf>();
            AnimationsList = new List<Animation2D>();
        }

        private void DestroyControl(object obj, EventArgs e)
        {
            LeafsList.Remove((Leaf)obj);

        }
        private void DestroyAnimation(object obj,EventArgs e)
        {
            AnimationsList.Remove((Animation2D)obj);
        }
        private void CreateAnimation(object obj,EventArgs e)
        {
            AnimationsList.Add((Animation2D)obj);
        }

        private void CreateControl(object obj, EventArgs e)
        {
            Enemy enemy = (Enemy)obj;
            LeafsList.Add(enemy.GetLeaf());
            foreach (Leaf leaf in LeafsList)
            {
                leaf.LoadContent();
            }

        }
        public void LoadModels()
        {
            
            foreach (Leaf leaf in LeafsList.ToArray())
            {

                leaf.LoadContent();

            }
        }

        public void addLeaf(Leaf leaf)
        {

            LeafsList.Add(leaf);

        }


        public void Draw(Vector3 lightpos)
        {
            foreach (Leaf leaf in LeafsList)
            {
                leaf.Draw(lightpos);

            }
        }
        public void DrawHud()
        {
            
            foreach(Animation2D anim in AnimationsList)
            {
                anim.Draw();
            }
            
        }
        public void UpdateScene(List<Enemy> enemies,GameTime gametime)
        {
            foreach (Animation2D anim in AnimationsList.ToList())
            {
                anim.Update(gametime,false);
                anim.OnDestroy += DestroyAnimation;
                
            }
            foreach (Enemy enemy in enemies)
            {
                if (enemy.canDestroy == true)
                {
                    enemy.OnDestroy += CreateControl;

                    enemy.canDestroy = false;
                }
            }
        }
        public void RefreshInventory(Player player)
        {
           
            foreach (Leaf leaf in LeafsList.ToList())
            {
                leaf.updatePosition(player);
                leaf.UpdateInventory(player,AnimationsList);

                leaf.OnDestroy += DestroyControl;

            }
        }
    }
}
