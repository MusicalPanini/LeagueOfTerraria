using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class EyeofGod_TentacleAttack : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tentacle");
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 108;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 100;
            Projectile.scale = 1f;
            Projectile.timeLeft = 6000;
            Projectile.sentry = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = true;
            Projectile.friendly = false;
            DrawOffsetX = -24;
        }

        public override void AI()
        {
            if (Projectile.ai[1] >= 0)
            {
                Projectile.timeLeft = (int)Projectile.ai[1];
                Projectile.ai[1] = -1;
            }

            if ((int)Projectile.ai[0] == 1)
                DrawOffsetX = -24;
            else
                DrawOffsetX = -116;
            Projectile.spriteDirection = (int)Projectile.ai[0];
            Lighting.AddLight(Projectile.Center, 0, 0.1f, 0.05f);

            Dust dust2 = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.Bottom.Y - 4), Projectile.width, 4, DustID.BubbleBlock, 0f, 0, 100, new Color(0, 255, 201), 1f);
            dust2.fadeIn = 1.3f;
            dust2.noGravity = true;
            dust2.velocity.Y *= 0.2f;

            AnimateProjectile();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            int frameDelay;

            switch (Projectile.frame)
            {
                // Preparing
                case 0:
                case 1:
                case 2:
                    frameDelay = 4;
                    break;
                // Swinging
                case 3:
                case 4:
                    frameDelay = 2;
                    break;
                // The Slam
                case 5:
                    frameDelay = 13;
                    break;
                // Reseting
                case 6:
                case 7:
                    frameDelay = 6;
                    break;
                default:
                    frameDelay = 0;
                    break;
            }

            if (Projectile.frameCounter >= frameDelay)
            {
                Projectile.frame++;
                if (Projectile.frame == 8)
                {
                    Projectile.ai[1] = Projectile.timeLeft;
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<EyeofGod_Tentacle>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 60 * Main.player[Projectile.owner].meleeSpeed, Projectile.ai[1]);
                    proj.originalDamage = Projectile.originalDamage;
                    Projectile.Kill();
                }
                Projectile.frame %= 8;
                Projectile.frameCounter = 0;

                if (Projectile.frame == 5)
                {
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), new Vector2(Projectile.Center.X + (72 * (int)Projectile.ai[0]), Projectile.Center.Y), Vector2.Zero, ModContent.ProjectileType<EyeofGod_TentacleHitbox>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    proj.originalDamage = Projectile.originalDamage;
                    TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 14, -0.25f);
                }
            }
        }
    }
}
