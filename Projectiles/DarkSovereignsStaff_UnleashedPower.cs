using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Projectiles.Homing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
	public class DarkSovereignsStaff_UnleashedPower : HomingProjectile
	{
        int totalProj = 1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Unleashed Power");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.alpha = 255;
            projectile.timeLeft = 1000;
            projectile.penetrate = 1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.minion = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;

            CanOnlyHitTarget = true;
            CanRetarget = false;
            MaxVelocity = 12;
            TurningFactor = 0.9f;
        }

        public override void AI()
        {
            NPC target = Main.npc[(int)projectile.ai[0]];

            if (projectile.soundDelay == 0)
            {
                totalProj = (int)projectile.velocity.X;
                projectile.velocity *= 0;
                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, 32, 32, DustID.Clentaminator_Purple, 0, 0, projectile.alpha);
                    dust.noGravity = true;
                    dust.noLight = true;
                }
            }
            projectile.soundDelay = 100;

            if (!target.active)
            {
                projectile.Kill();
                return;
            }
            if (projectile.timeLeft == 1000 - (int)(45f * projectile.ai[1] / (float)totalProj))
            {
                projectile.friendly = true;
                projectile.alpha = 0;
                projectile.localAI[1] = 1;
                projectile.velocity = new Vector2(12, 0).RotatedBy(projectile.AngleTo(target.Center));
                projectile.extraUpdates = 1;
                Main.PlaySound(SoundID.Item1, projectile.Center);
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 45), projectile.Center);
            }
            if ((int)projectile.localAI[1] == 1)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, 32, 32, DustID.Clentaminator_Purple, 0, 0, projectile.alpha);
                dust.noGravity = true;
                dust.noLight = true;

                HomingAI();
            }
            else
            {
                projectile.Center = Main.player[projectile.owner].MountedCenter + new Vector2(0, -48).RotatedBy(((MathHelper.TwoPi * projectile.ai[1]) / (float)totalProj));
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, 32, 32, DustID.Clentaminator_Purple, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, projectile.alpha);
                dust.noGravity = true;
                dust.noLight = true;
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    projectile.position.X - Main.screenPosition.X + projectile.width * 0.5f,
                    projectile.position.Y - Main.screenPosition.Y + projectile.height * 0.5f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                projectile.rotation,
                new Vector2(texture.Width, texture.Width) * 0.5f,
                projectile.scale,
                SpriteEffects.None,
                0f
            );
            base.PostDraw(spriteBatch, lightColor);
        }
    }
}
