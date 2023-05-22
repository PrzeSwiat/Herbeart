using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Assimp.Metadata;

namespace TheGame
{
    internal class AudioMenager
    {
        Song mainSong;
        Song mainMenuSong;
        ContentManager Content;
        TimeSpan timeMainSong;
        TimeSpan durationMainSong;
        TimeSpan durationMainMenuSong;
        public event EventHandler onPlay;
        public event EventHandler onMenuPlay;
        bool perv = false;

        public AudioMenager(ContentManager content)
        {
            Content = content;
            MediaPlayer.Volume = 0.5f;
        }


        public void LoadContent()
        {
            mainSong = Content.Load<Song>("SoundFx/ForestMain");
            mainMenuSong = Content.Load<Song>("SoundFx/wolves");

            durationMainSong = mainSong.Duration;
            durationMainMenuSong = mainMenuSong.Duration;
            onPlay += PlayAgain;
            onMenuPlay += PlayMainMenu;
        }

        public void MainPlay()
        {
            timeMainSong = MediaPlayer.PlayPosition;

            if (Globals.Pause && !perv)
            {
               onMenuPlay?.Invoke(this, EventArgs.Empty);
            }
            if ((timeMainSong.TotalSeconds>=(durationMainSong.TotalSeconds - 0.1f) || Math.Round(Globals.time,1) == 0) && !Globals.Pause)
            {
                onPlay?.Invoke(this, EventArgs.Empty);
            }

            perv = Globals.Pause;
        }

        private void PlayAgain(object obj, EventArgs e)
        {
            MediaPlayer.Stop();
            MediaPlayer.Play(mainSong);
        }

        private void PlayMainMenu(object obj, EventArgs e)
        {
            MediaPlayer.Stop();
            MediaPlayer.Play(mainMenuSong);
            
        }


    }

    class SoundActorPlayer
    {
        ContentManager Content;
        private SoundEffectInstance steps;
        private SoundEffectInstance attack;
        private SoundEffectInstance hit;
        private SoundEffectInstance death;
        private SoundEffectInstance pickupItem;
        private List<SoundEffectInstance> randomNoises;
        private List<SoundEffectInstance> speech;
        Player player;
        List<Enemy> enemies;
        

        public SoundActorPlayer(ContentManager content, Player _player, List<Enemy> _enemies)
        {
            Content = content;
            player = _player;
            enemies = _enemies;
            randomNoises = new List<SoundEffectInstance>();
            speech = new List<SoundEffectInstance>();
        }

        public void LoadContent()
        {
            //player Steps
            player.onMove += PlayerSteps;
            steps = Content.Load<SoundEffect>("SoundFX/steps").CreateInstance();
            steps.Volume = 0.1f;

            //player Attack
            player.OnAttackPressed += PlayerAttack;
            attack = Content.Load<SoundEffect>("SoundFX/punch").CreateInstance();
            attack.Volume = 0.5f;

            //player Hit

            //player Death
            player.OnDestroy += PlayerDeath;
            death = Content.Load<SoundEffect>("SoundFX/death").CreateInstance();
            death.Pitch = -1;
            death.Volume = 0.5f;

            //player Pickup item

            //player Drinks

            //player Random noises

            SoundEffectInstance roar = Content.Load<SoundEffect>("SoundFX/nastyRoar").CreateInstance();
            roar.Pitch = -0.3f;
            roar.Volume = 0.5f;
            randomNoises.Add(roar);
            player.onRandomNoise += PlayerRandomNoises;
            //player Speech


        }

        private void PlayerSteps(object obj, EventArgs e)
        {
            if(steps.State != SoundState.Playing)
            {
                steps.Play();
            }
        }

        private void PlayerAttack(object obj, EventArgs e)
        {
            if (attack.State != SoundState.Playing)
            {
                attack.Play();
            }
        }

        private void PlayerHit(object obj, EventArgs e)
        {

        }
        private void PlayerDeath(object obj, EventArgs e)
        {
            if (death.State != SoundState.Playing)
            {
                death.Play();
            }
        }

        private void PlayerPickupItem(object obj, EventArgs e)
        {

        }
        private void PlayerDrinks(object obj, EventArgs e)
        {

        }
        private void PlayerRandomNoises(object obj, EventArgs e)
        {
            Random random = new Random();
            int rand = random.Next(0, randomNoises.Count-1);
            if (death.State != SoundState.Playing)
            {
                randomNoises[rand].Play();
            }
        }

        private void PlayerSpeech(object obj, EventArgs e)
        {

        }
    }
}
