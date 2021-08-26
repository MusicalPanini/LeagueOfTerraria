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
                double rngatkspd = Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed;

                double multi = 1 / (rngatkspd);

                return  (int)(90 * multi);
            }
        }

        int UseTime
        {
            get
            {
                double rngatkspd = Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed;
                double multi = 1 / (rngatkspd);
                return (int)(18 * multi);
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darkin Bow Arrow Control");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 300;
            Projectile.friendly = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.alpha = 255;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().channelProjectile = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (player.channel)
            {
                Projectile.Center = player.MountedCenter;

                

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
                    float scale = Projectile.localAI[0]  / (2 * MaxCharge/3f);
                    if (k % 2 == 1)
                        scale = Projectile.localAI[0]  / (3 *MaxCharge/4f);

                    Vector2 postion = Projectile.Center + ((float)Main.rand.NextDouble() * 6.28318548f).ToRotationVector2() * (12f - (float)(2 * 2));
                    Dust dust = Dust.NewDustDirect(postion - Vector2.One * 8f, 16, 16, DustID.Blood, 0, 0, 0, new Color(255, 0, 0), scale);
                    dust.velocity = Vector2.Normalize(Projectile.Center - postion) * 1.5f * (10f - (float)2 * 2f) / 10f;
                    dust.noGravity = true;
                    dust.customData = player;
                }
                if (Projectile.localAI[0]  < MaxCharge)
                    Projectile.localAI[0] ++;

                if (Projectile.localAI[0]  >= MaxCharge)
                {
                    if (Projectile.soundDelay == 0)
                        TerraLeague.PlaySoundWithPitch(Projectile.Center, 25, 1, 0);
                    Projectile.soundDelay = 2;

                }

                player.itemTime = 5;
                player.itemAnimation = 5;
            }
            else
            {
                player.itemTime = 1 + (int)(UseTime * (1 - (Projectile.localAI[0]  / MaxCharge)));
                player.itemAnimation = 1 + (int)(UseTime * (1 - (Projectile.localAI[0]  / MaxCharge)));
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

            Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, new Vector2(6 + (10 * Projectile.localAI[0]  / MaxCharge), 0).RotatedBy(rot), (int)Projectile.ai[0], Projectile.damage * (int)(1 + Projectile.localAI[0]  / (MaxCharge/3f)), Projectile.knockBack, Projectile.owner, Projectile.localAI[0]  / (MaxCharge * 2f));

            TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 5, 0 - (Projectile.localAI[0]  / 90f));

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
