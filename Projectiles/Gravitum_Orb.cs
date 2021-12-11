using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using TerraLeague.Projectiles.Explosive;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
	public class Gravitum_Orb : ExplosiveProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gravitum");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            ExplosionWidth = 128;
            ExplosionHeight = 128;

            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.alpha = 255;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
        }

        public override void AI()
        {
            AnimateProjectile();

            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position + Vector2.One * 4, Projectile.width - 8, Projectile.height- 8, 71, 0f, 0f, 100, default, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
                dust.velocity += Projectile.velocity * 0.3f;
                //dust.position.X -= Projectile.velocity.X / 10f * (float)i;
                //dust.position.Y -= Projectile.velocity.Y / 10f * (float)i;
            }

            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 15;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            if (Projectile.timeLeft < 590)
                Projectile.velocity.Y += 0.3f;
            if (Projectile.velocity.Y > 16)
                Projectile.velocity.Y = 16;

            if (Projectile.timeLeft == 3)
            {
                Prime();
            }
        }

        public override void KillEffects()
        {
            SoundEngine.PlaySound(new LegacySoundStyle(2, 14), Projectile.position);
            TerraLeague.DustBorderRing(Projectile.width / 2, Projectile.Center, 71, default, 2f);
            Dust dust;
            for (int i = 0; i < 30; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X + Projectile.width / 4, Projectile.position.Y + Projectile.width / 4), Projectile.width / 2, Projectile.height / 2, 54, 0f, 0f, 100, new Color(0, 0, 0), 3f);
                dust.noGravity = true;
                dust.velocity = (dust.position - Projectile.Center) * -0.1f;

                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X + Projectile.width / 4, Projectile.position.Y + Projectile.width / 4), Projectile.width / 2, Projectile.height / 2, 71, 0f, 0f, 100, default, 2f);
                dust.noGravity = true;
                dust.velocity *= 3f;
                dust.velocity = (dust.position - Projectile.Center) * 0.1f;
                dust.fadeIn = 2.5f;

                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 71, 0f, 0f, 100, default, 1f);
                dust.noGravity = true;
                dust.velocity = (dust.position - Projectile.Center) * -0.05f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<GravitumMark>(), 60 * 6);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frame++;
                Projectile.frame %= 4;
                Projectile.frameCounter = 0;
            }
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f,
                    Projectile.position.Y - Main.screenPosition.Y + Projectile.height - (texture.Height / 4) * 0.5f
                ),
                new Rectangle(0, (texture.Height / 4) * Projectile.frame, texture.Width, texture.Height / 4),
                Color.White,
                Projectile.rotation,
                new Vector2(texture.Width, texture.Width) * 0.5f,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}
