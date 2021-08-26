using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class TargonBoss_SolarFlareControl : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solar Flare");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.timeLeft = 240;
            Projectile.extraUpdates = 0;
        }

        public override void AI()
        {
            //Lighting.AddLight(Projectile.Center, 1f, 1f, 0f);
            if (Projectile.timeLeft <= 180)
            {
                if (Projectile.timeLeft % 4 == 0)
                {
                    if (Projectile.timeLeft % 16 == 0)
                    {
                        TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 9, 0f);
                    }

                    float x = Main.rand.NextFloat(Projectile.position.X + 8, Projectile.position.X + 24);
                    float y = Main.rand.NextFloat(Projectile.position.Y + 8, Projectile.position.Y + 24);
                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), new Vector2(x, y - 8), new Vector2(0, 16), ModContent.ProjectileType<TargonBoss_SolarFlare>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

                    x = Main.rand.NextFloat(Projectile.position.X + 8, Projectile.position.X + 24);
                    y = Main.rand.NextFloat(Projectile.position.Y + 8, Projectile.position.Y + 24);
                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), new Vector2(x, y - 8), new Vector2(0, 16).RotatedBy(MathHelper.PiOver4), ModContent.ProjectileType<TargonBoss_SolarFlare>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

                    x = Main.rand.NextFloat(Projectile.position.X + 8, Projectile.position.X + 24);
                    y = Main.rand.NextFloat(Projectile.position.Y + 8, Projectile.position.Y + 24);
                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), new Vector2(x, y - 8), new Vector2(0, -16), ModContent.ProjectileType<TargonBoss_SolarFlare>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

                    x = Main.rand.NextFloat(Projectile.position.X + 8, Projectile.position.X + 24);
                    y = Main.rand.NextFloat(Projectile.position.Y + 8, Projectile.position.Y + 24);
                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), new Vector2(x, y - 8), new Vector2(0, -16).RotatedBy(MathHelper.PiOver4), ModContent.ProjectileType<TargonBoss_SolarFlare>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

                    x = Main.rand.NextFloat(Projectile.position.X + 8, Projectile.position.X + 24);
                    y = Main.rand.NextFloat(Projectile.position.Y + 8, Projectile.position.Y + 24);
                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), new Vector2(x, y - 8), new Vector2(16, 0), ModContent.ProjectileType<TargonBoss_SolarFlare>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

                    x = Main.rand.NextFloat(Projectile.position.X + 8, Projectile.position.X + 24);
                    y = Main.rand.NextFloat(Projectile.position.Y + 8, Projectile.position.Y + 24);
                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), new Vector2(x, y - 8), new Vector2(16, 0).RotatedBy(MathHelper.PiOver4), ModContent.ProjectileType<TargonBoss_SolarFlare>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

                    x = Main.rand.NextFloat(Projectile.position.X + 8, Projectile.position.X + 24);
                    y = Main.rand.NextFloat(Projectile.position.Y + 8, Projectile.position.Y + 24);
                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), new Vector2(x, y), new Vector2(-16, 0), ModContent.ProjectileType<TargonBoss_SolarFlare>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

                    x = Main.rand.NextFloat(Projectile.position.X + 8, Projectile.position.X + 24);
                    y = Main.rand.NextFloat(Projectile.position.Y + 8, Projectile.position.Y + 24);
                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), new Vector2(x, y - 8), new Vector2(-16, 0).RotatedBy(MathHelper.PiOver4), ModContent.ProjectileType<TargonBoss_SolarFlare>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

                }
            }
            else
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AmberBolt, 0, 0, 150, default, 2f);
                dust.velocity *= 0;
                dust.noGravity = true;
                dust.fadeIn = 0;

                for (int i = 0; i < 8; i++)
                {
                    Vector2 vel = new Vector2(16, 0).RotatedBy(MathHelper.PiOver4 * i);

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
