using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class TargonBoss_SolarFlare : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solar Flare");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.hostile = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.timeLeft = 80;
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

            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AmberBolt, 0, 0, 150, default, 2f);
            dust.velocity *= 0;
            dust.noGravity = true;
            dust.fadeIn = 0;

        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 4; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AmberBolt, 0, 0, 150, default, 2f);
                dust.noGravity = true;
            }

            base.Kill(timeLeft);
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            target.AddBuff(BuffID.OnFire, 4 * 60);

            base.ModifyHitPlayer(target, ref damage, ref crit);
        }
    }
}
