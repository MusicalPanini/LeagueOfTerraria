using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Item_ZzrotPortal : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zz'Rot Portal");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.alpha = 0;
            Projectile.timeLeft = 60 * 5 + 5;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.60f, 0f, 0.60f);

            if (Projectile.soundDelay == 0)
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y + 16), Projectile.width, Projectile.height, DustID.Shadowflame, 0f, 0, 0, default, 3f);
                    dust.noGravity = true;
                    dust.velocity.Y -= 2;

                    dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y + 16), Projectile.width, Projectile.height, DustID.Shadowflame, 0f, 0, 0, default, 2f);
                    dust.noGravity = true;
                    dust.velocity.Y -= 3;
                }
            }
            Projectile.soundDelay = 100;

            if (Projectile.ai[1] >= 60)
            {
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 103, -0.25f);

                for (int i = 0; i < (int)Projectile.ai[0] + Main.player[Projectile.owner].maxMinions; i++)
                {
                    Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, new Vector2(Main.rand.NextFloat(-4, 4), -6), ModContent.ProjectileType<VoidProphetsStaff_Zzrot>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }

                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, 0, -3);
                }

                Projectile.ai[1] = 0;
            }
            else
            {
                Projectile.ai[1]++;
            }

            if (Main.rand.Next(0, 30) == 0)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, 0, 0, 0, default, 1.5f);
                dust.fadeIn = 1;
                dust.velocity *= 0.1f;
            }

            Animation();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y + 16), Projectile.width, Projectile.height, DustID.Shadowflame, 0f, 0, 0, default, 3f);
                dust.noGravity = true;
                dust.velocity.Y -= 2;

                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y + 16), Projectile.width, Projectile.height, DustID.Shadowflame, 0f, 0, 0, default, 2f);
                dust.noGravity = true;
                dust.velocity.Y -= 3;
            }
            base.Kill(timeLeft);
        }

        void Animation()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6)
            {
                Projectile.frame++;
                Projectile.frame %= 4;
                Projectile.frameCounter = 0;
            }
        }
    }
}
