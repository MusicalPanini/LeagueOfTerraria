using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class DarkSovereignsStaff_DarkSphere : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Sphere");
            //ProjectileID.Sets.CountsAsHoming[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1000;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 1f;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 targetPosition = GetTarget();

            if (Projectile.soundDelay == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, 32, 32, 112, 0, 0, Projectile.alpha, default, 2);
                    dust.noGravity = true;
                    dust.noLight = true;
                    dust.velocity *= 1.5f;
                }
            }
            Projectile.soundDelay = 100;

            if (Projectile.Distance(player.Center) > 1500)
            {
                Projectile.Center = player.Top;
            }
            else if (Projectile.Distance(player.Center) > 1000)
            {
                targetPosition = player.Top;
            }

            if (Projectile.localAI[0]  == 0f)
            {
                AdjustMagnitude(ref Projectile.velocity);
                Projectile.localAI[0]  = 1f;
            }

            Vector2 move = targetPosition - Projectile.Center;

            if ((int)Projectile.ai[0] != -1)
            {
                AdjustMagnitude(ref move, 12);
                Projectile.velocity = (10f * Projectile.velocity + move) / 10f;
                AdjustMagnitude(ref Projectile.velocity);
            }
            else
            {
                AdjustMagnitude(ref move, 6);
                Projectile.velocity = (10 * Projectile.velocity + move) / 10f;
                AdjustMagnitude(ref Projectile.velocity, 8);
            }

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
                        vector77 *= 0.1f;
                        Projectile.velocity -= vector77;
                        Projectile projectile2 = Main.projectile[i];
                        projectile2.velocity += vector77;
                    }
                }
            }

            if (Main.player[Projectile.owner].HasBuff(ModContent.BuffType<DarkSphere>()))
                Projectile.timeLeft = 3;
        }

        private void AdjustMagnitude(ref Vector2 vector, float num = 12)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > num)
            {
                vector *= num / magnitude;
            }
        }

        Vector2 GetTarget()
        {
            Projectile.netUpdate = true;
            float distance = 500;
            NPC target = null;
            for (int k = 0; k < 200; k++)
            {
                NPC npcCheck = Main.npc[k];
                if (npcCheck.active && !npcCheck.friendly && !npcCheck.townNPC && npcCheck.lifeMax > 5 && !npcCheck.dontTakeDamage && !npcCheck.immortal && npcCheck.CanBeChasedBy())
                {
                    Vector2 newMove = Main.npc[k].Center - Main.player[Projectile.owner].Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (npcCheck == Projectile.OwnerMinionAttackTargetNPC && distanceTo < 900)
                    {
                        target = npcCheck;
                        break;
                    }
                    else if (distanceTo < distance)
                    {
                        distance = distanceTo;
                        target = npcCheck;
                    }
                }
            }

            Projectile.ai[0] = target == null ? -1 : target.whoAmI;
            return target == null ? Main.player[Projectile.owner].Top : target.Center;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f,
                    Projectile.position.Y - Main.screenPosition.Y + Projectile.height * 0.5f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                Projectile.rotation,
                new Vector2(texture.Width, texture.Width) * 0.5f,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );
            base.PostDraw(lightColor);
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += target.checkArmorPenetration(20);

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
    }
}
