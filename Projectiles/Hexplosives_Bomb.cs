using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.Projectiles.Explosive;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Hexplosives_Bomb : ExplosiveProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hexplosive");
        }

        public override void SetDefaults()
        {
            ExplosionWidth = 64;
            ExplosionHeight = 64;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.alpha = 0;
            Projectile.timeLeft = 1000;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.3f;

            Lighting.AddLight(Projectile.position, 0.5f, 0.45f, 0.30f);
            Projectile.rotation += Projectile.velocity.X * 0.01f;

            Vector2 dustPos = Projectile.position.RotatedBy(MathHelper.PiOver2 + Projectile.rotation, Projectile.Center);

            Dust dust = Dust.NewDustPerfect(dustPos, DustID.Smoke);
            dust.noGravity = true;
            dust.scale = 0.75f;

            dust = Dust.NewDustPerfect(dustPos, 6);
            dust.noGravity = true;
            dust.velocity *= 0;
        }

        public override void KillEffects()
        {
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 14), Projectile.position);

            Dust dust;
            for (int i = 0; i < 20; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f);
                dust.velocity *= 0.5f;
            }
            for (int i = 0; i < 30; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 3f);
                dust.noGravity = true;
                dust.velocity *= 3f;
                dust.color = new Color(255, 0, 220);

                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 2f);
                dust.color = new Color(255, 0, 220);
                dust.noGravity = true;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = Projectile.Center.X > target.Center.X ? -1 : 1;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
    }
}
