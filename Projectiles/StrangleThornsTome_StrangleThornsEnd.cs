using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Projectiles
{
    public class StrangleThornsTome_StrangleThornsEnd : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Strangle Thorns");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.ignoreWater = true;
            Projectile.minion = true;
        }

        public override void AI()
        {
            if (Projectile.velocity != Vector2.Zero)
            {
                Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

                for (int i = 0; i < Main.projectile.Length; i++)
                {
                    if (Main.projectile[i].type == ProjectileType<StrangleThornsTome_NightBloomingZychidsBulb>())
                    {
                        Projectile bulb = Main.projectile[i];

                        if (bulb.Hitbox.Intersects(new Rectangle((int)Projectile.Center.X - 75, (int)Projectile.Center.Y - 75, 150, 150)))
                        {
                            Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), bulb.position, Vector2.Zero, ProjectileType<HextechWrench_EvolutionTurret>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
                            bulb.Kill();
                        }
                    }
                }
            }
            Projectile.velocity = Vector2.Zero;

            if (Projectile.ai[0] == 0f)
            {
                Projectile.alpha -= 75;

                if (Projectile.alpha <= 0)
                {
                    Projectile.alpha = 0;
                    Projectile.ai[0] = 1f;
                    if (Projectile.ai[1] == 0f)
                    {
                        Projectile.ai[1] += 1f;
                        Projectile.position += Projectile.velocity * 1f;
                    }
                }
            }
            else
            {
                if (Projectile.alpha < 170 && Projectile.alpha + 5 >= 170)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CorruptPlants, Projectile.velocity.X * 0.025f, Projectile.velocity.Y * 0.025f, 170, default, 1.2f);
                    }
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 14, 0f, 0f, 170, default, 1.1f);
                }

                Projectile.alpha += 7;

                if (Projectile.alpha >= 255)
                {
                    Projectile.Kill();
                    return;
                }
            }
            base.AI();
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.AddBuff(BuffType<Buffs.Seeded>(), 5 * 60);
        }
    }
}
