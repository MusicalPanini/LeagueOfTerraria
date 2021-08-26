using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class Calibrum_Shot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("Calibrum");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.alpha = 255;
            Projectile.timeLeft = 900;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 90;
        }

        public override void AI()
        {
            if (Projectile.timeLeft < 896)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.position, 111, Vector2.Zero, 0, default, Projectile.timeLeft <= 575 ? 1.5f : 1);
                dust.noGravity = true;
                dust.alpha = 100;
            }

            if (Projectile.timeLeft == 575)
            {
                Projectile.damage *= 2;
                for (int i = 0; i < 36; i++)
                {
                    float XRad = 10;
                    float YRad = 20;
                    float rotation = Projectile.velocity.ToRotation();
                    float time = MathHelper.TwoPi * i / 36;

                    double X = XRad * System.Math.Cos(time) * System.Math.Cos(rotation) - YRad * System.Math.Sin(time) * System.Math.Sin(rotation);
                    double Y = XRad * System.Math.Cos(time) * System.Math.Sin(rotation) + YRad * System.Math.Sin(time) * System.Math.Cos(rotation);

                    Vector2 pos = new Vector2((float)X, (float)Y) + Projectile.Center;

                    Dust dust = Dust.NewDustPerfect(pos, 111, Vector2.Zero, 0, default, 1.5f);
                    dust.noGravity = true;
                }
            }

            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().SyncProjectileKill(Projectile);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.netUpdate = true;
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.ai[0] == 1)
            {
                Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 122), Projectile.Center);
                for (int i = 0; i < 8; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, 8, 8, 112, 0, 0, 0, new Color(59, 0, 255), 1f);
                }
            }

            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }
    }
}
