using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
	public class OrbofDeception_FoxFire : ModProjectile
	{
        Vector2 offset = new Vector2(65, 0);
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            DisplayName.SetDefault("Fox-Fire");
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 28;
            Projectile.alpha = 0;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Projectile.frame = Main.rand.Next(0, 4);
                Projectile.frameCounter = Main.rand.Next(0, 4);
            }
            Projectile.soundDelay = 10;

            Player player = Main.player[Projectile.owner];
            Projectile.ai[0] += .07f;
            Projectile.Center = player.MountedCenter + offset.RotatedBy(Projectile.ai[0]);

            Lighting.AddLight(Projectile.position, 1f, 1f, 1f);

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 180, 0f, 0f, 124, default, 1f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity.Y -= 2;
            }
            //for (int i = 0; i < 3; i++)
            //{
            //    Vector2 dustBoxPosition = new Vector2(Projectile.position.X + 6, Projectile.position.Y + 6);
            //    int dustBoxWidth = Projectile.width - 12;
            //    int dustBoxHeight = Projectile.height - 12;
            //    Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 180, 0f, 0f, 124, default, 1.8f);
            //    dust.noGravity = true;
            //    dust.velocity *= 0.1f;
            //    dust.rotation += Main.rand.NextFloat(0, MathHelper.PiOver2);
            //    dust.velocity += Projectile.velocity * 0.1f;
            //    dust.position.X -= Projectile.velocity.X / 3f * (float)i;
            //    dust.position.Y -= Projectile.velocity.Y / 3f * (float)i;
            //}

            AnimateProjectile();
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, 16, 16, 180, 0, 0, 50, new Color(255, 255, 255), 1.2f);
                dust.noGravity = true;
                dust.noLight = true;
            }
        }

        public void AnimateProjectile()
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
