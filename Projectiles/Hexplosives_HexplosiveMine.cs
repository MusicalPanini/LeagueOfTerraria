using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Hexplosives_HexplosiveMine : ModProjectile
    {
        bool grounded = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hexplosive Mine");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.alpha = 0;
            Projectile.timeLeft = 60 * 10;
            Projectile.penetrate = 100;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (Projectile.velocity.Y < 0)
                Projectile.tileCollide = false;
            else
                Projectile.tileCollide = true;

            if (!grounded)
            {
                Projectile.velocity.Y += 0.3f;
                Projectile.friendly = false;
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.friendly = true;
            }

            Lighting.AddLight(Projectile.position, 0.5f, 0.45f, 0.30f);
            Projectile.rotation += Projectile.velocity.X * 0.01f;

        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.penetrate == 1)
            {
                Prime();
            }
            else
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
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Prime();
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.Y <= 0)
                grounded = true;
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
    }
}
