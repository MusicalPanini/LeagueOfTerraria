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
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons.Abilities
{
    public class AquaPrison : Ability
    {
        public AquaPrison(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Aqua Prison";
        }

        public override string GetIconTexturePath()
        {
            return "TerraLeague/AbilityImages/AquaPrison";
        }

        public override string GetAbilityTooltip()
        {
            return "Launch a bubble that traps all hit enemies and raises them off the ground";
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
                    return 40;
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
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 16f);
                int projType = ProjectileType<TideCallerStaff_AquaPrison>();
                int damage = player.GetModPlayer<PLAYERGLOBAL>().ScaleValueWithHealPower(GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG));
                int knockback = 0;

                SetAnimation(player, position + velocity);
                DoEfx(player, type);
                Projectile.NewProjectile(player.GetProjectileSource_Item(abilityItem.Item), position, velocity, projType, damage, knockback, player.whoAmI);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item20, player.Center);
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 21), player.position);
        }
    }
}
