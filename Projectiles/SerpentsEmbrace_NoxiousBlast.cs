using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class SerpentsEmbrace_NoxiousBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Noxious Blast");
            Main.projFrames[projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            projectile.width = 75;
            projectile.height = 75;
            projectile.timeLeft = 42;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 1f;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            int num = Main.rand.Next(0, 3);
            Dust dust;
            if (num == 0)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.RedsWingsRun, 0, -1, 150);
                dust.velocity.X *= 0.3f;
                dust.color = new Color(0, 255, 150);
                dust.noGravity = false;
                dust.alpha = projectile.alpha;
            }
            else if (num == 2)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.RedsWingsRun, 0, -1, 150);
                dust.velocity.X *= 0.3f;
                dust.color = new Color(0, 255, 0);
                dust.noGravity = false;
                dust.alpha = projectile.alpha;
            }

            Lighting.AddLight(projectile.Center, 0f, 0.75f, 0.3f);
            if (projectile.timeLeft > 12)
            {
                projectile.alpha -= 1;
            }
            if (projectile.timeLeft == 12)
            {
                projectile.friendly = true;
                projectile.frame++;
                projectile.alpha = 20;
                TerraLeague.PlaySoundWithPitch(projectile.Center, 3, 54, -0.6f);

                for (int i = 0; i < 15; i++)
                {
                    dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.RedsWingsRun, 0, -1, 150);
                    dust.color = new Color(0, 255, 150);
                    dust.noGravity = false;
                }

                if (projectile.owner == Main.myPlayer)
                {
                    int spawnAmount = Main.rand.Next(10, 21);
                    for (int i = 0; i < spawnAmount; i++)
                    {
                        Vector2 vector14 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                        vector14.Normalize();
                        vector14 *= (float)Main.rand.Next(10, 201) * 0.01f;
                        Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vector14.X, vector14.Y, ModContent.ProjectileType<SerpentsEmbrace_NoxiousCloud>(), projectile.damage / 2, 0, projectile.owner, 0f, (float)Main.rand.Next(-45, 1));
                    }
                }
            }

            if (projectile.timeLeft <= 12)
            {
                AnimateProjectile();
            }
            if (projectile.timeLeft == 11)
            {
                projectile.friendly = false;
            }
        }

        public override void Kill(int timeLeft)
        {
            

            base.Kill(timeLeft);
        }

        public void AnimateProjectile() 
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 3)
            {
                projectile.frame++;
                projectile.frame %= 5;
                projectile.frameCounter = 0;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Main.player[projectile.owner].AddBuff(ModContent.BuffType<SerpentineGrace>(), 3 * 60);

            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}
