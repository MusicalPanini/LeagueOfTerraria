using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using System.Collections.Generic;

namespace TerraLeague.Projectiles
{
    class StarfireSpellblades_DivineJudgement : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Divine Judgement Shield");
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.timeLeft = 120;
            Projectile.penetrate = 1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1;
            Projectile.alpha = 150;
        }

        public override void AI()
        {
            Player player = Main.player[(int)Projectile.ai[0]];

            Projectile.Center = player.Center;
            player.AddBuff(BuffType<DivineJudgementBuff>(), Projectile.timeLeft);
            Lighting.AddLight(Projectile.Center, new Color(255, 226, 82).ToVector3());


            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GemTopaz, 0, -4, 200, default, 1f);
                dust.noGravity = true;
            }

            AnimateProjectile();
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[(int)Projectile.ai[0]];
            for (int i = 0; i < 7; i++)
            {
                Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), new Vector2(player.Center.X - 345 + (115 * i), player.position.Y - (Main.screenHeight / 2)), new Vector2(0, 25), ProjectileType<StarfireSpellblades_DivineJudgementSword>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            base.Kill(timeLeft);
        }

        public void AnimateProjectile() 
        {
            Projectile.friendly = false;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5) 
            {
                Projectile.frame++;
                Projectile.frame %= 4;
                Projectile.frameCounter = 0;
            }
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
    }
}
