using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class DarkinBow_ArrowControl : ModProjectile
    {
        int MaxCharge 
        { 
            get 
            {
                double ats = Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed - 1;
                return (int)(90 * ats);
            }
        }

        int UseTime
        {
            get
            {
                double ats = Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed - 1;
                return (int)(18 * ats);
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darkin Bow Arrow Control");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.timeLeft = 300;
            projectile.friendly = false;
            projectile.ranged = true;
            projectile.alpha = 255;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().channelProjectile = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            if (player.channel)
            {
                projectile.Center = player.MountedCenter;

                

                if (projectile.owner == Main.LocalPlayer.whoAmI)
                {
                    int dir = player.Center.X > Main.MouseWorld.X ? -1 : 1;
                    player.ChangeDir(dir);
                    projectile.ai[1] = (float)TerraLeague.CalcAngle(player.Center, Main.MouseWorld) - player.fullRotation;
                    projectile.netUpdate = true;
                }
                player.itemRotation = projectile.ai[1];

                for (int k = 0; k < 2 + 1; k++)
                {
                    float scale = projectile.localAI[0] / (2 * MaxCharge/3f);
                    if (k % 2 == 1)
                        scale = projectile.localAI[0] / (3 *MaxCharge/4f);

                    Vector2 postion = projectile.Center + ((float)Main.rand.NextDouble() * 6.28318548f).ToRotationVector2() * (12f - (float)(2 * 2));
                    Dust dust = Dust.NewDustDirect(postion - Vector2.One * 8f, 16, 16, DustID.Blood, 0, 0, 0, new Color(255, 0, 0), scale);
                    dust.velocity = Vector2.Normalize(projectile.Center - postion) * 1.5f * (10f - (float)2 * 2f) / 10f;
                    dust.noGravity = true;
                    dust.customData = player;
                }
                if (projectile.localAI[0] < MaxCharge)
                    projectile.localAI[0]++;

                if (projectile.localAI[0] >= MaxCharge)
                {
                    if (projectile.soundDelay == 0)
                        TerraLeague.PlaySoundWithPitch(projectile.Center, 25, 1, 0);
                    projectile.soundDelay = 2;

                }

                player.itemTime = 5;
                player.itemAnimation = 5;
            }
            else
            {
                player.itemTime = 1 + (int)(UseTime * (1 - (projectile.localAI[0] / MaxCharge)));
                player.itemAnimation = 1 + (int)(UseTime * (1 - (projectile.localAI[0] / MaxCharge)));
                projectile.Kill();
            }

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];

            float rot = projectile.ai[1] + (player.direction == -1 ? MathHelper.Pi : 0) + player.fullRotation;

            Projectile.NewProjectileDirect(projectile.Center, new Vector2(6 + (10 * projectile.localAI[0] / MaxCharge), 0).RotatedBy(rot), (int)projectile.ai[0], projectile.damage * (int)(1 + projectile.localAI[0] / (MaxCharge/3f)), projectile.knockBack, projectile.owner, projectile.localAI[0] / (MaxCharge * 2f));

            TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 5, 0 - (projectile.localAI[0] / 90f));

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
