using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class FishBones_Rocket : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fish Bones");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 100;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.knockBack = 2;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.position, 1f, 0.34f, 0.9f);
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.velocity.X < 0)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.scale = -1f;
                Projectile.spriteDirection = -1;
            }

            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.FireworksRGB, 0,0,0,default, 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }
            base.AI();
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = Projectile.Center.X > target.Center.X ? -1 : 1;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.life <= 0)
            {
                Main.player[Projectile.owner].AddBuff(BuffType<PowPowExcited>(), 300);
            }

            Prime();
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool PreKill(int timeLeft)
        {
           
            return base.PreKill(timeLeft);
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode.WithVolume(1f), Projectile.position);

            Dust dust;
            for (int i = 0; i < 50; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 2f);
                dust.velocity *= 1.4f;
            }
            for (int i = 0; i < 80; i++)
            {
                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight, 0, 0, 0, new Color(255, 255, 0, 150), 2f);
                dust.noGravity = true;
                dust.velocity *= 5f;
                dust.color = new Color(255,0,220);

                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 2f);
                dust.velocity *= 3f;
                dust.color = new Color(255, 0, 220);
            }
            for (int g = 0; g < 2; g++)
            {
                Gore gore;
                gore = Gore.NewGoreDirect(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default, Main.rand.Next(61, 64), 1f);
                gore.scale = 1.5f;
                gore.velocity.X = gore.velocity.X + 1.5f;
                gore.velocity.Y = gore.velocity.Y + 1.5f;
                gore = Gore.NewGoreDirect(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default, Main.rand.Next(61, 64), 1f);
                gore.scale = 1.5f;
                gore.velocity.X = gore.velocity.X - 1.5f;
                gore.velocity.Y = gore.velocity.Y + 1.5f;
                gore = Gore.NewGoreDirect(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default, Main.rand.Next(61, 64), 1f);
                gore.scale = 1.5f;
                gore.velocity.X = gore.velocity.X + 1.5f;
                gore.velocity.Y = gore.velocity.Y - 1.5f;
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
            Prime();
            return false;
        }

        public void Prime()
        {
            Projectile.tileCollide = false;
            Projectile.velocity = Vector2.Zero;
            Projectile.alpha = 255;
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 115;
            Projectile.height = 115;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.timeLeft = 3;
        }
    }
}
