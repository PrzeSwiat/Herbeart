using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGame.Leafs;
using System.Diagnostics;

namespace TheGame
{
    internal class LeafList
    {
        private List<Leaf> LeafsList;

        public LeafList()
        {
            LeafsList = new List<Leaf>();
        }

        ContentManager ContentManager;




        private void DestroyControl(object obj, EventArgs e)
        {
            LeafsList.Remove((Leaf)obj);

        }
        private void CreateControl(object obj, EventArgs e)
        {
            Enemy enemy = (Enemy)obj;
            
            
                LeafsList.Add(enemy.GetLeaf());

                
            foreach (Leaf leaf in LeafsList)
            {
                leaf.LoadContent(ContentManager);
            }

        }


        public void LoadModels(ContentManager content)
        {
            ContentManager = content;
            foreach (Leaf leaf in LeafsList.ToArray())
            {

                leaf.LoadContent(ContentManager);

            }
        }

        public void addLeaf(Leaf leaf)
        {

            LeafsList.Add(leaf);

        }


        public void Draw(EffectHandler effectHandler, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix, ContentManager content)
        {
            foreach (Leaf leaf in LeafsList)
            {
                leaf.Draw(effectHandler, worldMatrix, viewMatrix, projectionMatrix, leaf.color);

            }
        }
        public void UpdateScene(List<Enemy> enemies)
        {
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
                leaf.UpdateInventory(player);

                leaf.OnDestroy += DestroyControl;

            }
        }
    }
}
