using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class ProgressSystem
    {
        Player player;
        private static readonly Random random = new Random();
        public ProgressSystem(Player player)
        {
            this.player = player;
        }




        public void drawSelectMenu()
        {

        }



        public int[] generateRewards()
        {
            int valuefirst = random.Next(0, 3 + 1);
            int valueseccond = random.Next(0, 3 + 1);
            int valuethird = random.Next(0, 2 + 1);

            return new int[] { valuefirst, valueseccond, valuethird };

            
            
        }

        public void unlockRecepture()
        {
            int value = random.Next(0, player.Crafting.bools.Count());
            if (!player.Crafting.bools[value])
            {
                player.Crafting.bools[value] = true;
            }
            else
            {
                value = random.Next(0, player.Crafting.bools.Count());
            }
            
        }

        public void AddMultipleScore()
        {
            Globals.ScoreMultipler += 1;
        }
        public void RemoveMultipleScore() 
        {
            Globals.ScoreMultipler -= 1;
        }
        public void AddScore(int score) 
        {
            Globals.Score += score;
        }
        public void addMaxHealth(int health)
        {
            player.MaxHealth += health;
            player.Health += health;
        }
        public void addStrenght(int str) 
        {
            player.Strength += str;
            player.MaxStrength += str;
        }
        public void addAttackSpeed(int speed)
        {
            player.AttackSpeed += speed;
            player.ActualAttackSpeed += speed;
        }

        public void addSpeed(int speed)
        {
            player.ActualSpeed += speed;
            player.MaxSpeed += speed;
        }

        public void addLeafs(int amount)
        {
            for(int i = 0; i < amount; i++)
            {
                player.Inventory.addMintLeaf();
                player.Inventory.addNettleLeaf();
                player.Inventory.addMeliseLeaf();
                player.Inventory.addAppleLeaf();
            }
        }
        public void addNettleValue()
        {
            player.NettleValue+= 1;
            player.NettleTime+= 3;
        }
        public void addMintValue()
        {
            player.MintValue -= 0.3f;
            player.NettleTime += 5;
        }
        public void addAppleValue()
        {
            player.appleValue = 10;
        }

    }
}
