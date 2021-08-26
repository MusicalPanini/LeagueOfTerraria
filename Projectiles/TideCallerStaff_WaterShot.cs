using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class TideCallerStaff_WaterShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Water Pellet");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.alpha = 255;
            Projectile.timeLeft = 90;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = 0;
        }

        public override void AI()
        {
            if (Projectile.velocity.X < 0)
                Projectile.spriteDirection = -1;

            Lighting.AddLight(Projectile.position, 0f, 0f, 0.5f);

            for (int i = 0; i < 3; i++)
            {
                Vector2 dustBoxPosition = new Vector2(Projectile.position.X + 6, Projectile.position.Y + 6);
                int dustBoxWidth = Projectile.width - 12;
                int dustBoxHeight = Projectile.height - 12;
                Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, DustID.Ice, 0f, 0f, 100, default, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += Projectile.velocity * 0.1f;
                dust.position.X -= Projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= Projectile.velocity.Y / 3f * (float)i;
            }
            if (Main.rand.Next(5) == 0)
            {
                Vector2 dustBoxPosition = new Vector2(Projectile.position.X + 6, Projectile.position.Y + 6);
                int dustBoxWidth = Projectile.width - 12;
                int dustBoxHeight = Projectile.height - 12;
                Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, DustID.WaterCandle, 0f, 0f, 100, default, 0.5f);
                dust.velocity *= 0.25f;
                dust.velocity += Projectile.velocity * 0.5f;
            }
            if (Projectile.timeLeft < 30)
                Projectile.alpha += 9;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 17; i++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 211, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 0, default, 1f);
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10; 
            return true;
        }
    }
}