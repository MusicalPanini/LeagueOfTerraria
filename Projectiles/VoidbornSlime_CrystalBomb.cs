using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class VoidbornSlime_CrystalBomb : Explosive.ExplosiveProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystaline Void Bomb");
        }

        public override void SetDefaults()
        {
            ExplosionWidth = 512;
            ExplosionHeight = 512;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.timeLeft = 100;
            Projectile.aiStyle = -1;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            //Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 1)
            {
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 75, Projectile.localAI[0]);
                Projectile.localAI[0] += 0.15f;
                Projectile.localAI[1] += 3;
            }
            else if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 23 - (int)Projectile.localAI[1];
            }

            //Projectile.velocity.X *= 0.97f;

            Projectile.velocity.Y += 0.4f;
            if (Projectile.velocity.Y > 16)
                Projectile.velocity.Y = 16;

            //Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.03f * (float)Projectile.direction;

            base.AI();
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            if (Main.expertMode)
                damage /= 4;

            target.GetModPlayer<PLAYERGLOBAL>().AddVoidInfluence(200, false);
            base.ModifyHitPlayer(target, ref damage, ref crit);
        }

        public override void Kill(int timeLeft)
        {
            
            base.Kill(timeLeft);
        }

        public override void PrePrime()
        {
            Projectile.hostile = true;
            Projectile.friendly = true;
            base.PrePrime();
        }

        public override void KillEffects()
        {
            TerraLeague.DustElipce(16, 16, 0, Projectile.Center, 112, default, 2, 36 * 4, true, 1.5f);
            TerraLeague.DustElipce(256, 256, 0, Projectile.Center, 112, default, 3, 36 * 4, true);
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position + new Vector2(15, 15), 256 - 30, 256 - 30, 112, 0, 0, 0, default, Main.rand.NextFloat(0.75f, 2));
                dust.velocity *= Main.rand.NextFloat(4, 7);
                dust.noGravity = true;
            }

            SoundEngine.PlaySound(new LegacySoundStyle(2, 14), Projectile.position);
            TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 91, 0.5f);

            //TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 45, 0.25f);
            base.KillEffects();
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;

            width = height = 28;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            float pulse = 23 - (int)Projectile.localAI[1];

            Color color = Color.White * ( ((Projectile.soundDelay % pulse) / pulse));

            Main.spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f,
                    Projectile.position.Y - Main.screenPosition.Y + Projectile.height * 0.5f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                color,
                Projectile.rotation,
                new Vector2(texture.Width, texture.Width) * 0.5f,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );

            base.PostDraw(lightColor);
        }

        public override bool? CanCutTiles()
        {
            return true;
        }
    }
}
