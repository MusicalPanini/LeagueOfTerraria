using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class StarfireSpellblades_Firewave : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starfire");
        }

        public override void SetDefaults()
        {
            Projectile.width = 15;
            Projectile.height = 15;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 3;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.timeLeft = 60;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Dust dust;
            if (Projectile.soundDelay == 0 && (int)Projectile.ai[0] == 1)
            {
                Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 15), Projectile.Center);
                Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 34), Projectile.Center);
            }
            Projectile.soundDelay = 100;

            dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GemTopaz, 0, 0, 0, default, 2f);
            dust.noGravity = true;
            dust.velocity = Projectile.velocity * 2;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Daybreak, 60);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 8, 1f);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }
    }
}
