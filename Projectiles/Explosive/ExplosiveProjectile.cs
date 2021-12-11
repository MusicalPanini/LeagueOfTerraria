using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles.Explosive
{
    public abstract class ExplosiveProjectile : ModProjectile
    {
        protected bool explosionPrimed { get { return Projectile.ai[1] == 1; }}
        public int ExplosionWidth = 32;
        public int ExplosionHeight = 32;
        public bool PrimeOnHit = true;
        public bool PrimeOnTileCollide = true;

        public override bool PreAI()
        {
            if (!explosionPrimed && Projectile.timeLeft <= 1)
            {
                Prime();
            }
            return base.PreAI();
        }

        public virtual void PrePrime()
        {

        }

        public virtual void Prime()
        {
            if (!explosionPrimed)
            {
                PrePrime();

                Projectile.velocity = Vector2.Zero;
                Projectile.tileCollide = false;
                Projectile.alpha = 255;
                Projectile.timeLeft = 2;

                Vector2 originalCenter = Projectile.Center;

                Projectile.width = ExplosionWidth;
                Projectile.height = ExplosionHeight;

                Projectile.Center = originalCenter;
                //Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
                //Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);

                Projectile.ai[1] = 1;
                //Projectile.netUpdate = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            if (!explosionPrimed)
                Prime();
            KillEffects();
            base.Kill(timeLeft);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.penetrate <= 1)
            {
                Prime();
            }

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();
            return base.OnTileCollide(oldVelocity);
        }

        public virtual void KillEffects()
        {

        }
    }
}
