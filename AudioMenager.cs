using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class AudioMenager
    {
        Song mainSong;
        ContentManager Content;

        public AudioMenager(ContentManager content)
        {
            Content = content;

        }


        public void LoadContent()
        {
            mainSong = Content.Load<Song>("");
        }
    }

    class SoundActorPlayer
    {

    }
}
