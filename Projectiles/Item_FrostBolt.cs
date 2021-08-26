using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Item_FrostBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.timeLeft = 60;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Dust dust;

            dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1.5f);
            dust.noGravity = true;
            dust.noLight = true;
            dust.velocity *= 0.1f;

            for (int i = 0; i < 2; i++)
            {
                Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Vortex, Projectile.velocity.X, Projectile.velocity.Y, 124, default, 1.25f);
                dust2.noGravity = true;
                dust2.noLight = true;
                dust2.velocity *= 0.6f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Slowed>(), 5 * 60);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 8, 1f);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            var sound = TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 10, 0);
            if (sound != null)
            {
                sound.Volume *= 0.5f;
            }
            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }
    }
}
