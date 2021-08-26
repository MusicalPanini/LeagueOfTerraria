using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class ColossusFist_WindsofWar : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Winds of War");
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 162;
            Projectile.height = 42;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.timeLeft = 180;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (Projectile.timeLeft > 100)
            {
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 10;
                }
                if (Projectile.alpha < 100)
                {
                    Projectile.alpha = 100;
                }
            }
            else if (Projectile.timeLeft < 155/10f)
            {
                Projectile.alpha += 10;

                if (Projectile.alpha > 255)
                {
                    Projectile.alpha = 255;
                }
            }
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0, -3, Projectile.alpha/2, default, 0.5f);
            if (Projectile.owner == Main.LocalPlayer.whoAmI && (int)Projectile.ai[0] > 0 && Projectile.timeLeft == 175)
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center + new Vector2(0, -42), Vector2.Zero, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner, (int)Projectile.ai[0] - 1);
                proj.tileCollide = false;
            }

            AnimateProjectile();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frame++;
                Projectile.frame %= 6;
                Projectile.frameCounter = 0;
            }
        }
    }
}
