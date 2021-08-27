using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class Arrow_MagicArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Arrow");
        }

        public override void SetDefaults()
        {
            Projectile.arrow = true;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.alpha = 0;
            Projectile.timeLeft = 1200;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override void AI()
        {
            Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 113, 0f, 0f, 200, default, 0.5f);
            dust.noGravity = true;
            dust.noLight = true;
            Lighting.AddLight(Projectile.position, 0.2f, 0f, 0.5f);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.immortal)
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), target.Center, new Vector2(0, 6).RotatedByRandom(MathHelper.TwoPi), ProjectileType<Item_Echo>(), Projectile.damage/2, 0, Projectile.owner, -2, target.whoAmI);
            }

            base.OnHitNPC(target, damage, knockback, false);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Cloud, Projectile.velocity.X / 5, Projectile.velocity.Y / 5, 100, new Color(0, 0, 255), 0.7f);
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

    }
}
