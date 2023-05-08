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
        List<Leaf> leavesToRemove = new List<Leaf>();

      

        private void DestroyControl(object obj, EventArgs e)
        {
            LeafsList.Remove((Leaf)obj);

        }
        private void CreateControl(object obj, EventArgs e)
        {
            //Enemy enemy = (Enemy)obj;
            //LeafsList.Add(enemy.GetLeaf());
             Debug.Write(obj.GetType() + "\n");

            /*Leaf leaf = null;
            if(obj.GetType() == typeof(MelissaLeaf))
            {
                leaf = (MelissaLeaf)obj;
            }
            if (obj.GetType() == typeof(AppleLeaf))
            {
                leaf = (AppleLeaf)obj;
            }
            if (obj.GetType() == typeof(MintLeaf))
            {
                leaf = (MintLeaf)obj;
            }
            if (obj.GetType() == typeof(NettleLeaf))
            {
                leaf = (NettleLeaf)obj;
            }

            LeafsList.Add(leaf);
            /*
            foreach (Leaf leaf1 in LeafsList)
            {
                leaf1.LoadContent(ContentManager);
            }
            */
             //Debug.Write(LeafsList.Count + "\n");
        }


        public void LoadModels(ContentManager content)
        {
            ContentManager = content;
            foreach (Leaf leaf in LeafsList)
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

        public void RefreshInventory(Player player)
        {
            foreach (Leaf leaf in LeafsList)
            {
                leaf.UpdateInventory(player);

                //leaf.OnDestroy += DestroyControl;

            }
        }

        public void RefreshOnCreate(List<Enemy> enemies)
        {
            foreach (Enemy enemy in enemies.ToList())
            {
                enemy.leaf.OnCreate += CreateControl;
            }
        }

        public void RefreshOnDestroy()
        {
            foreach(Leaf leaf in LeafsList.ToList())
            {
                
                leaf.OnDestroy += DestroyControl;
            }
        }
    }
}
