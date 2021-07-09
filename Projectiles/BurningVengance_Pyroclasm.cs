using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using TerraLeague.Projectiles.Homing;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class BurningVengance_Pyroclasm : RichochetProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pyroclasm");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.timeLeft = 300;
            projectile.penetrate = 10;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.magic = true;
            projectile.alpha = 255;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;

            SetHomingDefaults(false, 480, 310);
            CanOnlyHitTarget = true;
            NPC_CanTargetCritters = true;
            MaxVelocity = 0;
        }


        public override void AI()
        {
            if (projectile.timeLeft <= 300)
            {
                if (hitCounter != 0 && projectile.timeLeft == 300)
                {
                    projectile.netUpdate = true;
                    GetNewTarget();
                }

                MaxVelocity = 16 * (1 - (projectile.timeLeft / 300f));
                MaxVelocity *= 6;
                if (MaxVelocity > 16)
                    MaxVelocity = 16;
                base.AI();
            }
            else
            {
                NPC npc = Main.npc[TargetWhoAmI];
                projectile.Center = npc.Center;
                MaxVelocity = 0;
            }

            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Fire, 0, 0, 0, default, 4f);
                dust.noGravity = true;
                dust.noLight = true;

                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Fire, 0, 3, 0, default, 1f);
                dust.noLight = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.netUpdate = true;

            if (target.HasBuff(BuffID.OnFire))
            {
                target.AddBuff(BuffType<Ablaze>(), 600);
                target.DelBuff(target.FindBuffIndex(BuffID.OnFire));
            }
            else if (target.HasBuff(BuffType<Ablaze>()))
            {
                target.AddBuff(BuffType<Ablaze>(), 600);
            }
            else
            {
                target.AddBuff(BuffID.OnFire, 1200);
            }
            projectile.timeLeft = PostHitTimeLeft;
            hitCounter++;
            //base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().ablaze)
            {
                damage *= 2;
                Projectile.NewProjectileDirect(target.Center, Vector2.Zero, ProjectileType<BurningVengance_PyroclasmExplosion>(), projectile.damage / 2, 5, projectile.owner);
            }
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (projectile.timeLeft < 300)
                return base.CanHitNPC(target);
            else
                return false;
        }

        public override void GetNewTarget()
        {
            projectile.netUpdate = true;
            TargetWhoAmI = Targeting.GetClosestNPC(projectile.Center, targetingRange, TargetWhoAmI, -1, NPC_CanTargetCritters, NPC_CanTargetDummy);
        }

        public override void Kill(int timeLeft)
        {
            if (projectile.penetrate == 0)
            {
                TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 45, -0.5f);
            }

            base.Kill(timeLeft);
        }
    }
}
