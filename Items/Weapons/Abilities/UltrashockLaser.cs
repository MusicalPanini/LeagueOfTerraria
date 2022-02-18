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
    public class UltrashockLaser : Ability
    {
        public UltrashockLaser(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Ultrashock Laser";
        }

        public override string GetIconTexturePath()
        {
            return "TerraLeague/AbilityImages/UltrashockLaser";
        }

        public override string GetAbilityTooltip()
        {
            return "Fire a shock blast that slows";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.Item.damage * 2);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.RNG:
                    return 100;
                case DamageType.MAG:
                    return 60;
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
              ) + " ranged damage";
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
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 12f);
                int projType = ProjectileType<ElectricRifle_UltrashockLaser>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.RNG);
                int knockback = 2;

                SetAnimation(player, 20, 20, position + velocity);
                DoEfx(player, type);
                Projectile.NewProjectile(player.GetProjectileSource_Item(abilityItem.Item), position, velocity, projType, damage, knockback, player.whoAmI);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            SoundEngine.PlaySound(new LegacySoundStyle(3, 53), player.Center);
        }
    }
}
