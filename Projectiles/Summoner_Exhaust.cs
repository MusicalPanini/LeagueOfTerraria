using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using TerraLeague.Projectiles.Homing;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class Summoner_Exhaust : HomingProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Exhaust");
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
                Main.PlaySound(new LegacySoundStyle(2, 88).WithPitchVariance(-0.3f), projectile.Center);
            projectile.soundDelay = 100;

            Dust dust = Dust.NewDustPerfect(projectile.position, 262, Vector2.Zero, 0, default, 1f);
            dust.noGravity = true;
            dust.alpha = 100;

            HomingAI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Exhausted>(), 600);
            projectile.ai[1] = 1;
            projectile.netUpdate = true;
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Dig, projectile.Center);

            Vector2 position = projectile.Center;
            if (Main.npc[(int)projectile.ai[0]].active)
                position = Main.npc[(int)projectile.ai[0]].Center;

            int type = 174;

            int arrow1Height = 32;
            int arrow1Width = 32;
            int arrow1Dis = 16;

            int arrow2Height = 24;
            int arrow2Width = 24;
            int arrow2Dis = 0;

            int arrow3Height = 16;
            int arrow3Width = 16;
            int arrow3Dis = -16;

            Vector2 offset = new Vector2(0, -64);

            Vector2 Arrow1BottomLeft = position + new Vector2(-arrow1Width, 0 + arrow1Dis) + offset;
            Vector2 Arrow1BottomRight = position + new Vector2(arrow1Width, 0 + arrow1Dis) + offset;
            Vector2 Arrow1Top = position + new Vector2(0, arrow1Height + arrow1Dis) + offset;

            Vector2 Arrow2BottomLeft = position + new Vector2(-arrow2Width, 0 + arrow2Dis) + offset;
            Vector2 Arrow2BottomRight = position + new Vector2(arrow2Width, 0 + arrow2Dis) + offset;
            Vector2 Arrow2Top = position + new Vector2(0, arrow2Height + arrow2Dis) + offset;

            Vector2 Arrow3BottomLeft = position + new Vector2(-arrow3Width, 0 + arrow3Dis) + offset;
            Vector2 Arrow3BottomRight = position + new Vector2(arrow3Width, 0 + arrow3Dis) + offset;
            Vector2 Arrow3Top = position + new Vector2(0, arrow3Height + arrow3Dis) + offset;

            TerraLeague.DustLine(Arrow1BottomLeft, Arrow1Top, type, 1, 2, default, true, 0, 6);
            TerraLeague.DustLine(Arrow1BottomRight, Arrow1Top, type, 1, 2, default, true, 0, 6);

            TerraLeague.DustLine(Arrow2BottomLeft, Arrow2Top, type, 1, 2, default, true, 0, 6);
            TerraLeague.DustLine(Arrow2BottomRight, Arrow2Top, type, 1, 2, default, true, 0, 6);

            TerraLeague.DustLine(Arrow3BottomLeft, Arrow3Top, type, 1, 2, default, true, 0, 6);
            TerraLeague.DustLine(Arrow3BottomRight, Arrow3Top, type, 1, 2, default, true, 0, 6);

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.AmberBolt, 0, 0, 0, default, 1f);
                dust.noGravity = true;
                dust.alpha = 100;
            }

            base.Kill(timeLeft);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
