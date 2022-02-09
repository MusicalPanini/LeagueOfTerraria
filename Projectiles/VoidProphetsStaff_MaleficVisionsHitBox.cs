using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class VoidProphetsStaff_MaleficVisionsHitBox : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //ProjectileID.Sets.CountsAsHoming[Projectile.type] = true;
            DisplayName.SetDefault("Malefic Visions");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.alpha = 255;
            Projectile.timeLeft = 60 * 5;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -2;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (Projectile.localAI[0]  == 30)
            {
                Projectile.friendly = true;
                Projectile.localAI[0]  = 0;
            }
            else
            {
                Projectile.friendly = false;
                Projectile.localAI[0] ++;
            }

            NPC npc = Main.npc[(int)Projectile.ai[0]];
            Projectile.Center = npc.Center;

            for (int i = 0; i < (npc.width* npc.height)/200; i++)
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Shadowflame, 0f, 0f, 100, default, 1.5f);
                dust.noGravity = true;
                dust.fadeIn = 1f;
                dust.velocity = npc.velocity;
            }

            if (!npc.active)
                Projectile.Kill();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().grievousWounds)
                damage = (int)(damage * 1.5);

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
            if (timeLeft > 0 && Projectile.owner == Main.LocalPlayer.whoAmI)
            {
                float distance = 700f;
                NPC target = null;
                for (int k = 0; k < 200; k++)
                {
                    NPC npcCheck = Main.npc[k];

                    if (npcCheck.active && !npcCheck.friendly && npcCheck.lifeMax > 5 && !npcCheck.dontTakeDamage && !npcCheck.immortal)
                    {
                        Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                        float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                        if (distanceTo < distance)
                        {
                            distance = distanceTo;
                            target = npcCheck;
                        }
                    }
                }

                if (target != null)
                {
                    Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, new Vector2(0, -10), ModContent.ProjectileType<VoidProphetsStaff_MaleficVisions>(), Projectile.damage, 0, Projectile.owner, target.whoAmI);
                }
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.whoAmI == (int)Projectile.ai[0])
                return base.CanHitNPC(target);
            else
                return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
