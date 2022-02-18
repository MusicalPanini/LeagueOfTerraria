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
    public class SteelTempest : Ability
    {
        public SteelTempest(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            if (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().gathering3)
                return "Gathering Storm";
            else if (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().gathering2)
                return "Steel Wind Rising";
            else
                return "Steel Tempest";
        }

        public override string GetIconTexturePath()
        {
            if (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().gathering3)
            {
                return "TerraLeague/AbilityImages/GatheringStorm";
            }
            else
            {
                return "TerraLeague/AbilityImages/SteelTempest";
            }
        }

        public override string GetAbilityTooltip()
        {
            if (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().gathering3)
            {
                return "Uses 2 stacks" +
                    "\nLaunch a tornado that knocks up enemies" +
                    "\nCan Crit";
            }
            else
            {
                return "Thrust your sword forward." +
                    "\nGain a stack of 'Gathering Storm' if you damage an enemy" +
                    "\nCreate a tornado at 2 stacks" +
                    "\nCan Crit";
            }
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().gathering3)
                return (int)(abilityItem.Item.damage * 2);
            else
                return (int)(abilityItem.Item.damage);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MEL:
                    return 100;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 4;
        }

        public override int GetBaseManaCost()
        {
            return 0;
        }

        public override string GetDamageTooltip(Player player)
        {
            return LeagueTooltip.TooltipValue(GetAbilityBaseDamage(player), false, "",
              new Tuple<int, ScaleType>(GetAbilityScalingAmount(player, DamageType.MEL), ScaleType.Melee)
              ) + " melee damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type))
            {
                player.itemAnimationMax = TerraLeague.ScaleWithUseTimeMulti(abilityItem.Item.useAnimation / 2, abilityItem.Item, player);

                if (player.GetModPlayer<PLAYERGLOBAL>().gathering3)
                {
                    DoEfx(player, type);
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 12f);
                    int projType = ProjectileType<LastBreath_Tornado>();//704;
                    int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MEL);
                    int knockback = 6;
                    player.ClearBuff(BuffType<LastBreath3>());

                    Projectile proj = Projectile.NewProjectileDirect(player.GetProjectileSource_Item(abilityItem.Item), position, velocity, projType, damage, knockback, player.whoAmI);
                    proj.DamageType = Terraria.ModLoader.DamageClass.Melee;
                    Projectile.NewProjectile(player.GetProjectileSource_Item(abilityItem.Item), position, velocity / 4, ProjectileType<LastBreath_SteelTempest>(), damage, knockback, player.whoAmI, 1);

                    SetCooldowns(player, type);
                    
                }
                else
                {
                    DoEfx(player, type);
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 3f);
                    int projType = ProjectileType<LastBreath_SteelTempest>();
                    int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MEL);
                    int knockback = 5;
                    player.ClearBuff(BuffType<LastBreath3>());

                    Projectile.NewProjectile(player.GetProjectileSource_Item(abilityItem.Item), position, velocity, projType, damage, knockback, player.whoAmI);
                    SetCooldowns(player, type);
                }
            }
        }
    }
}
