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
    public class TargonBoss_SolarBeam : BeamProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 22;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.magic = true;
            projectile.hide = false;
            projectile.timeLeft = 150;

            dust1 = DustID.AmberBolt;
            dust2 = DustID.AmberBolt;
            dustScale = 2;

            turningFactor = 70;
            TargetImmunityFrames = 10;

            SpriteStart = new Rectangle(0, 0, 26, 22);
            SpriteMid = new Rectangle(0, 24, 26, 30);
            SpriteEnd = new Rectangle(0, 56, 26, 22);

            MaxDistance = 260;
            maxCharge = 30;
            moveDistance = 32;
            lightColor = Color.Yellow;
        }

        public override void AI()
        {
            projectile.velocity.Normalize();
            BeamAI(projectile.Center);
        }
    }
}
