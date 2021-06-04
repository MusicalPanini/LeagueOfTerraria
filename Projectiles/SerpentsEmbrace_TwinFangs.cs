using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class SerpentsEmbrace_TwinFangs : ModProjectile
    {
        

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
            ProjectileID.Sets.Homing[projectile.type] = true;
            DisplayName.SetDefault("Twin Fangs");
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.alpha = 0;
            projectile.timeLeft = 90;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.magic = true;
            projectile.scale = 1.5f;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
            //AnimateProjectile();

            if (projectile.soundDelay == 0)
                TerraLeague.PlaySoundWithPitch(projectile.Center, 3, 23, -0.5f);
            projectile.soundDelay = 100;

            if (!Main.npc[(int)projectile.ai[0]].active)
            {
                projectile.Kill();
            }
            else
            {
                NPC npc = Main.npc[(int)projectile.ai[0]];

                projectile.velocity = TerraLeague.CalcVelocityToPoint(projectile.Center, npc.Center, projectile.velocity.Length());
                Lighting.AddLight(projectile.position, 0, 1, 0);
            }
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 20f)
            {
                vector *= 8f / magnitude;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Venom, projectile.velocity.X, projectile.velocity.Y, 50, default(Color), 1.2f);
                dust.noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.whoAmI == (int)projectile.ai[0] && !target.immortal && !target.townNPC)
                return true;
            else
                return false;
        }

        public void AnimateProjectile()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5)
            {
                projectile.frame++;
                projectile.frame %= 4;
                projectile.frameCounter = 0;
            }
        }
    }
}
