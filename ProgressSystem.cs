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
        int attackspeedcounter, dmgcounter, speedcounter;
        int nettlecounter,applecounter,Melissacounter;
        public ProgressSystem(Player player)
        {
            this.player = player;
            attackspeedcounter = 0;
            dmgcounter = 0;
            speedcounter = 0;
        }


        public void checkGeneratedRewards()
        {
            if (rewards[0] == 0 && rewards[1] == 0 && rewards[2] == 0)
            {
                rewards = generateRewards();
            }

            if (rewards[0] == 1 || rewards[0] == 2 || rewards[0] == 3)
            {
                if (isRecepturetoUnlock()) 
                {
                    //  Reward1=tekstura Nowej herbatki
                }
                else
                {
                    //  Reward1=tekstura Zestawow Listkow
                }

            }
            if (rewards[0]==4)
            {
                //  Reward1=tekstura Zestawow Listkow
            }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (rewards[1] == 1)
            {
                if(attackspeedcounter <3)
                {
                    // Reward2=tekstura dodatkowego attackspeeda
                }
                else
                {
                    rewards[1]++;
                }
            }
            if (rewards[1] == 2)
            {
                if(dmgcounter<3)
                {
                    // Reward2=tekstura dodatkowego Atacka
                }
                else
                {
                    rewards[1]++;
                }

            }
             if (rewards[1] == 3)
            {
                if(speedcounter<3)
                {
                    // Reward2=tekstura dodatkowego speeda
                }
                else
                {
                    rewards[1]++;
                }

            }
            if (rewards[1] == 4)
            {
                // Reward2=tekstura dodatkowego zycka
                
            }
///////////////////////////////////////////////////////////////////////////////////
            if (rewards[2] == 1)
            {
                if (nettlecounter < 3)
                {
                    //Reward3=AddNettleLeafBuff
                }
                else
                {
                    rewards[2]++;
                }
                
            }
            if (rewards[2] == 2)
            {
                if(applecounter < 3)
                {
                    //Reward3=AddAppleLeafBuff
                }
                else
                {
                    rewards[1]++;
                }
                
            }
            if (rewards[2] == 3)
            {
                if (Melissacounter < 3)
                {
                    //Reward3=AddMelissaLeafBuff
                }
                else
                {
                    rewards[1]++;
                }

            }
            if (rewards[2] == 4)
            {
                
                //Reward3=AddScore
            }
            if (rewards[2] == 5)
            {
                
                //Reward3=AddmultipleScore
            }
           
        }
        
        public void drawSelectMenu()
        {
            checkGeneratedRewards();
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
                   
                    if (rewards[0] == 1 || rewards[0] == 2 || rewards[0] == 3)
                    {
                        //dodaj nowy przepis
                        unlockRecepture();
                    }
                    if (rewards[0] == 4)
                    {
                        //dodaj zestaw lisci
                        addLeafs(5);
                    }
                    canDraw = false;
                    Globals.TutorialPause = false;
                    resetRewardArray();
                }
                if ((gamePadState.Buttons.A == ButtonState.Pressed && Globals.prevPauseState.Buttons.A == ButtonState.Released || (state.IsKeyDown(Keys.Enter)) && Globals.prevKeyBoardPauseState.IsKeyUp(Keys.Enter)) && MenuOption == 2)
                {
                    
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (rewards[1] == 1)
                    {
                        
                            // Reward2=tekstura dodatkowego attackspeeda
                        addAttackSpeed(5);
                        attackspeedcounter++;
                      
                    }
                    if (rewards[1] == 2)
                    {
                        addStrenght(2);
                        dmgcounter++;
                        // Reward2=tekstura dodatkowego Atacka

                    }
                    if (rewards[1] == 3)
                    {
                        addSpeed(3);
                        speedcounter++;
                        // Reward2=tekstura dodatkowego speeda

                    }
                    if (rewards[1] == 4)
                    {
                        addMaxHealth(4);
                        // Reward2=tekstura dodatkowego zycka

                    }
                    canDraw = false;
                    Globals.TutorialPause = false;
                    resetRewardArray();

                }
                if ((gamePadState.Buttons.A == ButtonState.Pressed && Globals.prevPauseState.Buttons.A == ButtonState.Released || (state.IsKeyDown(Keys.Enter)) && Globals.prevKeyBoardPauseState.IsKeyUp(Keys.Enter)) && MenuOption == 3)
                {
                    if (rewards[2] == 1)
                    {

                        //Reward3=AddNettleLeafBuff
                        addNettleValue();
                    }
                    if (rewards[2] == 2)
                    {
                        //Reward3=AddAppleLeafBuff
                        addAppleValue();
                    }
                    if (rewards[2] == 3)
                    {
                        addMintValue();
                        //Reward3=AddMelissaLeafBuff

                    }
                    if (rewards[2] == 4)
                    {
                        AddScore(100);
                        //Reward3=AddScore
                    }
                    if (rewards[2] == 5)
                    {
                        AddMultipleScore();
                        //Reward3=AddmultipleScore
                    }
                    canDraw = false;
                    Globals.TutorialPause = false;
                    resetRewardArray();
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

        public void resetRewardArray()
        {
            for(int i=0; i<rewards.Length; i++)
            {
                rewards[i] = 0;
            }
        }
        public void unlockRecepture()
        {

            int value = -1; 
            do
            {
               value = random.Next(0, player.Crafting.bools.Count());
                
            } while (!player.Crafting.bools[value]);
            player.Crafting.bools[value] = true;



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
        public bool isRecepturetoUnlock()
        {
            foreach(bool b in player.Crafting.bools)
            {
                if (!b)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
