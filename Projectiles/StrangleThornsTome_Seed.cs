using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class StrangleThornsTome_Seed : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Strangle Thorn Seed");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.minion = true;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.4f;

            if (Projectile.velocity.Y > 16)
            {
                Projectile.velocity.Y = 16;
            }

            Dust dust = Dust.NewDustPerfect(Projectile.Center, 18);
            dust.noGravity = true;
            dust.velocity *= 0;

            base.AI();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != Projectile.oldVelocity.X)
            {
                Projectile.velocity.X = -Projectile.oldVelocity.X * 0.3f;
            }
            else if (Projectile.velocity.Y != Projectile.oldVelocity.Y && Projectile.oldVelocity.Y > 0)
            {
                return true;
            }
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Vector2 pos = Projectile.Center.ToTileCoordinates16().ToWorldCoordinates();
            pos.Y += 12;
            pos.X -= 16;
            Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), pos, Vector2.Zero, ProjectileType<StrangleThornsTome_NightBloomingZychidsBulb>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;

            width = height = 8;
            return true;
        }
    }
}
