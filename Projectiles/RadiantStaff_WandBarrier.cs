using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class RadiantStaff_WandBarrier : ModProjectile
    {
        readonly bool[] hasHitPlayer = new bool[200];

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            DisplayName.SetDefault("Prismatic Barrier");
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.scale = 1f;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 240;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            AnimateProjectile();

            Lighting.AddLight(Projectile.position, 0.75f, 0.75f, 0.75f);
            for (int i = 0; i < 1; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, 16, 16, DustID.Cloud, 0f, 0f, 0, new Color(255, 255, 255), 2f);
                dust.noGravity = true;
                dust.noLight = true;
            }

            if (Projectile.timeLeft > 210)
            {
                if (Projectile.position.X + (float)(Projectile.width / 2) > player.position.X + (float)(player.width / 2))
                    player.ChangeDir(1);
                else
                    player.ChangeDir(-1);
            }

            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] >= 25f)
                {
                    Projectile.ai[0] = 1f;
                    Projectile.ai[1] = 0f;
                    Projectile.damage *= 2;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                float returnSpeed = 16f;
                float acceleration = 0.5f;

                Vector2 vector2 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float num44 = player.position.X + (float)(player.width / 2) - vector2.X;
                float num45 = player.position.Y + (float)(player.height / 2) - vector2.Y;
                float num46 = (float)Math.Sqrt((double)(num44 * num44 + num45 * num45));
                if (num46 > 3000f)
                {
                    Projectile.Kill();
                }
                num46 = returnSpeed / num46;
                num44 *= num46;
                num45 *= num46;
                if (Projectile.type == 383)
                {
                    Vector2 vector3 = new Vector2(num44, num45) - Projectile.velocity;
                    if (vector3 != Vector2.Zero)
                    {
                        Vector2 value = vector3;
                        value.Normalize();
                        Projectile.velocity += value * Math.Min(acceleration, vector3.Length());
                    }
                }
                else
                {
                    if (Projectile.velocity.X < num44)
                    {
                        Projectile.velocity.X = Projectile.velocity.X + acceleration;
                        if (Projectile.velocity.X < 0f && num44 > 0f)
                        {
                            Projectile.velocity.X = Projectile.velocity.X + acceleration;
                        }
                    }
                    else if (Projectile.velocity.X > num44)
                    {
                        Projectile.velocity.X = Projectile.velocity.X - acceleration;
                        if (Projectile.velocity.X > 0f && num44 < 0f)
                        {
                            Projectile.velocity.X = Projectile.velocity.X - acceleration;
                        }
                    }
                    if (Projectile.velocity.Y < num45)
                    {
                        Projectile.velocity.Y = Projectile.velocity.Y + acceleration;
                        if (Projectile.velocity.Y < 0f && num45 > 0f)
                        {
                            Projectile.velocity.Y = Projectile.velocity.Y + acceleration;
                        }
                    }
                    else if (Projectile.velocity.Y > num45)
                    {
                        Projectile.velocity.Y = Projectile.velocity.Y - acceleration;
                        if (Projectile.velocity.Y > 0f && num45 < 0f)
                        {
                            Projectile.velocity.Y = Projectile.velocity.Y - acceleration;
                        }
                    }
                }
                if (Main.myPlayer == Projectile.owner)
                {
                    Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                    Rectangle value2 = new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height);
                    if (rectangle.Intersects(value2))
                    {
                        Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().AddShield(Projectile.damage, 240, Color.LightGoldenrodYellow, ShieldType.Basic);
                        Projectile.Kill();
                    }
                }
                if (Projectile.ai[0] == 0f)
                {
                    Vector2 velocity = Projectile.velocity;
                    velocity.Normalize();
                    return;
                }
                Vector2 vector4 = Projectile.Center - player.Center;
                vector4.Normalize();
            }

            for (int i = 0; i < Main.player.Length; i++)
            {
                if (!hasHitPlayer[i] && Projectile.Hitbox.Intersects(Main.player[i].Hitbox))
                {
                    hasHitPlayer[i] = true;
                    Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().SendShieldPacket(Projectile.damage, i, ShieldType.Basic, 240, -1, Projectile.owner, Color.LightGoldenrodYellow);
                }
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 10; 
            return true;
        }

        public void AnimateProjectile() 
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frame++;
                Projectile.frame %= 4; 
                Projectile.frameCounter = 0;
            }
        }
    }
}
