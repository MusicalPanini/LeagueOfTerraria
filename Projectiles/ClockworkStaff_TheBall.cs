using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.CodeDom;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class ClockworkStaff_TheBall : ModProjectile
    {
        bool attacking = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Ball");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 0;
            Projectile.timeLeft = 2000;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 3;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Sandstorm, 0, 0, 0, default, 2);
                    dust.noGravity = true;
                }
            }
            Projectile.soundDelay = 100;

            Player player = Main.player[Projectile.owner];

            if (player.whoAmI == Main.LocalPlayer.whoAmI && (int)Projectile.localAI[0]  > 0)
            {
                Projectile.localAI[0] --;
            }
            if (!attacking && player.whoAmI == Main.LocalPlayer.whoAmI && (int)Projectile.localAI[0]  == 0)
            {
                Projectile.ai[0] = (int)player.MountedCenter.X - (32 * player.direction);
                Projectile.ai[1] = (int)player.MountedCenter.Y - 32;

                float distance = 700;

                if (player.MinionAttackTargetNPC != -1)
                {
                    NPC npc = Main.npc[player.MinionAttackTargetNPC];
                    if (npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage && !npc.immortal && npc.whoAmI != (int)Projectile.ai[0])
                    {
                        float distanceTo = Projectile.Distance(npc.Center);
                        if (distanceTo < distance)
                        {
                            distance = distanceTo;
                            Vector2 predictedPosition = npc.Center + (npc.velocity * distance / 10f);
                            Vector2 offset = new Vector2(64, 0).RotatedBy(Projectile.AngleTo(predictedPosition));
                            Projectile.ai[0] = predictedPosition.X + offset.X;
                            Projectile.ai[1] = predictedPosition.Y + offset.Y;
                            attacking = true;
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < 200; k++)
                    {
                        NPC npc = Main.npc[k];

                        if (player.Distance(npc.Center) <= 1000 && npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage && !npc.immortal && npc.whoAmI != (int)Projectile.ai[0] && k != (int)Projectile.localAI[1] - 1)
                        {
                            float distanceTo = Projectile.Distance(npc.Center);
                            if (distanceTo < distance && Collision.CanHit(player.position, Projectile.width * 2, Projectile.height * 2, npc.position, npc.width, npc.height))
                            {
                                distance = distanceTo;
                                Vector2 predictedPosition = npc.Center + (npc.velocity * distance / 10f);
                                Vector2 offset = new Vector2(64, 0).RotatedBy(Projectile.AngleTo(predictedPosition));
                                Projectile.ai[0] = predictedPosition.X + offset.X;
                                Projectile.ai[1] = predictedPosition.Y + offset.Y;
                                attacking = true;
                            }
                        }
                    }
                }

                Projectile.netUpdate = true;
            }

            float speed = Projectile.Distance(new Vector2(Projectile.ai[0], Projectile.ai[1]));
            if (speed > 10)
                speed = 10;
            if (speed < 0.5f)
            {
                if (player.whoAmI == Main.LocalPlayer.whoAmI)
                {
                    if (Projectile.Colliding(Projectile.Hitbox, new Rectangle((int)Projectile.ai[0], (int)Projectile.ai[1], 1, 1)) && Projectile.localAI[0]  == 0)
                    {
                        Projectile.localAI[0]  = 20;

                        if (!attacking)
                        {
                            Projectile.localAI[1] = 0;
                        }
                    }
                    if (attacking)
                    {
                        attacking = false;
                    }
                }
                Projectile.velocity = Vector2.Zero;

            }
            else
            {
                if (!Projectile.Colliding(Projectile.Hitbox, new Rectangle((int)player.MountedCenter.X, (int)player.MountedCenter.Y - 32, 1, 1)) && attacking )
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(Projectile.ai[0], Projectile.ai[1]), 1, 1, DustID.Sand, 0, 0, 0, default, 2);
                    dust.noGravity = true;
                    dust.velocity *= 0;
                }
                Projectile.velocity = TerraLeague.CalcVelocityToPoint(Projectile.Center, new Vector2(Projectile.ai[0], Projectile.ai[1]), speed);
            }

            if (Projectile.Distance(player.MountedCenter) > 1000)
            {
                Projectile.Center = new Vector2(player.MountedCenter.X - (32 * player.direction), player.MountedCenter.Y - 32);
                Projectile.netUpdate = true;
            }
            else if (Projectile.Distance(player.MountedCenter) > 700)
            {
                Projectile.ai[0] = (int)player.MountedCenter.X - (32 * player.direction);
                Projectile.ai[1] = (int)player.MountedCenter.Y - 32;
                Projectile.netUpdate = true;
            }
            
            if (player.HasBuff(ModContent.BuffType<TheBall>()))
                Projectile.timeLeft = 5;

            Lighting.AddLight(Projectile.Center, 0, 0.5f, 0.75f);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.whoAmI != (int)Projectile.localAI[1] - 1)
            {
                //Projectile.localAI[1] = target.whoAmI + 1;
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.velocity.Length() >= 10)
                return base.CanHitNPC(target);
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            //damage *= (int)Projectile.minionSlots;
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
    }
}
