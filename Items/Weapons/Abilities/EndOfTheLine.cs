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
    public class EndOfTheLine : Ability
    {
        public EndOfTheLine(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "End of the Line";
        }

        public override string GetIconTexturePath()
        {
            return "TerraLeague/AbilityImages/EndoftheLine";
        }

        public override string GetAbilityTooltip()
        {
            return "Fire an explosive round that detonates on contact with the ground." +
                    "\nThe explosions will home back to you";
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
            return 20;
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
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 12);
                int projType = ProjectileType<BrassShotgun_EndoftheLine>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.RNG);
                int knockback = 3;

                SetAnimation(player, position + velocity);
                DoEfx(player, type);
                Projectile.NewProjectile(player.GetProjectileSource_Item(abilityItem.Item), position, velocity, projType, damage, knockback, player.whoAmI);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 11), player.Center);
        }
    }
}
