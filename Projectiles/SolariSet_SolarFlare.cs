using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class SolariSet_SolarFlare : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solar Flare");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 3;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 7;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 1f, 0f);

            if (Projectile.ai[1] == 0f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            {
                Projectile.ai[1] = 1f;
                Projectile.netUpdate = true;
            }
            if (Projectile.ai[1] != 0f)
            {
                Projectile.tileCollide = true;
            }

            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AmberBolt, 0, 4, 150, default, 3f);
            dust.velocity.X *= 0;
            dust.noGravity = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Stunned>(), 60*6);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 4; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AmberBolt, 0, -4, 150, default, 2f);
                dust.velocity.X *= 3;
                dust.noGravity = true;
            }

            base.Kill(timeLeft);
        }
    }
}
