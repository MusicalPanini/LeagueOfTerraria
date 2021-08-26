using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Projectiles
{
    public class Hexplosives_HexplosiveMineField : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mine Clump");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.alpha = 0;
            Projectile.timeLeft = 1000;
            Projectile.penetrate = 1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.3f;

            Lighting.AddLight(Projectile.position, 0.5f, 0.45f, 0.30f);
            Projectile.rotation += Projectile.velocity.X * 0.01f;
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 14), Projectile.position);
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f);
                dust.velocity *= 0.5f;
            }

            for (int i = 0; i < 7; i++)
            {
                Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Bottom.Y - 10), new Vector2(6 - (i * 2), -5), ProjectileType<Hexplosives_HexplosiveMine>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Prime();
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != Projectile.oldVelocity.X)
                Projectile.velocity.X = -Projectile.oldVelocity.X * 0.3f;
            else
                Prime();

            return false;
        }

        public void Prime()
        {
            Projectile.tileCollide = false;
            Projectile.velocity = Vector2.Zero;
            Projectile.alpha = 255;
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.timeLeft = 2;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
