using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGame.Leafs;

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
        List<Leaf> leavesToRemove = new List<Leaf>();

      

        private void DestroyControl(object obj, EventArgs e)
        {
            LeafsList.Remove((Leaf)obj);

        }
        private void CreateControl(object obj, EventArgs e)
        {
            Enemy enemy = (Enemy)obj;
            LeafsList.Add(enemy.GetLeaf());
            //Debug.Write(LeafList.Count + "\n");

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
                enemy.OnDestroy += CreateControl;
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
