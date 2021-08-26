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
    class EmperoroftheSands_EmperorsDivide : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Emperor's Divide");
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 238;
            Projectile.timeLeft = 90;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            base.OnHitPlayer(target, damage, crit);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 90)
                Projectile.friendly = true;
            if(Projectile.velocity.Length() > 0 && Projectile.timeLeft <= 75)
            {
                Projectile.velocity.X *= .8f;
                Projectile.velocity.Y *= .8f;
            }

            Projectile.knockBack = 30 * (Projectile.velocity.Length() / 16);
            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Sand, 0, 0, Projectile.alpha);
            }
            if (Projectile.timeLeft < 30)
            {
                Projectile.alpha += 9;
            }
        }
    }
}
