using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using TerraLeague.Items.SummonerSpells;
using TerraLeague.Projectiles.Homing;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class Summoner_Ignite : HomingProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ignite");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.scale = 1.2f;
            Projectile.timeLeft = 301;
            Projectile.extraUpdates = 16;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            CanOnlyHitTarget = true;
            CanRetarget = false;
            TurningFactor = 0.93f;
            MaxVelocity = 4;
        }

        public override void AI()
        {
            if(Projectile.soundDelay == 0)
                Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 73), Projectile.Center);
            Projectile.soundDelay = 100;

            Dust dust = Dust.NewDustPerfect(Projectile.position, 6, Vector2.Zero, 0, default, 2f);
            dust.noGravity = true;

            HomingAI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Ignited>(), IgniteRune.debuffDuration * 60);
            target.AddBuff(BuffType<GrievousWounds>(), IgniteRune.debuffDuration * 60);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6,0,0,0,default,2);
                dust.noGravity = true;
            }
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

            base.Kill(timeLeft);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
