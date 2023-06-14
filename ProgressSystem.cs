using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        public bool canDraw = false;
        public int MenuOption = 1;
        public int[] rewards = { 0, 0, 0 };
        Texture2D Reward1,Reward2,Reward3;

        public ProgressSystem(Player player)
        {
            this.player = player;
        }




        public void drawSelectMenu()
        {

            if (rewards[0] == 0 && rewards[1] == 0 && rewards[2] == 0)
            {
                rewards = generateRewards();
            }

            if (rewards[0] == 1 || rewards[0] == 2 || rewards[0] == 3)
            {
                //  Reward1=tekstura Nowej herbatki
            }
            else
            {
                //  Reward1=tekstura Zestawow Listkow
            }

            if (rewards[1] == 1)
            {
                // Reward2=tekstura dodatkowego zycka
            }
            else if (rewards[1] == 2)
            {
                // Reward2=tekstura dodatkowego Atacka
            }
            else if (rewards[1] == 3)
            {
                // Reward2=tekstura dodatkowego speeda
            }
            else
            {
                // Reward2=tekstura dodatkowego attackspeeda
            }

            if (rewards[2]== 1)
            {
                //Reward3=AddmultipleScore
            }
            else if (rewards[2] == 2)
            {
                //Reward3=AddmultipleScore
            }
            else if (rewards[2] == 3)
            {
                //Reward3=AddMintLeafBuff
            }
            else if (rewards[2] == 4)
            {
                //Reward3=AddAppleLeafBuff
            }
            else if (rewards[2] == 5)
            {
                //Reward3=AddNettleLeafBuff
            }




            if (canDraw)
            {
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                KeyboardState state = Keyboard.GetState();
                Globals.TutorialPause = true;
                if ((gamePadState.ThumbSticks.Left.X >= 0.5f && !(Globals.prevPauseState.ThumbSticks.Left.X >= 0.5f)) || (state.IsKeyDown(Keys.D) && !Globals.prevKeyBoardPauseState.IsKeyDown(Keys.D)))
                {
                    MenuOption -= 1;
                }
                if ((gamePadState.ThumbSticks.Left.X <= -0.5f && !(Globals.prevPauseState.ThumbSticks.Left.X <= -0.5f)) || (state.IsKeyDown(Keys.A) && !Globals.prevKeyBoardPauseState.IsKeyDown(Keys.A)))
                {
                    MenuOption += 1;
                }
                if (MenuOption < 1)
                {
                    MenuOption = 1;
                }
                if (MenuOption > 3)
                {
                    MenuOption = 3;
                }
                Color one = Color.Gray;
                Color two = Color.Gray;
                Color three = Color.Gray;

                if (MenuOption == 1)
                {
                    one = Color.White;
                }
                if (MenuOption == 2)
                {
                    two = Color.White;
                }
                if (MenuOption == 3)
                {
                    three = Color.White;
                }

                if ((gamePadState.Buttons.A == ButtonState.Pressed && Globals.prevPauseState.Buttons.A == ButtonState.Released || (state.IsKeyDown(Keys.Enter)) && Globals.prevKeyBoardPauseState.IsKeyUp(Keys.Enter)) && MenuOption == 1)
                {
                   canDraw = false;
                   Globals.TutorialPause = false;
                }
                if ((gamePadState.Buttons.A == ButtonState.Pressed && Globals.prevPauseState.Buttons.A == ButtonState.Released || (state.IsKeyDown(Keys.Enter)) && Globals.prevKeyBoardPauseState.IsKeyUp(Keys.Enter)) && MenuOption == 2)
                {
                    canDraw = false;
                    Globals.TutorialPause = false;
                }
                if ((gamePadState.Buttons.A == ButtonState.Pressed && Globals.prevPauseState.Buttons.A == ButtonState.Released || (state.IsKeyDown(Keys.Enter)) && Globals.prevKeyBoardPauseState.IsKeyUp(Keys.Enter)) && MenuOption == 3)
                {
                    canDraw = false;
                    Globals.TutorialPause = false;
                }
                Globals.spriteBatch.Begin();
                //Globals.spriteBatch.Draw(texture, position, null, one, ang, Vector2.Zero, scale, SpriteEffects.None, 0f);
                //Globals.spriteBatch.Draw(texture, position, null, two, ang, Vector2.Zero, scale, SpriteEffects.None, 0f);
                //Globals.spriteBatch.Draw(texture, position, null, three, ang, Vector2.Zero, scale, SpriteEffects.None, 0f);
                Globals.spriteBatch.End();
            }
            
        }



        public int[] generateRewards()
        {
            int valuefirst = random.Next(1, 4 + 1);
            int valueseccond = random.Next(1, 4 + 1);
            int valuethird = random.Next(1, 3 + 1);

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
