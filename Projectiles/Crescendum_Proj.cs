using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
	public class Crescendum_Proj : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crescendum");
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.alpha = 0;
            Projectile.timeLeft = 1000;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            //Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
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
                Projectile.tileCollide = false;
                float num51 = 12;
                float num52 = 0.4f;

                Vector2 vector3 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float num53 = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - vector3.X;
                float num54 = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - vector3.Y;
                float num55 = (float)Math.Sqrt((double)(num53 * num53 + num54 * num54));
                if (num55 > 3000f)
                {
                    Projectile.Kill();
                }
                num55 = num51 / num55;
                num53 *= num55;
                num54 *= num55;

                {
                    if (Projectile.velocity.X < num53)
                    {
                        Projectile.velocity.X += num52;
                        if (Projectile.velocity.X < 0f && num53 > 0f)
                        {
                            Projectile.velocity.X += num52;
                        }
                    }
                    else if (Projectile.velocity.X > num53)
                    {
                        Projectile.velocity.X  -= num52;
                        if (Projectile.velocity.X > 0f && num53 < 0f)
                        {
                            Projectile.velocity.X -= num52;
                        }
                    }
                    if (Projectile.velocity.Y < num54)
                    {
                        Projectile.velocity.Y += num52;
                        if (Projectile.velocity.Y < 0f && num54 > 0f)
                        {
                            Projectile.velocity.Y += num52;
                        }
                    }
                    else if (Projectile.velocity.Y > num54)
                    {
                        Projectile.velocity.Y -= num52;
                        if (Projectile.velocity.Y > 0f && num54 < 0f)
                        {
                            Projectile.velocity.Y -= num52;
                        }
                    }
                }
                if (Main.myPlayer == Projectile.owner)
                {
                    Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                    Rectangle value2 = new Rectangle((int)Main.player[Projectile.owner].position.X, (int)Main.player[Projectile.owner].position.Y, Main.player[Projectile.owner].width, Main.player[Projectile.owner].height);
                    if (rectangle.Intersects(value2))
                    {
                        Projectile.Kill();
                    }
                }
            }
            Projectile.rotation += -0.6f;//* (float)Projectile.direction;

            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, 0, 0, 0, default, 0.5f);
            dust.noGravity = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if ((int)Projectile.ai[0] == 0)
            {
                Projectile.velocity = -Projectile.velocity;
                Projectile.netUpdate = true;
                Projectile.ai[0] = 1;
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            Projectile.ai[0] = 1;
            Projectile.velocity = -Projectile.velocity;
            return false;
        }

        public override void Kill(int timeLeft)
        {
            
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 16;
            return true;
        }
    }
}
