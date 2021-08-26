using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using TerraLeague.Projectiles.Homing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class Item_DoomBomb : HomingProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Doom");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 100;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.alpha = 255;

            CanOnlyHitTarget = true;
            CanRetarget = false;
            MaxVelocity = 0;
            TurningFactor = 0.93f;
        }


        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 45, -0.5f);
            }
            Projectile.soundDelay = 100;

            MaxVelocity = 16 * (1 - (Projectile.timeLeft / 300f));
            MaxVelocity *= 8;
            if (MaxVelocity > 16)
                MaxVelocity = 16;

            HomingAI();
            

            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 110, 0, 0, 0, new Color(0, 255, 201), 2.5f);
                dust.noGravity = true;
                dust.noLight = true;

                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 110, 0, -3, 0, new Color(0, 255, 201), 1f);
                dust.noLight = true;
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if ((int)Projectile.ai[0] != -2)
                Prime();
            target.AddBuff(BuffType<Buffs.Doom>(), 240);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            crit = false;
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }


        public override void Kill(int timeLeft)
        {
            TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 45, -0.5f);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode.WithVolume(1f), Projectile.position);

            Dust dust;
            for (int i = 0; i < 50; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 110, 0f, 0f, 100, new Color(0, 255, 201), 2f);
                dust.velocity *= 1.4f;
                dust.noGravity = true;
            }
            for (int i = 0; i < 80; i++)
            {
                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 110, 0, 0, 0, new Color(0, 255, 201), 2f);
                dust.noGravity = true;
                dust.velocity *= 5f;
                dust.color = new Color(255, 0, 220);
                dust.noGravity = true;

                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 110, 0f, 0f, 100, new Color(0, 255, 201), 2f);
                dust.velocity *= 3f;
                dust.color = new Color(255, 0, 220);
                dust.noGravity = true;
            }
            base.Kill(timeLeft);
        }

        public void Prime()
        {
            CanOnlyHitTarget = false;
            Projectile.tileCollide = false;
            Projectile.velocity = Vector2.Zero;
            Projectile.alpha = 255;
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 200;
            Projectile.height = 200;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.timeLeft = 2;
        }
    }
}
