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
using System.Diagnostics.Metrics;
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
        private int cont = 0;
        private DateTime lastAttackTime = DateTime.Now;
        private DateTime actualTime;
        private bool Atak;
        private bool combo = false;
        private List<AnimationPlayer> allAnimations;
        private SkinnedModel Steps;
        private SkinnedModel Sleep;
        private SkinnedModel Death;
        private SkinnedModel Steps_tired;
        private SkinnedModel Idle;
        private SkinnedModel Attack1;
        private SkinnedModel Attack2;
        private AnimationPlayer ASteps;
        private AnimationPlayer ADeath;
        private AnimationPlayer ASleep;
        private AnimationPlayer ASteps_tired;
        private AnimationPlayer AIdle;
        private AnimationPlayer AAttack1;
        private AnimationPlayer AAttack2;
        private bool sleeping = true;
        private Texture2D texture;
        private Effect effect1;
        private int Combocounter = 0;
        private DateTime LAT, AT;
        DateTime lastAttackExecutionTime;
        DateTime ComboCounter,lastcomboTimer;
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
            player.OnAttackPressed += PlayerAttack;
            Death = Content.Load<SkinnedModel>("Animations/mis_death3");
            Sleep = Content.Load<SkinnedModel>("Animations/mis_spanie");
            Steps = Content.Load<SkinnedModel>("Animations/mis_bieg_2");
            Idle = Content.Load<SkinnedModel>("Animations/mis_Standing_23");
            texture = Content.Load<Texture2D>("Textures/mis_texture");
            effect1 = Content.Load<Effect>("AnimationToon");
            Steps_tired=Content.Load<SkinnedModel>("Animations/mis_tired_run");
            Attack1 = Content.Load<SkinnedModel>("Animations/atak1blend2");
            Attack2 = Content.Load<SkinnedModel>("Animations/atak2blend2");

            //Death
            ADeath = new AnimationPlayer(Death);
            ADeath.Animation = Death.Animations[0];
            ADeath.PlaybackSpeed = 0.1f;
            ADeath.IsPlaying = false;
            ADeath.IsLooping = false;
            ADeath.CurrentTime = 1.0f;
            ADeath.CurrentTick = Death.Animations[0].DurationInTicks;

            //Sleep
            ASleep = new AnimationPlayer(Sleep);
            ASleep.Animation = Sleep.Animations[0];
            ASleep.PlaybackSpeed = 0.8f;
            ASleep.IsLooping = true;
            ASleep.CurrentTime = 1.0f;
            ASleep.CurrentTick = Sleep.Animations[0].DurationInTicks;
            //steps
            ASteps = new AnimationPlayer(Steps);
            ASteps.Animation = Steps.Animations[0];
            ASteps.PlaybackSpeed = 0.8f;
            ASteps.IsLooping = true;
            ASteps.CurrentTime = 1.0f;
            ASteps.CurrentTick = Steps.Animations[0].DurationInTicks;
            //steps_tired
            ASteps_tired = new AnimationPlayer(Steps_tired);
            ASteps_tired.Animation = Steps_tired.Animations[0];
            ASteps_tired.PlaybackSpeed = 0.8f;
            ASteps_tired.IsLooping = true;
            ASteps_tired.CurrentTime = 1.0f;
            ASteps_tired.CurrentTick = Steps_tired.Animations[0].DurationInTicks;
            //idle
            AIdle = new AnimationPlayer(Idle);
            AIdle.Animation = Idle.Animations[0];
            AIdle.PlaybackSpeed = 0.8f;
            AIdle.IsLooping = true;
            AIdle.IsPlaying = true;
            AIdle.CurrentTime = 1.0f;
            AIdle.CurrentTick = Idle.Animations[0].DurationInTicks;
            //atack1
            AAttack1 = new AnimationPlayer(Attack1);
            AAttack1.Animation = Attack1.Animations[0];
            AAttack1.PlaybackSpeed = 1.3f;
            AAttack1.IsLooping = false;
            AAttack1.CurrentTime = 1.0f;
            AAttack1.CurrentTick = Attack1.Animations[0].DurationInTicks;
            //atack1
            AAttack2 = new AnimationPlayer(Attack2);
            AAttack2.Animation = Attack2.Animations[0];
            AAttack2.PlaybackSpeed = 1.3f;
            AAttack2.IsLooping = false;
            AAttack2.CurrentTime = 1.0f;
            AAttack2.CurrentTick = Attack2.Animations[0].DurationInTicks;

            // ADD ALL ANIMATION HERE
            allAnimations.Add(ASteps);
            allAnimations.Add(AIdle);
            allAnimations.Add(ASteps_tired);
            allAnimations.Add(AAttack1);
            allAnimations.Add(AAttack2);
            allAnimations.Add(ASleep);
        }

        public void Update(GameTime gameTime)
        {

            foreach(AnimationPlayer animationPlayer in allAnimations)
            {
                animationPlayer.Update(gameTime);
            }
            if(sleeping)
            {
                AIdle.IsPlaying = false;
                ASleep.IsPlaying = true;
            }
            else
            {
                ASleep.IsPlaying = false;
            }
            

            if(Atak)
            {
                TimeSpan timeSinceLastAttack = DateTime.Now - lastAttackExecutionTime;
                if (timeSinceLastAttack.TotalSeconds >= 0.01f)
                {
                    player.ActualSpeed = 13;

                    if (Combocounter == 0)
                    {
                        AAttack1.IsPlaying = true;

                        Combocounter += 1;
                        lastcomboTimer = DateTime.Now;
                    }
                    else if (Combocounter == 1)
                    {
                        AAttack2.IsPlaying = true;

                        Combocounter = 0;
                    }
                    // Zaktualizuj czas ostatniego ataku
                    lastAttackExecutionTime = DateTime.Now;
                }

                Atak = false;
            }
            if (Combocounter != 0)
            {
                TimeSpan combotimerek = DateTime.Now - lastcomboTimer;
                if (combotimerek.TotalSeconds > 3)
                {
                    Combocounter = 0;   
                }

            }
            if (AAttack1.IsPlaying || AAttack2.IsPlaying)
            {
                player.ActualSpeed = 13;
            }
            else
            {
                player.ActualSpeed = player.MaxSpeed;
            }

        }

        public void DeathAnimationDraw()
        {
            if(ADeath.IsPlaying)
            {
                DrawAnimation(player, ADeath, Death);
            }
         
        }

        public void DeathAnimationUpdate(GameTime gameTime)
        {
            if (cont == 0)
            {
                ASteps.IsPlaying = false;
                AIdle.IsPlaying = false;
                AAttack1.IsPlaying = false;
                AAttack2.IsPlaying = false;
                ADeath.IsPlaying = true;
                cont++;
            }

            ADeath.Update(gameTime);
        }

        private void PlayerSteps(object obj, EventArgs e)
        {
            sleeping = false;
            ASteps.IsPlaying = true;
            ASteps_tired.IsPlaying = true;
            AIdle.IsPlaying = false;
            

        }
        private void PlayerAttack(object obj, EventArgs e)
        {
            sleeping = false;
            Atak = true; //ból
        }



        public void DrawAnimations()
        {
            #region PLAYER
         
            if(ASleep.IsPlaying)
            {
                AIdle.IsPlaying = false;
                ASteps.IsPlaying = false;
                ASteps_tired.IsPlaying = false;
                DrawAnimation(player, ASleep, Sleep);
            }
            if (AAttack1.IsPlaying)
            {

                AIdle.IsPlaying = false;
                ASteps.IsPlaying = false;
                ASteps_tired.IsPlaying = false;
                DrawAnimation(player, AAttack1, Attack1);
            }
            
            if (AAttack2.IsPlaying)
            {

                AIdle.IsPlaying = false;
                ASteps.IsPlaying = false;
                ASteps_tired.IsPlaying = false;
                DrawAnimation(player, AAttack2, Attack2);
            }
            if (AIdle.IsPlaying)
            {
                DrawAnimation(player, AIdle, Idle);

            }
            if (ASteps.IsPlaying && !AAttack1.IsPlaying && !AAttack2.IsPlaying)
            {
                if (player.Health <= player.maxHealth * 0.5)
                {
                    DrawAnimation(player, ASteps_tired, Steps_tired);
                }
                else
                {
                    DrawAnimation(player, ASteps, Steps);
                }
                
            }

            if(!Atak || !sleeping)
            {
                AIdle.IsPlaying = true;
            }
            ASteps.IsPlaying = false;
            ASteps_tired.IsPlaying = false;
            
            

            #endregion

            #region MIETA


            #endregion

            #region POKRZYWA


            #endregion

            #region MELISA


            #endregion

            #region JABLON


            #endregion

        }


        public void DrawAnimation(Creature creature, AnimationPlayer animation, SkinnedModel model)
        {
            Matrix[] boneTransforms = (Matrix[])animation.BoneSpaceTransforms;

            foreach (SkinnedMesh mesh in model.Meshes)
            {
                effect1.CurrentTechnique = effect1.Techniques["Toon"];

                effect1.Parameters["Bones"].SetValue(boneTransforms);
                effect1.Parameters["World"].SetValue(Globals.worldMatrix * Matrix.CreateScale(creature.GetScale()) * Matrix.CreateScale(0.009f)
                        * Matrix.CreateRotationX(creature.GetRotation().X)  * Matrix.CreateRotationY(creature.GetRotation().Y) *
                        Matrix.CreateRotationZ(creature.GetRotation().Z) * Matrix.CreateRotationX(-0.38f)
                        * Matrix.CreateTranslation(creature.GetPosition().X, creature.GetPosition().Y, creature.GetPosition().Z));
                effect1.Parameters["View"].SetValue(Globals.viewMatrix);
                effect1.Parameters["Projection"].SetValue(Globals.projectionMatrix);
                effect1.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Globals.worldMatrix)));
                effect1.Parameters["DiffuseLightDirection"].SetValue(new Vector3(0, 0, 0));
                effect1.Parameters["DiffuseColor"].SetValue(new Vector4(10f, 10F, 10f, 0.9f));
                effect1.Parameters["DiffuseIntensity"].SetValue(1);
                effect1.Parameters["LineColor"].SetValue(Color.Black.ToVector4());
                effect1.Parameters["LineThickness"].SetValue(7f);
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
