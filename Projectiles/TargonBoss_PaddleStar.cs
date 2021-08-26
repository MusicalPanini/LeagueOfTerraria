using Microsoft.Xna.Framework;
using TerraLeague.Dusts;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using TerraLeague.NPCs;
using TerraLeague.NPCs.TargonBoss;

namespace TerraLeague.Projectiles
{
    class TargonBoss_PaddleStar : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Paddle Star");
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.netImportant = true;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.position, TargonBossNPC.ZoeColor.ToVector3());
            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position + new Vector2(6, 6), 16, 16, DustID.PortalBolt, 0, 0, 1, TargonBossNPC.ZoeColor, 2f);
                dust.velocity = Projectile.velocity/2f;
                dust.noGravity = true;
            }

            Projectile.rotation += 0.02f * Projectile.velocity.Length();

            base.AI();
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            //if (Main.rand.Next(0, Main.expertMode ? 4 : 5) == 0)
            //    target.AddBuff(BuffID.Confused, 60);
            base.ModifyHitPlayer(target, ref damage, ref crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Rebound();

            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
            return false;
        }

        public void Rebound()
        {
            if (Projectile.velocity.X != Projectile.oldVelocity.X)
            {
                Projectile.velocity.X = -Projectile.oldVelocity.X;
            }
            else if (Projectile.velocity.Y != Projectile.oldVelocity.Y)
            {
                Projectile.velocity.Y = -Projectile.oldVelocity.Y;
            }

            //for (int i = 0; i < 3; i++)
            //{
            //    Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustType<Smoke>(), 0f, 0f, 150, new Color(255, 50, 255));
            //    dust.velocity *= 1f;
            //}
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PortalBolt, Projectile.velocity.X, Projectile.velocity.Y, 1, TargonBossNPC.ZoeColor);
                dust.noGravity = true;
                base.Kill(timeLeft);
            }
        }

    }
}
