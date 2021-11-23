using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using TerraLeague.Projectiles.Explosive;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace TerraLeague.Projectiles
{
    public class ArcaneEnergy_ShockingOrb : ExplosiveProjectile
    {
        int timeAlive { get { return 90 - Projectile.timeLeft; } }
        float currentScale { get { return 0.25f + (timeAlive / 120f); } }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shocking Orb");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 52;
            Projectile.alpha = 255;
            Projectile.timeLeft = 90;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.scale = 1;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void PrePrime()
        {
            ExplosionWidth = 50 + (int)(250 * Projectile.ai[0]);
            ExplosionHeight = 50 + (int)(250 * Projectile.ai[0]);

            base.PrePrime();
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
                Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            Projectile.soundDelay = 10;
            if (!explosionPrimed)
            {
                Projectile.rotation += 0.05f * (Projectile.velocity.X > 0 ? 1 : -1);
                Projectile.ai[0] = 1 - Projectile.timeLeft / 90f;

                float lenght = Projectile.width * currentScale;
                Dust dust;
                for (int i = 0; i < 2; i++)
                {
                    dust = Dust.NewDustDirect(new Vector2((Projectile.Center.X - (lenght / 2f)), Projectile.Center.Y - (lenght / 2f)), (int)lenght, (int)lenght, 113, 0, 0, 0, default, 2.5f * currentScale);
                    dust.velocity *= 0.1f;
                    dust.noGravity = true;

                }
                if (Main.rand.NextBool(3))
                {
                    dust = Dust.NewDustDirect(new Vector2((Projectile.Center.X - (lenght / 2f)), Projectile.Center.Y - (lenght / 2f)), (int)lenght, (int)lenght, 226, 0, 0, 0, default, 0.25f + currentScale);
                    dust.velocity *= 3;
                    dust.noLight = true;
                }
            }

            AnimateProjectile();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Stunned>(), 60 + (int)(180 * Projectile.ai[0] ));
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void KillEffects()
        {
            Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 53), Projectile.position);
            TerraLeague.DustBorderRing(ExplosionWidth / 2, Projectile.Center, 113, default, 2f, true, true, 1);

            Dust dust;
            for (int i = 0; i < 60 * Projectile.ai[0]; i++)
            {
                Vector2 position = Projectile.Center - new Vector2(ExplosionWidth / 2, ExplosionHeight / 2);

                Vector2 dustBoxPosition = new Vector2(position.X + (ExplosionWidth / 6), position.Y + (ExplosionHeight / 6));
                int dustBoxWidth = ExplosionWidth - (ExplosionWidth / 3);
                int dustBoxHeight = ExplosionHeight - (ExplosionHeight / 3);

                dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 113, 0, 0, 0, default, 2f);
                dust.noGravity = true;
                dust.velocity = TerraLeague.CalcVelocityToPoint(Projectile.Center, dust.position, 10 * Vector2.Distance(dust.position, Projectile.Center) / (ExplosionWidth / 2));

                dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 113, 0, 0, 0, default, 3f);
                dust.velocity *= 1f;
                dust.noGravity = true;
                dust.color = new Color(0, 220, 220);
            }

            base.KillEffects();
        }

        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frame++;
                Projectile.frame %= 4;
                Projectile.frameCounter = 0;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (!explosionPrimed)
            {
                float lenght = Projectile.width * currentScale;
                Rectangle rectangle = new Rectangle((int)(Projectile.Center.X - (lenght / 2f)), (int)(Projectile.Center.Y - (lenght / 2f)), (int)lenght, (int)lenght);
                return rectangle.Intersects(targetHitbox);
            }
            else
            {
                return Targeting.IsHitboxWithinRange(Projectile.Center, targetHitbox, Projectile.width/2f);
            }
        }

        public override void PostDraw(Color lightColor)
        {
            float scale = currentScale;
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
                scale,
                SpriteEffects.None,
                0f
            );
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = (int)(Projectile.width * currentScale - 4);
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
    }
}