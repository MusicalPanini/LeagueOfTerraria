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
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.scale = 1.2f;
            projectile.timeLeft = 301;
            projectile.extraUpdates = 16;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;

            CanOnlyHitTarget = true;
            CanRetarget = false;
            TurningFactor = 0.93f;
            MaxVelocity = 4;
        }

        public override void AI()
        {
            if(projectile.soundDelay == 0)
                Main.PlaySound(new LegacySoundStyle(2, 73), projectile.Center);
            projectile.soundDelay = 100;

            Dust dust = Dust.NewDustPerfect(projectile.position, DustID.Fire, Vector2.Zero, 0, default, 2f);
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
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Fire,0,0,0,default,2);
                dust.noGravity = true;
            }
            Main.PlaySound(SoundID.Dig, projectile.Center);

            base.Kill(timeLeft);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
