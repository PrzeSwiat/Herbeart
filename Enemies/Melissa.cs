using Liru3D.Animations;
using Liru3D.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class Melissa : Enemy
    {
        private DateTime lastAttackTime, actualTime;
        private int attackCounter = 0;
        private int attacksToStun = 3;
        private int stunTime = 2;

        public Melissa(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {

            AssignParameters(8, 15, 4f, 4.0f);
            score = 20;
            this.setBSRadius(3);
            this.visionRange = 30f;
            this.shadow.SetScale(1.35f);
            this.leaf = new Leafs.MelissaLeaf(worldPosition, "Objects/melise_pickup", "Textures/melise_pickup");
        }

        public override void Update(float deltaTime, Player player)
        {
            base.Update(deltaTime, player);
            AAttack.Update(Globals.gameTime);
            AIdle.Update(Globals.gameTime);
            ARun.Update(Globals.gameTime);

            this.shadow.UpdatingEnemy(this.GetPosition(), new Vector3(2.5f, 0, -2.5f));
            
        }
        public override void LoadContent()
        {
            Idle = Globals.content.Load<SkinnedModel>("Animations/melisa_idle4");
            Atak = Globals.content.Load<SkinnedModel>("Animations/melisa_atak3");
            Run = Globals.content.Load<SkinnedModel>("Animations/melisa_run3");
            SpecialAttack = Globals.content.Load<SkinnedModel>("Animations/melisa_stun_eksport");

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
            AAttack.PlaybackSpeed = 1f;
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

            base.LoadContent();
            float vlll = 2f;
            BoundingBox helper;
            helper.Min = new Vector3(-vlll, 0, -vlll);
            helper.Max = new Vector3(vlll, 6, vlll);

            SetBoundingBox(new BoundingBox(helper.Min + this.GetPosition(), helper.Max + this.GetPosition()));
        }
        protected override void Attack(Player player)
        {
            actualTime = DateTime.Now;
            TimeSpan time = actualTime - lastAttackTime;
            if (time.TotalSeconds > this.ActualAttackSpeed)
            {
                if (attackCounter == attacksToStun)
                {
                    attackCounter = 0;
                    OnSpecialAttackGo();
                    player.setStun(stunTime);
                }
                base.OnAttackGo();
                lastAttackTime = actualTime;
                attackCounter++;
                player.HitWithParticle(this.Strength);
            }
        }
    }
}
