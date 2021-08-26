using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class SolariSet_SolarSigil : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solar Sigil");
        }

        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 46;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.scale = 1.2f;
            Projectile.timeLeft = 257;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 1f, 0f);

            if (Projectile.timeLeft > 17)
            {
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 15;
                }
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }

                if (Projectile.timeLeft <= 240 && Projectile.timeLeft % 4 == 0)
                {
                    if (Projectile.timeLeft % 16 == 0)
                    {
                        TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 9, 0.5f);
                    }

                    float x = Main.rand.NextFloat(Projectile.position.X + 12, Projectile.position.X + 34);
                    float y = Main.rand.NextFloat(Projectile.position.Y + 12, Projectile.position.Y + 34);
                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), new Vector2(x, y - 8), new Vector2(0, 16), ModContent.ProjectileType<SolariSet_SolarFlare>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
            }
            else
            {
                Projectile.alpha += 15;
            }

            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            //for (int i = 0; i < 10; i++)
            //{
            //    Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 192, Projectile.velocity.X / 2, Projectile.velocity.Y / 2, 100, new Color(255, 192, 0), 0.5f);
            //}

            base.Kill(timeLeft);
        }
    }
}
