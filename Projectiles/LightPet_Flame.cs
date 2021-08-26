using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class LightPet_Flame : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eternal Flame");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            if (Main.rand.Next(3) == 0)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PortalBolt, 0, 0, 0, new Color(0, 255, 180), 2f);
                dust.velocity *= 0.1f;
                dust.velocity.X = Projectile.velocity.X;
                dust.fadeIn = 0.1f;
                dust.noGravity = true;
                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PortalBolt, 0, 0, 0, new Color(0, 255, 180), 1.5f);
                dust.velocity.X = Projectile.velocity.X;
                dust.velocity.Y = Main.rand.NextFloat(-2, 0);
                dust.fadeIn = 0.5f;
                dust.noGravity = true;
            }
            Lighting.AddLight(Projectile.Center, 0, 1, 0.6f);
            Projectile.velocity *= 0.99f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}
