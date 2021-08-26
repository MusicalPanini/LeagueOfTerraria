using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Crescendum_Sentry : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crescendum Sentry");
        }

        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 64;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            if (Main.player[Projectile.owner].ownedProjectileCounts[Projectile.type] != 0)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].owner == Projectile.owner && Main.projectile[i].type == Projectile.type)
                    {
                        if (Main.projectile[i].timeLeft > Projectile.timeLeft)
                        {
                            Projectile.Kill();
                        }
                    }
                }
            }

            if ((int)Projectile.ai[0] == -1)
            {
                Projectile.ai[0] = Targeting.GetClosestNPC(Projectile.Center, 700, Projectile.position, 24, 24, -1, Main.player[Projectile.owner].MinionAttackTargetNPC);
            }

            if ((int)Projectile.ai[0] != -1)
            {
                NPC npc = Main.npc[(int)Projectile.ai[0]];

                if (!npc.active || !Collision.CanHitLine(Projectile.position, 24, 24, npc.position, npc.width, npc.height))
                {
                    Projectile.ai[0] = -1;
                    return;
                }

                Player player = Main.player[Projectile.owner];

                if (Projectile.ai[1] > 5 && player.ownedProjectileCounts[ModContent.ProjectileType<Crescendum_SentryProj>()] <= player.maxMinions + 5)
                {
                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, (Projectile.Center - npc.Center).SafeNormalize(-Vector2.UnitY) * -16, ModContent.ProjectileType<Crescendum_SentryProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    Projectile.ai[1] = 0;
                }
                else
                {
                    Projectile.ai[1]++;
                }

            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }
    }
}
