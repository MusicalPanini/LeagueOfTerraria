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
    public class EyeOfTheVoid_Lazer : PlayerBeamProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.hide = true;

            dust1 = 112;
            dust2 = 112;
            dustScale = 2;

            trackMouse = true;
            isChannel = true;
            turningFactor = 70;
            TargetImmunityFrames = 10;

            SpriteStart = new Rectangle(0, 0, 26, 22);
            SpriteMid = new Rectangle(0, 24, 26, 30);
            SpriteEnd = new Rectangle(0, 56, 26, 22);

            MaxDistance = 600;
            maxCharge = 50;
            moveDistance = 60f;
            lightColor = new Color(64, 0, 64);
        }


    }
}
