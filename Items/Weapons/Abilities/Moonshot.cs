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
    public class Moonshot : Ability
    {
        public Moonshot(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Moonshot";
        }

        public override string GetIconTexturePath()
        {
            return "TerraLeague/AbilityImages/Moonshot";
        }

        public override string GetAbilityTooltip()
        {
            return "Fire a peircing bolt, damaging and marking enemies hit." +
                    "\nMarked enemies will take 50% more damage for 4 seconds";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.Item.damage);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.RNG:
                    return 100;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 10;
        }

        public override int GetBaseManaCost()
        {
            return 40;
        }

        public override string GetDamageTooltip(Player player)
        {
            return LeagueTooltip.TooltipValue(GetAbilityBaseDamage(player), false, "",
              new Tuple<int, ScaleType>(GetAbilityScalingAmount(player, DamageType.RNG), ScaleType.Ranged)
              ) + " ranged damage"
            + "\nUses 10% Calibrum Ammo";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override bool CanCurrentlyBeCast(Player player)
        {
            return player.GetModPlayer<PLAYERGLOBAL>().calibrumAmmo >= 10;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(), true))
            {
                player.GetModPlayer<PLAYERGLOBAL>().calibrumAmmo -= 10;
                int projType = ProjectileType<Calibrum_Moonshot>();
                int damage = GetAbilityBaseDamage(player) + (GetAbilityScaledDamage(player, DamageType.RNG));
                int knockback = 0;


                Vector2 position = player.MountedCenter + new Vector2(0, 8);
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 2f);

                Projectile.NewProjectile(player.GetProjectileSource_Item(abilityItem.Item), position, velocity, projType, damage, knockback, player.whoAmI);
                DoEfx(player, type);
                SetAnimation(player, position + (velocity * 100));
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 75, -1f);
        }
    }
}
