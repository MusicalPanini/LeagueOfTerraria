using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class EvolutionSet_Lazer : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("Death Ray");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.timeLeft = 100;
            Projectile.extraUpdates = 100;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0 && (int)Projectile.ai[0] == 0)
            {
                Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 15));
            }
            Projectile.soundDelay = 100;

            for (int i = 0; i < 4; i++)
            {
                Vector2 pos = Projectile.position;
                pos -= Projectile.velocity * ((float)i * 0.25f);

                Dust dust = Dust.NewDustDirect(pos, 1, 1, 162, 0f, 0f, 0, default, 1f);
                dust.position = pos;
                dust.position.X += (float)(Projectile.width / 2);
                dust.position.Y += (float)(Projectile.height / 2);
                dust.scale = (float)Main.rand.Next(70, 110) * 0.013f;
                dust.velocity *= 0.2f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, 1, 1, 162, 0f, 0f, 0, default,2f);
                dust.noGravity = true;
                dust.noLight = true;
            }

            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 20f)
            {
                vector *= 8f / magnitude;
            }
        }
    }
}
