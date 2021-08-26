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
    public class StrangleThornsTome_NightBloomingZychidsBulb : ModProjectile
    {
        bool grounded = false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Night Blooming Zychids Bulb");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 34;
            Projectile.timeLeft = 3600;
            Projectile.penetrate = 1;
            Projectile.friendly = false;
            Projectile.minion = true;
            Projectile.scale = 0;
        }

        public override void AI()
        {
            Projectile.rotation = 0;
            AnimateProjectile();
            if (!grounded)
            {
                grounded = true;
                Projectile.velocity.Y = 0f;
            }
            else
            {
                if (Projectile.scale < 1)
                {
                    Projectile.scale += 0.05f;
                    Projectile.position.Y -= 34 * 0.05f;
                }

                Projectile.velocity = Vector2.Zero;
            }

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = false;

            width = height = 10;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.Y <= 0)
            {
                grounded = true;
            }

            return false;
        }

        
        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 20)
            {
                Projectile.frame++;
                Projectile.frame %= 4;
                Projectile.frameCounter = 0;
            }
        }
    }
}
