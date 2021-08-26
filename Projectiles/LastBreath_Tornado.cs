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
    class LastBreath_Tornado : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tornado");
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.timeLeft = 90;
            Projectile.penetrate = 100;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1.5f;
            AIType = 0;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.velocity.Y = -12;
            base.OnHitPlayer(target, damage, crit);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!NPCID.Sets.ShouldBeCountedAsBoss[target.type])
            {
                target.velocity.Y = -12;
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            if(Projectile.velocity.Length() > 0)
            {
                Projectile.velocity.X *= .98f;
                Projectile.velocity.Y *= .98f;
            }
            

            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Cloud);
            if (Projectile.timeLeft < 30)
            {
                Projectile.alpha += 9;
            }
            AnimateProjectile();
        }

        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 3)
            {
                Projectile.frame++;
                Projectile.frame %= 6; 
                Projectile.frameCounter = 0;
            }
        }
    }
}
