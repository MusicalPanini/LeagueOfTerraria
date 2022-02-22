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
        int RotationDir => Projectile.ai[0] > 0 ? 1 : -1;

        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.hide = false;
            Projectile.timeLeft = 270;

            dust1 = DustID.AmberBolt;
            dust2 = DustID.AmberBolt;
            dustScale = 2;

            turningFactor = 70;
            TargetImmunityFrames = 10;

            SpriteStart = new Rectangle(0, 0, 42, 22);
            SpriteMid = new Rectangle(0, 24, 42, 34);
            SpriteEnd = new Rectangle(0, 56, 42, 22);

            MaxDistance = 1000;
            maxCharge = 60;
            moveDistance = 30;
            lightColor = Color.Yellow;
            tileCollision = false;
        }

        public override void AI()
        {
            Projectile.velocity.Normalize();

            if (Projectile.timeLeft >= 180)
            {
                TerraLeague.DustLine(Projectile.Center, Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitX) * MaxDistance, DustID.AmberBolt, 0.05f, 1, default, false);
            }
            else if (Projectile.timeLeft < 180 && Projectile.timeLeft >= 30 )
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.PiOver2 / 150f * RotationDir);
            }

            BeamAI(Projectile.Center);
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCs.Add(index);
            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        }
    }
}
