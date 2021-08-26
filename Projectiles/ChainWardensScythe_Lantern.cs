using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class ChainWardensScythe_Lantern : ModProjectile
    {
        readonly int effectRadius = 200;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lantern");
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 44;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 0;
            Projectile.timeLeft = 360;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Lighting.AddLight(Projectile.Center, Color.SeaGreen.ToVector3() * 2f);

            if ((int)Projectile.ai[1] == 0)
            {
                if (Projectile.timeLeft == 360)
                {
                    PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                    int shieldAmount = modPlayer.ScaleValueWithHealPower(Projectile.damage);

                    TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 8, 0);

                    for (int i = 0; i < effectRadius / 5; i++)
                    {
                        Vector2 pos = new Vector2(effectRadius, 0).RotatedBy(MathHelper.ToRadians(360 * (i / (effectRadius / 5f)))) + Projectile.Center;

                        Dust dustR = Dust.NewDustPerfect(pos, 188, Vector2.Zero, 0, new Color(0, 255, 0, 0), 2);
                        dustR.noGravity = true;
                    }

                    for (int j = 0; j < 50; j++)
                    {
                        Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 188);
                        dust.noGravity = true;
                        dust.scale = 2;
                    }

                    if (Main.LocalPlayer.whoAmI == player.whoAmI)
                    {
                        var players = Targeting.GetAllPlayersInRange(Projectile.Center, effectRadius, -1, player.team);

                        for (int i = 0; i < players.Count; i++)
                        {
                            if (i == Projectile.owner)
                            {
                                modPlayer.AddShield(shieldAmount, 240, Color.SeaGreen, ShieldType.Basic);
                            }
                            else if (Main.netMode == NetmodeID.MultiplayerClient)
                            {
                                modPlayer.SendShieldPacket(shieldAmount, i, ShieldType.Basic, 240, -1, player.whoAmI, Color.SeaGreen);
                            }
                        }
                    }
                }

                if (Projectile.timeLeft <= 350)
                {
                    for (int i = 0; i < Main.player.Length; i++)
                    {
                        if (Projectile.Hitbox.Intersects(Main.player[i].Hitbox) && i != Projectile.owner && player.team == Main.player[i].team)
                        {
                            Main.player[i].Teleport(player.position);
                            Projectile.Kill();
                        }
                    }
                }
            }

            if (Projectile.Distance(player.MountedCenter) > 1000)
            {
                Projectile.Kill();
            }

            Projectile.position.Y += (float)Math.Sin(Projectile.timeLeft * 0.05) * 0.4f;

            Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 188, 0, -3);
            dust2.scale = 0.75f;
            dust2.alpha = 150;
            dust2.velocity /= 3;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("TerraLeague/Projectiles/ChainWardensScythe_Chain").Value;

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
                }
            }

            return true;
        }
    }
}
