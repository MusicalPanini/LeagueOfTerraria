using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class TrueIceFlail_Ball : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Ice Flail");
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.alpha = 0;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.scale = 1.25f;
            Projectile.tileCollide = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("TerraLeague/Projectiles/TrueIceFlail_Chain").Value;

            Vector2 position = Projectile.Bottom.RotatedBy(Projectile.rotation, Projectile.Center);
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
            float num1 = (float)texture.Height;
            Vector2 vector2_4 = mountedCenter - position;
            float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
            bool flag = true;
            if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                flag = false;
            if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
                flag = false;
            while (flag)
            {
                if ((double)vector2_4.Length() < (double)num1 + 1.0)
                {
                    flag = false;
                }
                else
                {
                    Vector2 vector2_1 = vector2_4;
                    vector2_1.Normalize();
                    position += vector2_1 * num1;
                    vector2_4 = mountedCenter - position;
                    Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
                    color2 = Projectile.GetAlpha(color2);
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1.35f, SpriteEffects.None, 0.0f);
                }
            }

            return true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 113, 0f, 0f, 100, default);
            dust.velocity *= 0.2f;
            dust.scale *= 0.7f;

            if (!player.active || player.dead || Vector2.Distance(Projectile.Center, player.Center) > 900f)
            {
                Projectile.Kill();
            }
            else if (Main.myPlayer == Projectile.owner && Main.mapFullscreen)
            {
                Projectile.Kill();
            }
            else
            {
                Vector2 mountedCenter = player.MountedCenter;
                bool flag2 = false;
                int throwTimerMax = 14;
                float throwSpeed = 24;//24f;
                float ProjectileMaxDistance = 800f;
                float maxReturnSpeed = 3f;
                float returnMagnitude = 16f;
                int generalHitCooldown = 60;
                int swingHitCooldown = 60;
                int throwHitCooldown = 60;
                float swingDistance = 45f;
                float swingSpeed = 90f;

                float speedMultiplier = 1f / player.meleeSpeed;
                throwSpeed *= speedMultiplier;
                maxReturnSpeed *= speedMultiplier;
                returnMagnitude *= speedMultiplier;
                swingSpeed *= player.meleeSpeed;

                Projectile.localNPCHitCooldown = generalHitCooldown;
                switch ((int)Projectile.ai[0])
                {
                    case 0:
                        flag2 = true;
                        if (Projectile.owner == Main.myPlayer)
                        {
                            Vector2 CursorPosition = Main.MouseWorld - mountedCenter;
                            CursorPosition = CursorPosition.SafeNormalize(Vector2.UnitX * (float)player.direction);
                            player.ChangeDir((CursorPosition.X > 0f) ? 1 : (-1));
                            if (!player.channel)
                            {
                                Projectile.ai[0] = 1f;
                                Projectile.ai[1] = 0f;
                                Projectile.velocity = CursorPosition * throwSpeed + player.velocity;
                                Projectile.Center = mountedCenter;
                                Projectile.netUpdate = true;
                                Projectile.tileCollide = true;
                                for (int i = 0; i < Projectile.localNPCImmunity.Length; i++)
                                {
                                    Projectile.localNPCImmunity[i] = 0;
                                }
                                Projectile.localNPCHitCooldown = throwHitCooldown;
                                break;
                            }
                        }
                        Projectile.localAI[1] += 1f;
                        Vector2 vector3 = new Vector2((float)player.direction).RotatedBy((double)(31.4159279f * (Projectile.localAI[1] / swingSpeed) * (float)player.direction), default);
                        vector3.Y *= 0.8f;
                        if (vector3.Y * player.gravDir > 0f)
                        {
                            vector3.Y *= 0.5f;
                        }
                        Projectile.Center = mountedCenter + vector3 * swingDistance;
                        Projectile.velocity = Vector2.Zero;
                        Projectile.localNPCHitCooldown = swingHitCooldown;

                        break;
                    case 1:
                        Projectile.ai[1]++;
                        bool PullProjectileBack = Projectile.ai[1] >= (float)throwTimerMax;
                        PullProjectileBack |= (Projectile.Distance(mountedCenter) >= ProjectileMaxDistance);

                        if (PullProjectileBack)
                        {
                            Projectile.ai[0] = 2f;
                            Projectile.ai[1] = 0f;
                            Projectile.netUpdate = true;
                            Projectile.velocity *= 0.3f;
                        }
                        player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
                        Projectile.localNPCHitCooldown = throwHitCooldown;

                        break;
                    case 2:
                        Projectile.tileCollide = false;
                        Vector2 direction = Projectile.DirectionTo(mountedCenter).SafeNormalize(Vector2.Zero);
                        if (Projectile.Distance(mountedCenter) <= returnMagnitude)
                        {
                            Projectile.Kill();
                            return;
                        }
                        Projectile.velocity *= 0.98f;
                        Projectile.velocity = MoveTowards(Projectile.velocity, direction * returnMagnitude, maxReturnSpeed);
                        player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));

                        break;
                }
                Projectile.spriteDirection = player.direction;
                Projectile.ownerHitCheck = flag2;
                player.itemTime = 5;
                player.itemAnimation = 5;
                Projectile.timeLeft = 2;
                player.heldProj = Projectile.whoAmI;
                player.itemRotation = Projectile.DirectionFrom(mountedCenter).ToRotation();
                if (Projectile.Center.X < mountedCenter.X)
                {
                    player.itemRotation += 3.14159274f;
                }
                player.itemRotation = MathHelper.WrapAngle(player.itemRotation);

                Projectile.rotation = (Projectile.Center - mountedCenter).ToRotation() + (MathHelper.Pi * 0.5f);
                //projectile.AI_015_Flails_Dust(doFastThrowDust);
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if ((int)Projectile.ai[0] == 0)
            {
                damage = (int)(damage * 0.25);
                knockback = (int)(knockback * 0.25);
            }

            if (Main.rand.Next(0, 4) == 0)
            {
                if (target.HasBuff(BuffType<Slowed>()))
                {
                    target.AddBuff(BuffType<Frozen>(), 180);
                    target.DelBuff(target.FindBuffIndex(BuffType<Slowed>()));
                }
                else
                {
                    target.AddBuff(BuffType<Slowed>(), 180);
                }
            }
            else if (target.HasBuff(BuffType<Slowed>()))
            {
                target.AddBuff(BuffType<Slowed>(), 180);
            }
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            Projectile.ai[0] = 2;

            if (oldVelocity.X != Projectile.velocity.X)
                Projectile.velocity.X *= 0.1f;
            if (oldVelocity.Y != Projectile.velocity.Y)
                Projectile.velocity.Y *= 0.1f;
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 16;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public Vector2 MoveTowards(Vector2 currentPosition, Vector2 targetPosition, float maxAmountAllowedToMove)
        {
            Vector2 v = targetPosition - currentPosition;
            if (v.Length() < maxAmountAllowedToMove)
            {
                return targetPosition;
            }
            return currentPosition + v.SafeNormalize(Vector2.Zero) * maxAmountAllowedToMove;
        }

        public Vector2 ClosestPointInRect(Rectangle r, Vector2 point)
        {
            Vector2 vector = point;
            if (vector.X < (float)r.Left)
            {
                vector.X = (float)r.Left;
            }
            if (vector.X > (float)r.Right)
            {
                vector.X = (float)r.Right;
            }
            if (vector.Y < (float)r.Top)
            {
                vector.Y = (float)r.Top;
            }
            if (vector.Y > (float)r.Bottom)
            {
                vector.Y = (float)r.Bottom;
            }
            return vector;
        }
    }
}
