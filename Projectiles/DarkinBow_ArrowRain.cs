using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class DarkinBow_ArrowRain : ModProjectile
    {
        public int State { get { return (int)projectile.ai[0]; } set { projectile.ai[0] = value; } }
        int State_Grounded = 1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rain of Arrows");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.alpha = 0;
            projectile.scale = 1f;
            projectile.timeLeft = 300;
            projectile.ranged = true;
            //projectile.extraUpdates = 1;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (State == State_Grounded)
            {
                if (Main.rand.Next(6) == 0)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position + new Vector2(0, 10), projectile.width, projectile.height - 10, DustID.Blood, Main.rand.NextFloat(-3, 3), 0, 0, default, 1f);
                    dust.fadeIn = 1.5f;
                    dust.noGravity = true;
                    dust.velocity.Y = 0f;
                }
            }
            else
            {
                projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
                if (projectile.velocity.Y > 0)
                    projectile.velocity.Y += 0.6f;
                else
                    projectile.velocity.Y += 0.3f;

                if (projectile.velocity.Y > 16f)
                    projectile.velocity.Y = 16f;
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Blood, 0, 0, 0, default, 1f);
                dust.noGravity = true;
                dust.velocity *= 0f;
            }
            Lighting.AddLight(projectile.Center, 0.5f, 0f, 0f);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.AddBuff(BuffType<Slowed>(), 60);
            target.AddBuff(BuffType<GrievousWounds>(), 60);

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (State == State_Grounded)
            {

            }
            else
            {
                projectile.extraUpdates = 0;
                projectile.height = 30;
                projectile.timeLeft = 360;
                projectile.knockBack = 0;
                projectile.damage /= 2;
                State = State_Grounded;
                //projectile.position += new Vector2(2, 0).RotatedBy(projectile.oldVelocity.ToRotation());
                projectile.velocity *= 0;
                Main.PlaySound(SoundID.Item10, projectile.position);
            }

            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Blood, projectile.velocity.X / 2, projectile.velocity.Y / 2, 100, new Color(33, 66, 133), 0.5f);
            }

            base.Kill(timeLeft);
        }
    }
}
