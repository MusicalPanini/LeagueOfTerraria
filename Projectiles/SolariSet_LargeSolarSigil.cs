using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class SolariSet_LargeSolarSigil : ModProjectile
    {
        const int attackDuration = 300;
        const int fadeDuration = 17;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solar Sigil");
        }

        public override void SetDefaults()
        {
            Projectile.width = 92;
            Projectile.height = 92;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.timeLeft = attackDuration + (fadeDuration * 2);
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 0;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 1f, 0f);

            if (Projectile.timeLeft > fadeDuration)
            {
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 15;
                }
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }

                //if (Projectile.timeLeft <= attackDuration && Projectile.timeLeft % 4 == 0)
                //{
                //    if (Projectile.timeLeft % 16 == 0)
                //    {
                //        TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 9, 0.5f);
                //    }

                //    float x = Main.rand.NextFloat(Projectile.Center.X - attackWidth, Projectile.Center.X + attackWidth) - 12;
                //    float y = Main.rand.NextFloat(Projectile.position.Y + 24, Projectile.position.Y + 68);
                //    Projectile proj = Projectile.NewProjectileDirect(new Vector2(x, y - 8), new Vector2(0, 16), ModContent.ProjectileType<SolariSet_SolarFlare>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                //}

                if (Projectile.timeLeft <= attackDuration)
                {
                    if (Projectile.timeLeft % 8 == 0)
                    {
                        TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 9, 0.5f);
                    }

                    if (Projectile.timeLeft == attackDuration)
                    {
                        TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 91, -0.5f);
                        Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Vector2.UnitY, ModContent.ProjectileType<SolariSet_SolarLaser>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.whoAmI);
                    }
                }
            }
            else
            {
                Projectile.alpha += 15;
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
