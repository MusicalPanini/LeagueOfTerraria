using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class Bullet_HextechChloroShot : ModProjectile
    {
        bool split = false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech CH-300 Balistic");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.scale = 1.2f;
            Projectile.timeLeft = 50;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Left, Color.LightGreen.ToVector3());

            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 15;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }

            if (Projectile.timeLeft == 1)
            {
                Split(-1);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Split(target.whoAmI);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Cloud, Projectile.velocity.X/2, Projectile.velocity.Y/2, 100, new Color(0, 255, 0), 0.5f);
            }

            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            return true;
        }

        public void Split(int num = -1)
        {
            Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 14), Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f);
                dust.velocity *= 0.5f;
            }

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.CursedTorch);
                dust.noGravity = true;
            }

            if (!split)
            {
                if (num == -1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (Main.rand.Next(3) == 0)
                        {
                            Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.position, Projectile.velocity.RotatedByRandom(MathHelper.TwoPi) / 2, ProjectileType<Bullet_HextechChloroShotSplit>(), Projectile.damage / 2, 0, Projectile.owner, -2, num == -1 ? 255 : num);
                        }
                        else
                        {
                            Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.position, Projectile.velocity.RotatedByRandom(MathHelper.TwoPi) / 2, ProjectileType<Bullet_HextechShotSplit>(), Projectile.damage / 4, 0, Projectile.owner, num == -1 ? 255 : num);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (Main.rand.Next(3) == 0)
                        {
                            Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.position, Projectile.velocity.RotatedBy(MathHelper.ToRadians(-15 + (15 * i))) / 2, ProjectileType<Bullet_HextechChloroShotSplit>(), Projectile.damage / 2, 0, Projectile.owner, -2, num == -1 ? 255 : num);

                        }
                        else
                        {
                            Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.position, Projectile.velocity.RotatedBy(MathHelper.ToRadians(-15 + (15 * i))) / 2, ProjectileType<Bullet_HextechShotSplit>(), Projectile.damage / 4, 0, Projectile.owner, num == -1 ? 255 : num);

                        }
                    }
                }

                split = true;
            }
        }
    }
}
