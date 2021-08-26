using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class Hexplosives_BouncingBomb : ModProjectile
    {
        int bounces = 3;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bouncing Bomb");
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 100;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            Projectile.rotation += Projectile.velocity.X * 0.05f;
            Lighting.AddLight(Projectile.position, 0.5f, 0.45f, 0.30f);
            Projectile.velocity.Y += 0.4f;

            Vector2 dustPos = Projectile.position.RotatedBy(MathHelper.Pi + Projectile.rotation, Projectile.Center);

            Dust dust = Dust.NewDustPerfect(dustPos, DustID.Smoke);
            dust.noGravity = true;

            dust = Dust.NewDustPerfect(dustPos, 6);
            dust.noGravity = true;
            dust.velocity *= 0;

            base.AI();
        }
        
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Prime();
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Rebound();

            if (bounces <= 0)
                Prime();
            else
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, (int)Projectile.position.X, (int)Projectile.position.Y);

            return false;
        }

        public void Rebound()
        {
            if (Projectile.velocity.X != Projectile.oldVelocity.X)
            {
                Projectile.velocity.X = -Projectile.oldVelocity.X;
            }
            else if (Projectile.velocity.Y != Projectile.oldVelocity.Y)
            {
                if (Projectile.oldVelocity.Y > 0)
                {
                    //if (Projectile.oldVelocity.Y < 6)
                        Projectile.velocity.Y = -6;

                    if (Projectile.oldVelocity.X > 6)
                        Projectile.velocity.X = 6;
                    if (Projectile.oldVelocity.X < -6)
                        Projectile.velocity.X = -6;
                    //else if (Projectile.oldVelocity.Y > 10)
                    //    Projectile.velocity.Y = -10;
                    //else
                    //    Projectile.velocity.Y *= -1f;

                    bounces--;
                }
                else
                {
                    Projectile.velocity.Y = 6;
                }

                
            }
            if (bounces == 0)
            {
                Prime();
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = Projectile.Center.X > target.Center.X ? -1 : 1;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.penetrate == 1)
            {
                Prime();
            }
            else
            {
                Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 14), Projectile.position);

                Dust dust;
                for (int i = 0; i < 20; i++)
                {
                    dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f);
                    dust.velocity *= 0.5f;

                }
                for (int i = 0; i < 50; i++)
                {
                    dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 3f);
                    dust.noGravity = true;
                    dust.velocity *= 3f;
                    dust.color = new Color(255, 0, 220);

                    dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 2f);
                    dust.color = new Color(255, 0, 220);
                    dust.noGravity = true;

                }
            }

        }

        public void Prime()
        {
            Projectile.tileCollide = false;
            Projectile.velocity = Vector2.Zero;
            Projectile.alpha = 255;
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 128;
            Projectile.height = 128;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.timeLeft = 2;
        }
    }
}
