using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Projectiles.Explosive;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class MercuryCannon_Shot : ExplosiveProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mercury Cannon");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            ExplosionWidth = 64;
            ExplosionHeight = 64;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1, 1, 1);

            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 20;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }


            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }

            if (Projectile.ai[0] != 0)
            {
                ExplosionWidth = 256;
                ExplosionHeight = 256;
            }

            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 264, 0, 0, 0, default, Projectile.ai[0] == 0 ? 1 : 2);
            dust.noGravity = true;
            dust.velocity = Projectile.velocity / 2;
            AnimateProjectile();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            
            return true;
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.ai[0] != 0)
                TerraLeague.DustElipce(16, 16, 0, Projectile.Center, 264, default, 2, 36 * 4, true, 0.75f);
            else
                TerraLeague.DustElipce(4, 4, 0, Projectile.Center, 264, default, 2, 36, true, 0.75f);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

            base.Kill(timeLeft);
        }

        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frame++;
                Projectile.frame %= 4;
                Projectile.frameCounter = 0;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 10;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
    }
}
