using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles.Homing
{
    public abstract class RichochetProjectile : HomingProjectile
    {
        public int[] HaveHit;
        public int hitCounter = 0;
        public bool exlusiveTargeting = false;
        public int PostHitTimeLeft = 300;

        public override void SendExtraAI(BinaryWriter writer)
        {
            if (projectile.owner == Main.LocalPlayer.whoAmI)
            {
                for (int i = 0; i < HaveHit.Length; i++)
                {
                    writer.Write(HaveHit[i]);
                }
                writer.Write(hitCounter);
            }

            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            for (int i = 0; i < HaveHit.Length; i++)
            {
                HaveHit[i] = reader.ReadInt32();
            }
            hitCounter = reader.ReadInt32();
            base.ReceiveExtraAI(reader);
        }

        public override void AI()
        {
            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.netUpdate = true;
            projectile.timeLeft = PostHitTimeLeft;

            if (exlusiveTargeting)
            {
                HaveHit[hitCounter] = target.whoAmI;
            }
            hitCounter++;

            GetNewTarget();

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void GetNewTarget()
        {
            projectile.netUpdate = true;

            if (exlusiveTargeting)
                TargetWhoAmI = TerraLeague.GetClosestNPC(projectile.Center, 480, HaveHit, true, true);
            else
                TargetWhoAmI = TerraLeague.GetClosestNPC(projectile.Center, 480);

            if (!CouldNotFindTarget)
            {
                Main.npc[TargetWhoAmI].immune[projectile.owner] = 0;
            }
            else
            {
                projectile.Kill();
            }
        }

        public void SetHomingDefaults(bool exclusiveTargeting, int targetingRange, int timeLeftAfterHit)
        {
            this.exlusiveTargeting = exclusiveTargeting;
            this.targetingRange = targetingRange;
            PostHitTimeLeft = timeLeftAfterHit;

            if (exclusiveTargeting)
            {
                HaveHit = new int[projectile.penetrate];
                for (int i = 0; i < HaveHit.Length; i++)
                {
                    HaveHit[i] = -1;
                }
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (exlusiveTargeting)
            {
                if (!HaveHit.Contains(TargetWhoAmI) || (CanOnlyHitTarget && TargetWhoAmI == target.whoAmI))
                    return base.CanHitNPC(target);
                else
                    return false;
            }
            else
            {
                return base.CanHitNPC(target);
            }
        }
    }
}
