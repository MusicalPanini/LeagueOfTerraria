using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class ColossusFist_Fist : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Colossus Fist");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.GolemFist);
            Projectile.aiStyle = 0;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            if (Projectile.alpha > 0)
                Projectile.alpha -= 20;
            Player player = Main.player[Projectile.owner];
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
            player.itemAnimation = 2;
            player.itemTime = 2;
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
            float maxDistance = (float)Math.Sqrt((double)(num167 * num167 + num168 * num168));
            if (Projectile.ai[0] == 0f)
            {
                if (maxDistance > 300)
                {
                    Projectile.ai[0] = 1f;
                }
                Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
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
            else if (Projectile.ai[0] == 1f)
            {
                Projectile.tileCollide = false;
                Projectile.rotation = (float)Math.Atan2((double)num168, (double)num167) - 1.57f;
                float num170 = 13f;
                if (maxDistance < 50f)
                {
                    Projectile.Kill();
                }
                maxDistance = num170 / maxDistance;
                num167 *= maxDistance;
                num168 *= maxDistance;
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.ai[0] = 1f;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 16;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 10), Projectile.Center);
            for (int i = 0; i < 6; i++)
            {
                Dust.NewDustDirect(Projectile.Center, 8, 8, 51,0,0,0,default, 0.75f);
            }
            Projectile.ai[0] = 1f;
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("TerraLeague/Projectiles/ColossusFist_Chain").Value;

            Vector2 position = Projectile.Center;
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            mountedCenter.Y += 4;
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
                }
            }
            Texture2D robotBase = ModContent.Request<Texture2D>("TerraLeague/Projectiles/ColossusFist_Base").Value;
            origin = new Vector2((float)robotBase.Width * 0.5f, (float)robotBase.Height * 0.5f);
            Color color3 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
            Main.spriteBatch.Draw(robotBase, mountedCenter - Main.screenPosition, sourceRectangle, color3, rotation, new Vector2((float)robotBase.Width * 0.5f, robotBase.Height), 1f, SpriteEffects.None, 0f);
            return true;
        }
    }
}
