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

        public PlayerEffectHandler(Player player)
        {
            this.player = player;
            timer1 = new IntervalTimer(5000, 1000, EffectAddHealth);
            timer2 = new IntervalTimer(0, 1000, EffectTakeLife);
        }

        public void Start()
        {
            timer1.Start();
            timer2.Start();
        }

        public void EffectAddHealth()
        {
            player.AddHealth(2);
        }

        public void EffectTakeLife()
        {
            player.SubstractHealth(3);
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