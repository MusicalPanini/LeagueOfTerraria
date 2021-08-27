using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Projectiles.Homing;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
	public class Item_Heal : HomingProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heal");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.alpha = 0;
            Projectile.timeLeft = 1000;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;

            CanOnlyHitTarget = true;
            TargetPlayers = true;
            TurningFactor = 0.93f;
        }

        public override void AI()
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 dustBoxPosition = new Vector2(Projectile.position.X + 6, Projectile.position.Y + 6);
                int dustBoxWidth = Projectile.width - 12;
                int dustBoxHeight = Projectile.height - 12;
                Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, DustID.Ice, 0, 0, 50, new Color(0, 255, 100), 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += Projectile.velocity * 0.1f;
                dust.position.X -= Projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= Projectile.velocity.Y / 3f * (float)i;
            }

            HomingAI();

            if (TargetPlayers && TargetWhoAmI >= 0)
            {
                if (Projectile.Hitbox.Intersects(TargetEntity.Hitbox))
                {
                    OnHitFriendlyPlayer(Main.player[TargetWhoAmI]);
                }
            }
        }

        public override void OnHitFriendlyPlayer(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 4, 0);

            Projectile.netUpdate = true;
            if (Projectile.owner == Main.LocalPlayer.whoAmI)
            {
                if (player.whoAmI != Projectile.owner)
                    Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().SendHealPacket(Projectile.damage, player.whoAmI, -1, Projectile.owner);
                if (player.whoAmI == Main.myPlayer)
                {
                    player.GetModPlayer<PLAYERGLOBAL>().lifeToHeal += Projectile.damage;
                }
            }
            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, 0, 0, 50, new Color(0, 255, 100), 1.2f);
                dust.noGravity = true;
            }

            base.OnHitFriendlyPlayer(player);
        }

        public override void Kill(int timeLeft)
        {
            
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 16;
            return true;
        }
    }
}
