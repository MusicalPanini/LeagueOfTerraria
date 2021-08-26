using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class RadiantStaff_PrismaticBarrier : ModProjectile
    {
        readonly List<int> hitPlayers = new List<int>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Radiant Staff");
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.scale = 1f;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 240;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.rotation += -0.3f;

            for (int i = 0; i < 3; i++)
            {
                Vector2 dustBoxPosition = new Vector2(Projectile.position.X, Projectile.position.Y);
                int dustBoxWidth = Projectile.width;
                int dustBoxHeight = Projectile.height;
                Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, DustID.GoldFlame, 0f, 0f, 100, new Color(229, 242, 249), 1.5f);
                dust.noGravity = true;
                dust.noLight = true;
                dust.velocity *= 0.1f;
                dust.velocity += Projectile.velocity * 0.1f;
                dust.position.X -= Projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= Projectile.velocity.Y / 3f * (float)i;
            }

            //player.itemTime = 5;
            //if (Projectile.timeLeft > 210)
            //{
            //    if (Projectile.position.X + (float)(Projectile.width / 2) > player.position.X + (float)(player.width / 2))
            //    {
            //        player.ChangeDir(1);
            //    }
            //    else
            //    {
            //        player.ChangeDir(-1);
            //    }
            //}

            if (Projectile.localAI[0]  == 0f)
            {
                Projectile.localAI[1] += 1f;
                if (Projectile.localAI[1] >= 25f)
                {
                    Projectile.localAI[0]  = 1f;
                    Projectile.localAI[1] = 0f;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                float returnSpeed = 12f;
                float acceleration = 0.3f;

                float xDif = player.Center.X - Projectile.Center.X;
                float yDif = player.Center.Y - Projectile.Center.Y;
                float distance = Projectile.Distance(player.Center);

                if (distance > 3000f)
                {
                    Projectile.Kill();
                }
                distance = returnSpeed / distance;
                xDif *= distance;
                yDif *= distance;

                if (Projectile.velocity.X < xDif)
                {
                    Projectile.velocity.X = Projectile.velocity.X + acceleration;
                    if (Projectile.velocity.X < 0f && xDif > 0f)
                    {
                        Projectile.velocity.X = Projectile.velocity.X + acceleration;
                    }
                }
                else if (Projectile.velocity.X > xDif)
                {
                    Projectile.velocity.X = Projectile.velocity.X - acceleration;
                    if (Projectile.velocity.X > 0f && xDif < 0f)
                    {
                        Projectile.velocity.X = Projectile.velocity.X - acceleration;
                    }
                }

                if (Projectile.velocity.Y < yDif)
                {
                    Projectile.velocity.Y = Projectile.velocity.Y + acceleration;
                    if (Projectile.velocity.Y < 0f && yDif > 0f)
                    {
                        Projectile.velocity.Y = Projectile.velocity.Y + acceleration;
                    }
                }
                else if (Projectile.velocity.Y > yDif)
                {
                    Projectile.velocity.Y = Projectile.velocity.Y - acceleration;
                    if (Projectile.velocity.Y > 0f && yDif < 0f)
                    {
                        Projectile.velocity.Y = Projectile.velocity.Y - acceleration;
                    }
                }

                if (Main.myPlayer == Projectile.owner)
                {
                    Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                    Rectangle value2 = new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height);
                    if (rectangle.Intersects(value2))
                    {
                        Projectile.Kill();
                    }
                }

                if (Projectile.localAI[0]  == 0f)
                {
                    Vector2 velocity = Projectile.velocity;
                    velocity.Normalize();
                    return;
                }
                Vector2 vector4 = Projectile.Center - player.Center;
                vector4.Normalize();
            }

            if (Projectile.owner == Main.LocalPlayer.whoAmI)
            {
                for (int i = 0; i < Main.player.Length; i++)
                {
                    if (!hitPlayers.Contains(i) && i != Projectile.owner)
                    {
                        if (Projectile.Hitbox.Intersects(Main.player[i].Hitbox))
                        {
                            if (Main.player[i].team == player.team && Main.player[i].active)
                            {
                                player.GetModPlayer<PLAYERGLOBAL>().SendShieldPacket((int)Projectile.ai[0], i, ShieldType.Basic, 120, -1, player.whoAmI, Color.Yellow);
                                hitPlayers.Add(i);
                            }
                        }
                    }
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Projectile.localAI[0]  = 1f;
            if (target.GetGlobalNPC<NPCs.TerraLeagueNPCsGLOBAL>().illuminated)
                Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().magicOnHit += 40 + (int)(Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().MAG * 0.2);

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(4) == 0)
                target.AddBuff(BuffType<Illuminated>(), 300);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10; 
            return true;
        }
    }
}
