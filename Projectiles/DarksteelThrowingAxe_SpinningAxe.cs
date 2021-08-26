using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class DarksteelThrowingAxe_SpinningAxe : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darksteel Throwing Axe");
        }

        public override void SetDefaults()
        {
            Projectile.width = 78;
            Projectile.height = 78;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 12;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }

            if (Projectile.velocity.X < 0)
                Projectile.spriteDirection = -1;

            Projectile.rotation += 0.5f * Projectile.spriteDirection;

            Lighting.AddLight(Projectile.position, 0.75f, 0, 0);
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 211, 0, 0, 0, new Color(255, 0, 0), 1.4f);
            dust.noGravity = true;

            if (Projectile.timeLeft < 270 && Projectile.velocity.Y < 15)
                Projectile.velocity.Y += 0.8f;

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            float distance = Main.player[Projectile.owner].position.X - Projectile.position.X;
            Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.oldPosition, new Vector2((distance * 0.013f) + (Main.player[Projectile.owner].velocity.X * 0.6f), -12), ProjectileType<DarksteelThrowingAxe_CatchingAxe>(), 0, 0, Projectile.owner, Projectile.spriteDirection);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 6; i++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Iron, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f);
            }

            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10; 
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Iron, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f);
            }
            TerraLeague.PlaySoundWithPitch(Projectile.Center, 3, 4, -0.5f);
            return true;
        }
    }
}
