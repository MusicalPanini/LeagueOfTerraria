using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class ElectricRifle_UltrashockLaser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            DisplayName.SetDefault("Ultrashock Laser");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.timeLeft = 30;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            AnimateProjectile();
            Lighting.AddLight(Projectile.Left, 0.55f, 0.6f, 0.3f);

            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 30;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;

            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 264, 0, 0, 100, new Color(110, 254, 125), 0.75f);
                dust.noGravity = true;
                dust.velocity *= 2.5f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ElectricRifle_UltrashockBeam>(), Projectile.damage, Projectile.knockBack, Projectile.owner, -1, Projectile.rotation + 1.57f);

            return base.OnTileCollide(oldVelocity);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ElectricRifle_UltrashockBeam>(), Projectile.damage, Projectile.knockBack, Projectile.owner, target.whoAmI, Projectile.rotation + 1.57f);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Slowed>(), 2 * 60);
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 264, 0, 0, 100, new Color(110, 254, 125), 0.75f);
                dust.noGravity = true;
                dust.velocity *= 3;
            }

            base.Kill(timeLeft);
        }

        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 3)
            {
                Projectile.frame++;
                Projectile.frame %= 4;
                Projectile.frameCounter = 0;
            }
        }
    }
}
