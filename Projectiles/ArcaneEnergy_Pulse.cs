using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class ArcaneEnergy_Pulse : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CountsAsHoming[Projectile.type] = true;
            DisplayName.SetDefault("Arcanopulse");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.scale = 1;
            Projectile.timeLeft = 900;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 32;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Projectile.timeLeft = (int)Projectile.ai[0] / 2;
                Projectile.extraUpdates = Projectile.timeLeft - 1;
            }
            Projectile.soundDelay = 1000;

            Lighting.AddLight(Projectile.Left, 0.09f, 0.40f, 0.60f);

            for (int i = 0; i < 4; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 113, 0, 0, 0, default, 4f);
                dust.velocity *= 0;
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }
    }
}
