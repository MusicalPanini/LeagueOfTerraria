using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class ArcaneEnergy_PulseControl : ModProjectile
    {
        float orbWidth { get { return 32 * (Projectile.localAI[0] * 2 / 540f); } }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arcane Energy Pulse Control");
            Main.projFrames[Projectile.type] = 4;
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

                for (int k = 0; k < 1; k++)
                {
                    float scale = Projectile.localAI[0] / 240f;

                    Vector2 postion = player.Top + new Vector2(0, -16) - (Vector2.One * orbWidth * 0.5f);
                    Dust dust = Dust.NewDustDirect(postion, (int)orbWidth, (int)orbWidth, 113, 0, 0, 225, default, scale);
                    dust.velocity *= 0.2f;
                    dust.noGravity = true;
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
            AnimateProjectile();
            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];

            if (player.whoAmI == Projectile.owner)
            {
                float rot = Projectile.ai[1] + (player.direction == -1 ? MathHelper.Pi : 0) + player.fullRotation;
                Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, new Vector2(10, 0).RotatedBy(rot), ModContent.ProjectileType<ArcaneEnergy_Pulse>(), (int)(Projectile.damage * (1 + Projectile.localAI[0] / 60f)), Projectile.knockBack, Projectile.owner, Projectile.localAI[0]);
                TerraLeague.PlaySoundWithPitch(player.MountedCenter, 3, 53, -0.5f);
            }

            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 10;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D orb = null;
            TerraLeague.GetTextureIfNull(ref orb, "TerraLeague/Projectiles/ArcaneEnergy_Artillery");
            Texture2D background = null;
            TerraLeague.GetTextureIfNull(ref background, "TerraLeague/Projectiles/ArcaneEnergy_PulseControlEFX");
            Vector2 position = (Main.player[Projectile.owner].Top + new Vector2(0, -16));
            if (Projectile.localAI[0] > 60 * 4)
            {
                Main.spriteBatch.Draw
                (
                    background,
                    new Vector2
                    (
                        position.X - Main.screenPosition.X,
                        position.Y - Main.screenPosition.Y
                    ),
                    new Rectangle(0, 0, background.Width, background.Height),
                    Color.White * ((Projectile.localAI[0] - 240) / 520f),
                    MathHelper.ToRadians(Projectile.timeLeft),
                    new Vector2(background.Width/2f, background.Height/2f),
                    Projectile.localAI[0] * 3 / 540f,
                    SpriteEffects.None,
                    0f
                );

                Main.spriteBatch.Draw
                (
                    background,
                    new Vector2
                    (
                        position.X - Main.screenPosition.X,
                        position.Y - Main.screenPosition.Y
                    ),
                    new Rectangle(0, 0, background.Width, background.Height),
                    Color.White * ((Projectile.localAI[0] - 240) / 450),
                    MathHelper.ToRadians(-Projectile.timeLeft),
                    new Vector2(background.Width / 2f, background.Height / 2f),
                    Projectile.localAI[0] * 2 / 540f,
                    SpriteEffects.FlipHorizontally,
                    0f
                );
            }

            Main.spriteBatch.Draw
                (
                    orb,
                    new Vector2
                    (
                        position.X - Main.screenPosition.X,
                        position.Y - Main.screenPosition.Y/* - (orb.Height / 4)*/
                    ),
                    new Rectangle(0, (orb.Height / 4) * Projectile.frame, orb.Width, orb.Height / 4),
                    Color.White * 0.5f,
                    MathHelper.ToRadians(Projectile.timeLeft / 2f),
                    new Vector2(orb.Width / 2f, orb.Height / 8f),
                    Projectile.localAI[0] * 2 / 600f,
                    SpriteEffects.None,
                    0f
                );



            return base.PreDraw(ref lightColor);
        }

        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 8)
            {
                Projectile.frame++;
                Projectile.frame %= 4;
                Projectile.frameCounter = 0;
            }
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        }
    }
}
