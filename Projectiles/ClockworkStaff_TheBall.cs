using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.CodeDom;
using System.IO;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class ClockworkStaff_TheBall : ModProjectile
    {
        Vector2 targetLocation = Vector2.Zero;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Ball");
        }

        const int IDLE = 0;
        const int ATTACKING = 1;
        const int POST_ATTACK = 2;
        const int RETURNING = 3;
        int AIState 
        { 
            get { return (int)Projectile.ai[0]; }
            set { Projectile.ai[0] = value; }
        }

        int AITimer
        {
            get { return (int)Projectile.ai[1]; }
            set { Projectile.ai[1] = value; }
        }

        Vector2 IdlePosition
        {
            get { return new Vector2(Main.player[Projectile.owner].MountedCenter.X - (32 * Main.player[Projectile.owner].direction), Main.player[Projectile.owner].MountedCenter.Y - 32); }
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

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(targetLocation);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            targetLocation = reader.ReadVector2();
            base.ReceiveExtraAI(reader);
        }

        void GetTarget()
        {
            Player player = Main.player[Projectile.owner];
            if (player.whoAmI == Main.LocalPlayer.whoAmI)
            {
                int targetNPC;
                if (AIState == IDLE)
                    targetNPC = Targeting.GetClosestNPCInLOS(Projectile, 700, -1, player.MinionAttackTargetNPC);
                else
                    targetNPC = Targeting.GetClosestNPC(Projectile.Center, 700, -1, player.MinionAttackTargetNPC);
                if (targetNPC >= 0)
                {
                    NPC npc = Main.npc[targetNPC];
                    Vector2 predictedPosition = npc.Center + (npc.velocity * npc.Center.Distance(Projectile.Center) / 10f);
                    Vector2 offset = new Vector2(64, 0).RotatedBy(Projectile.AngleTo(predictedPosition));

                    targetLocation = predictedPosition + offset;
                    Projectile.netUpdate = true;
                    AIState = ATTACKING;
                }
                else
                {
                    targetLocation = Vector2.Zero;
                    Projectile.netUpdate = true;
                    if (AIState == POST_ATTACK)
                        AIState = RETURNING;
                    else
                        AIState = IDLE;
                }

                Projectile.netUpdate = true;
            }
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
            Projectile.velocity = Vector2.Zero;
            Player player = Main.player[Projectile.owner];

            // Idle
            if (AIState == IDLE)
            {
                Projectile.Center = new Vector2(player.MountedCenter.X - (32 * player.direction), player.MountedCenter.Y - 32);
                GetTarget();
            }
            // Attacking
            else if(AIState == ATTACKING && targetLocation != Vector2.Zero)
            {
                float distance = Projectile.Distance(targetLocation);
                if (distance > 10)
                {
                    Projectile.velocity = new Vector2(10, 0).RotatedBy(Projectile.AngleTo(targetLocation));
                }
                else
                {
                    Projectile.Center = targetLocation;
                    
                    AITimer = 30;
                    AIState = POST_ATTACK;
                    Projectile.netUpdate = true;
                }
            }
            // Post Attack
            else if (AIState == POST_ATTACK)
            {
                Projectile.velocity = Vector2.Zero;
                AITimer--;
                if (AITimer <= 0)
                {
                    GetTarget();
                }
            }
            // Returning
            else if (AIState == RETURNING) 
            {
                float distance = Projectile.Distance(IdlePosition);
                if (distance > 10)
                {
                    Projectile.velocity = new Vector2(10, 0).RotatedBy(Projectile.AngleTo(IdlePosition));
                }
                else
                {
                    Projectile.Center = IdlePosition;
                    if (player.whoAmI == Main.LocalPlayer.whoAmI)
                    {
                        AIState = IDLE;
                        Projectile.netUpdate = true;
                    }
                }
            }

            if (Projectile.Distance(player.MountedCenter) > 1000)
            {
                targetLocation = Vector2.Zero;
                AIState = IDLE;
                Projectile.netUpdate = true;
            }
            else if (Projectile.Distance(player.MountedCenter) > 700 && AITimer == 0 && AIState != ATTACKING)
            {
                targetLocation = Vector2.Zero;
                AIState = RETURNING;
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

        public override bool PreDraw(ref Color lightColor)
        {
            if (AIState == ATTACKING || AIState == RETURNING)
            {
                Texture2D texture = null;
                TerraLeague.GetTextureIfNull(ref texture, "TerraLeague/Projectiles/ClockworkStaff_TheBallAttack");
                Main.spriteBatch.Draw
                (
                    texture,
                    new Vector2
                    (
                        Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f,
                        Projectile.position.Y - Main.screenPosition.Y + Projectile.height * 0.5f
                    ),
                    new Rectangle(0, 0, texture.Width, texture.Height),
                    Color.White * 0.4f,
                    (float)Main.timeForVisualEffects * 0.3f,
                    new Vector2(texture.Width, texture.Width) * 0.5f,
                    Projectile.scale * 1.5f,
                    SpriteEffects.None,
                    0f
                );
            }

            return base.PreDraw(ref lightColor);
        }

        public override bool MinionContactDamage()
        {
            return AIState == ATTACKING || AIState == RETURNING;
        }
    }
}
