using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class Item_Damnation : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //ProjectileID.Sets.CountsAsHoming[Projectile.type] = true;
            DisplayName.SetDefault("Damnation");
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
            Projectile.extraUpdates = 6;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Vector2 move = player.Center - Projectile.Center;

            AdjustMagnitude(ref move);
            Projectile.velocity = (10 * Projectile.velocity + move) / 11f;
            AdjustMagnitude(ref Projectile.velocity);

            Dust dust = Dust.NewDustPerfect(Projectile.position, 156, null, 50, default, 1f);
            dust.noGravity = true;
            dust.velocity *= 0;

            if (Projectile.Hitbox.Intersects(player.Hitbox))
            {
                HitPlayer(player);
            }

        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 3f)
            {
                vector *= 3f / magnitude;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }

        public void HitPlayer(Player player)
        {
            Projectile.netUpdate = true;
            player.AddBuff(BuffID.Swiftness, 180);

            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.UltraBrightTorch, 0, -2, 0);
                dust.noGravity = true;
            }

            Projectile.Kill();
        }



        public override void Kill(int timeLeft)
        {
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
