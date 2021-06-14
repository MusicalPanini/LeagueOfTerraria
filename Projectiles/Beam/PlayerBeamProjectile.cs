using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Enums;
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
			Player player = Main.player[projectile.owner];

			Vector2 target = trackMouse ? Main.MouseWorld : player.MountedCenter + projectile.velocity;

			BeamAI(player.MountedCenter, target);
		}

		public override void BeamAI(Vector2 Center, Vector2 TargetPosition)
        {
			Player player = Main.player[projectile.owner];
			projectile.position = Center + projectile.velocity;

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
                    projectile.Kill();
                }
				else if (Main.time % 10 < 1 && !player.CheckMana(player.inventory[player.selectedItem].mana, true))
				{
					projectile.Kill();
				}
				else
                {
					projectile.timeLeft = 2;
				}
			}

			// Multiplayer support here, only run this code if the client running it is the owner of the projectile
			if (projectile.owner == Main.myPlayer)
			{
				projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
				projectile.netUpdate = true;
			}
			int dir = projectile.velocity.X > 0 ? 1 : -1;
			player.ChangeDir(dir); // Set player direction to where we are shooting
			player.heldProj = projectile.whoAmI; // Update player's held projectile
			player.itemTime = 2; // Set item time to 2 frames while we are used
			player.itemAnimation = 2; // Set item animation time to 2 frames while we are used
			player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir); // Set the item rotation to where we are shooting
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
			normalizedMouse = Vector2.Normalize(Vector2.Lerp(normalizedMouse, Vector2.Normalize(projectile.velocity), 1 - (1f / turningFactor)));
			if (normalizedMouse.X != projectile.velocity.X || normalizedMouse.Y != projectile.velocity.Y)
			{
				projectile.netUpdate = true;
			}
			projectile.velocity = normalizedMouse;
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			// We start drawing the laser if we have charged up
			if (IsAtMaxCharge)
			{
				DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center,
					projectile.velocity, SpriteMid.Height-2, projectile.damage, -1.57f, 1f, MaxDistance, Color.White, (int)moveDistance);
			}
			return false;
		}
	}
}
