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
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.scale = 1.2f;
            Projectile.timeLeft = 900;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 2;

            NPC_CanTargetCritters = true;
            CanRetarget = true;
            TurningFactor = 0.97f;
        }

        public override void AI()
        {
            if (Projectile.timeLeft < 700)
                base.AI();

            Lighting.AddLight(Projectile.Left, 0.00f, 0.80f, 0.30f);

            for (int i = 0; i < 10; i++)
            {
                float x2 = Projectile.position.X - Projectile.velocity.X / 10f * (float)i;
                float y2 = Projectile.position.Y - Projectile.velocity.Y / 10f * (float)i;
                int num141 = Dust.NewDust(new Vector2(x2, y2), 1, 1, 75, 0f, 0f, 0, default(Color), 0.5f);
                Main.dust[num141].alpha = Projectile.alpha;
                Main.dust[num141].position.X = x2;
                Main.dust[num141].position.Y = y2;
                Dust obj77 = Main.dust[num141];
                obj77.velocity *= 0f;
                Main.dust[num141].noGravity = true;
            }

            //Dust dust = Dust.NewDustPerfect(Projectile.position, 75, Vector2.Zero, 0, new Color(0,255,0), 1f);
            //dust.noGravity = true;
            //dust.alpha = 100;
        }

        public override void GetNewTarget()
        {
            Projectile.netUpdate = true;
            TargetWhoAmI = Targeting.GetClosestNPC(Projectile.Center, targetingRange, (int)Projectile.ai[1], -1, NPC_CanTargetCritters, NPC_CanTargetDummy);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)Projectile.ai[1] == target.whoAmI)
                return false;
            else
                return base.CanHitNPC(target);
        }
    }
}
