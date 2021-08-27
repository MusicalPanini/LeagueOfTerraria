using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class MagicCards_RedCard : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Red Card");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 100;
            Projectile.aiStyle = 2;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight, 0, 0, 0, new Color(255, 0, 0));
            dust.noGravity = true;
            dust.scale = 1f;
            dust.velocity *= 0.1f;
            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Prime();
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Dust dust;
            TerraLeague.DustBorderRing(Projectile.width / 2, Projectile.Center, 6, default, 4, true, true, 0.5f);

            for (int i = 0; i < 20; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 3f);
                dust.noGravity = true;
                dust.velocity *= 5f;
                dust.color = new Color(255, 0, 220);
                dust.noLight = true;

                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 2f);
                dust.velocity *= 3f;
                dust.color = new Color(255, 0, 220);
                dust.noLight = true;
            }

            Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 14, Terraria.Audio.SoundType.Sound), Projectile.position);
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 10;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();
            return false;
        }

        public void Prime()
        {
            if (Projectile.width != 200)
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.tileCollide = false;
                Projectile.alpha = 255;
                Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
                Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
                Projectile.width = 200;
                Projectile.height = 200;
                Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
                Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
                Projectile.timeLeft = 3;
            }
        }
    }
}
