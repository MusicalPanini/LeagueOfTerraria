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
    class TheFallenCelestialsDarkMagic_TormentedShadow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tormented Shadow");
        }

        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 200;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1;
            Projectile.alpha = 150;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Purple.ToVector3());

            Dust dust;
            if (Projectile.timeLeft == 300)
            {
                for (int i = 0; i < 80; i++)
                {
                    dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.EnchantedNightcrawler, 0, 0, 0, new Color(159, 0, 255), 2);
                    dust.noGravity = true;
                }
            }
            if (Projectile.timeLeft < 30)
            {
                Projectile.alpha += 5;
            }
            int num = Main.rand.Next(0, 2);
            if (num == 0)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.EnchantedNightcrawler, 0, -1, 150);
                Main.dust[dustIndex].velocity.X *= 0.3f;
                Main.dust[dustIndex].color = new Color(159, 0, 255);
                Main.dust[dustIndex].noGravity = false;
            }
            if (Projectile.timeLeft < 15)
                Projectile.alpha += 5;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += (int)(damage * (1 - (target.life / (float)target.lifeMax)));

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
