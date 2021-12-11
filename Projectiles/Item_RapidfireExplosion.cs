using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using TerraLeague.Projectiles.Explosive;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Item_RapidfireExplosion : ExplosiveProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rapidfire Cannon");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.timeLeft = 2;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 1; 

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = Projectile.Center.X > target.Center.X ? -1 : 1;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void KillEffects()
        {
            SoundEngine.PlaySound(new LegacySoundStyle(2, 14), Projectile.position);

            Dust dust;
            for (int i = 0; i < 15; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f);
                dust.velocity *= 1.4f;
            }
            for (int i = 0; i < 10; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 5f;

                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 1f);
                dust.velocity *= 3f;
            }
            for (int g = 0; g < 3; g++)
            {
                Gore.NewGore(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default, Main.rand.Next(61, 64), 1f);
            }
        }

        public override void PrePrime()
        {
            ExplosionWidth = (int)Projectile.ai[0] == 0 ? 90 : 135;
            ExplosionHeight = ExplosionWidth;

            base.PrePrime();
        }
    }
}
