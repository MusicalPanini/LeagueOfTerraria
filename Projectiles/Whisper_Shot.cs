using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Whisper_Shot : ModProjectile
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
            Projectile.extraUpdates = 24;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 36, 0);
                //Terraria.Audio.SoundEngine.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Custom/WhisperShot"), Projectile.position);
            Projectile.soundDelay = 100;

            Lighting.AddLight(Projectile.Left, 1f, 0.5f, 0.01f);

            if (Projectile.timeLeft < 354)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, 75, Vector2.Zero, 0, new Color(255, 0, 0));
                dust.noGravity = true;
                dust.velocity *= 0;
                dust = Dust.NewDustPerfect(Projectile.Center - Projectile.velocity.SafeNormalize(Vector2.Zero), 75, Vector2.Zero, 0, new Color(255, 0, 0));
                dust.noGravity = true;
                dust.velocity *= 0;
            }

            //if (Projectile.alpha > 0)
            //    Projectile.alpha -= 15;
            //if (Projectile.alpha < 0)
            //    Projectile.alpha = 0;

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
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Gold, Projectile.velocity.X / 5, Projectile.velocity.Y / 5, 100, default, 0.7f);
            }
        }
    }
}
