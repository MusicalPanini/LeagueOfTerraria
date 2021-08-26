using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Whisper_ForthShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Whisper");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.alpha = 0;
            Projectile.timeLeft = 360;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.extraUpdates = 48;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                TerraLeague.PlaySoundWithPitch(Projectile.position, 2, 36, -0.5f);
                //SoundEffectInstance sound = Terraria.Audio.SoundEngine.PlaySound(ModContent.GetContent<LegacySoundStyle>("Sounds/Custom/WhisperShot"), Projectile.position);
                //if (sound != null)
                //    sound.Pitch = -0.5f;
            }
            Projectile.soundDelay = 100;

            if (Projectile.timeLeft < 354)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, 182, Vector2.Zero);
                dust.noGravity = true;
                dust.velocity *= 0;
                dust = Dust.NewDustPerfect(Projectile.Center - Projectile.velocity.SafeNormalize(Vector2.Zero), 182, Vector2.Zero);
                dust.noGravity = true;
                dust.velocity *= 0;
            }

            Lighting.AddLight(Projectile.position, 1f, 0.0f, 0.0f);
            if (Projectile.alpha > 0)
                Projectile.alpha -= 15;
            if (Projectile.alpha < 0)
                Projectile.alpha = 0;

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            if (Projectile.velocity.Y > 16f)
                Projectile.velocity.Y = 16f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 225, Projectile.velocity.X / 5, Projectile.velocity.Y / 5, 0, default, 0.7f);
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            crit = true;
        }
    }
}
