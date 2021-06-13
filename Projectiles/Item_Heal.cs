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
            projectile.width = 8;
            projectile.height = 8;
            projectile.alpha = 0;
            projectile.timeLeft = 1000;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.alpha = 255;

            CanOnlyHitTarget = true;
            TargetPlayers = true;
            TurningFactor = 0.93f;
        }

        public override void AI()
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 dustBoxPosition = new Vector2(projectile.position.X + 6, projectile.position.Y + 6);
                int dustBoxWidth = projectile.width - 12;
                int dustBoxHeight = projectile.height - 12;
                Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, DustID.IcyMerman, 0, 0, 50, new Color(0, 255, 100), 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += projectile.velocity * 0.1f;
                dust.position.X -= projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= projectile.velocity.Y / 3f * (float)i;
            }

            HomingAI();

            if (TargetPlayers && TargetWhoAmI >= 0)
            {
                if (projectile.Hitbox.Intersects(TargetEntity.Hitbox))
                {
                    OnHitFriendlyPlayer(Main.player[TargetWhoAmI]);
                }
            }
        }

        public override void OnHitFriendlyPlayer(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 4, 0);

            projectile.netUpdate = true;
            if (projectile.owner == Main.LocalPlayer.whoAmI)
            {
                if (player.whoAmI != projectile.owner)
                    Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().SendHealPacket(projectile.damage, player.whoAmI, -1, projectile.owner);
                if (player.whoAmI == Main.myPlayer)
                {
                    player.GetModPlayer<PLAYERGLOBAL>().lifeToHeal += projectile.damage;
                }
            }
            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.IcyMerman, 0, 0, 50, new Color(0, 255, 100), 1.2f);
                dust.noGravity = true;
            }

            base.OnHitFriendlyPlayer(player);
        }

        public override void Kill(int timeLeft)
        {
            
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 16;
            return true;
        }
    }
}
