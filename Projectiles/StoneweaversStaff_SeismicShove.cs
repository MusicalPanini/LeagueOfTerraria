using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class StoneweaversStaff_SeismicShove : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Seismic Shove");
        }

        public override void SetDefaults()
        {
            Projectile.width = 48*3;
            Projectile.height = 48*3;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.timeLeft = 90;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.rotation = MathHelper.PiOver4;
        }

        public override void AI()
        {

            if (Projectile.timeLeft > 31)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, 16, DustID.t_Slime, 0, -1, 100, new Color(255, 125, 0));

                if (Projectile.soundDelay == 0 && Projectile.type != 383)
                {
                    Projectile.soundDelay = 20;
                    Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 14), Projectile.position);
                }
                Projectile.velocity = Vector2.Zero;

            }
            else if (Projectile.timeLeft == 31)
            {
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 14, -0.5f);
            }
            else if (Projectile.timeLeft <= 31 && Projectile.timeLeft >= 29)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.t_Slime, Projectile.oldVelocity.X * 0.25f, Projectile.oldVelocity.Y * 0.25f, 0, new Color(255, 125, 0));
                }

                Projectile.alpha = 0;

                Projectile.friendly = true;

                Projectile.velocity = new Vector2(Main.player[Projectile.owner].position.X < Projectile.position.X ? 20 : -20, -20);
            }
            else if (Projectile.timeLeft <= 29)
            {


                Projectile.friendly = false;

                Projectile.velocity = Vector2.Zero;
            }
            else
            {
                Projectile.velocity = Vector2.Zero;

            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.velocity = Projectile.velocity/2;

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 10;

            return true;
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 27), Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                Dust dustIndex = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, Projectile.velocity.X / 1.5f, Projectile.velocity.Y / 1.5f, 100, default, 1.5f);
                dustIndex.noGravity = true;
            }
            base.Kill(timeLeft);
        }
    }
}
