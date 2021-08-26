using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class EyeofGod_TestofSpirit : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Test of Spirit");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.alpha = 0;
            Projectile.timeLeft = 360;
            Projectile.tileCollide = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().channelProjectile = true;
        }

        public override void AI()
        {
            if (Projectile.alpha > 0)
                Projectile.alpha -= 20;
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            player.itemRotation = player.AngleTo(Projectile.Center);
            if (player.itemRotation > MathHelper.PiOver2)
            {
                player.itemRotation -= MathHelper.Pi;
            }
            else if (player.itemRotation <= -MathHelper.PiOver2)
            {
                player.itemRotation += MathHelper.Pi;
            }

            if (player.dead)
            {
                Projectile.Kill();
                return;
            }
            player.itemAnimation = 5;
            player.itemTime = 5;
            if (Projectile.alpha == 0)
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
            Vector2 vector15 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
            float num167 = player.position.X + (float)(player.width / 2) - vector15.X;
            float num168 = player.position.Y + (float)(player.height / 2) - vector15.Y;
            float Distance = (float)Math.Sqrt((double)(num167 * num167 + num168 * num168));
            if (Projectile.ai[0] == 0f)
            {
                if (Distance > 400)
                {
                    Projectile.ai[0] = 1f;
                }
                Projectile.rotation = (float)Math.Atan2((double)(player.MountedCenter - (Projectile.Top + new Vector2(0, 6))).Y, (double)(player.MountedCenter - (Projectile.Top + new Vector2(0, 6))).X) - 1.57f;
                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] > 8f)
                {
                    Projectile.ai[1] = 8f;
                }
                if (Projectile.velocity.X < 0f)
                {
                    Projectile.spriteDirection = -1;
                }
                else
                {
                    Projectile.spriteDirection = 1;

                }
            }
            else if (Projectile.ai[0] >= 1f)
            {
                if ((int)Projectile.ai[0] == 2f)
                {
                    TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 73, 0.5f);
                    Projectile.ai[0] = 1f;
                }

                Projectile.tileCollide = false;
                Projectile.friendly = false;
                Projectile.rotation = (float)Math.Atan2((double)num168, (double)num167) - 1.57f;
                float num170 = 16f;
                if (Distance < 50f)
                {
                    Projectile.Kill();
                }
                Distance = num170 / Distance;
                num167 *= Distance;
                num168 *= Distance;
                Projectile.velocity.X = num167;
                Projectile.velocity.Y = num168;
                if (Projectile.velocity.X < 0f)
                {
                    Projectile.spriteDirection = 1;

                }
                else
                {
                    Projectile.spriteDirection = -1;
                }
            }
            base.AI();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.ai[0] = 1f;
            return false;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.friendly = false;
            Projectile.ai[0] = 2f;
            Projectile.netUpdate = true;

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().vessel || target.immortal)
                return false;

            return base.CanHitNPC(target);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("TerraLeague/Projectiles/EyeofGod_TestofSpiritChain").Value;

            Vector2 position = Projectile.Top + new Vector2(0, 6);
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
            float num1 = (float)texture.Height;
            Vector2 vector2_4 = mountedCenter - position;
            float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
            bool flag = true;
            if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                flag = false;
            if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
                flag = false;
            while (flag)
            {
                if ((double)vector2_4.Length() < (double)num1 + 1.0)
                {
                    flag = false;
                }
                else
                {
                    Vector2 vector2_1 = vector2_4;
                    vector2_1.Normalize();
                    position += vector2_1 * num1;
                    vector2_4 = mountedCenter - position;
                    Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
                    color2 = Projectile.GetAlpha(color2);
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);

                    if (Main.rand.Next(0, 6) == 0)
                    {
                        Dust dust2 = Dust.NewDustPerfect(position, 59, null, 100, new Color(0, 255, 201), 1f);
                        dust2.fadeIn = 0.5f;
                        dust2.noGravity = true;
                    }
                }
            }

            return true;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
