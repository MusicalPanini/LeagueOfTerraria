using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class LightPistol_Bullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Pistol");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.alpha = 255;
            Projectile.timeLeft = 180;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 2;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 12, -0.25f);
            }
            Projectile.soundDelay = 100;

            Lighting.AddLight(Projectile.position, Color.White.ToVector3());

            for (int i = 0; i < 3; i++)
            {
                
                Dust dust = Dust.NewDustPerfect(Projectile.position, 264);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += Projectile.velocity * 0.1f;
                dust.position.X -= Projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= Projectile.velocity.Y / 3f * (float)i;
            }

            if (Projectile.timeLeft < 30)
            {
                Projectile.alpha += 9;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 17; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 264, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 0, default, 1f);
                dust.noGravity = true;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 10; 
            return true;
        }
    }
}