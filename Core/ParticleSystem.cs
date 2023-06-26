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
        private List<Texture2D> StunTextures;
        private List<Texture2D> EnemyHitTextures;
        private List<Texture2D> HerbTextures;
        private List<Texture2D> PlayerTextures;

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
            StunTextures = new List<Texture2D>
            {
                Globals.content.Load<Texture2D>("Particles/Textures/stunParticle"),
            };

            HerbTextures = new List<Texture2D>
            {
                Globals.content.Load<Texture2D>("HUD/ikona_pokrzywa_crafting"),
                Globals.content.Load<Texture2D>("HUD/ikona_mieta_crafting"),
                Globals.content.Load<Texture2D>("HUD/ikona_melisa_crafting"),
                Globals.content.Load<Texture2D>("HUD/ikona_japco_crafting")
            };

            EnemyHitTextures = new List<Texture2D>
            {
                Globals.content.Load<Texture2D>("Particles/Textures/particleenemyhit1"),
                Globals.content.Load<Texture2D>("Particles/Textures/particleenemyhit2")
            };

            PlayerTextures = new List<Texture2D>
            {
                Globals.content.Load<Texture2D>("Particles/Textures/mis1"),
                Globals.content.Load<Texture2D>("Particles/Textures/mis2"),
                Globals.content.Load<Texture2D>("Particles/Textures/mis3")
            };



            this.particles = new List<Particle>();
            random = new Random();
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


        private Particle GenerateStunParticle(Vector2 position)
        {
            Texture2D texture = StunTextures[random.Next(StunTextures.Count)];
            Vector2 velocity = new Vector2(
                    4.5f * (float)(random.NextDouble() * 2 - 1),
                    4.5f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            float size = (float)(random.NextDouble() / 3.5f);
            int ttl = 10 + random.Next(10);

            return new Particle(texture, position, velocity, angle, angularVelocity, Color.White, size, ttl);
        }

        private Particle GenerateHerbParticle(Vector2 position)
        {
            Texture2D texture = HerbTextures[random.Next(HerbTextures.Count)];
            Vector2 velocity = new Vector2(
                    2f * (float)(random.NextDouble() * 2 - 1),
                    2f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            float size = 0.3f;
            int ttl = 10 + random.Next(20);

            return new Particle(texture, position, velocity, angle, angularVelocity, Color.White, size, ttl);
        }

        private Particle GenerateEnemyParticle(Vector2 position)
        {
            Texture2D texture = EnemyHitTextures[random.Next(EnemyHitTextures.Count)];
            Vector2 velocity = new Vector2(
                    3.5f * (float)(random.NextDouble() * 2 - 1),
                    3.5f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            float size = (float)(random.NextDouble() / 3.5f);
            int ttl = 10 + random.Next(10);

            return new Particle(texture, position, velocity, angle, angularVelocity, Color.White, size, ttl);
        }

        private Particle GeneratePlayerParticle(Vector2 position)
        {
            Texture2D texture = PlayerTextures[random.Next(PlayerTextures.Count)];
            Vector2 velocity = new Vector2(
                    3.5f * (float)(random.NextDouble() * 2 - 1),
                    3.5f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            float size = (float)(random.NextDouble() / 3.5f);
            int ttl = 10 + random.Next(10);

            return new Particle(texture, position, velocity, angle, angularVelocity, Color.White, size, ttl);
        }


        public void addHerbParticles(Vector2 position)
        {
            int total = 10;

            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateHerbParticle(position));
            }
        }

        public void addEnemyParticles(Vector2 position)
        {
            int total = 20;

            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateEnemyParticle(position));
            }
        }

        public void addPlayerParticles(Vector2 position)
        {
            int total = 20;

            for (int i = 0; i < total; i++)
            {
                particles.Add(GeneratePlayerParticle(position));
            }
        }

        public void addStunParticles(Vector2 position)
        {
            int total = 20;

            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateStunParticle(position));
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
