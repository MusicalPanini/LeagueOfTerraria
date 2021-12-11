﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons.Abilities
{
    public class TwilightShroud : Ability
    {
        public TwilightShroud(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Twilight Shroud";
        }

        public override string GetIconTexturePath()
        {
            return "TerraLeague/AbilityImages/TwilightShroud";
        }

        public override string GetAbilityTooltip()
        {
            return "Drop a smoke bomb at your feet." +
                "\nWhile standing in the shroud and not using items," +
                "\nincrease mana regeneration and become obsucured and immune to projectiles";
        }

        public override int GetBaseManaCost()
        {
            return 100;
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override int GetRawCooldown()
        {
            return 30;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
            {
                DoEfx(player, type);
                int[] order = new int[] { -4, 4, -3, 3, -2, 2, -1, 1, 0 };

                for (int i = 0; i < 9; i++)
                {
                    Projectile.NewProjectile(player.GetProjectileSource_Item(abilityItem.Item), player.Center, new Vector2(order[i], 0), ProjectileType<AssassinsKunai_ShroudSmoke>(), 0, 0, player.whoAmI);
                    Projectile.NewProjectile(player.GetProjectileSource_Item(abilityItem.Item), player.Center, new Vector2(order[i], 0), ProjectileType<AssassinsKunai_ShroudSmoke>(), 0, 0, player.whoAmI);
                    Projectile.NewProjectile(player.GetProjectileSource_Item(abilityItem.Item), player.Center, new Vector2(order[i], 0), ProjectileType<AssassinsKunai_ShroudSmoke>(), 0, 0, player.whoAmI);
                    Projectile.NewProjectile(player.GetProjectileSource_Item(abilityItem.Item), player.Center, new Vector2(order[i], 0), ProjectileType<AssassinsKunai_ShroudSmoke>(), 0, 0, player.whoAmI);
                }
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            player.itemAnimation = ItemUseStyleID.Swing;
            player.itemAnimationMax = 24;
            player.reuseDelay = 24;
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 11).WithPitchVariance(-1), player.Center);
        }
    }
}
