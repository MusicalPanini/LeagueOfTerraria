using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class BoneSkewer_DeathFromBelow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Death From Below");
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.alpha = 255;
            Projectile.timeLeft = 100;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if ((int)Projectile.ai[1] != 1)
            {
                Projectile.rotation = -MathHelper.PiOver4 * (int)Projectile.ai[0];
                if ((int)Projectile.ai[0] == 1)
                {
                    Projectile.spriteDirection = -1;
                }

                if (Projectile.alpha < 85)
                {
                    Projectile.alpha = 85;
                    Projectile.friendly = true;
                    Projectile.timeLeft = 54;
                    Projectile.velocity = new Vector2(-10 * (int)Projectile.ai[0], -10);
                    Projectile.extraUpdates = 7;
                    Projectile.ai[1] = 1;
                    TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 71, -0.5f);
                }
                else
                {
                    Projectile.alpha -= 10;
                }

            }

            if (Projectile.timeLeft <= 30 && (int)Projectile.ai[1] == 1)
            {
                Projectile.velocity *= 0;
                Projectile.extraUpdates = 0;
                Projectile.friendly = false;
                Projectile.alpha += 255 / 20;
            }

            if (Main.rand.Next(0, 3) == 0)
            {
                Dust dustIndex = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 202, 0f, 0f, 200, default, 2f);
                dustIndex.noGravity = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 27), Projectile.position);

            return true;
        }

        public override void Kill(int timeLeft)
        {
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.life <= 0)
            {
                Main.player[Projectile.owner].AddBuff(BuffType<DeathFromBelowRefresh>(), 600);
            }

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            int alpha = 255 - Projectile.alpha;
            return new Color(alpha, alpha, alpha, alpha);
        }
    }
}
