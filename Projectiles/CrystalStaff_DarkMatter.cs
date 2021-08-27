using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class CrystalStaff_DarkMatter : ModProjectile
    {
        readonly int radius = 100;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Matter");
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.timeLeft = 100;
            Projectile.penetrate = 100;
            Projectile.friendly = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.knockBack = 0;
            Projectile.extraUpdates = 0;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;

            Projectile.alpha = 255;
        }

        public override void AI()
        {
                //if (Projectile.ai[1] == 0f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                //{
                //    Projectile.ai[1] = 1f;
                //    Projectile.netUpdate = true;
                //}
                //if (Projectile.ai[1] != 0f)
                //{
                //    Projectile.tileCollide = true;
                //}

            if (Projectile.ai[0] > 60)
            {
                Projectile.rotation = MathHelper.Pi;

                for (int i = 0; i < 2; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 113, 0, 3, 0, default, 3f);
                    dust.noGravity = true;
                    dust.noLight = true;
                    dust.velocity *= 0.3f;
                }
            }
            else
            {
                TerraLeague.DustLine(Projectile.Center, Projectile.Center - (Vector2.UnitY * 1000), Projectile.ai[0] % 2 == 0 ? 112 : 113, 0.01f, Projectile.ai[0] / 45f);

                TerraLeague.DustBorderRing(radius, Projectile.Center, Projectile.ai[0] % 2 == 0 ? 112 : 113, default, Projectile.ai[0] / 45f, true, true, 0.05f);

                Projectile.ai[0]++;

                if (Projectile.ai[0] > 60)
                {
                    Projectile.velocity.Y = 25;
                    Projectile.position.Y -= 1000;
                    Projectile.tileCollide = false;
                    Projectile.friendly = true;
                    Projectile.extraUpdates = 4;
                    Projectile.timeLeft = 1000 / 25;
                    //Projectile.ai[1] = 0f;
                }
            }

            if (Projectile.timeLeft == 1 && Projectile.ai[1] == 0)
            {
                Prime();
                Projectile.ai[1] = 1;
            }

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = Projectile.Center.X > target.Center.X ? -1 : 1;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool PreKill(int timeLeft)
        {
           
            return base.PreKill(timeLeft);
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode.WithVolume(1f), Projectile.position);

            Dust dust;
            for (int i = 0; i < 30; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, new Color(110,70,200), 2f);
                dust.velocity *= 1.4f;
            }
            for (int i = 0; i < 20; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 112, 0f, 0f, 100, default, 2f);
                dust.noGravity = true;
                dust.velocity *= 1f;

                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 113, 0f, 0f, 100, default, 3f);
                dust.noGravity = true;
                dust.velocity *= 2f;
            }

            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            
            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[0] > 60)
                Prime();
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 10;
            height = 10;
            return true;
        }

        public void Prime()
        {
            Projectile.velocity = Vector2.Zero;
            Projectile.tileCollide = false;
            Projectile.knockBack = 8;
            Projectile.alpha = 255;
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = radius * 2;
            Projectile.height = radius * 2;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.timeLeft = 3;
        }

        public override void PostDraw(Color lightColor)
        {
            if (Projectile.ai[0] <= 60)
                TerraLeague.DrawCircle(Projectile.Center, radius, new Color(100, 100, 255) * (Projectile.ai[0] / 60f));


            base.PostDraw(lightColor);
        }
    }
}
