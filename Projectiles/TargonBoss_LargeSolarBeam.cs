using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ID;
using TerraLeague.Projectiles.Beam;

namespace TerraLeague.Projectiles
{
    public class TargonBoss_LargeSolarBeam : BeamProjectile
    {
        int RotationDir => projectile.ai[0] > 0 ? 1 : -1;

        public override void SetDefaults()
        {
            projectile.width = 42;
            projectile.height = 42;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.magic = true;
            projectile.hide = false;
            projectile.timeLeft = 240;

            dust1 = DustID.AmberBolt;
            dust2 = DustID.AmberBolt;
            dustScale = 2;

            turningFactor = 70;
            TargetImmunityFrames = 10;

            SpriteStart = new Rectangle(0, 0, 42, 22);
            SpriteMid = new Rectangle(0, 24, 42, 34);
            SpriteEnd = new Rectangle(0, 56, 42, 22);

            MaxDistance = 2000;
            maxCharge = 60;
            moveDistance = 30;
            lightColor = Color.Yellow;
        }

        public override void AI()
        {
            projectile.velocity.Normalize();

            if (projectile.timeLeft < 150 && projectile.timeLeft >= 30 )
            {
                projectile.velocity = projectile.velocity.RotatedBy(MathHelper.PiOver2 / 120f * RotationDir);
            }

            BeamAI(projectile.Center);
        }
    }
}
