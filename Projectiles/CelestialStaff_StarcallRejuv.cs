using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class CelestialStaff_StarcallRejuv : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //ProjectileID.Sets.CountsAsHoming[Projectile.type] = true;
            DisplayName.SetDefault("Starcall Rejuvenation");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.alpha = 255;
            Projectile.timeLeft = 1000;
            Projectile.penetrate = 1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Vector2 move = player.Center - Projectile.Center;

            AdjustMagnitude(ref move);
            Projectile.velocity = (Projectile.velocity + move);
            AdjustMagnitude(ref Projectile.velocity);

            if (Projectile.Hitbox.Intersects(player.Hitbox))
            {
                HitPlayer(player);
            }

            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PortalBolt, 0, 0, 0, new Color(248, 137, 89), 1.5f);
                dust.velocity *= 0.3f;
                dust.noGravity = true;
                dust.noLight = true;

                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PortalBolt, 0, 0, 0, new Color(237, 137, 164), 1.5f);
                dust.velocity *= 0.3f;
                dust.noGravity = true;
                dust.noLight = true;
            }
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 10f)
            {
                vector *= 10f / magnitude;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }

        public void HitPlayer(Player player)
        {
            Projectile.netUpdate = true;
            player.AddBuff(BuffType<Rejuvenation>(), 300);

            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.PortalBolt, 0, -2, 0, new Color(237, 137, 164));
                dust.noGravity = true;

                dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.PortalBolt, 0, -2, 0, new Color(248, 137, 89));
                dust.noGravity = true;
            }

            Projectile.Kill();
        }



        public override void Kill(int timeLeft)
        {
        }
    }
}
