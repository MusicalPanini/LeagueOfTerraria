using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class DarksteelThrowingAxe_ThrowingAxe : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darksteel Throwing Axe");
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            if (Projectile.velocity.X < 0)
                Projectile.spriteDirection = -1;

            if (Projectile.velocity.X > 0)
                Projectile.rotation += 0.5f;
            else
                Projectile.rotation += -0.5f;

            if (Projectile.timeLeft < 270 && Projectile.velocity.Y < 15)
                Projectile.velocity.Y += 0.8f;
            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 6; i++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Iron, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f);
            }
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 10;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Iron, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f);
            }
            TerraLeague.PlaySoundWithPitch(Projectile.Center, 3, 4, -0.5f);
            return true;
        }
    }
}
