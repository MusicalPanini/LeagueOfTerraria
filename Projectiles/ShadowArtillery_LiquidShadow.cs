using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class ShadowArtillery_LiquidShadow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Liquid Shadow");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            Projectile.tileCollide = true;
            Projectile.localAI[1] = 0f;
            if (Main.myPlayer == Projectile.owner && Projectile.ai[0] == 0f)
            {
                Projectile.tileCollide = false;
                if (Main.player[Projectile.owner].channel)
                {
                    Projectile.localAI[1] = -1f;
                    float num145 = 12f;
                    Vector2 vector13 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                    float num146 = (float)Main.mouseX + Main.screenPosition.X - vector13.X;
                    float num147 = (float)Main.mouseY + Main.screenPosition.Y - vector13.Y;
                    if (Main.player[Projectile.owner].gravDir == -1f)
                    {
                        num147 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector13.Y;
                    }
                    float num148 = (float)Math.Sqrt((double)(num146 * num146 + num147 * num147));
                    num148 = (float)Math.Sqrt((double)(num146 * num146 + num147 * num147));
                    if (num148 > num145)
                    {
                        num148 = num145 / num148;
                        num146 *= num148;
                        num147 *= num148;
                        if (num146 != Projectile.velocity.X || num147 != Projectile.velocity.Y)
                        {
                            Projectile.netUpdate = true;
                        }
                        Projectile.velocity.X = num146;
                        Projectile.velocity.Y = num147;
                    }
                    else
                    {
                        if (num146 != Projectile.velocity.X || num147 != Projectile.velocity.Y)
                        {
                            Projectile.netUpdate = true;
                        }
                        Projectile.velocity.X = num146;
                        Projectile.velocity.Y = num147;
                    }
                }
                else
                {
                    Projectile.ai[0] = 1f;
                    Projectile.netUpdate = true;
                }
            }
            if (Projectile.ai[0] == 1f && Projectile.type != 109)
            {
                if (Projectile.type == 42 || Projectile.type == 65 || Projectile.type == 68 || Projectile.type == 354)
                {
                    Projectile.ai[1] += 1f;
                    if (Projectile.ai[1] >= 60f)
                    {
                        Projectile.ai[1] = 60f;
                        Projectile.velocity.Y += 0.2f;
                    }
                }
                else
                {
                    Projectile.velocity.Y += 0.41f;
                }
            }
            else if (Projectile.ai[0] == 2f && Projectile.type != 109)
            {
                Projectile.velocity.Y += 0.2f;
                if ((double)Projectile.velocity.X < -0.04)
                {
                    Projectile.velocity.X += 0.04f;
                }
                else if ((double)Projectile.velocity.X > 0.04)
                {
                    Projectile.velocity.X -= 0.04f;
                }
                else
                {
                    Projectile.velocity.X = 0f;
                }
            }
            Projectile.rotation += 0.1f;
            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }

            Dust dust;
            for (int i = 0; i < 3; i++)
            {
                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.WhiteTorch, 0f, 0f, 0, new Color(5, 245, 150), 2.5f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += Projectile.velocity * 0.1f;
                dust.position.X -= Projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= Projectile.velocity.Y / 3f * (float)i;
            }

            dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Cloud, 0, 0, 50, Color.DarkSeaGreen, 2f);
            dust.noGravity = true;
            dust.velocity /= 2f;


        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Blackout, 30);

            base.OnHitPlayer(target, damage, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Cloud, Projectile.velocity.X / 2, Projectile.velocity.Y / 2, 100, Color.DarkSeaGreen, 1f);
            }

            base.Kill(timeLeft);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
