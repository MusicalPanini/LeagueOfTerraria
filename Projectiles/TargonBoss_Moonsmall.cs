using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using TerraLeague.NPCs.TargonBoss;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class TargonBoss_Moonsmall : ModProjectile
    {
        int effectRadius = 16 * 8;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moonfall");
        }

        public override void SetDefaults()
        {
            effectRadius = 16 * 10;
            Projectile.width = effectRadius*2;
            Projectile.height = effectRadius * 2;
            Projectile.timeLeft = (120);
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            if (Projectile.timeLeft % 30 == 1)
            {
                //TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 15, 0f - (0.05f * Projectile.timeLeft / 30));
            }

            if (Projectile.timeLeft == 1)
                Projectile.hostile = true;
        }

        public override void Kill(int timeLeft)
        {
            TerraLeague.DustElipce(2, 2, 0, Projectile.Center, 263, TargonBossNPC.DianaColor, 1f, 180, true, 10);
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 74), Projectile.Center);

            base.Kill(timeLeft);
        }

        public override bool CanHitPlayer(Player target)
        {
            if (!Targeting.IsHitboxWithinRange(Projectile.Center, target.Hitbox, effectRadius))
                return false;
            return base.CanHitPlayer(target);
        }

        public override void PostDraw(Color lightColor)
        {
            TerraLeague.DrawCircle(Projectile.Center, effectRadius, TargonBossNPC.DianaColor);
            TerraLeague.DrawCircle(Projectile.Center, effectRadius - (effectRadius * Projectile.timeLeft / 120f), TargonBossNPC.DianaColor);

            base.PostDraw(lightColor);
        }
    }
}
