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
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 2;
            Projectile.alpha = 255;
            Projectile.scale = 1.2f;
            Projectile.timeLeft = 305;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            CanOnlyHitTarget = true;
            TargetPlayers = false;
            TurningFactor = 0.93f;
            CanRetarget = false;
            MaxVelocity = 4;
        }

        public override void AI()
        {
            if(Projectile.soundDelay == 0)
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 3, 54, -0.5f);
            Projectile.soundDelay = 100;

            if (Projectile.timeLeft == 301)
            {
                for (int i = 0; i < 10; i++)
                {
                    float x2 = Projectile.position.X - Projectile.velocity.X / 10f * (float)i;
                    float y2 = Projectile.position.Y - Projectile.velocity.Y / 10f * (float)i;
                    int num141 = Dust.NewDust(new Vector2(x2, y2), 1, 1, DustID.PortalBolt, 0f, 0f, 0, new Color(255, 0, 0), 0.5f);
                    Main.dust[num141].alpha = Projectile.alpha;
                    Main.dust[num141].position.X = x2;
                    Main.dust[num141].position.Y = y2;
                    Dust obj77 = Main.dust[num141];
                    obj77.velocity *= 0f;
                    Main.dust[num141].noGravity = true;
                }
            }

            if ((int)Projectile.ai[1] == 0)
            {
                if (!TargetEntity.active)
                {
                    Projectile.Kill();
                }
                else
                {
                    Projectile.Center = TargetEntity.Center;
                }
            }
            else
            {
                HomingAI();

                if (TargetPlayers && TargetWhoAmI >= 0)
                {
                    if (Projectile.Hitbox.Intersects(TargetEntity.Hitbox))
                    {
                        OnHitFriendlyPlayer(Main.player[TargetWhoAmI]);
                    }
                }
            }

            Dust dust = Dust.NewDustPerfect(Projectile.position, 263, Vector2.Zero, 0, new Color(255, 0, 110), 1f);
            dust.noGravity = true;
        }

        public override void OnHitFriendlyPlayer(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().lifeToHeal += player.GetModPlayer<PLAYERGLOBAL>().ScaleValueWithHealPower(10, true);
            Projectile.Kill();
            base.OnHitFriendlyPlayer(player);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.netUpdate = true;
            Projectile.timeLeft = 302;
            Projectile.ai[1] = 1;
            Projectile.ai[0] = Projectile.owner;
            TargetPlayers = true;

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PortalBolt, Projectile.velocity.X, Projectile.velocity.Y, 0, new Color(255, 0, 0), 2);
                dust.noGravity = true;
            }

            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)Projectile.ai[1] == 0)
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
