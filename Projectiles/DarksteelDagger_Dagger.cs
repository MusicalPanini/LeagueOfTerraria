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
            Projectile.penetrate = 2;
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

        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[0] == 0 && Projectile.penetrate > 1 || Projectile.ai[0] != 0)
            {
                return base.CanHitNPC(target);
            }
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[0] == 0 && Projectile.penetrate <= 1)
            {
                int dir = 1;

                if (Projectile.velocity.X != Projectile.oldVelocity.X)
                {
                    if (Projectile.oldVelocity.X > 0)
                        dir = 0;
                    else
                        dir = 2;
                }
                else if (Projectile.velocity.Y != Projectile.oldVelocity.Y)
                {
                    if (Projectile.oldVelocity.Y > 0)
                        dir = 1;
                    else
                        dir = 3;
                }

                Point point = (Projectile.Center + Projectile.velocity).ToTileCoordinates();
                Vector2 offset = Projectile.Center;

                switch (dir)
                {
                    case 0:
                        offset.X = point.X * 16 + 10;
                        break;
                    case 1:
                        offset.Y = point.Y * 16 + 10;
                        break;
                    case 2:
                        offset.X = point.X * 16 + 6;
                        break;
                    default:
                        offset.Y = point.Y * 16 + 6;
                        break;
                }

                Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), offset, new Vector2(0, 0), ModContent.ProjectileType<DarksteelDagger_DroppedDagger>(), Projectile.damage, 0, Projectile.owner, dir);
            }

            return base.OnTileCollide(oldVelocity);
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

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 10;

            fallThrough = !(Projectile.ai[0] == 0 && Projectile.penetrate <= 1);

            return true;
        }
    }
}
