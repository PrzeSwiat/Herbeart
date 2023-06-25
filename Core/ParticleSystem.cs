using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame.Core
{
    internal class ParticleSystem
    {
        private static ParticleSystem instance = null;

        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<Particle> particles;
        private List<Texture2D> textures;
        private List<Texture2D> HerbTextures;

        public static ParticleSystem Instance 
        {
            get
            {
                if (instance == null)
                {
                    instance = new ParticleSystem();
                }
                return instance;
            }
        }


        public ParticleSystem()
        {
            textures = new List<Texture2D>
            {
                Globals.content.Load<Texture2D>("Particles/Textures/circle"),
                Globals.content.Load<Texture2D>("Particles/Textures/star"),
                Globals.content.Load<Texture2D>("Particles/Textures/diamond")
            };

            HerbTextures = new List<Texture2D>
            {
                Globals.content.Load<Texture2D>("HUD/ikona_pokrzywa_crafting"),
                Globals.content.Load<Texture2D>("HUD/ikona_mieta_crafting"),
                Globals.content.Load<Texture2D>("HUD/ikona_melisa_crafting"),
                Globals.content.Load<Texture2D>("HUD/ikona_japco_crafting")
            };


            this.particles = new List<Particle>();
            random = new Random();
        }

        private Particle GenerateNewParticle(Vector2 position)
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 velocity = new Vector2(
                    2f * (float)(random.NextDouble() * 2 - 1),
                    2f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Color color = new Color(
                    (float)random.NextDouble(),
                    (float)random.NextDouble(),
                    (float)random.NextDouble());
            float size = (float)random.NextDouble();
            int ttl = 10 + random.Next(20);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        private Particle GenerateHerbParticle(Vector2 position)
        {
            Texture2D texture = HerbTextures[random.Next(textures.Count)];
            Vector2 velocity = new Vector2(
                    2f * (float)(random.NextDouble() * 2 - 1),
                    2f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            float size = 0.3f;
            int ttl = 10 + random.Next(20);

            return new Particle(texture, position, velocity, angle, angularVelocity, Color.White, size, ttl);
        }


        public void Update()
        {
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public void addHerbParticles(Vector2 position)
        {
            int total = 10;

            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateHerbParticle(position));
            }
        }

        public void addParticles(Vector2 position)
        {
            int total = 20;

            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateNewParticle(position));
            }
        }

        public void Draw()
        {
            Globals.spriteBatch.Begin();
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(Globals.spriteBatch);
            }
            Globals.spriteBatch.End();
        }


    }
}
