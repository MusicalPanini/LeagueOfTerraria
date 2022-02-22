using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles.Beam
{
    public abstract class BeamProjectile : ModProjectile
    {
		protected int TargetImmunityFrames = 5;
		protected int turningFactor = 1;

		protected int dust1 = DustID.GemDiamond;
		protected int dust2 = DustID.GemDiamond;
		protected int dustScale = 2;
		protected Color lightColor = Color.White;
		protected int soundID = 2;
		protected int soundStyle = 15;
		protected float soundPitch = 0;
		protected int soundDelay = 25;

		protected Rectangle SpriteStart;
		protected Rectangle SpriteMid;
		protected Rectangle SpriteEnd;

		protected int MaxDistance = 2200;

		protected float maxCharge = 0;
		protected float moveDistance = 60f;
		protected bool tileCollision = true;

		public float Distance
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}
		public float Charge
		{
			get => Projectile.localAI[0];
			set => Projectile.localAI[0] = value;
		}

		public bool IsAtMaxCharge => Charge == maxCharge;

		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.hide = true;
		}

		public override void AI()
		{
			BeamAI(Projectile.Center, Projectile.Center + Projectile.velocity);
		}

		public virtual void BeamAI(Vector2 Center, Vector2 TargetPosition)
		{
			//Projectile.position = Center + Projectile.velocity * moveDistance;

			BeamPositioning(Center, TargetPosition);
			ChargeLaser(Center);

			// If laser is not charged yet, stop the AI here.
			if (Charge < maxCharge) return;

			SoundEffect();
			SetLaserPosition(Center);
			SpawnDusts(Center);
			CastLights();
		}

		public virtual void BeamAI(Vector2 Center)
		{
			ChargeLaser(Center);

			// If laser is not charged yet, stop the AI here.
			if (Charge < maxCharge) return;

			SoundEffect();
			SetLaserPosition(Center);
			SpawnDusts(Center);
			CastLights();
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			// We can only collide if we are at max charge, which is when the laser is actually fired
			if (!IsAtMaxCharge) return false;

			Player player = Main.player[Projectile.owner];
			Vector2 unit = Projectile.velocity;
			float point = 0f;
			// Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
			// It will look for collisions on the given line using AABB
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
				Projectile.Center + unit * Distance, Projectile.width, ref point);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[Projectile.owner] = TargetImmunityFrames;
		}

		protected virtual void SpawnDusts(Vector2 Center)
		{
			Vector2 dustPos = Center + Projectile.velocity * Distance;

			for (int i = 0; i < 2; ++i)
			{
                float num1 = Projectile.velocity.ToRotation() + (Main.rand.Next(2) == 1 ? -1.0f : 1.0f) * 1.57f;
                float num2 = (float)(Main.rand.NextDouble() * 0.8f + 1.0f);
                Vector2 dustVel = new Vector2((float)Math.Cos(num1) * num2, (float)Math.Sin(num1) * num2);
                Dust dust = Dust.NewDustDirect(dustPos, 0, 0, dust1, dustVel.X, dustVel.Y);
                dust.noGravity = true;
                dust.scale = dustScale;
            }

			if (Main.rand.NextBool(5))
			{
                Vector2 offset = Projectile.velocity.RotatedBy(1.57f) * ((float)Main.rand.NextDouble() - 0.5f) * Projectile.width;
                Dust dust = Dust.NewDustDirect(dustPos - (Vector2.One * 4f), 8, 8, dust2, 0.0f, 0.0f, 100, new Color(), dustScale * 1.25f);
                dust.velocity *= 0.5f;
                dust.velocity.Y = -Math.Abs(dust.velocity.Y);
				Vector2 unit = dustPos - Center;
                unit.Normalize();
				dust.noGravity = true;
				dust = Dust.NewDustDirect(Center + 55 * unit, 8, 8, dust2, 0.0f, 0.0f, 100, new Color(), dustScale * 1.25f);
                dust.velocity *= 0.5f;
                dust.velocity.Y = -Math.Abs(dust.velocity.Y);
				dust.noGravity = true;
			}
		}

		protected virtual void SetLaserPosition(Vector2 Center)
		{
			for (Distance = moveDistance; Distance <= MaxDistance; Distance += 5f)
			{
				var start = Center + Projectile.velocity * Distance;
				//var point = start.ToTileCoordinates();
				//if (Collision.SolidTiles(point.X, point.X, point.Y, point.Y))
				if (!Collision.CanHitLine(Center, 1, 1, start, 1, 1) && tileCollision)
				{
					Distance -= 5f;
					break;
				}
			}
		}

		protected virtual void BeamPositioning(Vector2 Center, Vector2 TargetPosition)
		{
			Vector2 relativeMousePosition = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
			Vector2 normalizedMouse = Vector2.Normalize(relativeMousePosition);
			if (float.IsNaN(normalizedMouse.X) || float.IsNaN(normalizedMouse.Y))
			{
				normalizedMouse = -Vector2.UnitY;
			}
			normalizedMouse = Vector2.Normalize(Vector2.Lerp(normalizedMouse, Vector2.Normalize(Projectile.velocity), 1 - (1f / turningFactor)));
			if (normalizedMouse.X != Projectile.velocity.X || normalizedMouse.Y != Projectile.velocity.Y)
			{
				Projectile.netUpdate = true;
			}
			Projectile.velocity = normalizedMouse;
		}

		protected void ChargeLaser(Vector2 Center)
		{
			Vector2 offset = Projectile.velocity;
			offset *= moveDistance - 20;
			Vector2 pos = Center + offset - new Vector2(10, 10);
			if (Charge < maxCharge)
			{
				Charge++;
			}
			int chargeFact = (int)(Charge / 20f);
			Vector2 dustVelocity = Vector2.UnitX * 18f;
			dustVelocity = dustVelocity.RotatedBy(Projectile.rotation - 1.57f);
			Vector2 spawnPos = Projectile.Center + dustVelocity;
			for (int k = 0; k < chargeFact + 1; k++)
			{
				Vector2 spawn = spawnPos + ((float)Main.rand.NextDouble() * 6.28f).ToRotationVector2() * (12f - chargeFact * 2);
				Dust dust = Dust.NewDustDirect(pos, 20, 20, dust1, Projectile.velocity.X / 2f, Projectile.velocity.Y / 2f);
				dust.velocity = Vector2.Normalize(spawnPos - spawn) * 1.5f * (10f - chargeFact * 2f) / 10f;
				dust.noGravity = true;
				dust.scale = Main.rand.Next(10, 20) * 0.05f;
			}
		}

		protected void CastLights()
		{
			// Cast a light along the line of the laser
			DelegateMethods.v3_1 = lightColor.ToVector3();
			Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * (Distance - moveDistance), 26, DelegateMethods.CastLight);
		}

		public override bool ShouldUpdatePosition() => false;

		public override void CutTiles()
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Vector2 unit = Projectile.velocity;
			Utils.PlotTileLine(Projectile.Center, Projectile.Center + unit * Distance, (Projectile.width + 16) * Projectile.scale, DelegateMethods.CutTiles);
		}

		protected void SoundEffect()
        {
			if (Projectile.soundDelay == 0 && IsAtMaxCharge)
			{
				Projectile.soundDelay = soundDelay;
				TerraLeague.PlaySoundWithPitch(Projectile.Center, soundID, soundStyle, soundPitch);
			}
		}

        public override bool PreDraw(ref Color lightColor)
        {
			// We start drawing the laser if we have charged up
			if (IsAtMaxCharge)
			{
				// texture pls
				DrawLaser(Main.spriteBatch, TextureAssets.Projectile[Projectile.type].Value, Projectile.Center,
					Projectile.velocity, SpriteMid.Height, Projectile.damage, -1.57f, 1f, MaxDistance, Color.White, (int)moveDistance);
			}
			return false;
        }

		public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 2000f, Color color = default, int transDist = 50)
		{
			float spriteRotation = unit.ToRotation() + rotation;

			// Uses projectile alpha to give the beam transparency
			color *= 1 - (Projectile.alpha / 255f);

			// Draws the laser 'Mid'
			for (float i = transDist; i <= Distance; i += step)
			{
				var origin = start + i * unit;
				
				spriteBatch.Draw(
					texture, 
					origin - Main.screenPosition,
					SpriteMid,
					i < transDist ? Color.Transparent : color,
					spriteRotation,
					new Vector2(SpriteMid.Width * .5f, SpriteMid.Height * .5f), 
					scale,
					0, 
					0);
			}

			

			// Draws the laser 'Start'
			spriteBatch.Draw(
				texture,
				start + unit * (transDist - SpriteStart.Height) - Main.screenPosition,
				SpriteStart,
				color,
				spriteRotation, 
				new Vector2(SpriteStart.Width * .5f, SpriteStart.Height * .5f), 
				scale,
				0, 
				0);

			// Draws the laser 'End'
			spriteBatch.Draw(
				texture, 
				start + (Distance) * unit - Main.screenPosition,
				SpriteEnd,
				color, 
				spriteRotation, 
				new Vector2(SpriteEnd.Width * .5f, SpriteEnd.Height * .5f),
				scale,
				0, 
				0);
		}
	}
}
