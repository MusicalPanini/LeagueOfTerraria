using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using TerraLeague.Projectiles.Homing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class Item_SpookyGhost : HomingProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spooky Ghost");
            Main.projFrames[projectile.type] = 3;
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.alpha = 0;
            projectile.timeLeft = 1200;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.minion = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;

            targetingRange = 700;
            CanRetarget = true;
            TurningFactor = 0.95f;
        }

        public override void AI()
        {
            if(projectile.timeLeft < 1170)
            {
                HomingAI();
            }
            
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(-90);
            Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.IceRod, 0f, 0f, 100, default);
            dust.noGravity = true;
            Lighting.AddLight(projectile.position, 0f, 0f, 0.5f);

            AnimateProjectile();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Slowed>(), 300);
            projectile.Kill();
            base.OnHitNPC(target, damage, knockback, false);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Ice, 0f, 0f, 100, default, 0.7f);
            }
        }

        public void AnimateProjectile()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5)
            {
                projectile.frame++;
                projectile.frame %= 3;
                projectile.frameCounter = 0;
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
