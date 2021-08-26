﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons.Abilities
{
    public class Defile : Ability
    {
        public Defile(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Defile";
        }

        public override string GetIconTexturePath()
        {
            return "TerraLeague/AbilityImages/Defile";
        }

        public override string GetAbilityTooltip()
        {
            return "Curse the area around you";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.Item.damage * 0.6);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MAG:
                    return 20;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 8;
        }

        public override int GetBaseManaCost()
        {
            return 75;
        }

        public override string GetDamageTooltip(Player player)
        {
            return LeagueTooltip.TooltipValue(GetAbilityBaseDamage(player), false, "",
              new Tuple<int, ScaleType>(GetAbilityScalingAmount(player, DamageType.MAG), ScaleType.Magic)
              ) + " magic damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(), true))
            {
                Vector2 position = player.MountedCenter;
                Vector2 velocity = Vector2.Zero;
                int projType = ProjectileType<DeathsingerTome_Defile>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG);
                int knockback = 0;
                SetAnimation(player, player.MountedCenter + (Vector2.UnitX * player.direction));
                Projectile.NewProjectile(player.GetProjectileSource_Item(abilityItem.Item), position, velocity, projType, damage, knockback, player.whoAmI);
                SetCooldowns(player, type);
            }
        }
    }
}
