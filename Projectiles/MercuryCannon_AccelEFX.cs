using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class MercuryCannon_AccelEFX : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("AccelEFX");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.scale = 1;
            Projectile.timeLeft = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void Kill(int timeLeft)
        {
            TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 15, 1);
            TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 117, 0.5f);
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Electric, Projectile.velocity.X/Main.rand.NextFloat(1, 2), Projectile.velocity.Y / Main.rand.NextFloat(1, 2), 100, default, 1f);
                dust.noGravity = true;
            }

            base.Kill(timeLeft);
        }
    }
}
