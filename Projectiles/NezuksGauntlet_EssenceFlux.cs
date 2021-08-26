using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Projectiles
{
    public class NezuksGauntlet_EssenceFlux : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Essence Flux");
        }

        public override void SetDefaults()
        {
            Projectile.width = 21;
            Projectile.height = 21;
            Projectile.alpha = 30;
            Projectile.timeLeft = 60;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
        }

        public override void AI()
        {
            Projectile.damage = 1;

            if (Projectile.velocity.X < 0)
            {
                Projectile.spriteDirection = -1;
            }
            Projectile.rotation += (float)Projectile.direction * 0.6f;

            Lighting.AddLight(Projectile.position, 0.75f, 0.75f, 0);
            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 159, Projectile.velocity.X, Projectile.velocity.Y, 50, default, 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<EssenceFluxDebuff>(), 240);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            crit = false;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 159, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 50, default, 2f);
                dust.noGravity = true;
                dust.velocity *= 0.6f;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}

