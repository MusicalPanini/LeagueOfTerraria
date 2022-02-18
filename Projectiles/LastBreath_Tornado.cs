using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class LastBreath_Tornado : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tornado");
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 150;
            Projectile.timeLeft = 90;
            Projectile.penetrate = 100;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.velocity.Y = -12;
            base.OnHitPlayer(target, damage, crit);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!NPCID.Sets.ShouldBeCountedAsBoss[target.type] && target.type != NPCID.TargetDummy)
            {
                target.velocity.Y = -12;
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            if(Projectile.velocity.Length() > 0)
            {
                Projectile.velocity.X *= .98f;
                Projectile.velocity.Y *= .98f;
            }

            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, Projectile.velocity.X, Projectile.velocity.Y, 150);
                dust.velocity.Y *= 0.1f;
                dust.scale = (Projectile.Bottom.Y - dust.position.Y) / 150f;
                dust.scale += 0.25f;
                dust.velocity.X *= dust.scale;
            }
            if (Projectile.timeLeft < 30)
            {
                Projectile.alpha += 9;
            }
        }

        public override void PostDraw(Color lightColor)
        {
            float rotation = MathHelper.ToRadians(Projectile.timeLeft * 15);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            for (int i = 1; i <= 30; i++)
            {
                Main.spriteBatch.Draw
                (
                    texture,
                    new Vector2
                    (
                        Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f,
                        Projectile.position.Y - Main.screenPosition.Y + Projectile.height - (125 * (i - 1) / 30)
                    ),
                    new Rectangle(0, 0, texture.Width, texture.Height),
                    Color.LightSteelBlue * (0.7f + (0.7f *  (1 -(Projectile.alpha / 255f)))) ,
                    rotation + (MathHelper.PiOver4 * i/3f),
                    new Vector2(texture.Width, texture.Width) * 0.5f,
                    i / 20f,
                    SpriteEffects.None,
                    0f
                );
            }
        }
    }
}
