using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Projectiles.Homing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class SerpentsEmbrace_TwinFangs : HomingProjectile
    {
        

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            DisplayName.SetDefault("Twin Fangs");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.alpha = 0;
            Projectile.timeLeft = 90;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.scale = 1.5f;

            CanOnlyHitTarget = false;
            CanRetarget = true;
            TurningFactor = 0.9f;
        }

        public override void GetNewTarget()
        {
            TargetWhoAmI = Targeting.GetClosestNPCWithBuff(Projectile.Center, targetingRange, BuffID.Venom, -1, NPC_CanTargetCritters, NPC_CanTargetDummy);
        }

        public override void AI()
        {
            AnimateProjectile();

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            //AnimateProjectile();

            if (Projectile.soundDelay == 0)
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 3, 23, -0.5f);
            Projectile.soundDelay = 100;

            HomingAI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.CorruptPlants, Projectile.oldVelocity.X/2f, Projectile.oldVelocity.Y/2f, 50, default, 1.2f);
                dust.noGravity = true;
            }
        }

        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frame++;
                Projectile.frame %= 4;
                Projectile.frameCounter = 0;
            }
        }
    }
}
