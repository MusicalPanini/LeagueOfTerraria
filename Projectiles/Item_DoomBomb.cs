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
            projectile.width = 16;
            projectile.height = 16;
            projectile.timeLeft = 300;
            projectile.penetrate = 100;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.magic = true;
            projectile.alpha = 255;

            CanOnlyHitTarget = true;
            CanRetarget = false;
            MaxVelocity = 0;
            TurningFactor = 0.93f;
        }


        public override void AI()
        {
            if (projectile.soundDelay == 0)
            {
                TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 45, -0.5f);
            }
            projectile.soundDelay = 100;

            MaxVelocity = 16 * (1 - (projectile.timeLeft / 300f));
            MaxVelocity *= 8;
            if (MaxVelocity > 16)
                MaxVelocity = 16;

            HomingAI();
            

            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Clentaminator_Green, 0, 0, 0, new Color(0, 255, 201), 2.5f);
                dust.noGravity = true;
                dust.noLight = true;

                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Clentaminator_Green, 0, -3, 0, new Color(0, 255, 201), 1f);
                dust.noLight = true;
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if ((int)projectile.ai[0] != -2)
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
            TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 45, -0.5f);
            Main.PlaySound(SoundID.DD2_ExplosiveTrapExplode.WithVolume(1f), projectile.position);

            Dust dust;
            for (int i = 0; i < 50; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Clentaminator_Green, 0f, 0f, 100, new Color(0, 255, 201), 2f);
                dust.velocity *= 1.4f;
                dust.noGravity = true;
            }
            for (int i = 0; i < 80; i++)
            {
                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Clentaminator_Green, 0, 0, 0, new Color(0, 255, 201), 2f);
                dust.noGravity = true;
                dust.velocity *= 5f;
                dust.color = new Color(255, 0, 220);
                dust.noGravity = true;

                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Clentaminator_Green, 0f, 0f, 100, new Color(0, 255, 201), 2f);
                dust.velocity *= 3f;
                dust.color = new Color(255, 0, 220);
                dust.noGravity = true;
            }
            base.Kill(timeLeft);
        }

        public void Prime()
        {
            CanOnlyHitTarget = false;
            projectile.tileCollide = false;
            projectile.velocity = Vector2.Zero;
            projectile.alpha = 255;
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 200;
            projectile.height = 200;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.timeLeft = 2;
        }
    }
}
