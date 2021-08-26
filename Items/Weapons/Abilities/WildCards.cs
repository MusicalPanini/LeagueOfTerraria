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
    public class WildCards : Ability
    {
        public WildCards(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Wild Cards";
        }

        public override string GetIconTexturePath()
        {
            return "TerraLeague/AbilityImages/WildCards";
        }

        public override string GetAbilityTooltip()
        {
            return "Throw " + LeagueTooltip.TooltipValue(2, false, "",
              new Tuple<int, ScaleType>(100, ScaleType.Minions)
              ) + " cards in a cone";
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
                case DamageType.SUM:
                    return 30;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 6;
        }

        public override int GetBaseManaCost()
        {
            return 35;
        }

        public override string GetDamageTooltip(Player player)
        {
            return LeagueTooltip.TooltipValue(GetAbilityBaseDamage(player), false, "",
              new Tuple<int, ScaleType>(GetAbilityScalingAmount(player, DamageType.MAG), ScaleType.Magic),
              new Tuple<int, ScaleType>(GetAbilityScalingAmount(player, DamageType.SUM), ScaleType.Summon)
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
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 15 * 0.6f);
                int projType = ProjectileType<MagicCards_GreenCard>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG) + GetAbilityScaledDamage(player, DamageType.SUM);
                int knockback = 4;

                int numberProjectiles = player.maxMinions + 2;
                float baseAngle = 30 + (2.5f * (player.maxMinions - 1));
                float startingAngle = baseAngle;
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(startingAngle));
                    Projectile.NewProjectile(player.GetProjectileSource_Item(abilityItem.Item), position, perturbedSpeed, projType, damage, knockback, player.whoAmI, 1);
                    startingAngle -= baseAngle * 2 / (numberProjectiles - 1);
                }
                SetAnimation(player, position + velocity);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 19, SoundType.Sound));
        }
    }
}
