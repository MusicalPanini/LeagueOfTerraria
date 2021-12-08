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
    public class MercuryTransform : Ability
    {
        readonly MercuryType Type;
        readonly ModItem Item;

        public MercuryTransform(ModItem item, MercuryType type)
        {
            Item = item;
            Type = type;
        }

        public override string GetAbilityName()
        {
            if (Type == MercuryType.Hammer)
                return "Transform Mercury Hammer";
            else
                return "Transform Mercury Cannon";
        }

        public override string GetIconTexturePath()
        {
            if (Type == MercuryType.Hammer)
                return "TerraLeague/AbilityImages/TransformHammer";
            else
                return "TerraLeague/AbilityImages/TransformCannon";
        }

        public override string GetAbilityTooltip()
        {
            switch (Type)
            {
                case MercuryType.Hammer:
                    return "Transform the Mercury Cannon into Hammer configuration";
                case MercuryType.Cannon:
                    return "Transform the Mercury Hammer into Cannon configuration";
                default:
                    return "Swap weapon to ???";
            }
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                //case DamageType.MAG:
                //    return 100;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 4;
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type))
            {
                switch (Type)
                {
                    case MercuryType.Hammer:
                        if (ChangeWeapon(player, ItemType<MercuryCannon>(), ItemType<MercuryHammer>()))
                        {
                            DoEfx(player, type);
                            SetCooldowns(player, type);
                        }
                        break;
                    case MercuryType.Cannon:
                        if (ChangeWeapon(player, ItemType<MercuryHammer>(), ItemType<MercuryCannon>()))
                        {
                            DoEfx(player, type);
                            SetCooldowns(player, type);
                        }
                        break;
                    default:
                        break;
                }

            }
        }

        bool ChangeWeapon(Player player, int currentItem, int newItem)
        {
            if (player.HeldItem.type == currentItem && player.HeldItem.type != player.inventory[58].type)
            {
                int prefix = 0;
                int oldPrefix = player.HeldItem.prefix;

                if (Type == MercuryType.Hammer)
                {
                    prefix = player.HeldItem.GetGlobalItem<MercuryWeapon>().HammerPrefix;
                    player.HeldItem.GetGlobalItem<MercuryWeapon>().CannonPrefix = player.HeldItem.prefix;

                }
                else if (Type == MercuryType.Cannon)
                {
                    prefix = player.HeldItem.GetGlobalItem<MercuryWeapon>().CannonPrefix;
                    player.HeldItem.GetGlobalItem<MercuryWeapon>().HammerPrefix = player.HeldItem.prefix;
                }

                player.HeldItem.SetDefaults(newItem);
                player.HeldItem.Prefix(prefix);

                if (Type == MercuryType.Hammer)
                {
                    player.HeldItem.GetGlobalItem<MercuryWeapon>().CannonPrefix = oldPrefix;
                }
                else if (Type == MercuryType.Cannon)
                {
                    player.HeldItem.GetGlobalItem<MercuryWeapon>().HammerPrefix = oldPrefix;

                }

                return true;
            }
            else
            {
                for (int i = 0; i < player.inventory.Length; i++)
                {
                    if (player.inventory[i].type == currentItem && i != 58)
                    {
                        if (Type == MercuryType.Hammer)
                            player.inventory[i].GetGlobalItem<MercuryWeapon>().CannonPrefix = player.inventory[i].prefix;
                        else if (Type == MercuryType.Cannon)
                            player.inventory[i].GetGlobalItem<MercuryWeapon>().HammerPrefix = player.inventory[i].prefix;

                        player.inventory[i].SetDefaults(newItem);

                        if (Type == MercuryType.Hammer)
                            player.inventory[i].Prefix(player.inventory[i].GetGlobalItem<MercuryWeapon>().HammerPrefix);
                        else if (Type == MercuryType.Cannon)
                            player.inventory[i].Prefix(player.inventory[i].GetGlobalItem<MercuryWeapon>().CannonPrefix);
                        return true;
                    }
                }
            }

            return false;
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 42, 25, -0.5f);
            for (int j = 0; j < 10; j++)
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 264, 0, 0);
                //dust.velocity.X *= 0;
                //dust.velocity.Y -= 4;
                dust.velocity *= 2;
                dust.noGravity = true;
                dust.scale = 2;
                if (Type == MercuryType.Hammer)
                    dust.color = Color.Orange;
                else
                    dust.color = Color.Blue;
            }
            base.Efx(player);
        }
    }

    public enum MercuryType
    {
        Hammer,
        Cannon
    }
}
