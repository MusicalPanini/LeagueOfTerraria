using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class TerrorOfTheVoid_VorpalSpike : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vorpal Spike");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            if (Projectile.alpha > 0)
                Projectile.alpha -= 15;
            if (Projectile.alpha < 0)
                Projectile.alpha = 0;

            if (Projectile.timeLeft < 600 - 15)
                Projectile.velocity.Y += 0.4f;

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            Dust dustIndex = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleMoss, 0f, 0f, Projectile.alpha, default);
            dustIndex.noGravity = true;

            if (Projectile.velocity.Y < -16f)
                Projectile.velocity.Y = -16f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.PurpleMoss, Projectile.velocity.X / 2, Projectile.velocity.Y / 2);
            }

            base.Kill(timeLeft);
        }
    }
}
