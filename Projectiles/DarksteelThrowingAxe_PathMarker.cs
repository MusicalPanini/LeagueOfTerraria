using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class DarksteelThrowingAxe_PathMarker : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("AxePathMarker");
        }

        public override void SetDefaults()
        {
            Projectile.width = 78;
            Projectile.height = 78;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.timeLeft = 301;
            Projectile.extraUpdates = 32;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Projectile.velocity.Y < 15)
            {
                Projectile.velocity.Y += 0.3f;
            }

            if (Projectile.timeLeft % 8 == 0 && Main.myPlayer == Projectile.owner)
            {
                Dust dust2 = Dust.NewDustPerfect(Projectile.Center, 211, null, 0, new Color(255, 0, 0), 2);
                dust2.noGravity = true;
                dust2.velocity *= 0;
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.position.X + (int)((Projectile.width / 19.0) * i), Projectile.position.Y + Projectile.height - 24), 6, null, 0, new Color(255, 125, 0), 4f);
                    dust.velocity *= 0f;
                    dust.noGravity = true;
                }
            }

            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 24;
            return true;
        }
    }
}
