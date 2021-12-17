using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class XerSaiBrute_Spit : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plasma Acid");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 185;
            Projectile.hostile = true;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 95), Projectile.Center);
            }
            Projectile.soundDelay = 100;

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            if (Projectile.timeLeft < 185 - 20)
            {
                Projectile.velocity.Y += 0.4f;
            }

            if (Projectile.velocity.Y > 16)
                Projectile.velocity.Y = 16;

            Lighting.AddLight(Projectile.position, 0.75f, 0f, 0.75f);
            for (int i = 0; i < 3; i++)
            {
                Vector2 dustBoxPosition = new Vector2(Projectile.position.X + 0, Projectile.position.Y + 0);
                int dustBoxWidth = Projectile.width - 8;
                int dustBoxHeight = Projectile.height - 8;
                Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 112, 0f, 0f, 255, new Color(59, 0, 255), 1f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.position.X -= Projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= Projectile.velocity.Y / 3f * (float)i;
            }

            base.AI();
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            target.GetModPlayer<PLAYERGLOBAL>().AddVoidInfluence(50, false);
            target.AddBuff(BuffID.Weak, 60 * 10);

            base.ModifyHitPlayer(target, ref damage, ref crit);
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 10;
            return true;
        }
    }
}
