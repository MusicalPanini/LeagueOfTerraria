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
    public class RainOfArrows : Ability
    {
        public RainOfArrows(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Rain of Arrows";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/RainofArrows";
        }

        public override string GetAbilityTooltip()
        {
            return "Fire a rain of arrows that slow enemies and apply 'Grievous Wounds'" +
                "\nThe arrows will remain stuck in the ground for 6 seconds";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 0.75f);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.RNG:
                    return 40;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 13;
        }

        public override int GetBaseManaCost()
        {
            return 30;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.RNG) + " ranged damage";
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
                int projType = ProjectileType<DarkinBow_ArrowRain>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.RNG);
                int knockback = 0;

                int arrows = Main.rand.Next(30, 41);
                for (int i = 0; i < arrows; i++)
                {
                    Vector2 VirtualMousePos;

                    if (Main.MouseWorld.X < player.MountedCenter.X)
                        VirtualMousePos = new Vector2(Math.Max(Main.MouseWorld.X, player.MountedCenter.X - 200) + Main.rand.NextFloat(-80, 80), player.MountedCenter.Y - 600);
                    else
                        VirtualMousePos = new Vector2(Math.Min(Main.MouseWorld.X, player.MountedCenter.X + 200) + Main.rand.NextFloat(-80, 80), player.MountedCenter.Y - 600);


                    Vector2 velocity = TerraLeague.CalcVelocityToPoint(player.MountedCenter, VirtualMousePos, Main.rand.NextFloat(14, 16));

                    //Vector2 velocity = new Vector2(Main.rand.NextFloat(-3, 0), Main.rand.NextFloat(-6, -5) * 1.5f);
                    //Vector2 velocity2 = new Vector2(Main.rand.NextFloat(0, 3), Main.rand.NextFloat(-6, -5) * 1.5f);

                    Projectile.NewProjectileDirect(position, velocity, projType, damage, knockback, player.whoAmI);
                    //Projectile.NewProjectileDirect(position, velocity2, projType, damage, knockback, player.whoAmI);
                }

                SetAnimation(player, 30, 30, new Vector2(player.MountedCenter.X, player.MountedCenter.Y - 30));
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            Main.PlaySound(SoundID.Item5, player.Center);
        }
    }
}
