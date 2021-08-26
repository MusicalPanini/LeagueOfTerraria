using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class HextechWrench_EvoTurretShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Evolution Turret");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.alpha = 255;
            Projectile.timeLeft = 60;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = Terraria.ModLoader.DamageClass.Summon;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;

        }

        public override void AI()
        {
            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 159, Projectile.velocity.X, Projectile.velocity.Y, 50, default, 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }
            Lighting.AddLight(Projectile.position, 0.5f, 0.5f, 0f);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 159, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 50, default, 2f);
                dust.noGravity = true;
                dust.velocity *= 0.6f;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }
    }
}
