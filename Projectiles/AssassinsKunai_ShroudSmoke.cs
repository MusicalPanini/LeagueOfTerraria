using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class AssassinsKunai_ShroudSmoke : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Twilight Shroud");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.scale = 2f;
            base.SetDefaults();
        }

        public override void AI()
        {
            Projectile.tileCollide = false;
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] > 7*60f)
            {
                Projectile.alpha += 10;
            }
            if (Projectile.alpha >= 255)
            {
                Projectile.Kill();
                Projectile.alpha = 255;
            }

            Projectile.rotation += Projectile.velocity.X * 0.1f;
            Projectile.rotation += (float)Projectile.direction * 0.003f;
            Projectile.velocity *= 0.96f;
            Rectangle projectileHitBox = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
            for (int i = 0; i < 1000; i++)
            {
                if (i != Projectile.whoAmI && Main.projectile[i].active && Main.projectile[i].type == Projectile.type)
                {
                    Rectangle targetHitBox = new Rectangle((int)Main.projectile[i].position.X, (int)Main.projectile[i].position.Y, Main.projectile[i].width, Main.projectile[i].height);
                    if (projectileHitBox.Intersects(targetHitBox))
                    {
                        Vector2 vector77 = Main.projectile[i].Center - Projectile.Center;
                        if (vector77.X == 0f && vector77.Y == 0f)
                        {
                            if (i < Projectile.whoAmI)
                            {
                                vector77.X = -1f;
                                vector77.Y = 1f;
                            }
                            else
                            {
                                vector77.X = 1f;
                                vector77.Y = -1f;
                            }
                        }
                        vector77.Normalize();
                        vector77 *= 0.005f;
                        Projectile.velocity -= vector77;
                        Projectile projectile2 = Main.projectile[i];
                        projectile2.velocity += vector77;
                    }
                }
            }

            Player player = Main.player[Projectile.owner];
            if (Projectile.Hitbox.Intersects(player.Hitbox))
            {
                player.AddBuff(BuffType<Shrouded>(), 2);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
