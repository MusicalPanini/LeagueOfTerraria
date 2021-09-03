﻿using Microsoft.Xna.Framework;
using System;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class FireBolt : Active
    {
        readonly int baseDamage;
        readonly int magicMinionScaling;

        public FireBolt(int BaseDamage, int MagicMinionScaling, int Cooldown)
        {
            baseDamage = BaseDamage;
            magicMinionScaling = MagicMinionScaling;
            activeCooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            string scaleText;
            if (modPlayer.SUM > modPlayer.MAG)
                scaleText = LeagueTooltip.TooltipValue(baseDamage, false, "", new Tuple<int, ScaleType>(magicMinionScaling, ScaleType.Summon));
            else
                scaleText = LeagueTooltip.TooltipValue(baseDamage, false, "", new Tuple<int, ScaleType>(magicMinionScaling, ScaleType.Magic));

            return TooltipName("FIRE BOLT") + LeagueTooltip.CreateColorString(ActiveSecondaryColor, "Launch yourself towards the cursor while firing 7 bolts in a cone and 1 backwards." +
                "\nThe bolts deal ") + scaleText + LeagueTooltip.CreateColorString(ActiveSecondaryColor, " magic damage")
                + "\n" + LeagueTooltip.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown. Damage scales with either MAG or SUM");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (cooldownCount <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                player.velocity = TerraLeague.CalcVelocityToMouse(player.Center, 18f);

                Vector2 position = player.Center;
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 20f);
                int projType = ProjectileType<Item_FlameBolt>();
                int damage = baseDamage + (int)(Math.Max(modPlayer.SUM, modPlayer.MAG) * magicMinionScaling / 100d);
                int knockback = 1;
                int numberProjectiles = 7;
                float startingAngle = 27;
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(startingAngle));
                    Projectile.NewProjectileDirect(player.GetProjectileSource_Item(modItem.Item), position, perturbedSpeed, projType, damage, knockback, player.whoAmI);
                    startingAngle -= 9f;
                }
                Projectile.NewProjectileDirect(player.GetProjectileSource_Item(modItem.Item), position, -velocity, projType, damage, knockback, player.whoAmI);

                Efx(player);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.Item.type);

                SetCooldown(player);
            }
        }

        public override void PostPlayerUpdate(Player player)
        {
            base.PostPlayerUpdate(player);
        }

        public override void Efx(Player user)
        {
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 11), user.Center);
        }
    }
}

