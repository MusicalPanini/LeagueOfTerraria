using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class PowPow_Zap : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CountsAsHoming[Projectile.type] = true;
            DisplayName.SetDefault("ZAP!");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.scale = 1;
            Projectile.timeLeft = 900;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 32;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 11), Projectile.position);
            }

            Projectile.soundDelay = 2;
            Lighting.AddLight(Projectile.Left, 0.09f, 0.40f, 0.60f);

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }

            for (int i = 0; i < 4; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight, 0, 0, 0, i < 3 ? new Color(0, 255, 255, 150) : new Color(255, 0, 226), 1f);
                dust.velocity *= Main.rand.Next(6) == 0 ? 2 : 0.3f;
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Slowed>(), 240);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(3, 53), Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 0, i < 5 ? new Color(0, 255, 255, 150) : new Color(255, 0, 226), 1f);
                dust.velocity *= 3;
                dust.noGravity = true;
            }
            base.Kill(timeLeft);
        }
    }
}
