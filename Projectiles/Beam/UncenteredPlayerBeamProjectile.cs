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
    public abstract class UncenteredPlayerBeamProjectile : BeamProjectile
    {
		protected bool trackMouse = true;

		// The AI of the projectile
		public override void AI()
		{
			BeamAI(Projectile.Center, Projectile.Center + Projectile.velocity);
		}

		public override void BeamAI(Vector2 Center, Vector2 TargetPosition)
        {
			Player player = Main.player[Projectile.owner];

			BeamPositioning(Center, TargetPosition);
			ChargeLaser(Center);

			// If laser is not charged yet, stop the AI here.
			if (Charge < maxCharge) return;

			SetLaserPosition(player.Center);
			SpawnDusts(Center);
			CastLights();
		}

        protected override void BeamPositioning(Vector2 Center, Vector2 TargetPosition)
        {
			Vector2 normalizedTarget = Vector2.Normalize(TargetPosition - Center);
			if (float.IsNaN(normalizedTarget.X) || float.IsNaN(normalizedTarget.Y))
			{
				normalizedTarget = -Vector2.UnitY;
			}
			normalizedTarget = Vector2.Normalize(Vector2.Lerp(normalizedTarget, Vector2.Normalize(Projectile.velocity), 1 - (1f / turningFactor)));
			if (normalizedTarget.X != Projectile.velocity.X || normalizedTarget.Y != Projectile.velocity.Y)
			{
				Projectile.netUpdate = true;
			}
			Projectile.velocity = normalizedTarget;
        }
	}
}
