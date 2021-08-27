using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class HexCoreStaff_GravityField : ModProjectile
    {
        readonly int radius = 200;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gravity Field");
        }

        public override void SetDefaults()
        {
            Projectile.width = 128;
            Projectile.height = 32;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 0;
            Projectile.timeLeft = 6 * 60;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 600;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (Projectile.timeLeft <= 360 - 60)
            {
                TerraLeague.DustBorderRing(radius, new Vector2(Projectile.Center.X - 2, Projectile.Top.Y), 21, new Color(255, 0, 255), 2, true, true, 0.05f);
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 8, Projectile.Top.Y - 8), 16, 16, 21, 0, 0, 0, new Color(255, 0, 255), 2);
                dust.velocity *= 0.1f;
                dust.noGravity = true;

                dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - radius, Projectile.Top.Y - radius), radius * 2, radius * 2, 21, 0, 0, 0, new Color(255, 0, 255), 2);
                dust.velocity = TerraLeague.CalcVelocityToPoint(dust.position, new Vector2(Projectile.Center.X - 2, Projectile.Top.Y), 10);
                dust.noGravity = true;

                Projectile.friendly = true;
                if (Projectile.timeLeft % 30 == 0)
                {
                    Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 15), Projectile.position);
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Stunned>(), Projectile.timeLeft);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 16;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Cloud, Projectile.velocity.X / 2, Projectile.velocity.Y / 2, 100, new Color(255, 192, 0), 0.5f);
            }

            base.Kill(timeLeft);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox = new Rectangle((int)Projectile.Center.X - radius, (int)Projectile.Top.Y - radius, radius*2, radius*2);

            base.ModifyDamageHitbox(ref hitbox);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.friendly && !target.townNPC)
                return Targeting.IsHitboxWithinRange(Projectile.Center, target.Hitbox, radius);
            else
                return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            if (Projectile.timeLeft <= 360 - 60)
            {
                TerraLeague.DrawCircle(new Vector2(Projectile.Center.X - 2, Projectile.Top.Y), radius, new Color(255, 0, 255));
            }

            base.PostDraw(lightColor);
        }
    }
}
