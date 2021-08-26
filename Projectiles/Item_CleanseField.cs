using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Terraria.GameContent;

namespace TerraLeague.Projectiles
{
    class Item_CleanseField : ModProjectile
    {
        readonly int effectRadius = 16 * 16;
        readonly int nodeFrames = 20;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cleanse Field");
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.timeLeft = (60 * 10) + 2;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            if (Projectile.ai[0] < nodeFrames)
                Projectile.ai[0]++;

            if (Projectile.soundDelay == 0)
            {
                Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 29).WithPitchVariance(-0.3f), Projectile.Center);
                for (int j = 0; j < 40; j++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight, 0, -Main.rand.Next(6, 18), 0, new Color(0, 255, 255, 0), Main.rand.Next(Main.rand.Next(2, 3)));
                    dust.noGravity = true;
                }
            }
            Projectile.soundDelay = 10;

            TerraLeague.DustBorderRing((int)(effectRadius * Projectile.ai[0] / nodeFrames), Projectile.Center, 261, new Color(0, 255, 255), 1, true, true, 0.05f);

            if (Projectile.timeLeft % 120 == 1)
            {
                float rad = 2;

                TerraLeague.DustElipce(rad, rad / 4f, 0, Projectile.Center, 111, new Color(0, 255, 255), 1.5f, 180, true, 10);
                TerraLeague.DustElipce(rad / 4f, rad, 0, Projectile.Center, 111, new Color(0, 255, 255), 1.5f, 180, true, 10);
            }

            if (Projectile.timeLeft % 15 == 0)
            {
                List<int> playersInRange = Targeting.GetAllPlayersInRange(Projectile.Center, effectRadius * Projectile.ai[0] / nodeFrames, -1, Main.player[Projectile.owner].team);

                for (int i = 0; i < playersInRange.Count; i++)
                {
                    Main.player[i].AddBuff(BuffType<GeneralCleanse>(), 20);
                }
            }

        }

        public override void PostDraw(Color lightColor)
        {
            TerraLeague.DrawCircle(Projectile.Center, effectRadius, new Color(0, 255, 255));

            int alpha = 255;
            if (Projectile.timeLeft % 120 < 15)
            {
                alpha = (int)(255 * (Projectile.timeLeft % 120) / 15f);
            }
            Color color = new Color(255, 255, 255, alpha);



            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f,
                    (Projectile.position.Y - Main.screenPosition.Y + Projectile.height * 0.5f) + (float)System.Math.Sin(Main.time * 0.1) * 3
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                color,
                Projectile.rotation,
                new Vector2(texture.Width, texture.Width) * 0.5f,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );

            Vector2 nodeOffset = new Vector2(-effectRadius * Projectile.ai[0] / nodeFrames, 0).RotatedBy(MathHelper.PiOver4 + MathHelper.ToRadians(Projectile.timeLeft));

            texture = Request<Texture2D>("TerraLeague/Projectiles/Item_CleanseField_Node").Value;
            Main.spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f + nodeOffset.X,
                    Projectile.position.Y - Main.screenPosition.Y + Projectile.height * 0.5f + nodeOffset.Y
                ),
                new Rectangle(0, 0, 18, 18),
                color,
                Projectile.rotation + MathHelper.ToRadians(Projectile.timeLeft),
                new Vector2(18, 18) * 0.5f,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );

            nodeOffset = nodeOffset.RotatedBy(MathHelper.PiOver2);
            Main.spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f + nodeOffset.X,
                    Projectile.position.Y - Main.screenPosition.Y + Projectile.height * 0.5f + nodeOffset.Y
                ),
                new Rectangle(0, 0, 18, 18),
                color,
                Projectile.rotation + MathHelper.ToRadians(Projectile.timeLeft),
                new Vector2(18, 18) * 0.5f,
                Projectile.scale,
                SpriteEffects.FlipHorizontally,
                0f
            );

            nodeOffset = nodeOffset.RotatedBy(MathHelper.PiOver2);
            Main.spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f + nodeOffset.X,
                    Projectile.position.Y - Main.screenPosition.Y + Projectile.height * 0.5f + nodeOffset.Y
                ),
                new Rectangle(0, 0, 18, 18),
                color,
                Projectile.rotation + MathHelper.ToRadians(Projectile.timeLeft),
                new Vector2(18, 18) * 0.5f,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );

            nodeOffset = nodeOffset.RotatedBy(MathHelper.PiOver2);
            Main.spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f + nodeOffset.X,
                    Projectile.position.Y - Main.screenPosition.Y + Projectile.height * 0.5f + nodeOffset.Y
                ),
                new Rectangle(0, 0, 18, 18),
                color,
                Projectile.rotation + MathHelper.ToRadians(Projectile.timeLeft),
                new Vector2(18, 18) * 0.5f,
                Projectile.scale,
                SpriteEffects.FlipHorizontally,
                0f
            );
        }
    }
}
