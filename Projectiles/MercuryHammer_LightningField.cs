using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class MercuryHammer_LightningField : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning Field");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 200;
            Projectile.timeLeft = 10;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1;
            Projectile.alpha = 100;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 3, 53, 0.25f);
            Projectile.soundDelay = 10;

            if (Main.player[Projectile.owner].channel)
                Projectile.timeLeft = Main.player[Projectile.owner].itemTimeMax;

            Projectile.Center = Main.player[Projectile.owner].Center;
                Lighting.AddLight(Projectile.Center, 0.5f, 0.25f, 0.1f);

            Projectile.rotation += 0.01f;

            int num = Main.rand.Next(0, 3);
            Dust dust;
            if (num == 0)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 264, 0, -1, 150);
                dust.color = Color.Orange;
                dust.noGravity = true;
            }
            AnimateProjectile();
        }

        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frame++;
                Projectile.frame %= 4; 
                Projectile.frameCounter = 0;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Targeting.IsHitboxWithinRange(Projectile.Center, targetHitbox, Projectile.width / 2);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
