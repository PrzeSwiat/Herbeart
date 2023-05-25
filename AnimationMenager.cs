using Assimp;
using Assimp.Unmanaged;
using Liru3D.Animations;
using Liru3D.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TheGame
{
    internal class AnimationMenager
    {
        ContentManager Content;
        Player player;
        List<Enemy> enemies;
        private List<AnimationPlayer> allAnimations;
        private SkinnedModel debil;
        private AnimationPlayer Adebil;

        private SkinnedModel playerModel;
        private AnimationPlayer playerAnimations;

        public AnimationMenager(ContentManager content, Player _player, List<Enemy> _enemies)
        {
            Content = content;
            player = _player;
            enemies = _enemies;
            allAnimations = new List<AnimationPlayer>();

        }

        public void LoadContent()
        {

            player.onMove += PlayerSteps;

            //debil
            debil = Content.Load<SkinnedModel>("Animations/abominejszyn");
            Adebil = new AnimationPlayer(debil);
            Adebil.Animation = debil.Animations[0];
            Adebil.PlaybackSpeed = 1f;
            Adebil.IsLooping = true;
            Adebil.IsPlaying = true;
            Adebil.CurrentTime = 1.0f;
            Adebil.CurrentTick = debil.Animations[0].DurationInSeconds;


            // ADD ALL ANIMATION HERE
            allAnimations.Add(Adebil);

        }

        public void Update(GameTime gameTime)
        {
            foreach(AnimationPlayer animationPlayer in allAnimations)
            {
                animationPlayer.Update(gameTime);
            }
        }


        private void PlayerSteps(object obj, EventArgs e)
        {
            
        }

        public void DrawAnimation(GraphicsDevice graphicsDevice)
        {
            Effect _effect;
            SkinnedEffect effect = new SkinnedEffect(graphicsDevice);

            effect.EnableDefaultLighting();
            effect.Texture = player.GetTexture2D();
            effect.View = Globals.viewMatrix;
            effect.Projection = Globals.projectionMatrix;
            effect.World = Globals.worldMatrix * Matrix.CreateScale(player.GetScale()) * Matrix.CreateScale(0.01f)
                        * Matrix.CreateRotationX(player.GetRotation().X) * Matrix.CreateRotationY(player.GetRotation().Y) *
                        Matrix.CreateRotationZ(player.GetRotation().Z)
                        * Matrix.CreateTranslation(player.GetPosition().X, player.GetPosition().Y, player.GetPosition().Z);


            foreach (SkinnedMesh mesh in debil.Meshes)
            {
                Adebil.SetEffectBones(effect);

                effect.CurrentTechnique.Passes[0].Apply();

                _effect = effect.Clone();
                mesh.Draw();
            }
        }
    }
}
