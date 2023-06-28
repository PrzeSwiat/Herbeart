using Liru3D.Animations;
using Liru3D.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class Mint : Enemy
    {

        private DateTime lastAttackTime, actualTime;

        public Mint(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {
            AssignParameters(2, 20, 12, 2f);
            this.score = 5;
            this.shadow.SetScale(0.7f);
            this.setBSRadius(2);
            this.visionRange = 30f;
            this.leaf = new Leafs.MintLeaf(worldPosition, "Objects/mint_pickup", "Textures/mint_pickup");
            
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Idle = Globals.content.Load<SkinnedModel>("Animations/mieta_idle2");
            Atak = Globals.content.Load<SkinnedModel>("Animations/mieta_debug");
            Run = Globals.content.Load<SkinnedModel>("Animations/mieta_run");
            SpecialAttack = Globals.content.Load<SkinnedModel>("Animations/mis_bieg_2");

            //Idle
            AIdle = new AnimationPlayer(Idle);
            AIdle.Animation = Idle.Animations[0];
            AIdle.PlaybackSpeed = 0.5f;
            AIdle.IsPlaying = true;
            AIdle.IsLooping = false;
            AIdle.CurrentTime = 1.0f;
            AIdle.CurrentTick = Idle.Animations[0].DurationInTicks;
            //Attack
            AAttack = new AnimationPlayer(Atak);
            AAttack.Animation = Atak.Animations[0];
            AAttack.PlaybackSpeed = 0.5f;
            AAttack.IsPlaying = false;
            AAttack.IsLooping = false;
            AAttack.CurrentTime = 1.0f;
            AAttack.CurrentTick = Atak.Animations[0].DurationInTicks;
            //Run
            ARun = new AnimationPlayer(Run);
            ARun.Animation = Run.Animations[0];
            ARun.PlaybackSpeed = 0.5f;
            ARun.IsPlaying = false;
            ARun.IsLooping = false;
            ARun.CurrentTime = 1.0f;
            ARun.CurrentTick = Run.Animations[0].DurationInTicks;
            //SpecialAtack
            ASpecialAttack = new AnimationPlayer(SpecialAttack);
            ASpecialAttack.Animation = SpecialAttack.Animations[0];
            ASpecialAttack.PlaybackSpeed = 1f;
            ASpecialAttack.IsPlaying = false;
            ASpecialAttack.IsLooping = false;
            ASpecialAttack.CurrentTime = 1.0f;
            ASpecialAttack.CurrentTick = SpecialAttack.Animations[0].DurationInTicks;


            float vlll = 1f;
            BoundingBox helper;
            helper.Min = new Vector3(-vlll, 0, -vlll);
            helper.Max = new Vector3(vlll, 4, vlll);

            SetBoundingBox(new BoundingBox(helper.Min + this.GetPosition(), helper.Max + this.GetPosition()));
        }

        public override void Update(float deltaTime, Player player)
        {
            base.Update(deltaTime, player);
            AAttack.Update(Globals.gameTime);
            AIdle.Update(Globals.gameTime);
            ARun.Update(Globals.gameTime);
            this.shadow.UpdatingEnemy(this.GetPosition(), new Vector3(1.2f,0,-1));
            this.shadow.SetPositionY(-0.5f);

        }

        protected override void Attack(Player player)
        {
            actualTime = DateTime.Now;
            TimeSpan time = actualTime - lastAttackTime;
            if (time.TotalSeconds > 1)
            {
                base.OnAttackGo();

                lastAttackTime = actualTime;
                player.HitWithParticle(this.Strength);
            }
        }
    }
}
