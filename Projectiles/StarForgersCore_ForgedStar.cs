using Microsoft.Xna.Framework;
using System.Linq;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Projectiles
{
	public class StarForgersCore_ForgedStar : ModProjectile
	{
        Vector2 offset = new Vector2(0, 0);
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forged Star");
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.alpha = 255;
            Projectile.timeLeft = 3;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 1;
            Projectile.netImportant = true;
            Projectile.netUpdate = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 3)
            {
                if ((int)Projectile.ai[0] != 1)
                {
                    Projectile.ai[1] = Main.projectile.Where(x => (int)x.ai[0] == 1 && x.owner == Projectile.owner).First().identity;
                }
                else
                {
                    Projectile.ai[1] = 0;
                }
            }

            Player player = Main.player[Projectile.owner];
            if (player.HasBuff(BuffType<CenterOfTheUniverse>()))
            {
                Projectile.timeLeft = 2;
            }

            if (player.HasBuff(BuffType<CelestialExpansion>()) && offset.X < 300)
            {
                offset.X += 5;
            }
            else if (!player.HasBuff(BuffType<CelestialExpansion>()) && offset.X > 150)
            {
                offset.X -= 5;
            }
            else if (!player.HasBuff(BuffType<CelestialExpansion>()) && offset.X < 150)
            {
                offset.X += 5;
            }
            Projectile.width = (int)((4 / 75f) * offset.X) + 8;
            Projectile.height = Projectile.width;

            if ((int)Projectile.ai[0] != 1)
            {
                float angle = Main.projectile[Projectile.GetByUUID(Projectile.owner, Projectile.ai[1])].ai[1];
                int numOfProj = Main.player[Projectile.owner].ownedProjectileCounts[ProjectileType<StarForgersCore_ForgedStar>()];
                Projectile.Center = player.MountedCenter + offset.RotatedBy(angle + ((MathHelper.TwoPi * ((int)Projectile.ai[0] - 1)) /numOfProj));
            }
            else
            {
                Projectile.ai[1] += .035f;
                Projectile.Center = player.MountedCenter + offset.RotatedBy(Projectile.ai[1]);
            }

            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 111, Projectile.velocity.X, Projectile.velocity.Y, 200, default, 1.5f * (offset.X / 300f + 0.5f));
            dust.noGravity = true;
            dust.noLight = true;
            dust.velocity *= 0.1f;

            for (int i = 0; i < 2; i++)
            {
                Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 162, Projectile.velocity.X, Projectile.velocity.Y, 124, default, 2.5f * (offset.X / 300f + 0.5f));
                dust2.noGravity = true;
                dust2.noLight = true;
                dust2.velocity *= 0.6f;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.AddBuff(Terraria.ID.BuffID.Daybreak, 120);

            int count = Main.player[Projectile.owner].ownedProjectileCounts[Projectile.type];
            damage = (int)(damage * (1 + ((count - 1) * 0.1)));

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
            for (int j = 0; j < 10; j++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 111, Projectile.velocity.X, Projectile.velocity.Y, 200, default, 1.5f * (offset.X / 300f + 0.5f));
                dust.noGravity = true;
                dust.noLight = true;
                dust.velocity *= 0.1f;

                for (int i = 0; i < 2; i++)
                {
                    Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 162, Projectile.velocity.X, Projectile.velocity.Y, 124, default, 2.5f * (offset.X / 300f + 0.5f));
                    dust2.noGravity = true;
                    dust2.noLight = true;
                    dust2.velocity *= 0.6f;
                }
            }

            if ((int)Projectile.ai[0] == 1)
            {
                for (int i = 0; i < 200; i++)
                {
                    if (Main.projectile[i].type == Projectile.type && Main.projectile[i].owner == Projectile.owner && Projectile.whoAmI != i)
                    {
                        Main.projectile[i].Kill();
                    }
                }
            }
        }
    }
}
