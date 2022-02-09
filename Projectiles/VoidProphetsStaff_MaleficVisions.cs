﻿using Microsoft.Xna.Framework;
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
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.alpha = 255;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;

            CanOnlyHitTarget = true;
            CanRetarget = false;
            TurningFactor = 0.93f;
        }

        public override void AI()
        {
            NPC target = Main.npc[(int)Projectile.ai[0]];

            if (Projectile.timeLeft < 300 - 10)
            {
                Projectile.friendly = true;

                HomingAI();
            }

            for (int i = 0; i < 3; i++)
            {
                Vector2 dustBoxPosition = new Vector2(Projectile.position.X + 6, Projectile.position.Y + 6);
                int dustBoxWidth = Projectile.width - 12;
                int dustBoxHeight = Projectile.height - 12;
                Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, DustID.Shadowflame, 0f, 0f, 100, default, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += Projectile.velocity * 0.1f;
                dust.position.X -= Projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= Projectile.velocity.Y / 3f * (float)i;
            }

            Lighting.AddLight(Projectile.position, 0f, 0f, 0.5f);

            if (!target.active)
                Projectile.Kill();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<VoidProphetsStaff_MaleficVisionsHitBox>(), Projectile.damage, 0, Projectile.owner, target.whoAmI);
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
