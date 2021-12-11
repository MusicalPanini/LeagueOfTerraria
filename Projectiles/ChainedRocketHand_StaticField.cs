using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class ChainedRocketHand_StaticField : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Static Field");
        }

        public override void SetDefaults()
        {
            Projectile.width = 700;
            Projectile.height = 700;
            Projectile.timeLeft = 2;
            Projectile.penetrate = 100;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.velocity = Vector2.Zero;
            Projectile.alpha = 255;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Confused, 300);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool PreKill(int timeLeft)
        {
            return base.PreKill(timeLeft);
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(3, 53), Projectile.position);
            for (int i = 0; i < 50; i++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 225, Projectile.Center.Y - 225), 450, 450, 226, 0, 0, 50, new Color(0, 255, 255), 1f);
                dust.velocity *= 5f;
                dust.noGravity = true;
                dust.noLight = true;
                dust.fadeIn = 1;

                dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 225, Projectile.Center.Y - 225), 450, 450, 226, 0, 0, 50, new Color(0, 255, 255), 1);
                dust.velocity *= 5f;
            }

            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 18; i++)
                {
                    Vector2 pos = new Vector2(350, 0).RotatedBy(MathHelper.ToRadians((20 * i) + (j * 6))) + Projectile.Center;

                    Dust dustR = Dust.NewDustPerfect(pos, 261, Vector2.Zero, 0, new Color(0, 255, 255), 1);
                    dustR.noGravity = true;
                    dustR.fadeIn = 1.5f;
                }
            }

            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Targeting.IsHitboxWithinRange(Projectile.Center, targetHitbox, Projectile.width / 2);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
