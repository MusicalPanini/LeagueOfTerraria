using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Infernum_Flame : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infernum");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.timeLeft = 75;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0 && (int)Projectile.ai[1] == 1)
                Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
            Projectile.soundDelay = 100;

            if (Main.rand.Next(0, 1) == 0)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GemSapphire, 0, 0, 0, default, 2.5f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += Projectile.velocity;
                Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GemSapphire, 0, 0, 0, default, 0.75f);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);


            target.AddBuff(ModContent.BuffType<InfernumMark>(), 60 * 5);

            int critAI = crit ? 1 : 0;

            if ((int)Projectile.ai[1] == 1)
            {
                for (int i = 0; i < 16; i++)
                {
                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.TwoPi / 16 * i) * 0.75f, ModContent.ProjectileType<Infernum_FlameSpread>(), (int)(Projectile.damage * 0.75), Projectile.knockBack, Projectile.owner, target.whoAmI, critAI);
                }
            }
            else
            {
                if (crit)
                {
                    float startRad = MathHelper.ToRadians(50);
                    float rotation = startRad * 2 / 5f;

                    for (int i = 0; i < 6; i++)
                    {
                        Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(startRad - (rotation * i)) * 0.75f, ModContent.ProjectileType<Infernum_FlameSpread>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner, target.whoAmI, critAI);
                    }
                }
                else
                {
                    float startRad = MathHelper.ToRadians(20);
                    float rotation = startRad * 2 / 3f;

                    for (int i = 0; i < 4; i++)
                    {
                        Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(startRad - (rotation * i)) * 0.75f, ModContent.ProjectileType<Infernum_FlameSpread>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner, target.whoAmI);
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
    }
}
