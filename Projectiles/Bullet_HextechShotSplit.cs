using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Bullet_HextechShotSplit : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CountsAsHoming[Projectile.type] = true;
            DisplayName.SetDefault("Hextech Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 3;
            Projectile.alpha = 255;
            Projectile.scale = 1.2f;
            Projectile.timeLeft = 900;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 2;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Left, 0.09f, 0.40f, 0.60f);

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }

                for (int i = 0; i < 10; i++)
                {
                    float x2 = Projectile.position.X - Projectile.velocity.X / 10f * (float)i;
                    float y2 = Projectile.position.Y - Projectile.velocity.Y / 10f * (float)i;
                    int num141 = Dust.NewDust(new Vector2(x2, y2), 1, 1, 111, 0f, 0f, 0, default(Color), 0.5f);
                    Main.dust[num141].alpha = Projectile.alpha;
                    Main.dust[num141].position.X = x2;
                    Main.dust[num141].position.Y = y2;
                    Dust obj77 = Main.dust[num141];
                    obj77.velocity *= 0f;
                    Main.dust[num141].noGravity = true;
                }

            //Dust dust = Dust.NewDustPerfect(Projectile.position, 111, Vector2.Zero, 0, default, 0.5f);
            //dust.noGravity = true;
            //dust.alpha = 100;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[0] != target.whoAmI && !target.friendly)
                return true;
            else
                return false;
        }
    }
}
