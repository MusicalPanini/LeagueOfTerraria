using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Severum_Onslaught : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Onslaught");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.timeLeft = 240;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.extraUpdates = 60;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if ((int)Projectile.ai[1] == 1)
            {
                NPC target = Main.npc[(int)Projectile.ai[0]];
                if (!Projectile.Hitbox.Intersects(target.Hitbox))
                {
                    Projectile.Kill();
                }
            }

            Dust dust = Dust.NewDustPerfect(Projectile.Center, 235/*182*/, Vector2.Zero);
            dust.noGravity = true;
            dust.velocity *= 0;
            dust.noLight = true;
            dust = Dust.NewDustPerfect(Projectile.Center - Projectile.velocity.SafeNormalize(Vector2.Zero), 235, Vector2.Zero);
            dust.noGravity = true;
            dust.velocity *= 0;
            dust.noLight = true;

            //Lighting.AddLight(Projectile.Center, 1f, 0.0f, 0f);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.ai[1] = 1;
            Projectile.penetrate = 2;
            //Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().lifeStealMelee += 2;
            Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().lifeToHeal += 2;
            //Projectile.NewProjectileDirect(target.Center, Vector2.Zero, ProjectileID.VampireHeal, 0, 0, Projectile.owner, Projectile.owner, 1);
            Projectile.friendly = false;
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.whoAmI == (int)Projectile.ai[0])
            {
                return base.CanHitNPC(target);
            }
            return false;

        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override bool? CanCutTiles()
        {
            return Projectile.friendly;
        }
    }
}
