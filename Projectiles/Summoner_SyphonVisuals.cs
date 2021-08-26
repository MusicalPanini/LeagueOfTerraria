using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Summoner_SyphonVisuals : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Syphon Visuals");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.alpha = 255;
            Projectile.timeLeft = 90;
            Projectile.penetrate = 1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 3;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            for (int i = 0; i < 6; i++)
            {
                Vector2 pos = Projectile.Center + new Vector2(Projectile.timeLeft * 1.5f).RotatedBy((Projectile.timeLeft * 0.03f) + (MathHelper.TwoPi * i / 6));

                Dust dust = Dust.NewDustPerfect(pos, 263, Vector2.Zero, 0, new Color(255, 0, 120), 2);
                dust.noLight = true;
                dust.noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
        }
    }
}
