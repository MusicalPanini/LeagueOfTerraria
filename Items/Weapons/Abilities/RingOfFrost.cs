using Microsoft.Xna.Framework;
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
    public class RingOfFrost : Ability
    {
        public RingOfFrost(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Ring of Frost";
        }

        public override string GetIconTexturePath()
        {
            return "TerraLeague/AbilityImages/RingOfFrost";
        }

        public override string GetAbilityTooltip()
        {
            return "Slow all nearby enemies in a cold burst";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.Item.damage * 1.5);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MAG:
                    return 50;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 12;
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
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
            {
                Vector2 position = player.Center;
                int projType = ProjectileType<DarkIceTome_FrostRing>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG);
                int knockback = 0;

                Projectile.NewProjectile(player.GetProjectileSource_Item(abilityItem.Item), position, Vector2.Zero, projType, damage, knockback, player.whoAmI);
                SetCooldowns(player, type);
            }
        }
    }
}
