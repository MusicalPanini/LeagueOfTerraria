using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
	public class HexCoreStaff_ChaosStormZap : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chaos Storm");
            ProjectileID.Sets.CountsAsHoming[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.alpha = 255;
            Projectile.timeLeft = 3;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            //Projectile.extraUpdates = 100;
        }

        public override void AI()
        {
            for (int i = 0; i < 4; i++)
            {
                Vector2 dustPos = Projectile.position;
                dustPos -= Projectile.velocity * ((float)i * 0.25f);
                Vector2 position125 = dustPos;
                Dust dust = Dust.NewDustDirect(position125, 1, 1, DustID.Electric, 0f, 0f, 0, default, (float)Main.rand.Next(70, 110) * 0.013f);
                dust.position = dustPos;
                dust.position.X += (float)(Projectile.width / 2);
                dust.position.Y += (float)(Projectile.height / 2);
                dust.velocity *= 0.2f;
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            return (target.whoAmI == (int)Projectile.ai[0]);
        }
    }
}
