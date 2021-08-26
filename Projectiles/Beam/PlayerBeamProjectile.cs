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
    public abstract class PlayerBeamProjectile : BeamProjectile
    {
		protected bool trackMouse = true;
		protected bool isChannel = true;
		protected bool keepPlayerUsing = true;

		// The AI of the projectile
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			Vector2 target = trackMouse ? Main.MouseWorld : player.MountedCenter + Projectile.velocity;

			BeamAI(player.MountedCenter, target);
		}

		public override void BeamAI(Vector2 Center, Vector2 TargetPosition)
        {
			Player player = Main.player[Projectile.owner];
			Projectile.position = Center + Projectile.velocity;

			BeamPositioning(Center, TargetPosition);
			ChargeLaser(Center);
			UpdatePlayer(player);

			// If laser is not charged yet, stop the AI here.
			if (Charge < maxCharge) return;

			SoundEffect();
			SetLaserPosition(player.Center);
			SpawnDusts(Center);
			CastLights();
		}

		private void UpdatePlayer(Player player)
		{
			if (isChannel)
            {
                // Kill the projectile if the player stops channeling
                if (!player.channel)
                {
                    Projectile.Kill();
                }
				else if (Main.time % 10 < 1 && !player.CheckMana(player.inventory[player.selectedItem].mana, true))
				{
					Projectile.Kill();
				}
				else
                {
					Projectile.timeLeft = 2;
				}
			}

			// Multiplayer support here, only run this code if the client running it is the owner of the projectile
			if (Projectile.owner == Main.myPlayer)
			{
				Projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
				Projectile.netUpdate = true;
			}
			int dir = Projectile.velocity.X > 0 ? 1 : -1;
			player.ChangeDir(dir); // Set player direction to where we are shooting
			player.heldProj = Projectile.whoAmI; // Update player's held projectile
			player.itemTime = 2; // Set item time to 2 frames while we are used
			player.itemAnimation = 2; // Set item animation time to 2 frames while we are used
			player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * dir, Projectile.velocity.X * dir); // Set the item rotation to where we are shooting
		}

        protected override void BeamPositioning(Vector2 Center, Vector2 TargetPosition)
        {
			Player player = Main.LocalPlayer;

			Vector2 rotatedRelativePoint = player.RotatedRelativePoint(player.MountedCenter, false);
			Vector2 relativeMousePosition = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - rotatedRelativePoint;
			if (player.gravDir == -1f)
			{
				relativeMousePosition.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - rotatedRelativePoint.Y;
			}
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

        public override bool PreDraw(ref Color lightColor)
        {
			// We start drawing the laser if we have charged up
			if (IsAtMaxCharge)
			{
				DrawLaser(Main.spriteBatch, TextureAssets.Projectile[Projectile.type].Value, Main.player[Projectile.owner].Center,
					Projectile.velocity, SpriteMid.Height-2, Projectile.damage, -1.57f, 1f, MaxDistance, Color.White, (int)moveDistance);
			}
			return false;
		}
	}
}
