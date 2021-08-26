using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class ToxicBlowgun_NoxiousCloud : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Noxious Trap");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.minion = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
            Projectile.scale = 1.5f;
            base.SetDefaults();
        }

        public override void AI()
        {
            Projectile.tileCollide = false;
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] > 60f)
            {
                Projectile.ai[0] += 10f;
            }
            if (Projectile.ai[0] > 255f)
            {
                Projectile.Kill();
                Projectile.ai[0] = 255f;
            }
            Projectile.alpha = (int)(100.0 + (double)Projectile.ai[0] * 0.7);
            Projectile.rotation += Projectile.velocity.X * 0.1f;
            Projectile.rotation += (float)Projectile.direction * 0.003f;
            Projectile.velocity *= 0.96f;
            Rectangle projectileHitBox = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
            for (int i = 0; i < 1000; i++)
            {
                if (i != Projectile.whoAmI && Main.projectile[i].active && Main.projectile[i].type == Projectile.type)
                {
                    Rectangle targetHitBox = new Rectangle((int)Main.projectile[i].position.X, (int)Main.projectile[i].position.Y, Main.projectile[i].width, Main.projectile[i].height);
                    if (projectileHitBox.Intersects(targetHitBox))
                    {
                        Vector2 vector77 = Main.projectile[i].Center - Projectile.Center;
                        if (vector77.X == 0f && vector77.Y == 0f)
                        {
                            if (i < Projectile.whoAmI)
                            {
                                vector77.X = -1f;
                                vector77.Y = 1f;
                            }
                            else
                            {
                                vector77.X = 1f;
                                vector77.Y = -1f;
                            }
                        }
                        vector77.Normalize();
                        vector77 *= 0.005f;
                        Projectile.velocity -= vector77;
                        Projectile projectile2 = Main.projectile[i];
                        projectile2.velocity += vector77;
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Venom, 240);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
