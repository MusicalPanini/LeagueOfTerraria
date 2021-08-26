using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class BurningVengance_PillarOfFlame : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CountsAsHoming[Projectile.type] = true;
            DisplayName.SetDefault("Pillar Of Flame");
        }

        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.timeLeft = 1080;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 24;
            Projectile.tileCollide = false;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 34, -0.5f);
                Projectile.soundDelay = 240;
            }

            if (Main.rand.Next(0, 2) == 0)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0, 0, 0, default, 4f);
                dust.noGravity = true;
                dust.velocity = new Vector2(0, -5f);
                dust.noLight = true;
                dust.fadeIn = 2;

                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0, 0, 0, default, 6f);
                dust.noGravity = true;
                dust.noLight = true;

                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0, 3, 0, default, 1f);
                dust.noLight = true;

            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
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

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }
    }
}
