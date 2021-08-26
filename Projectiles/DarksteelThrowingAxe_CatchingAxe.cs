using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class DarksteelThrowingAxe_CatchingAxe : ModProjectile
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
            Projectile.penetrate = -1;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }

            Projectile.spriteDirection = (int)Projectile.ai[0];

            Projectile.rotation += 0.5f * (int)Projectile.ai[0];

            if (Projectile.velocity.Y < 15)
            {
                Projectile.velocity.Y += 0.3f;
            }
            Lighting.AddLight(Projectile.position, 0.75f, 0, 0);
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 211, 0, 0, 0, new Color(255, 0, 0));
            dust.noGravity = true;
            dust.scale = 1.4f;

            if (Projectile.timeLeft % 30 == 0)
            {
                Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Projectile.velocity, ProjectileType<DarksteelThrowingAxe_PathMarker>(), 0, 0, Projectile.owner);
            }

            if (new Rectangle((int)Projectile.position.X - 30, (int)Projectile.position.Y - 30, 102, 102).Intersects(Main.player[Projectile.owner].Hitbox) && Projectile.timeLeft < 270)
            {
                Main.player[Projectile.owner].AddBuff(BuffType<Buffs.SpinningAxe>(), 240);
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab, Projectile.position);
                Projectile.Kill();
            }

            base.AI();
        }


        public override void Kill(int timeLeft)
        {
            

            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 24;
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
