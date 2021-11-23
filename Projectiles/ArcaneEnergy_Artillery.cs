using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using TerraLeague.Projectiles.Explosive;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class ArcaneEnergy_Artillery : ExplosiveProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Right of the Arcane");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            ExplosionWidth = 200;
            ExplosionHeight = 200;

            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (!explosionPrimed)
            {
                if (Projectile.ai[0] == 0f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                {
                    Projectile.ai[0] = 1f;
                    Projectile.netUpdate = true;
                }
                if (Projectile.ai[0] != 0f)
                {
                    Projectile.tileCollide = true;
                }

                if (Projectile.soundDelay == 0)
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, Projectile.position);
                Projectile.soundDelay = 10;

                //if (Projectile.timeLeft < 590)
                //{
                //    Projectile.friendly = true;
                //}

                //if (Projectile.velocity.X > 12)
                //    Projectile.velocity.X = 12;
                //else if (Projectile.velocity.X < -12)
                //    Projectile.velocity.X = -12;

                //for (int i = 0; i < 2; i++)
                //{
                //    Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 113, 0, 0, 124, default(Color), 2.5f);
                //    dust2.noGravity = true;
                //    dust2.noLight = true;
                //    dust2.velocity *= 0.6f;
                //}

                for (int i = 0; i < 2; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 113, 0f, 0f, 124, default, 1f);
                    dust.noGravity = true;
                    dust.velocity *= 0.1f;
                    dust.velocity += Projectile.velocity * 0.1f;
                }

                //Projectile.velocity.Y += 0.4f;

                AnimateProjectile();
            }
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 89), Projectile.position);

            TerraLeague.DustBorderRing(ExplosionWidth / 2, Projectile.Center, 113, default, 2f);

            Dust dust;
            for (int i = 0; i < 50; i++)
            {
                dust = Dust.NewDustDirect(Projectile.position, ExplosionWidth, ExplosionHeight, 113, 0, 0, 0, default, 2f);
                dust.noGravity = true;
                dust.velocity *= 2f;

                //dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 113, 0, -4, 0, default(Color), 3f);
                //dust.velocity *= 1f;
                //dust.noGravity = true;
                //dust.color = new Color(0, 220, 220);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = null;
            TerraLeague.GetTextureIfNull(ref texture, "TerraLeague/Projectiles/ArcaneEnergy_ArtilleryEFX");
            Main.spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f,
                    Projectile.position.Y - Main.screenPosition.Y + Projectile.height * 0.5f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White * 0.75f,
                Projectile.velocity.ToRotation(),
                new Vector2(72, 17),
                1.5f,
                SpriteEffects.None,
                0f
            );
            return base.PreDraw(ref lightColor);
        }

        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 8)
            {
                Projectile.frame++;
                Projectile.frame %= 4;
                Projectile.frameCounter = 0;
            }
        }
    }
}
