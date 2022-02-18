using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class MagicCards_YellowCard : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yellow Card");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 2;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight, 0, 0, 0, new Color(255, 255, 0));
            dust.noGravity = true;
            dust.scale = 1f;
            dust.velocity *= 0.1f;

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Stunned>(), 120);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 37, 0.5f);

            for (int i = 0; i < 12; i++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.AncientLight, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 0, new Color(255, 255, 0), 0.5f);
            }
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 10; 
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            return true;
        }
    }
}
