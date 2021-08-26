using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class MouthoftheAbyss_AcidBlob : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caustic Spittle");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.timeLeft = 185;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.scale = 0.75f;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 95), Projectile.Center);
            }
            Projectile.soundDelay = 100;

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            if (Projectile.timeLeft < 185 - 30)
            {
                Projectile.velocity.Y += 0.4f;
                Projectile.velocity.X *= 0.97f;

            }

            if (Projectile.velocity.Y > 16)
                Projectile.velocity.Y = 16;

            if (Main.rand.Next(0, 3) == 0)
            {
                Dust dustIndex = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 167, 0, 0, 50);
                dustIndex.velocity *= 0.3f;
            }

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Ichor, 120);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(3, 13), Projectile.Center);
            for (int i = 0; i < 6; i++)
            {
                Dust dustIndex = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 167, Projectile.velocity.X * 0.35f, Projectile.velocity.Y * 0.35f, 50);
            }
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }
    }
}
