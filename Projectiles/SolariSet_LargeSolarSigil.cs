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
        const int widenDuration = attackDuration - 60;
        const int fadeDuration = 17;
        const int flareWidth = 16;

        float attackWidth
        {
            get
            {
                float scale = Math.Min(widenDuration - projectile.timeLeft + 60, 180);
                scale = (scale / 180);

                return projectile.width * 0.35f * scale;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solar Sigil");
        }

        public override void SetDefaults()
        {
            projectile.width = 92;
            projectile.height = 92;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.scale = 1f;
            projectile.timeLeft = attackDuration + (fadeDuration * 2);
            projectile.magic = true;
            projectile.extraUpdates = 0;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 1f, 1f, 0f);

            if (projectile.timeLeft > fadeDuration)
            {
                if (projectile.alpha > 0)
                {
                    projectile.alpha -= 15;
                }
                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                }

                //if (projectile.timeLeft <= attackDuration && projectile.timeLeft % 4 == 0)
                //{
                //    if (projectile.timeLeft % 16 == 0)
                //    {
                //        TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 9, 0.5f);
                //    }

                //    float x = Main.rand.NextFloat(projectile.Center.X - attackWidth, projectile.Center.X + attackWidth) - 12;
                //    float y = Main.rand.NextFloat(projectile.position.Y + 24, projectile.position.Y + 68);
                //    Projectile proj = Projectile.NewProjectileDirect(new Vector2(x, y - 8), new Vector2(0, 16), ModContent.ProjectileType<SolariSet_SolarFlare>(), projectile.damage, projectile.knockBack, projectile.owner);
                //}

                if (projectile.timeLeft <= attackDuration)
                {
                    if (projectile.timeLeft % 8 == 0)
                    {
                        TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 9, 0.5f);
                    }

                    if (projectile.timeLeft == attackDuration)
                    {
                        TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 91, -0.5f);
                        Projectile proj = Projectile.NewProjectileDirect(projectile.Center, Vector2.UnitY, ModContent.ProjectileType<SolariSet_SolarLaser>(), projectile.damage, projectile.knockBack, projectile.owner, projectile.whoAmI);
                    }
                }
            }
            else
            {
                projectile.alpha += 15;
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
            //    Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 192, projectile.velocity.X / 2, projectile.velocity.Y / 2, 100, new Color(255, 192, 0), 0.5f);
            //}

            base.Kill(timeLeft);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            
            base.PostDraw(spriteBatch, lightColor);
        }
    }
}
