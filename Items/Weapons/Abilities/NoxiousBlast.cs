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
    public class NoxiousBlast : Ability
    {
        public NoxiousBlast(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Noxious Blast";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/NoxiousBlast";
        }

        public override string GetAbilityTooltip()
        {
            return "Create a toxic blast at the target location releasing venomous clouds" +
                "\nDirect hits will grant movement and ranged attack speed";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 0.5);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.RNG:
                    return 10;
                case DamageType.MAG:
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
            return 50;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.RNG) + " + " + GetScalingTooltip(player, DamageType.MAG) + " magic damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(), true))
            {
                int projType = ProjectileType<SerpentsEmbrace_NoxiousBlast>();
                Vector2 position = Main.MouseWorld;
                Vector2 velocity = Vector2.Zero;
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.RNG) + GetAbilityScaledDamage(player, DamageType.MAG);
                int knockback = 0;


                Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                SetAnimation(player, position + velocity);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.Center, 3, 23, -0.75f);
        }
    }
}
