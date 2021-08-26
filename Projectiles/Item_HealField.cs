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
    class Item_HealField : ModProjectile
    {
        readonly int effectRadius = 16 * 20;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Item_HealField");
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.timeLeft = (150);
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
            if (Projectile.soundDelay == 0)
            {
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 8, 0);
                for (int j = 0; j < 40; j++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight, 0, -Main.rand.Next(6, 18), 0, new Color(0, 255, 100, 0), Main.rand.Next(Main.rand.Next(2, 3)));
                    dust.noGravity = true;
                }
            }
            Projectile.soundDelay = 10;

            TerraLeague.DustBorderRing((int)(effectRadius), Projectile.Center, 261, new Color(0, 255, 100), 1.5f, true, true, 0.05f);

            if (Projectile.timeLeft % 30 == 1)
            {
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 15, 0f - (0.05f * Projectile.timeLeft / 30));
                //TerraLeague.DustElipce(rad, rad, 0, Projectile.Center, 261, new Color(0, 255, 100), 1.5f, 180, true);
            }
        }

        public override void Kill(int timeLeft)
        {
            TerraLeague.DustElipce(2, 2, 0, Projectile.Center, 261, new Color(0, 255, 100), 1f, 180, true, 30);
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 29), Projectile.Center);

            Player player = Main.player[Projectile.owner];
            if (Projectile.owner == Main.LocalPlayer.whoAmI)
            {
                var players = Targeting.GetAllPlayersInRange(Projectile.Center, effectRadius, -1, player.team);

                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i] == Projectile.owner)
                        player.GetModPlayer<PLAYERGLOBAL>().lifeToHeal += Projectile.damage;
                    else
                        player.GetModPlayer<PLAYERGLOBAL>().SendHealPacket(Projectile.damage, players[i], -1, player.whoAmI);
                }

                var npcs = Targeting.GetAllNPCsInRange(Projectile.Center, effectRadius, true, true);
                for (int i = 0; i < npcs.Count; i++)
                {
                    player.ApplyDamageToNPC(Main.npc[npcs[i]], Projectile.damage * 2, 0, 0, false);
                }
            }

            base.Kill(timeLeft);
        }

        public override void PostDraw(Color lightColor)
        {
            TerraLeague.DrawCircle(Projectile.Center, effectRadius, new Color(0, 255, 100));
            TerraLeague.DrawCircle(Projectile.Center, effectRadius - (effectRadius * Projectile.timeLeft / 150f), new Color(0, 255, 100));

            int alpha = 255;
            if (Projectile.timeLeft % 30 < 15)
            {
                alpha = (int)(255 * (Projectile.timeLeft % 30) / 15f);
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
        }
    }
}
