using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class ArcaneEnergy_PulseControl : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arcane Energy Pulse Control");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 900;
            Projectile.friendly = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().channelProjectile = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.slow = true;
            if (Projectile.owner == Main.LocalPlayer.whoAmI)
            {
                int dir = player.Center.X > Main.MouseWorld.X ? -1 : 1;
                player.ChangeDir(dir);
                Projectile.ai[1] = (float)TerraLeague.CalcAngle(player.Center, Main.MouseWorld) - player.fullRotation;
                Projectile.netUpdate = true;

                float rot = Projectile.ai[1] + (player.direction == -1 ? MathHelper.Pi : 0) + player.fullRotation;

                //for (int i = 0; i < Projectile.localAI[0]  / 4; i++)
                //{
                //    Vector2 indicatorPos = player.MountedCenter + new Vector2((5 * Projectile.localAI[0] ) * (i/ (Projectile.localAI[0]  / 4f)), 0).RotatedBy(rot);
                //    Dust dust = Dust.NewDustPerfect(indicatorPos, 113);
                //    dust.velocity *= 0;
                //}

                //Vector2 indicatorPos = player.MountedCenter + new Vector2((5 * Projectile.localAI[0] ), 0).RotatedBy(rot);
                //Dust dust = Dust.NewDustPerfect(indicatorPos, 113);
                //dust.velocity *= 0;
            }

            if (player.channel)
            {
                Projectile.Center = player.MountedCenter;

                for (int k = 0; k < 2 + 1; k++)
                {
                    float scale = Projectile.localAI[0]  / 60f;
                    if (k % 2 == 1)
                        scale = Projectile.localAI[0]  / 75f;

                    Vector2 postion = (player.Top + new Vector2(0, -16)) + ((float)Main.rand.NextDouble() * 6.28318548f).ToRotationVector2() * (12f - (float)(2 * 2));
                    Dust dust = Dust.NewDustDirect(postion - Vector2.One * 8f, 16, 16, 113, 0, 0, 0, new Color(255, 0, 0), scale);
                    dust.velocity = Vector2.Normalize(Projectile.Center - postion) * 1.5f * (10f - (float)2 * 2f) / 10f;
                    dust.noGravity = true;
                    dust.customData = player;
                }

                if (Projectile.localAI[0]  < 540)
                {

                    if (Projectile.soundDelay == 0 && Projectile.localAI[0]  < 540 && Projectile.localAI[0]  > 29)
                    {
                        Projectile.soundDelay = 30;
                        TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 15, -0.5f + (Projectile.localAI[0]  - 30) / 540f);
                    }

                    if ((int)Projectile.localAI[0]  % 60 == 0)
                    {
                        string percent = (((int)Projectile.localAI[0]  / 60) * 100 + 100) + "%";
                        CombatText.NewText(player.Hitbox, new Color(0, 100, 255), percent, false, true);
                    }

                    Projectile.localAI[0] ++;

                    if (Projectile.localAI[0]  >= 540)
                    {
                        CombatText.NewText(player.Hitbox, new Color(0, 100, 255), "1000%", true, true);
                        TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 15, -0.5f + 510f / 540f);
                    }
                }
                player.itemTime = 24;
                player.itemAnimation = 24;
            }
            else
            {
                player.itemTime = 24;
                player.itemAnimation = 24;
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
            Player player = Main.player[Projectile.owner];

            float rot = Projectile.ai[1] + (player.direction == -1 ? MathHelper.Pi : 0) + player.fullRotation;
            Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, new Vector2(10, 0).RotatedBy(rot), ModContent.ProjectileType<ArcaneEnergy_Pulse>(), (int)(Projectile.damage * (1 + Projectile.localAI[0] /60f)), Projectile.knockBack, Projectile.owner, Projectile.localAI[0] );
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 3, 53, -0.5f);

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
