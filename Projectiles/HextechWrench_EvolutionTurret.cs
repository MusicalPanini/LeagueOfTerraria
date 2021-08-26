using System;
using TerraLeague.Buffs;
using TerraLeague.Projectiles.Minions;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class HextechWrench_EvolutionTurret : SentryMinion
    {
        int frameCount = 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 48;
            Projectile.height = 47;
            Projectile.friendly = false;
            Projectile.sentry = true;
            Projectile.DamageType = Terraria.ModLoader.DamageClass.Summon;
            Projectile.penetrate = 1;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            shoot = ProjectileType<HextechWrench_EvoTurretShot>();
            shootSpeed = 20f;
            shootCool = 75;
            shootCountdown = 40;
            shootSound = new Terraria.Audio.LegacySoundStyle(2, 12, Terraria.Audio.SoundType.Sound).WithPitchVariance(1);
        }

        public override void CheckActive()
        {
            

            
        }

        public override void Behavior()
        {
            Player player = Main.player[Projectile.owner];
            //if (player.HasBuff(BuffType<EvolutionTurrets>()))
            //{
            //    Projectile.timeLeft = 2;
            //}
            base.Behavior();
        }

        public override void CreateDust()
        {

        }

        public void CheckIfNearOwner()
        {
            Player checkTarget = Main.player[Projectile.owner];
            float distance = Vector2.Distance(checkTarget.Center, Projectile.Center);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = false;
            return true;
        }

        public override void SelectFrame()
        {
            int currentAnim;

            if (frameCount > 20)
                currentAnim = 1;
            else
                currentAnim = 0;

            if (frameCount >= 40)
                frameCount = 0;
            else
                frameCount++;

            if (target != null)
            {
                double xdif = Projectile.Center.X - (target.position.X + (target.width / 2.0));
                double ydif = Projectile.Center.Y - (target.position.Y + (target.height / 2.0));

                if (xdif > 0)
                {
                    Projectile.spriteDirection = -1;
                }
                else
                {
                    Projectile.spriteDirection = 1;
                }

                if (ydif > 0)
                {
                    if (Math.Abs(ydif) * Math.Sqrt(3) < Math.Abs(xdif) * (1 / Math.Sqrt(3)))
                    {
                        Projectile.frame = 0;
                    }
                    else
                    {
                        Projectile.frame = 4;
                    }
                }
                else
                {
                    if (Math.Abs(ydif) * Math.Sqrt(3) < Math.Abs(xdif)* (1/Math.Sqrt(3)))
                    {
                        Projectile.frame = 0;
                    }
                    else
                    {
                        Projectile.frame = 2;
                    }
                }
            }
            else
            {
                Projectile.frame = 0;
            }

            Projectile.frame += currentAnim;
        }
    }
}
