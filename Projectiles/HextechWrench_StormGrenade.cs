using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using TerraLeague.Projectiles.Explosive;

namespace TerraLeague.Projectiles
{
    class HextechWrench_StormGrenade : ExplosiveProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("CH-2 Electron Storm Grenade");
        }

        public override void SetDefaults()
        {
            ExplosionWidth = 115;
            ExplosionHeight = 115;

            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1000;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
            Projectile.ignoreWater = false;
            Projectile.aiStyle = 0;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
                Projectile.rotation = Main.rand.NextFloat(0, 6.282f);
            Projectile.soundDelay = 100;

            Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight, 0,0,0, new Color(0,255, 255));

            Projectile.rotation += Projectile.velocity.X * 0.05f;

            Projectile.velocity.Y += 0.4f;

            if(Projectile.velocity.X > 8)
                Projectile.velocity.X = 8;
            else if(Projectile.velocity.X < -8)
                Projectile.velocity.X = -8;

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Stunned>(), 60);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void KillEffects()
        {
            SoundEngine.PlaySound(new LegacySoundStyle(2, 14), Projectile.position);
            SoundEngine.PlaySound(new LegacySoundStyle(3, 53), Projectile.position);

            Dust dust;
            for (int i = 0; i < 10; i++)
            {
                dust = Dust.NewDustDirect(Projectile.Center, 1, 1, DustID.Smoke, 0f, 0f, 100, default, 2f);
                dust.velocity *= 2f;
            }
            for (int i = 0; i < 40; i++)
            {
                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight, 0, 0, 0, new Color(0, 220, 220), 1f);
                dust.noGravity = true;
                dust.velocity = (dust.position - Projectile.Center) * 0.1f;
            }
            int effectRadius = 75/2;
            for (int i = 0; i < effectRadius / 2; i++)
            {
                Vector2 pos = new Vector2(effectRadius, 0).RotatedBy(MathHelper.ToRadians(360 * (i / (effectRadius / 2f)))) + Projectile.Center;

                Dust dustR = Dust.NewDustPerfect(pos, 261, Vector2.Zero, 0, new Color(0, 220, 220), 2);
                dustR.noGravity = true;
                dustR.velocity = (dustR.position - Projectile.Center) * 0.1f;
            }

            for (int i = 0; i < 30; i++)
            {
                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0, 0, 0, new Color(0, 220, 220), 0.6f);
                dust.velocity = (dust.position - Projectile.Center) * 0.075f;
                dust.velocity.Y -= 1f;
                dust.noLight = true;
            }
        }
    }
}
