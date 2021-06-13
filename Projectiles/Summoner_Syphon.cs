using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
    public class Summoner_Syphon : HomingProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Syphon");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 2;
            projectile.alpha = 255;
            projectile.scale = 1.2f;
            projectile.timeLeft = 305;
            projectile.extraUpdates = 4;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;

            CanOnlyHitTarget = true;
            TargetPlayers = false;
            TurningFactor = 0.93f;
            CanRetarget = false;
            MaxVelocity = 4;
        }

        public override void AI()
        {
            if(projectile.soundDelay == 0)
                TerraLeague.PlaySoundWithPitch(projectile.Center, 3, 54, -0.5f);
            projectile.soundDelay = 100;

            if (projectile.timeLeft == 301)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust dust2 = Dust.NewDustDirect(Main.npc[(int)projectile.ai[0]].position, Main.npc[(int)projectile.ai[0]].width, Main.npc[(int)projectile.ai[0]].height, DustID.PortalBolt, 0, 0, 0, new Color(255, 0, 0), 2);
                    dust2.noGravity = true;
                }
            }

            if ((int)projectile.ai[1] == 0)
            {
                if (!TargetEntity.active)
                {
                    projectile.Kill();
                }
                else
                {
                    projectile.Center = TargetEntity.Center;
                }
            }
            else
            {
                HomingAI();

                if (TargetPlayers && TargetWhoAmI >= 0)
                {
                    if (projectile.Hitbox.Intersects(TargetEntity.Hitbox))
                    {
                        OnHitFriendlyPlayer(Main.player[TargetWhoAmI]);
                    }
                }
            }

            Dust dust = Dust.NewDustPerfect(projectile.position, 263, Vector2.Zero, 0, new Color(255, 0, 110), 1f);
            dust.noGravity = true;
        }

        public override void OnHitFriendlyPlayer(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().lifeToHeal += player.GetModPlayer<PLAYERGLOBAL>().ScaleValueWithHealPower(10, true);
            projectile.Kill();
            base.OnHitFriendlyPlayer(player);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.netUpdate = true;
            projectile.timeLeft = 302;
            projectile.ai[1] = 1;
            projectile.ai[0] = projectile.owner;
            TargetPlayers = true;

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.PortalBolt, projectile.velocity.X, projectile.velocity.Y, 0, new Color(255, 0, 0), 2);
                dust.noGravity = true;
            }

            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)projectile.ai[1] == 0)
                return base.CanHitNPC(target);
            else
                return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
