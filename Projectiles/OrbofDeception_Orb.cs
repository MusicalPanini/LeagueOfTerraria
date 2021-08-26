using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using TerraLeague.Buffs;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class OrbofDeception_Orb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            DisplayName.SetDefault("Orb of Deception");
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
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

            for (int i = 0; i < 3; i++)
            {
                Vector2 dustBoxPosition = new Vector2(Projectile.position.X, Projectile.position.Y);
                int dustBoxWidth = Projectile.width;
                int dustBoxHeight = Projectile.height;
                Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, DustID.PortalBolt, 0f, 0f, 100, new Color(229, 242, 249), 1.5f);
                dust.noGravity = true;
                dust.noLight = true;
                dust.velocity *= 0.1f;
                dust.velocity += Projectile.velocity * 0.1f;
                dust.position.X -= Projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= Projectile.velocity.Y / 3f * (float)i;
            }


            //player.itemTime = 5;
            if (Projectile.timeLeft > 210)
            {
                if (Projectile.position.X + (float)(Projectile.width / 2) > player.position.X + (float)(player.width / 2))
                {
                    player.ChangeDir(1);
                }
                else
                {
                    player.ChangeDir(-1);
                }
            }

            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] >= 25f)
                {
                    Projectile.ai[0] = 1f;
                    Projectile.ai[1] = 0f;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                float returnSpeed = 16f;
                float acceleration = 0.5f;

                float xDif = player.Center.X - Projectile.Center.X;
                float yDif = player.Center.Y - Projectile.Center.Y;
                float distance = Projectile.Distance(player.Center);

                if (distance > 3000f)
                {
                    Projectile.Kill();
                }
                distance = returnSpeed / distance;
                xDif *= distance;
                yDif *= distance;

                if (Projectile.velocity.X < xDif)
                {
                    Projectile.velocity.X = Projectile.velocity.X + acceleration;
                    if (Projectile.velocity.X < 0f && xDif > 0f)
                    {
                        Projectile.velocity.X = Projectile.velocity.X + acceleration;
                    }
                }
                else if (Projectile.velocity.X > xDif)
                {
                    Projectile.velocity.X = Projectile.velocity.X - acceleration;
                    if (Projectile.velocity.X > 0f && xDif < 0f)
                    {
                        Projectile.velocity.X = Projectile.velocity.X - acceleration;
                    }
                }

                if (Projectile.velocity.Y < yDif)
                {
                    Projectile.velocity.Y = Projectile.velocity.Y + acceleration;
                    if (Projectile.velocity.Y < 0f && yDif > 0f)
                    {
                        Projectile.velocity.Y = Projectile.velocity.Y + acceleration;
                    }
                }
                else if (Projectile.velocity.Y > yDif)
                {
                    Projectile.velocity.Y = Projectile.velocity.Y - acceleration;
                    if (Projectile.velocity.Y > 0f && yDif < 0f)
                    {
                        Projectile.velocity.Y = Projectile.velocity.Y - acceleration;
                    }
                }

                if (Main.myPlayer == Projectile.owner)
                {
                    Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                    Rectangle value2 = new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height);
                    if (rectangle.Intersects(value2))
                    {
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
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, 8, 8, DustID.Cloud, 0f, 0f, 0, new Color(255, 255, 255), 1f);
                dust.noGravity = true;
                dust.noLight = true;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Projectile.ai[0] == 1f)
                crit = true;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
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

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f,
                    Projectile.position.Y - Main.screenPosition.Y + Projectile.height - (texture.Height / 4) * 0.5f
                ),
                new Rectangle(0, (texture.Height / 4) * Projectile.frame, texture.Width, texture.Height/4),
                Color.White,
                Projectile.rotation,
                new Vector2(texture.Width, texture.Width) * 0.5f,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}
