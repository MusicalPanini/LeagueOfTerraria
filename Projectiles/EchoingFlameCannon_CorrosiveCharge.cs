using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class EchoingFlameCannon_CorrosiveCharge : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corrosive Charge");
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            base.SetDefaults();
        }
        public override void AI()
        {
            //Lighting.AddLight(Projectile.position, 0f, 0f, 0.5f);
            //Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 67, 0f, 0f, 100, new Color(0, 255, 0));
            //dust.noLight = true;
            //dust.alpha = 0;
            //dust.noLight = false;
            //dust.noGravity = true;
            //dust.scale = 1.4f;


            if ((int)Projectile.ai[0] == 0)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + (MathHelper.PiOver2 * 3);
                Projectile.velocity.Y += 0.4f;
                if (Projectile.velocity.Y > 16)
                    Projectile.velocity.Y = 16;
            }
            base.AI();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if ((int)Projectile.ai[0] == 0)
            {
                Projectile.timeLeft = 30;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            }
            Projectile.velocity *= 0;

            Projectile.ai[0] = 1;
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Shatter, Projectile.Center);
            Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 14), Projectile.Center);
            if (Projectile.owner == Main.myPlayer)
            {
                int spawnAmount = Main.rand.Next(20, 31);
                for (int i = 0; i < spawnAmount; i++)
                {
                    Vector2 vector14 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    vector14.Normalize();
                    vector14 *= (float)Main.rand.Next(10, 201) * 0.01f;
                    Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, vector14.X, vector14.Y, ProjectileType<EchoingFlameCannon_CorrosiveCloud>(), Projectile.damage, 1f, Projectile.owner, 0f, (float)Main.rand.Next(-45, 1));
                }
            }

            for (int i = 0; i < 40; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.CursedTorch, 0, 0, 0, default, 0.5f);
                dust.noLight = true;
                dust.velocity *= 3;
                dust.velocity.X *= 2;
                dust.fadeIn = 1;
            }
           

            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10; 
            return true;
        }
    }
}
