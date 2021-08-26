using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Projectiles.Homing;
using Terraria;
using Terraria.GameContent;
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
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.alpha = 255;
            Projectile.timeLeft = 1000;
            Projectile.penetrate = 1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;

            CanOnlyHitTarget = true;
            CanRetarget = false;
            MaxVelocity = 12;
            TurningFactor = 0.9f;
        }

        public override void AI()
        {
            NPC target = Main.npc[(int)Projectile.ai[0]];

            if (Projectile.soundDelay == 0)
            {
                totalProj = (int)Projectile.velocity.X;
                Projectile.velocity *= 0;
                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, 32, 32, 112, 0, 0, Projectile.alpha);
                    dust.noGravity = true;
                    dust.noLight = true;
                }
            }
            Projectile.soundDelay = 100;

            if (!target.active)
            {
                Projectile.Kill();
                return;
            }
            if (Projectile.timeLeft == 1000 - (int)(45f * Projectile.ai[1] / (float)totalProj))
            {
                Projectile.friendly = true;
                Projectile.alpha = 0;
                Projectile.localAI[1] = 1;
                Projectile.velocity = new Vector2(12, 0).RotatedBy(Projectile.AngleTo(target.Center));
                Projectile.extraUpdates = 1;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 45), Projectile.Center);
            }
            if ((int)Projectile.localAI[1] == 1)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, 32, 32, 112, 0, 0, Projectile.alpha);
                dust.noGravity = true;
                dust.noLight = true;

                HomingAI();
            }
            else
            {
                Vector2 move = Main.player[Projectile.owner].Top - Projectile.Center;

                AdjustMagnitude(ref move, 6);
                Projectile.velocity = (10 * Projectile.velocity + move) / 10f;
                AdjustMagnitude(ref Projectile.velocity, 8);

                Rectangle projectileHitBox = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                for (int i = 0; i < 1000; i++)
                {
                    if (i != Projectile.whoAmI && Main.projectile[i].active && Main.projectile[i].type == Projectile.type)
                    {
                        Rectangle targetHitBox = new Rectangle((int)Main.projectile[i].position.X, (int)Main.projectile[i].position.Y, Main.projectile[i].width, Main.projectile[i].height);
                        if (projectileHitBox.Intersects(targetHitBox))
                        {
                            Vector2 vector77 = Main.projectile[i].Center - Projectile.Center;
                            if (vector77.X == 0f && vector77.Y == 0f)
                            {
                                if (i < Projectile.whoAmI)
                                {
                                    vector77.X = -1f;
                                    vector77.Y = 1f;
                                }
                                else
                                {
                                    vector77.X = 1f;
                                    vector77.Y = -1f;
                                }
                            }
                            vector77.Normalize();
                            vector77 *= 0.1f;
                            Projectile.velocity -= vector77;
                            Projectile projectile2 = Main.projectile[i];
                            projectile2.velocity += vector77;
                        }
                    }
                }

                //Projectile.Center = Main.player[Projectile.owner].MountedCenter + new Vector2(0, -48).RotatedBy(((MathHelper.TwoPi * Projectile.ai[1]) / (float)totalProj));
            }
        }

        private void AdjustMagnitude(ref Vector2 vector, float num = 12)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > num)
            {
                vector *= num / magnitude;
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
                Dust dust = Dust.NewDustDirect(Projectile.position, 32, 32, 112, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, Projectile.alpha);
                dust.noGravity = true;
                dust.noLight = true;
            }
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f,
                    Projectile.position.Y - Main.screenPosition.Y + Projectile.height * 0.5f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                Projectile.rotation,
                new Vector2(texture.Width, texture.Width) * 0.5f,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );
            base.PostDraw(lightColor);
        }
    }
}
