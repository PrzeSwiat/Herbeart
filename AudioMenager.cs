using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
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
        private Cue cue;
        private AudioEmitter emitter = new AudioEmitter();
        private AudioListener listener = new AudioListener();
        Player player;
        List<Enemy> enemies;


        public SoundActorPlayer(ContentManager content, Player _player, List<Enemy> _enemies)
        {
            Content = content;
            emitter = new AudioEmitter();
            listener = new AudioListener();
            player = _player;
            enemies = _enemies;
        }

        public void LoadContent()
        {
            cue = Content.Load<Cue>("SoundFX/steps");
            player.onMove += PlayerSteps;
        }

        public void Update(Vector3 CamPos)
        {
            listener.Position = CamPos;
        }


        private void PlayerSteps(object obj, EventArgs e)
        {
            emitter.Position = player.GetPosition();
            cue.Play();
        }

    }
}
