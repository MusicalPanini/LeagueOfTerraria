using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Projectiles.Homing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class Item_Echo : HomingProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Echo");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.alpha = 255;
            Projectile.timeLeft = 240;
            Projectile.penetrate = 1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;

            CanRetarget = true;
            TurningFactor = 0.93f;
            MaxVelocity = 12;
        }

        public override void AI()
        {
            if (Projectile.timeLeft < 240 - 15)
            {
                HomingAI();

                Projectile.friendly = true;
            }
            if (Projectile.timeLeft < 15)
            {
                Projectile.alpha += 10;
            }

            for (int i = 0; i < 3; i++)
            {
                Vector2 dustBoxPosition = new Vector2(Projectile.position.X + 6, Projectile.position.Y + 6);
                int dustBoxWidth = Projectile.width - 12;
                int dustBoxHeight = Projectile.height - 12;
                Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 113, 0f, 0f, 100, default, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += Projectile.velocity * 0.1f;
                dust.position.X -= Projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= Projectile.velocity.Y / 3f * (float)i;
            }

            Lighting.AddLight(Projectile.position, 0f, 0f, 0.5f);
        }

        public override void GetNewTarget()
        {
            Projectile.netUpdate = true;
            TargetWhoAmI = Targeting.GetClosestNPC(Projectile.Center, targetingRange, (int)Projectile.ai[1], -1, NPC_CanTargetCritters, NPC_CanTargetDummy);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 113, Projectile.velocity.X, Projectile.velocity.Y, 50, default, 1.2f);
                dust.noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
        }
    }
}
