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
    public class DarksteelDagger_Dagger : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darksteel Dagger");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.timeLeft = 180;
            Projectile.penetrate = 3;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            if (Projectile.ai[0] > 0)
                Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;

            Projectile.spriteDirection = Projectile.direction;

            if (Projectile.timeLeft < 150 && (int)Projectile.ai[1] == 0)
            {
                Projectile.velocity.Y += 0.4f;
                Projectile.velocity.X *= 0.97f;
                Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.03f * (float)Projectile.direction;
            }
            else if ((int)Projectile.ai[1] > 0)
            {
                Projectile.velocity.Y += 0.4f;
                Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.03f * (float)Projectile.direction;

            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
            
            if ((int)Projectile.ai[1] == 2)
            {
                    Projectile.velocity = new Vector2(Projectile.velocity.X * 0.2f, -6);

                Projectile.ai[1] = 1;
            }

            if (Projectile.velocity.Y > 16)
                Projectile.velocity.Y = 16;

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.netUpdate = true;
                Projectile.ai[1] = 2;
                Projectile.timeLeft += 30;
            }
            else
            {
                target.immune[Projectile.owner] = 2;
            }

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

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }
    }
}
