using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TerraLeague.Projectiles.Homing;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Chalicar_Ricochet : RichochetProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chalicar Ricochet");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            projectile.width = 36;
            projectile.height = 36;
            projectile.timeLeft = 60;
            projectile.penetrate = 6;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.alpha = 0;
            projectile.netImportant = true;

            SetHomingDefaults(true, 480, 301);
            CanOnlyHitTarget = false;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
            {
                projectile.soundDelay = 8;
                Main.PlaySound(SoundID.Item7, projectile.position);
            }

            projectile.rotation += 0.3f * (float)projectile.direction;

            if (hitCounter != 0)
            {
                base.AI();
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (hitCounter == 0)
            {
                projectile.tileCollide = false;
                projectile.velocity *= 0.75f;
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Silver, projectile.oldVelocity.X * 0.25f, projectile.oldVelocity.Y * 0.25f, 157, new Color(234, 255, 0));
            }

            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 16;
            return true;
        }

        public override bool? CanHitNPC(NPC target)
        {
                return base.CanHitNPC(target);
        }
    }
}
