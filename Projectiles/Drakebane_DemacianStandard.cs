using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Projectiles
{
    class Drakebane_DemacianStandard : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demacian Standard");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 64;
            Projectile.timeLeft = 60*8;
            Projectile.penetrate = 1000;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1;
            Projectile.alpha = 0;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            if (Projectile.friendly && Projectile.velocity.Length() < 0.1f && Projectile.tileCollide)
            {
                for (int i = 0; i < 5; i++)
                {
                    Collision.HitTiles(Projectile.Bottom, Projectile.oldVelocity, Projectile.width, 1);
                }
                Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.Item10, Projectile.position);
                Projectile.extraUpdates = 0;
                Projectile.timeLeft = 60 * 6;
                Projectile.friendly = false;
            }

            Lighting.AddLight(Projectile.Center, 0.75f, 0.75f, 0.75f);

            if (Projectile.ai[1] == 0f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            {
                Projectile.ai[1] = 1f;
                Projectile.netUpdate = true;
            }
            if (Projectile.ai[1] != 0f)
            {
                Projectile.tileCollide = true;
            }

            if (Projectile.friendly)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Stone);
                dust.velocity /= 3;
            }
            else
            {
                Dust dust = Dust.NewDustDirect(new Vector2(0, -2) + Projectile.BottomLeft, Projectile.width, 3, 204, 0, 0, 0, default, 1.3f);
                dust.noGravity = true;
                dust.velocity.X *= 2;
                dust.velocity.Y = 0;

                TerraLeague.DustBorderRing(500, Projectile.Center, 204, default, 1.5f, true, true, 0.025f);
            }

            var players = Targeting.GetAllPlayersInRange(Projectile.Center, 500, -1, Main.player[Projectile.owner].team);
            for (int i = 0; i < players.Count; i++)
            {
                Player target = Main.player[players[i]];
                target.AddBuff(BuffType<ForDemacia>(), 2);
            }

            AnimateProjectile();
        }

        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frame++;
                Projectile.frame %= 4; 
                Projectile.frameCounter = 0;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            if (!Projectile.friendly)
            {
                TerraLeague.DrawCircle(Projectile.Center, 500, Color.PaleGoldenrod);
            }

            base.PostDraw(lightColor);
        }
    }
}
