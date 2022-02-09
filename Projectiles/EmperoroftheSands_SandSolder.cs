using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using TerraLeague.Projectiles.Minions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class EmperoroftheSands_SandSolder : GroundMinion
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sand Solder");
            //Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            //ProjectileID.Sets.CountsAsHoming[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 6;

            DrawOriginOffsetY -= 12;
            DrawOffsetX -= 12;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.netImportant = true;
            Projectile.alpha = 0;
            Projectile.timeLeft = 100;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;

            // Animation
            attackFrame = 1;
            attackFrameCount = 4;

            runFrame = 1;
            runFrameCount = 4;
            runFrameSpeedMod = 0.1f;

            flyFrame = 5;
            flyFrameCount = 1;
            flyFrameSpeed = 1;
            flyRotationMod = 0.5f;

            fallFrame = 5;
            fallFrameCount = 1;

            idleFrame = 0;
            idleFrameCount = 1;

            AIPrioritiseNearPlayer = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 100)
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y + 16), Projectile.width, Projectile.height, DustID.Sand, 0f, 0, 0, default, 3f);
                    dust.noGravity = true;
                    dust.velocity.Y -= 2;

                    dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y + 16), Projectile.width, Projectile.height, DustID.Sand, 0f, 0, 0, default, 2f);
                    dust.noGravity = true;
                    dust.velocity.Y -= 3;
                }
            }

            if (Main.player[Projectile.owner].HasBuff(ModContent.BuffType<SandSolder>()))
                Projectile.timeLeft = 10;

            if (Main.rand.Next(0, 4) == 0)
            {
                Dust dust = Dust.NewDustDirect(Projectile.BottomLeft, Projectile.width - 4, 2, DustID.Sand);
                dust.position.Y -= 2;
                dust.velocity.Y *= 0.2f;
                dust.fadeIn = 0.5f;
                dust.noGravity = true;
            }

            if (Main.rand.Next(0, 30) == 0)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Sand);
                dust.fadeIn = 0.5f;
            }

            base.AI();
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return (int)Projectile.ai[0] == 2;
        }
    }
}
