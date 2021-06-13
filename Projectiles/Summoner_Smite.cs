using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using TerraLeague.Projectiles.Homing;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Summoner_Smite : HomingProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Smite");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.scale = 1.2f;
            projectile.timeLeft = 301;
            projectile.extraUpdates = 8;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;

            CanOnlyHitTarget = true;
            CanRetarget = false;
            TurningFactor = 0f;
            MaxVelocity = 16;
        }

        public override void AI()
        {
            if(projectile.soundDelay == 0)
            {
                Main.PlaySound(new LegacySoundStyle(2, 88), projectile.Center);
                if (projectile.owner == Main.LocalPlayer.whoAmI)
                    projectile.netUpdate = true;
            }
            projectile.soundDelay = 100;

            if (projectile.Hitbox.Intersects(Main.npc[(int)projectile.ai[0]].Hitbox))
            {
                projectile.velocity = Vector2.Zero;
                projectile.Center = Main.npc[(int)projectile.ai[0]].Center;
            }
            else
            {
                HomingAI();

                for (int i = 0; i < 3; i++)
                {
                    Vector2 dustBoxPosition = new Vector2(projectile.position.X + 6, projectile.position.Y + 6);
                    int dustBoxWidth = projectile.width - 12;
                    int dustBoxHeight = projectile.height - 12;
                    Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, DustID.AncientLight, 0f, 0f, 100, new Color(255, 106, 0, 150), 1.5f);
                    dust.noGravity = true;
                    dust.velocity *= 0.1f;
                    dust.velocity += projectile.velocity * 0.1f;
                    dust.position.X -= projectile.velocity.X / 3f * (float)i;
                    dust.position.Y -= projectile.velocity.Y / 3f * (float)i;
                }

                Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.AncientLight, 0, 0, 0, new Color(255, 106, 0, 150), 1f);
                dust2.noGravity = true;
                dust2.velocity *= 3f;
                Lighting.AddLight(projectile.position, 1f, 1f, 0f);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.life <= 0 || target.boss)
                Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().lifeToHeal += (int)(Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().maxLifeLastStep * 0.15);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            Main.PlaySound(new LegacySoundStyle(2, 92), projectile.Center);
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.AncientLight, 0, 0, 0, new Color(255, 106, 0, 150), 2f);
                dust.velocity *= 3f;
                dust.noGravity = true;
            }

            base.Kill(timeLeft);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
