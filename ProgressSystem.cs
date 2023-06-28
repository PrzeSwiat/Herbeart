using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;

namespace TheGame
{
    internal class ProgressSystem
    {
        Player player;
        private static readonly Random random = new Random();
        public bool canDraw =false;
        public int MenuOption = 1;
        public int[] rewards = { 0, 0, 0 };
        Texture2D Reward1,Reward2,Reward3;
        Texture2D picked_Reward1,picked_Reward2,picked_Reward3;
        Texture2D rewe;
        Texture2D apple_normal,apple_pick,dmg_normal,dmg_pick,leafs_normal,leafs_pick,m_normal,m_pick,mint_normal,mint_pick,nettle_pick,
            nettle_normal,tea_normal,tea_pick,aspeed_normal,aspeed_pick,healt_normal,health_pick,point_normal,point_pick,speed_normal,speed_pick;
        public ProgressSystem(Player player)
        {
            this.player = player;
            
            rewe = Globals.content.Load<Texture2D>("Textures/itemFrame");
            apple_normal = Globals.content.Load<Texture2D>("ProgressSystem/apple_normal");
            apple_pick = Globals.content.Load<Texture2D>("ProgressSystem/apple_pick");
            dmg_normal = Globals.content.Load<Texture2D>("ProgressSystem/dmg_normal");
            dmg_pick = Globals.content.Load<Texture2D>("ProgressSystem/dmg_pick");
            leafs_normal = Globals.content.Load<Texture2D>("ProgressSystem/leafs_normal");
            leafs_pick = Globals.content.Load<Texture2D>("ProgressSystem/leafs_pick");
            m_normal = Globals.content.Load<Texture2D>("ProgressSystem/m_normal");
            m_pick = Globals.content.Load<Texture2D>("ProgressSystem/m_pick");
            mint_normal = Globals.content.Load<Texture2D>("ProgressSystem/mint_normal");
            mint_pick = Globals.content.Load<Texture2D>("ProgressSystem/mint_pick");
            nettle_pick = Globals.content.Load<Texture2D>("ProgressSystem/nettle_pick");
            nettle_normal = Globals.content.Load<Texture2D>("ProgressSystem/nettle_normal");
            tea_normal = Globals.content.Load<Texture2D>("ProgressSystem/tea_normal");
            tea_pick = Globals.content.Load<Texture2D>("ProgressSystem/tea_pick");
            aspeed_normal = Globals.content.Load<Texture2D>("ProgressSystem/aspeed_normal");
            aspeed_pick = Globals.content.Load<Texture2D>("ProgressSystem/aspeed_pick");
            healt_normal = Globals.content.Load<Texture2D>("ProgressSystem/health_normal");
            health_pick = Globals.content.Load<Texture2D>("ProgressSystem/health_pick");
            point_normal = Globals.content.Load<Texture2D>("ProgressSystem/point_normal");
            point_pick = Globals.content.Load<Texture2D>("ProgressSystem/point_pick");
            speed_normal = Globals.content.Load<Texture2D>("ProgressSystem/speed_normal");
            speed_pick = Globals.content.Load<Texture2D>("ProgressSystem/speed_pick");
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
                    Reward1 = tea_normal;
                    picked_Reward1 = tea_pick;
                }
                else
                {
                      
                    rewards[0] = 4;
                }

            }
            if (rewards[0]==4)
            {
                  Reward1= leafs_normal;
                picked_Reward1 = leafs_pick;
            }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (rewards[1] == 1)
            {
                if(Globals.attackspeedupgrade <2)
                {
                    Reward2 = aspeed_normal;
                    picked_Reward2 = aspeed_pick;
                }
                else
                {
                    rewards[1]++;
                }
            }
            if (rewards[1] == 2)
            {
                if(Globals.dmgupgrade<2)
                {
                     Reward2= dmg_normal;
                    picked_Reward2= dmg_pick;
                }
                else
                {
                    rewards[1]++;
                }

            }
             if (rewards[1] == 3)
            {
                if(Globals.speedupgrade<2)
                {
                    Reward2 = speed_normal;
                    picked_Reward2 = speed_pick;
                }
                else
                {
                    rewards[1]++;
                }

            }
            if (rewards[1] == 4)
            {
                Reward2 = healt_normal;
                picked_Reward2 = health_pick;

            }
///////////////////////////////////////////////////////////////////////////////////
            if (rewards[2] == 1)
            {
                if (Globals.nettleupgrade < 2)
                {
                    Reward3= nettle_normal;
                    picked_Reward3 = nettle_pick;
                }
                else
                {
                    rewards[2]++;
                }
                
            }
            if (rewards[2] == 2)
            {
                if(Globals.appleupgrade < 2)
                {
                    Reward3= apple_normal;
                    picked_Reward3 = apple_pick;
                }
                else
                {
                    rewards[1]++;
                }
                
            }
            if (rewards[2] == 3)
            {
                if (Globals.mintupgrade < 2)
                {
                    Reward3= mint_normal;
                    picked_Reward3 = mint_pick;
                }
                else
                {
                    rewards[1]++;
                }

            }
            if (rewards[2] == 4)
            {
                
                Reward3= point_normal;
                picked_Reward3= point_pick;
            }
            if (rewards[2] == 5)
            {
                
                Reward3= m_normal;
                picked_Reward3= m_pick;
            }
           
        }
        
        public void drawSelectMenu(DateTime dt)
        {
            Texture2D t1, t2, t3;
            checkGeneratedRewards();
            if (canDraw)
            {
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                KeyboardState state = Keyboard.GetState();
                Globals.TutorialPause = true;
               
                if ((gamePadState.ThumbSticks.Left.X >= 0.5f && !(Globals.prevProggresState.ThumbSticks.Left.X >= 0.5f)) || (state.IsKeyDown(Keys.D) && !Globals.prevKeyBoardProggresState.IsKeyDown(Keys.D)))
                {
                    MenuOption += 1;
                }
                if ((gamePadState.ThumbSticks.Left.X <= -0.5f && !(Globals.prevProggresState.ThumbSticks.Left.X <= -0.5f)) || (state.IsKeyDown(Keys.A) && !Globals.prevKeyBoardProggresState.IsKeyDown(Keys.A)))
                {
                    MenuOption -= 1;
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

                
                TimeSpan ts = DateTime.Now - dt;
               
                if (((gamePadState.Buttons.A == ButtonState.Pressed && Globals.prevProggresState .Buttons.A == ButtonState.Released || (state.IsKeyDown(Keys.Enter)) && Globals.prevKeyBoardProggresState.IsKeyUp(Keys.Enter)) && MenuOption == 1)&& ts.TotalSeconds > 1)
                {
                    if (rewards[0] == 1 || rewards[0] == 2 || rewards[0] == 3)
                    {
                        //dodaj nowy przepis
                        unlockRecepture();

                    }
                    if (rewards[0] == 4)
                    {
                        //dodaj zestaw lisci
                        rewards[0] = 0; rewards[1] = 0; rewards[2] = 0;
                        addLeafs(5);
                    }
                    canDraw = false;
                    Globals.TutorialPause = false;
                    player.Start();
                    resetRewardArray();
                }
                if (((gamePadState.Buttons.A == ButtonState.Pressed && Globals.prevProggresState.Buttons.A == ButtonState.Released || (state.IsKeyDown(Keys.Enter)) && Globals.prevKeyBoardProggresState.IsKeyUp(Keys.Enter)) && MenuOption == 2) &&ts.TotalSeconds > 1)
                {
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (rewards[1] == 1)
                    {
                        
                            // Reward2=tekstura dodatkowego attackspeeda
                        addAttackSpeed(0.2f);
                        rewards[0] = 0; rewards[1] = 0; rewards[2] = 0;
                        Globals.attackspeedupgrade++;
                      
                    }
                    if (rewards[1] == 2)
                    {
                        addStrenght();
                        rewards[0] = 0; rewards[1] = 0; rewards[2] = 0;
                        Globals.dmgupgrade++;
                        // Reward2=tekstura dodatkowego Atacka

                    }
                    if (rewards[1] == 3)
                    {
                        addSpeed(5);
                        rewards[0] = 0; rewards[1] = 0; rewards[2] = 0;
                        Globals.speedupgrade++;
                        // Reward2=tekstura dodatkowego speeda

                    }
                    if (rewards[1] == 4)
                    {
                        addMaxHealth(50);
                        rewards[0] = 0; rewards[1] = 0; rewards[2] = 0;
                        // Reward2=tekstura dodatkowego zycka

                    }
                    canDraw = false;
                    Globals.TutorialPause = false;
                    player.Start();
                    resetRewardArray();
                    
                }
                if (((gamePadState.Buttons.A == ButtonState.Pressed && Globals.prevProggresState.Buttons.A == ButtonState.Released || (state.IsKeyDown(Keys.Enter)) && Globals.prevKeyBoardPauseState.IsKeyUp(Keys.Enter)) && MenuOption == 3)&& ts.TotalSeconds > 1)
                {
                    if (rewards[2] == 1)
                    {

                        //Reward3=AddNettleLeafBuff
                        addNettleValue();
                        rewards[0] = 0; rewards[1] = 0; rewards[2] = 0;
                    }
                    if (rewards[2] == 2)
                    {
                        //Reward3=AddAppleLeafBuff
                        addAppleValue();
                        rewards[0] = 0; rewards[1] = 0; rewards[2] = 0;
                    }
                    if (rewards[2] == 3)
                    {
                        addMintValue();
                        rewards[0] = 0; rewards[1] = 0; rewards[2] = 0;
                        //Reward3=AddMelissaLeafBuff

                    }
                    if (rewards[2] == 4)
                    {
                        AddScore();
                        rewards[0] = 0; rewards[1] = 0; rewards[2] = 0;
                        //Reward3=AddScore
                    }
                    if (rewards[2] == 5)
                    {
                        AddMultipleScore();
                        rewards[0] = 0; rewards[1] = 0; rewards[2] = 0;
                        //Reward3=AddmultipleScore
                    }
                    
                    Globals.TutorialPause = false;
                    player.Start();
                    canDraw = false;
                    resetRewardArray();
                }
                
                Globals.spriteBatch.Begin();
                if (MenuOption == 1)
                {
                    t1 = picked_Reward1;
                    Globals.spriteBatch.Draw(t1, new Vector2(400, 400), null, Color.White, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 0f);
                }
                else
                {
                    t1 = Reward1;
                    Globals.spriteBatch.Draw(t1, new Vector2(400, 400), null, Color.White, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 0f);
                }
                if (MenuOption == 2)
                {
                    t2 = picked_Reward2;
                    Globals.spriteBatch.Draw(t2, new Vector2(700, 400), null, Color.White, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 0f);
                }
                else
                {
                    t2= Reward2;
                    Globals.spriteBatch.Draw(t2, new Vector2(700, 400), null, Color.White, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 0f);
                }
                if (MenuOption == 3)
                {
                    t3 = picked_Reward3;
                    Globals.spriteBatch.Draw(t3, new Vector2(1000, 400), null, Color.White, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 0f);
                }
                else
                {
                    t3 = Reward3;
                    Globals.spriteBatch.Draw(t3, new Vector2(1000, 400), null, Color.White, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 0f);

                }




                Globals.spriteBatch.End();
                Globals.prevProggresState = gamePadState;
                Globals.prevKeyBoardProggresState = state;
            }

            
        }



        public int[] generateRewards()
        {
            int valuefirst = random.Next(1, 4 + 1);
            int valueseccond = random.Next(1, 4 + 1);
            int valuethird = random.Next(1, 5 + 1);

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
                
            } while (player.Crafting.bools[value]);
            if (value == 1) 
            {
                Globals.learnedRecepture.Add("AAB");
            }
            if (value == 2)
            {
                Globals.learnedRecepture.Add("AXY");
            }
            if (value == 3)
            {
                Globals.learnedRecepture.Add("XBY");
            }
            if (value == 4)
            {
                Globals.learnedRecepture.Add("ABX");
            }
           Globals.maxReceptures++;
            player.Crafting.bools[value] = true;



        }

        public void AddMultipleScore()
        {
            Globals.ScoreMultiplier += 1;
        }
        public void RemoveMultipleScore() 
        {
            Globals.ScoreMultiplier -= 1;
        }
        public void AddScore() 
        {
            int value = random.Next(0, 12);
            Globals.Score += value*Globals.ScoreMultiplier ;
        }
        public void addMaxHealth(int health)
        {
            player.MaxHealth += health;
            player.Health += health;
        }
        public void addStrenght() 
        {
            player.MaxStrength+=1;
            player.Strength +=1;
            
        }
        public void addAttackSpeed(float speed)
        {
            player.AttackSpeed -= speed;
            player.ActualAttackSpeed -= speed;
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
            player.MintValue += 1f;
            player.MintTime += 3;
        }
        public void addAppleValue()
        {
            player.appleValue +=2;
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
