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
    public class AssassinsKunai_Kunai : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Assassin's Kunai");
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.timeLeft = 185;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.scale = 0.75f;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            if (Projectile.timeLeft < 180)
            {
                Projectile.velocity.Y += 0.4f;
                Projectile.velocity.X *= 0.97f;
            }

            if (Projectile.velocity.Y > 16)
                Projectile.velocity.Y = 16;

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            for (int i = 0; i < 6; i++)
            {
                Dust dustIndex = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Iron, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f);
            }
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 10;
            return true;
        }
    }
}
