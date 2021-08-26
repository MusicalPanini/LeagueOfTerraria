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
            Main.projFrames[Projectile.type] = 3;
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.alpha = 0;
            Projectile.timeLeft = 1200;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;

            targetingRange = 700;
            CanRetarget = true;
            TurningFactor = 0.95f;
        }

        public override void AI()
        {
            if(Projectile.timeLeft < 1170)
            {
                HomingAI();
            }
            
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(-90);
            Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Ice, 0f, 0f, 100, default);
            dust.noGravity = true;
            Lighting.AddLight(Projectile.position, 0f, 0f, 0.5f);

            AnimateProjectile();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Slowed>(), 300);
            Projectile.Kill();
            base.OnHitNPC(target, damage, knockback, false);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Ice, 0f, 0f, 100, default, 0.7f);
            }
        }

        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frame++;
                Projectile.frame %= 3;
                Projectile.frameCounter = 0;
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
