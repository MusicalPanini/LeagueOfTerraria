using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.NPCs;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{

    public class Item_SummonedSwordB : ModProjectile
    {
        Vector2 lastCenter = Vector2.Zero;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            DisplayName.SetDefault("Summoned Sword");
            ProjectileID.Sets.CountsAsHoming[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.alpha = 0;
            Projectile.timeLeft = 1200;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.scale = 1.5f;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            
            Projectile.netUpdate = true;

            if (Projectile.owner == Main.LocalPlayer.whoAmI)
            {
                Player player = Main.player[Projectile.owner];
                if ((int)Projectile.ai[1] != -1)
                {
                    NPC targetNPC = Main.npc[(int)Projectile.ai[1]];
                    lastCenter = targetNPC.Center;
                    Projectile.ai[1] = -1;
                }

                double angle = Projectile.ai[0];

                Projectile.rotation = (float)angle + MathHelper.ToRadians(135);

                if (Projectile.timeLeft < 1170)
                    Projectile.localAI[0]  += (1200 - Projectile.timeLeft) / 25f;

                Vector2 offset = new Vector2(200 + Projectile.localAI[0] , 0);

                Projectile.ai[0] -= .06f;
                Projectile.Center = lastCenter + offset.RotatedBy(Projectile.ai[0]);

                if (Projectile.localAI[0]  > 700)
                {
                    if (Projectile.alpha > 250)
                        Projectile.Kill();
                    Projectile.alpha += 12;
                }
            }

            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.CorruptPlants, 0f, 0f, 100, default, 1);
            dust.noGravity = true;
            Lighting.AddLight(Projectile.position, 1f, 0.5f, 0.05f);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 10;
            base.OnHitNPC(target, damage, knockback, false);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }
            return true;
        }
    }
}
