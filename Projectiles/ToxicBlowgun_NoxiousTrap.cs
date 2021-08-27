using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Projectiles
{
    public class ToxicBlowgun_NoxiousTrap : ModProjectile
    {
        bool grounded = false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Noxious Trap");
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 32;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = 1;
            Projectile.friendly = false;
            Projectile.minion = true;
            Projectile.scale = 1.2f;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            Projectile.rotation = 0;

            if (Projectile.alpha >= 60)
            {
                if (Targeting.IsThereAnNPCInRange(Projectile.Center, 90))
                {
                    Prime();
                }
            }

            if (!grounded)
            {
                Projectile.velocity.Y += 0.3f;
                if (Projectile.velocity.Y > 0)
                    for (int i = 0; i < Main.projectile.Length; i++)
                    {
                        if (Main.projectile[i].active)
                            if (Main.projectile[i].type == Projectile.type)
                                if (Main.projectile[i].velocity.Length() < 0.0001f)
                                    if (Main.projectile[i].Hitbox.Intersects(Projectile.Hitbox))
                                    {
                                        Projectile.velocity.Y = -6;
                                        if (Projectile.velocity.X < 3 && Projectile.velocity.X >= 0)
                                            Projectile.velocity.X = 3;
                                        if (Projectile.velocity.X > -3 && Projectile.velocity.X < 0)
                                            Projectile.velocity.X = -3;
                                    }
                    }
            }
            else
            {
                if (Projectile.alpha < 100)
                    Projectile.alpha++;

                Projectile.velocity = Vector2.Zero;
            }

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 102, -1f);

            Dust dust;
            for (int i = 0; i < 50; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f);
                dust.velocity *= 1.4f;
            }
            for (int i = 0; i < 80; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 186, 0f, 0f, 100, default, 2f);
                dust.noGravity = true;

                Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 186, 0f, 0f, 100, default, 1f);
            }

            if (Projectile.owner == Main.myPlayer)
            {
                int spawnAmount = Main.rand.Next(20, 31);
                for (int i = 0; i < spawnAmount; i++)
                {
                    Vector2 vector14 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    vector14.Normalize();
                    vector14 *= (float)Main.rand.Next(10, 201) * 0.01f;
                    Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, vector14.X, vector14.Y, ProjectileType<ToxicBlowgun_NoxiousCloud>(), Projectile.damage, 0, Projectile.owner, 0f, (float)Main.rand.Next(-45, 1));
                }
            }

            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            base.Kill(timeLeft);
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;

            width = height = 10;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X == 0)
            {
                Projectile.velocity.X = oldVelocity.X * -0.5f;

                if (Projectile.velocity.X < 3 && Projectile.velocity.X >= 0)
                    Projectile.velocity.X = 3;
                if (Projectile.velocity.X > -3 && Projectile.velocity.X < 0)
                    Projectile.velocity.X = -3;
            }
            else if (Projectile.velocity.Y == 0)
                grounded = true;

            return false;
        }

        public void Prime()
        {
            Projectile.alpha = 255;
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);

            Projectile.Kill();
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
