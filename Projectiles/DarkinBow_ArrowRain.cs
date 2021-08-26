using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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
        public int State { get { return (int)Projectile.ai[0]; } set { Projectile.ai[0] = value; } }
        int State_Grounded = 1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rain of Arrows");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 0;
            Projectile.scale = 1f;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Ranged;
            //Projectile.extraUpdates = 1;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (State == State_Grounded)
            {
                if (Main.rand.Next(6) == 0)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position + new Vector2(0, 10), Projectile.width, Projectile.height - 10, DustID.Blood, Main.rand.NextFloat(-3, 3), 0, 0, default, 1f);
                    dust.fadeIn = 1.5f;
                    dust.noGravity = true;
                    dust.velocity.Y = 0f;
                }
            }
            else
            {
                Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
                if (Projectile.velocity.Y > 0)
                    Projectile.velocity.Y += 0.6f;
                else
                    Projectile.velocity.Y += 0.3f;

                if (Projectile.velocity.Y > 16f)
                    Projectile.velocity.Y = 16f;
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0, 0, 0, default, 1f);
                dust.noGravity = true;
                dust.velocity *= 0f;
            }
            Lighting.AddLight(Projectile.Center, 0.5f, 0f, 0f);
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
                Projectile.extraUpdates = 0;
                Projectile.height = 30;
                Projectile.timeLeft = 360;
                Projectile.knockBack = 0;
                Projectile.damage /= 2;
                State = State_Grounded;
                //Projectile.position += new Vector2(2, 0).RotatedBy(Projectile.oldVelocity.ToRotation());
                Projectile.velocity *= 0;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            }

            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Blood, Projectile.velocity.X / 2, Projectile.velocity.Y / 2, 100, new Color(33, 66, 133), 0.5f);
            }

            base.Kill(timeLeft);
        }
    }
}
