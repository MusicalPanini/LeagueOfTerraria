using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class HeartoftheTempest_Yoyo : ModProjectile
    {
        int hits = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 10f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 450f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 13f;
            DisplayName.SetDefault("Heart of the Tempest");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 99;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 10f;

            if (Main.rand.NextBool())
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0, 0, 0, new Color(0, 255, 255), 0.5f);
                dust.noGravity = true;
            }


            Lighting.AddLight(Projectile.Center, 0f, 0.3f, 0.75f);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.owner == Main.LocalPlayer.whoAmI)
            {
                if (hits < 3)
                {
                    hits++;

                    if (hits >= 3)
                    {
                        Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<HeartoftheTempest_SlicingMaelstrom>(), damage, 0, Projectile.owner, Projectile.whoAmI);
                    }
                }
                else if (Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<HeartoftheTempest_SlicingMaelstrom>()] == 0 && hits >= 3)
                {
                    hits = 1;
                }
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }
    }
}
