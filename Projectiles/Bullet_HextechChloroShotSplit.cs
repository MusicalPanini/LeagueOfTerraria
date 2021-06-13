using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Projectiles.Homing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Bullet_HextechChloroShotSplit : HomingProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorophyte Bolt");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.scale = 1.2f;
            projectile.timeLeft = 900;
            projectile.ranged = true;
            projectile.extraUpdates = 12;

            NPC_CanTargetCritters = true;
            CanRetarget = true;
            TurningFactor = 0.97f;
        }

        public override void AI()
        {
            if (projectile.timeLeft < 700)
                base.AI();

            Lighting.AddLight(projectile.Left, 0.00f, 0.80f, 0.30f);
            Dust dust = Dust.NewDustPerfect(projectile.position, 75, Vector2.Zero, 0, new Color(0,255,0), 1f);
            dust.noGravity = true;
            dust.alpha = 100;
        }

        public override void GetNewTarget()
        {
            projectile.netUpdate = true;
            TargetWhoAmI = TerraLeague.GetClosestNPC(projectile.Center, targetingRange, (int)projectile.ai[1], -1, NPC_CanTargetCritters, NPC_CanTargetDummy);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)projectile.ai[1] == target.whoAmI)
                return false;
            else
                return base.CanHitNPC(target);
        }
    }
}
