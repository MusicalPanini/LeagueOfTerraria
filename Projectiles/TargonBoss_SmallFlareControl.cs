using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class TargonBoss_SmallFlareControl : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solar Flare");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.timeLeft = 150;
            Projectile.extraUpdates = 0;
        }

        public override void AI()
        {
            if (Projectile.timeLeft <= 90)
            {
                if (Projectile.timeLeft % 4 == 0)
                {
                    if (Projectile.timeLeft % 16 == 0)
                    {
                        TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 34, 0f);
                    }

                    float x = Main.rand.NextFloat(Projectile.position.X + 4, Projectile.position.X + 12);
                    float y = Main.rand.NextFloat(Projectile.position.Y + 4, Projectile.position.Y + 12);
                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), new Vector2(x, y), new Vector2(0, 16), ModContent.ProjectileType<TargonBoss_SmallFlare>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

                    x = Main.rand.NextFloat(Projectile.position.X + 4, Projectile.position.X + 12);
                    y = Main.rand.NextFloat(Projectile.position.Y + 4, Projectile.position.Y + 12);
                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), new Vector2(x, y), new Vector2(0, -16), ModContent.ProjectileType<TargonBoss_SmallFlare>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

                    x = Main.rand.NextFloat(Projectile.position.X + 4, Projectile.position.X + 12);
                    y = Main.rand.NextFloat(Projectile.position.Y + 4, Projectile.position.Y + 12);
                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), new Vector2(x, y), new Vector2(16, 0), ModContent.ProjectileType<TargonBoss_SmallFlare>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

                    x = Main.rand.NextFloat(Projectile.position.X + 4, Projectile.position.X + 12);
                    y = Main.rand.NextFloat(Projectile.position.Y + 4, Projectile.position.Y + 12);
                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), new Vector2(x, y), new Vector2(-16, 0), ModContent.ProjectileType<TargonBoss_SmallFlare>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

                }
            }
            else
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AmberBolt, 0, 0, 150, default, 2f);
                dust.velocity *= 0;
                dust.noGravity = true;
                dust.fadeIn = 0;

                for (int i = 0; i < 4; i++)
                {
                    Vector2 vel = new Vector2(8, 0).RotatedBy(MathHelper.PiOver2 * i);

                    dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AmberBolt, 0, 0, 150, default, 1f);
                    dust.velocity = vel;
                    dust.noGravity = true;
                }
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
