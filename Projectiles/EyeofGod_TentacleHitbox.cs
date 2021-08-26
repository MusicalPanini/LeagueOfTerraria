using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class EyeofGod_TentacleHitbox : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tentacle Smash");
        }

        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 144;
            Projectile.alpha = 255;
            Projectile.timeLeft = 1;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            base.AI();
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
            Dust dust;
            for (int i = 0; i < 20; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.Center.Y + 48), Projectile.width, 2, DustID.Smoke, 0f, -3f, 100, default, 1f);
                dust.velocity *= 0.5f;
            }
            for (int i = 0; i < 10; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.Center.Y + 48), Projectile.width, 2, DustID.BlueTorch, 0f, -6f, 200, new Color(0, 255, 201), 2f);
                dust.noGravity = true;
                dust.velocity.Y -= 3f;
                dust.noLight = true;

                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.Center.Y + 48), Projectile.width, 2, DustID.BlueTorch, 0f, -6f, 200, new Color(0, 255, 201), 3f);
                dust.noGravity = true;
                dust.noLight = true;
            }
        }
    }
}
