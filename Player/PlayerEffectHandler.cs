using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Timers;

namespace TheGame
{
    internal class PlayerEffectHandler
    {
        private Player player;
        private IntervalTimer timer1;
        private IntervalTimer timer2;

        int damage = 3;

        public PlayerEffectHandler(Player player)
        {
            this.player = player;
            timer1 = new IntervalTimer(5000, 1000, EffectAddHealth);
            timer2 = new IntervalTimer(0, 1000, EffectTakeHealth);
        }

        public void Start()
        {
            timer1.Start();
            timer2.Start();
        }

        public void DamagePlayer(int damage)
        {
            this.damage = damage;
            timer2.Start();
        }


        public void EffectAddHealth()
        {
            player.AddHealth(3);
        }

        public void EffectTakeHealth()
        {
            player.SubstractHealth(2);
        }


        private class IntervalTimer
        {
            private Timer timer;
            private Action timerCallBack;
            private int maxTime;
            private int elapsedTime;

            public IntervalTimer(int maxTime, int intervalTime, Action timerCallBack)
            {
                timer = new System.Timers.Timer();
                timer.Interval = intervalTime;
                this.maxTime = maxTime;
                this.timerCallBack = timerCallBack;
                this.elapsedTime = 0;

                timer.Elapsed += Timer_Elapsed;
            }

            public void Start()
            {
                timer.Start();
            }

            public void setTimerMaxTime(int maxTime)
            {
                this.maxTime = maxTime;
            }

            public void setTimerInterval(int interval)
            {
                this.timer.Interval = interval;
            }

            void Timer_Elapsed(object sender, ElapsedEventArgs e)
            {
                // Sprawdzenie, czy upłynął już maksymalny czas
                if (this.maxTime == 0)
                {
                    timerCallBack();
                }
                else
                {
                    if (this.elapsedTime >= this.maxTime)
                    {
                        // Zatrzymanie timera
                        this.elapsedTime = 0;
                        timer.Stop();
                    }
                    else
                    {
                        // Dodanie czasu interwału timera do zmiennej elapsedTime
                        this.elapsedTime += (int)timer.Interval;
                        timerCallBack();
                    }
                }

            }


        }

    }
}