using Microsoft.Xna.Framework;
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
    public abstract class HomingProjectile : ModProjectile
    {
        public int targetingRange = 450;
        public float MaxVelocity = -1;
        public float TurningFactor = 0;
        public bool CanOnlyHitTarget = false;
        public bool NPC_CanTargetDummy = false;
        public bool NPC_CanTargetCritters = false;
        public bool CanRetarget = false;

        public bool TargetPlayers = false;
        public bool Player_OnlyTargetTeam = false;
        public bool Player_CanTargetDead = false;

        public int TargetWhoAmI { get { return (int)projectile.ai[0]; } set { projectile.ai[0] = value; } }
        public bool CouldNotFindTarget { get { return -1 == (int)projectile.ai[0]; } }
        public bool NeedsNewTarget { get { return -2 == (int)projectile.ai[0]; } }
        public Entity TargetEntity { get { if (TargetPlayers) { return Main.player[TargetWhoAmI]; } else { return Main.npc[TargetWhoAmI]; } } }

        public override void SendExtraAI(BinaryWriter writer)
        {
            if (projectile.owner == Main.LocalPlayer.whoAmI)
            {
                writer.Write(targetingRange);
                writer.Write(MaxVelocity);
                writer.Write(TurningFactor);
                writer.Write(CanOnlyHitTarget);
                writer.Write(NPC_CanTargetDummy);
                writer.Write(NPC_CanTargetCritters);
                writer.Write(CanRetarget);
                writer.Write(TargetPlayers);
                writer.Write(Player_OnlyTargetTeam);
                writer.Write(Player_CanTargetDead);
            }

            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            targetingRange = reader.ReadInt32(); ;
            MaxVelocity = reader.ReadSingle(); ;
            TurningFactor = reader.ReadSingle(); ;
            CanOnlyHitTarget = reader.ReadBoolean();
            NPC_CanTargetDummy = reader.ReadBoolean();
            NPC_CanTargetCritters = reader.ReadBoolean();
            CanRetarget = reader.ReadBoolean(); ;

            TargetPlayers = reader.ReadBoolean();
            Player_OnlyTargetTeam = reader.ReadBoolean();
            Player_CanTargetDead = reader.ReadBoolean();
            base.ReceiveExtraAI(reader);
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void AI()
        {
            HomingAI();

            base.AI();
        }

        public virtual void HomingAI()
        {
            if (MaxVelocity == -1)
                MaxVelocity = projectile.velocity.Length();

            if (NeedsNewTarget)
            {
                GetNewTarget();
            }

            if (CouldNotFindTarget)
            {
                CouldNotFindNewTarget();
                return;
            }


            if (!TargetEntity.active)
            {
                TargetIsNotActive();
            }
            else
            {
                Vector2 NewVelocity = (TurningFactor * projectile.velocity) + ((1 - TurningFactor) * TerraLeague.CalcVelocityToPoint(projectile.Center, TargetEntity.Center, MaxVelocity));

#pragma warning disable CS1718 // Comparison made to same variable
                if (NewVelocity != NewVelocity)
#pragma warning restore CS1718 // Comparison made to same variable
                    projectile.velocity = Vector2.Zero;
                else
                    projectile.velocity = NewVelocity;
            }
        }

        /// <summary>
        /// Runs when the specified target in ai[0] is not active.
        /// </summary>
        public virtual void TargetIsNotActive()
        {
            if (CanRetarget)
                GetNewTarget();
            else
                projectile.Kill();

        }

        public virtual void CouldNotFindNewTarget()
        {
            if (CanRetarget)
                TargetWhoAmI = -2;
            else
                projectile.Kill();
        }

        public virtual void GetNewTarget()
        {
            projectile.netUpdate = true;
            if (TargetPlayers)
                TargetWhoAmI = Targeting.GetClosestPlayer(projectile.Center, targetingRange, -1, -1);
            else
                TargetWhoAmI = Targeting.GetClosestNPC(projectile.Center, targetingRange, -1, -1, NPC_CanTargetCritters, NPC_CanTargetDummy);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (CanOnlyHitTarget)
            {
                return target.whoAmI == TargetWhoAmI;
            }
            else
            {
                return base.CanHitNPC(target);
            }
        }

        public virtual bool IsHittingPlayer(Player player)
        {
            if (TargetPlayers)
            {
                return projectile.Hitbox.Intersects(player.Hitbox);
            }

            return false;
        }

        public virtual void OnHitFriendlyPlayer(Player player)
        {
            projectile.netUpdate = true;
            projectile.penetrate--;
        }
    }
}
