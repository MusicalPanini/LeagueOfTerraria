using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class TideCallerStaff_BubbleVisual : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Aqua Prison");
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.alpha = 0;
            Projectile.timeLeft = 120;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 120)
            {
                 Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 86), Projectile.Center);
            }

            Projectile.scale = ((Main.npc[(int)Projectile.ai[0]].width / 30) + (Main.npc[(int)Projectile.ai[0]].height / 30) + 5) / 2;
            Projectile.Center = Main.npc[(int)Projectile.ai[0]].Center;
            if (Main.npc[(int)Projectile.ai[0]].life <= 0)
            {
                Projectile.Kill();
            }

            int trueWidth = (int)(Projectile.width * Projectile.scale);
            int trueHeight = (int)(Projectile.height * Projectile.scale);
            Vector2 truePosition = new Vector2(Projectile.Center.X - (trueWidth/2), Projectile.Center.Y - (trueHeight/2));

            Lighting.AddLight(Projectile.Center, 0f, 0f, 0.5f);
            Dust dust = Dust.NewDustDirect(truePosition, trueWidth, trueHeight, 211, 0, 2, 150, default, 1.5f);
            dust.noGravity = true;

            base.AI();
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 54), Projectile.Center);

            int trueWidth = (int)(Projectile.width * Projectile.scale);
            int trueHeight = (int)(Projectile.height * Projectile.scale);
            Vector2 truePosition = new Vector2(Projectile.Center.X - (trueWidth / 2), Projectile.Center.Y - (trueHeight / 2));

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(truePosition, trueWidth, trueHeight, DustType<Dusts.BubbledBubble>(), -5 + i, 0, 100, default, 4f);
                dust.noGravity = true;

                dust = Dust.NewDustDirect(truePosition, trueWidth, trueHeight, 211, 0, 0, 150, default, 2.5f);
                dust.noGravity = true;
                dust.velocity *= 2;
            }

            base.Kill(timeLeft);
        }
    }
}
