using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using TerraLeague.Projectiles.Homing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class VoidProphetsStaff_MaleficVisions : HomingProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Malefic Visions");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.alpha = 255;
            projectile.timeLeft = 300;
            projectile.penetrate = 1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.minion = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;

            CanOnlyHitTarget = true;
            CanRetarget = false;
            TurningFactor = 0.93f;
        }

        public override void AI()
        {
            NPC target = Main.npc[(int)projectile.ai[0]];

            if (projectile.timeLeft < 300 - 10)
            {
                projectile.friendly = true;

                HomingAI();
            }

            for (int i = 0; i < 3; i++)
            {
                Vector2 dustBoxPosition = new Vector2(projectile.position.X + 6, projectile.position.Y + 6);
                int dustBoxWidth = projectile.width - 12;
                int dustBoxHeight = projectile.height - 12;
                Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, DustID.Shadowflame, 0f, 0f, 100, default, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += projectile.velocity * 0.1f;
                dust.position.X -= projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= projectile.velocity.Y / 3f * (float)i;
            }

            Lighting.AddLight(projectile.position, 0f, 0f, 0.5f);

            if (!target.active)
                projectile.Kill();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.NewProjectileDirect(target.Center, Vector2.Zero, ModContent.ProjectileType<VoidProphetsStaff_MaleficVisionsHitBox>(), projectile.damage, 0, projectile.owner, target.whoAmI);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
