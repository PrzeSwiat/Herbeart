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
using System.Threading.Tasks;
using static Assimp.Metadata;

namespace TheGame
{
    internal class AudioMenager
    {
        Song mainSong;
        ContentManager Content;
        TimeSpan timeMainSong;
        TimeSpan durationMainSong;
        public event EventHandler onPlay;

        public AudioMenager(ContentManager content)
        {
            Content = content;
            MediaPlayer.Volume = 0.5f;
        }


        public void LoadContent()
        {
            mainSong = Content.Load<Song>("SoundFx/ForestMain");
            
            durationMainSong = mainSong.Duration;
            onPlay += PlayAgain;
            onPlay?.Invoke(this, EventArgs.Empty);
        }

        public void MainPlay()
        {
            timeMainSong = MediaPlayer.PlayPosition;
           // Debug.WriteLine(timeMainSong.TotalSeconds + " : " + durationMainSong.TotalSeconds);
            if (timeMainSong.TotalSeconds>=(durationMainSong.TotalSeconds - 0.1f))
            {
                onPlay?.Invoke(this, EventArgs.Empty);
            }
        }

        private void PlayAgain(object obj, EventArgs e)
        {
            MediaPlayer.Play(mainSong);
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
        private List<SoundEffectInstance> Speech;
        Player player;
        List<Enemy> enemies;
        

        public SoundActorPlayer(ContentManager content, Player _player, List<Enemy> _enemies)
        {
            Content = content;
            player = _player;
            enemies = _enemies;
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

        }

        private void PlayerSpeech(object obj, EventArgs e)
        {

        }
    }
}
