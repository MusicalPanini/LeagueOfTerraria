using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons.Abilities
{
    public class AccelGate : Ability
    {
        public AccelGate(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Acceleration Gate";
        }

        public override string GetIconTexturePath()
        {
            return "TerraLeague/AbilityImages/AccelGate";
        }

        public override string GetAbilityTooltip()
        {
            return "Deply an Acceleration Gate for 10 seconds." +
                "\nAllies that walk through it gain increased movement and attack speed for 4 seconds";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return 0;
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                default:
                    return 0;
            }
        }

        public override int GetBaseManaCost()
        {
            return 60;
        }

        public override string GetDamageTooltip(Player player)
        {
            return "";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override int GetRawCooldown()
        {
            return 20;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
            {
                Vector2 position = Main.MouseWorld;
                Vector2 velocity = new Vector2(0, 8).RotatedBy(player.Center.AngleTo(position));
                int projType = ProjectileType<MercuryCannon_AccelGate>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG);
                int knockback = 0;

                DoEfx(player, type);
                Projectile proj = Projectile.NewProjectileDirect(player.GetProjectileSource_Item(abilityItem.Item), position, velocity, projType, damage, knockback, player.whoAmI);
                
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 42, 25, 0f);
        }
    }
}
