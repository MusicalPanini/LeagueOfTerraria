using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class ChainWardensScythe_Scythe : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chain Warden's Scythe");
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.alpha = 0;
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
			Projectile.tileCollide = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("TerraLeague/Projectiles/ChainWardensScythe_Chain").Value;
            Vector2 position;
            if (Projectile.spriteDirection == 1)
                position = new Vector2(Projectile.position.X + 8, Projectile.position.Y + 7).RotatedBy(Projectile.rotation, Projectile.Center);
            else
                position = new Vector2(Projectile.position.X + Projectile.width - 8, Projectile.position.Y + 7).RotatedBy(Projectile.rotation, Projectile.Center);

            

            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
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
                    Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
                    color2 = Projectile.GetAlpha(color2);
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
                }
            }

            return true;
        }

        public override void AI()
        {
			Player player = Main.player[Projectile.owner];

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
				int num = 10;
				float num2 = 24;//24f;
				float num3 = 800f;
				float num4 = 3f;
				float num5 = 16f;
				int generalHitCooldown = 10;
				int swingHitCooldown = 20;
				int throwHitCooldown = 10;
				float swingDistance = 50f;
				float swingSpeed = 75f;

				float speedMultiplier = 1f / player.meleeSpeed;
				num2 *= speedMultiplier;
				num4 *= speedMultiplier;
				num5 *= speedMultiplier;

				Projectile.localNPCHitCooldown = generalHitCooldown;
                switch ((int)Projectile.ai[0])
                {
                    case 0:
                        flag2 = true;
                        if (Projectile.owner == Main.myPlayer)
                        {
                            Vector2 position = Main.MouseWorld - mountedCenter;
                            position = position.SafeNormalize(Vector2.UnitX * (float)player.direction);
                            player.ChangeDir((position.X > 0f) ? 1 : (-1));
                            if (!player.channel)
                            {
                                Projectile.ai[0] = 1f;
                                Projectile.ai[1] = 0f;
                                Projectile.velocity = position * num2 + player.velocity;
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
                        Projectile.Center = mountedCenter + vector3 * swingDistance; ;
                        Projectile.velocity = Vector2.Zero;
                        Projectile.localNPCHitCooldown = swingHitCooldown;

                        break;
                    case 1:
                        ref float val = ref Projectile.ai[1];
                        float num19 = val;
                        val = num19 + 1f;
                        bool flag3 = num19 >= (float)num;
                        flag3 |= (Projectile.Distance(mountedCenter) >= num3);

                        if (flag3)
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
                        Vector2 value = Projectile.DirectionTo(mountedCenter).SafeNormalize(Vector2.Zero);
                        if (Projectile.Distance(mountedCenter) <= num5)
                        {
                            Projectile.Kill();
                            return;
                        }
                        Projectile.velocity *= 0.98f;
                        Projectile.velocity = MoveTowards(Projectile.velocity, value * num5, num4);
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

				if (Projectile.spriteDirection == -1)
					Projectile.rotation = (Projectile.Center - mountedCenter).ToRotation() -  (MathHelper.Pi * 0.75f);
				else
					Projectile.rotation = (Projectile.Center - mountedCenter).ToRotation() - (MathHelper.Pi * 0.25f);
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

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (Projectile.ai[0] == 0f)
			{
				Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
				Vector2 vector = ClosestPointInRect(targetHitbox, mountedCenter) - mountedCenter;
				vector.Y /= 0.8f;
				float num = 90f;
				return vector.Length() <= num;
			}

			return base.Colliding(projHitbox, targetHitbox);
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
