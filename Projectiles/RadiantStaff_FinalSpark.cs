using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Projectiles
{
    class RadiantStaff_FinalSpark : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Final Spark");
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.timeLeft = 1000;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.hide = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().illuminated)
                damage *= 2;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void AI()
        {
            if ((int)Projectile.ai[1] == 0)
            {
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 4;
                }
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }

                if (Projectile.timeLeft == 1000)
                {
                    Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
                    TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 13, -1f);
                }

                Projectile.velocity = Vector2.Zero;
                Projectile.Center = Main.player[Projectile.owner].Center;

                if (Projectile.timeLeft == 940)
                {
                    Projectile.ai[1] = 1;
                    Projectile.velocity = new Vector2(0, -10).RotatedBy(Projectile.rotation);
                    Projectile.friendly = true;
                    Projectile.extraUpdates = 40;
                    TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 72, -1f);
                }
            }

            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - (Vector2.One * 8), 16, 16, DustID.GoldFlame, 0, 0, 0, default, 3f - (3f* (Projectile.alpha/255f)));
                dust.noGravity = true;
                dust.fadeIn = 1.2f;
            }
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }
    }

    public class FinalSparkGlobalNPC : GlobalNPC
    {
        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            if (npc.HasBuff(BuffType<Buffs.Illuminated>()))
            {
                if (projectile.type == ProjectileType<RadiantStaff_FinalSpark>())
                    npc.DelBuff(npc.FindBuffIndex(BuffType<Buffs.Illuminated>()));
            }
        }
    }
}
