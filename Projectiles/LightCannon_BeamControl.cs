using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class LightCannon_BeamControl : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Cannon Beam Control");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 300;
            Projectile.friendly = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().channelProjectile = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.itemTime = 18;
            player.itemAnimation = 18;
            if (player.channel)
            {
                Projectile.Center = player.MountedCenter + new Vector2(-16, -14) + new Vector2(80 * player.direction, 0).RotatedBy(player.itemRotation + player.fullRotation) + Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56];

                if (Projectile.owner == Main.LocalPlayer.whoAmI)
                {
                    int dir = player.Center.X > Main.MouseWorld.X ? -1 : 1;
                    player.ChangeDir(dir);
                    Projectile.ai[1] = (float)TerraLeague.CalcAngle(player.Center, Main.MouseWorld) - player.fullRotation;
                    Projectile.netUpdate = true;
                }
                player.itemRotation = Projectile.ai[1];

                for (int k = 0; k < 2 + 1; k++)
                {
                    float scale = 0.8f;
                    if (k % 2 == 1)
                    {
                        scale = 0.6f;
                    }

                    Vector2 postion = Projectile.Center + ((float)Main.rand.NextDouble() * 6.28318548f).ToRotationVector2() * (12f - (float)(2 * 2));
                    Dust dust = Dust.NewDustDirect(postion - Vector2.One * 8f, 16, 16, 264, 0, 0, 0, default, scale);
                    dust.velocity = Vector2.Normalize(Projectile.Center - postion) * 1.5f * (10f - (float)2 * 2f) / 10f;
                    dust.noGravity = true;
                    dust.customData = player;
                }
                if (Projectile.localAI[0]  < 30)
                {
                    Projectile.localAI[0] ++;
                }
                else
                {
                    Projectile.ai[0] = 1;
                    Projectile.Kill();
                }
            }
            else
            {
                Projectile.Kill();
            }

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.ai[0] != 0)
            {
                if (Projectile.owner == Main.LocalPlayer.whoAmI)
                {
                    Player player = Main.player[Projectile.owner];

                    float rot = Projectile.ai[1] + (player.direction == -1 ? MathHelper.Pi : 0) + player.fullRotation;

                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, new Vector2(10, 0).RotatedBy(rot), ProjectileType<LightCannon_Beam>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }

                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 72, -1f);
            }
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
    }
}
