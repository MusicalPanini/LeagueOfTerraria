using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class MercuryCannon_AccelGate : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Acceleration Gate");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 315;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Projectile.timeLeft > 300)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
                Projectile.localAI[0] += 16;
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
            }

            Vector2 beamPoint = Projectile.Top.RotatedBy(Projectile.rotation, Projectile.Center);
            Vector2 beamEnd = beamPoint + new Vector2(0, -Projectile.localAI[0] + 16).RotatedBy(Projectile.rotation);

            if (Projectile.timeLeft % 5 == 0)
                TerraLeague.DustLine(beamPoint, beamEnd, DustID.Electric, 0.1f, 1f);

            if (Main.LocalPlayer.whoAmI == Projectile.owner)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player player = Main.player[i];
                    if (player.active && !player.dead)
                    {
                        if (!player.InOpposingTeam(Main.player[Projectile.owner]))
                        {
                            if (!player.HasBuff(ModContent.BuffType<Buffs.HyperCharge>()))
                            {
                                float point = 0;
                                bool collides = Collision.CheckAABBvLineCollision(
                                    player.Hitbox.TopLeft(),
                                    player.Hitbox.Size(),
                                    beamPoint,
                                    beamEnd,
                                    Projectile.width,
                                    ref point);
                                if (collides)
                                {
                                    if (i == Projectile.owner)
                                        player.AddBuff(ModContent.BuffType<Buffs.HyperCharge>(), 240, false);
                                    else
                                    {
                                        player.AddBuff(ModContent.BuffType<Buffs.HyperCharge>(), 240, false);
                                        Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().SendBuffPacket(ModContent.BuffType<Buffs.HyperCharge>(), 240, i, -1, Projectile.owner);
                                    }
                                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), player.Center, player.velocity, ModContent.ProjectileType<MercuryCannon_AccelEFX>(), 0, 0);
                                }
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (Projectile.active)
                {
                    if (proj.type == ModContent.ProjectileType<MercuryCannon_Shot>())
                    {
                        if (!Main.player[proj.owner].InOpposingTeam(Main.player[Projectile.owner]))
                        {
                            if (proj.ai[0] == 0)
                            {
                                float point = 0;
                                bool collides = Collision.CheckAABBvLineCollision(
                                    proj.Hitbox.TopLeft(),
                                    proj.Hitbox.Size(),
                                    beamPoint,
                                    beamEnd,
                                    Projectile.width,
                                    ref point);
                                if (collides)
                                {
                                    proj.ai[0] = 1;
                                    proj.netUpdate = true;
                                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), proj.Center, proj.velocity, ModContent.ProjectileType<MercuryCannon_AccelEFX>(), 0, 0);
                                }
                            }
                        }
                    }
                }
            }
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
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Cloud, Projectile.velocity.X / 2, Projectile.velocity.Y / 2, 100, new Color(255, 192, 0), 0.5f);
            }

            base.Kill(timeLeft);
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
                        Projectile.position.Y - Main.screenPosition.Y + Projectile.height* 0.5f
                    ) + new Vector2(0, -Projectile.localAI[0]).RotatedBy(Projectile.rotation),
                    new Rectangle(0, 0, texture.Width, texture.Height),
                    Color.White,
                    Projectile.rotation,
                    new Vector2(texture.Width / 2f, texture.Height / 2f),
                    -1,
                    SpriteEffects.None,
                    0f
                );

            base.PostDraw(lightColor);
        }
    }
}
