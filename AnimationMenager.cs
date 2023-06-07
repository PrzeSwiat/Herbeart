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
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using PrimitiveType = Microsoft.Xna.Framework.Graphics.PrimitiveType;

namespace TheGame
{
    internal class AnimationMenager
    {
        ContentManager Content;
        Player player;
        List<Enemy> enemies;
        private List<AnimationPlayer> allAnimations;
        private SkinnedModel Steps;
        private AnimationPlayer ASteps;
        private Texture2D texture;
        private Effect effect1;

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
            Steps = Content.Load<SkinnedModel>("Animations/mis_bieg");
            texture = Content.Load<Texture2D>("Textures/mis_texture");
            effect1 = Content.Load<Effect>("AnimationToon");
            ASteps = new AnimationPlayer(Steps);
            ASteps.Animation = Steps.Animations[0];
            ASteps.PlaybackSpeed = 1f;
            ASteps.IsLooping = true;
            ASteps.CurrentTime = 1.0f;
            ASteps.CurrentTick = Steps.Animations[0].DurationInTicks;


            // ADD ALL ANIMATION HERE
            allAnimations.Add(ASteps);

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
            ASteps.IsPlaying = true;
        }

        public void DrawAnimations()
        {
            if(ASteps.IsPlaying)
            {
                DrawAnimation(player, ASteps, Steps);
            }


            ASteps.IsPlaying=false;
        }


        public void DrawAnimation(Creature creature, AnimationPlayer animation, SkinnedModel model)
        {
            Matrix[] boneTransforms = (Matrix[])animation.BoneSpaceTransforms;

            foreach (SkinnedMesh mesh in model.Meshes)
            {
                effect1.CurrentTechnique = effect1.Techniques["Toon"];

                effect1.Parameters["Bones"].SetValue(boneTransforms);
                effect1.Parameters["World"].SetValue(Globals.worldMatrix * Matrix.CreateScale(creature.GetScale()) * Matrix.CreateScale(0.009f)
                        * Matrix.CreateRotationX(creature.GetRotation().X) * Matrix.CreateRotationX(-0.38f) * Matrix.CreateRotationY(creature.GetRotation().Y) *
                        Matrix.CreateRotationZ(creature.GetRotation().Z)
                        * Matrix.CreateTranslation(creature.GetPosition().X, creature.GetPosition().Y, creature.GetPosition().Z));
                effect1.Parameters["View"].SetValue(Globals.viewMatrix);
                effect1.Parameters["Projection"].SetValue(Globals.projectionMatrix);
                effect1.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Globals.worldMatrix)));
                effect1.Parameters["DiffuseLightDirection"].SetValue(new Vector3(0, 0, 0));
                effect1.Parameters["DiffuseColor"].SetValue(new Vector4(10f, 10F, 10f, 1));
                effect1.Parameters["DiffuseIntensity"].SetValue(1);
                effect1.Parameters["LineColor"].SetValue(Color.Black.ToVector4());
                effect1.Parameters["LineThickness"].SetValue(0.03f);
                effect1.Parameters["Texture"].SetValue(texture);

                foreach (var pass in effect1.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    effect1.GraphicsDevice.SetVertexBuffer(mesh.VertexBuffer);
                    effect1.GraphicsDevice.Indices = mesh.IndexBuffer;
                    effect1.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, mesh.VertexBuffer.VertexCount);

                }


            }
        }
    }
}
