using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.CompleteItems
{
    public class Seraphs : LeagueItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Seraph's Embrase");
            Tooltip.SetDefault("5% increased magic and summon damage" +
                "\nIncreases maximum mana by 100" +
                "\nIncreases ability haste by 15" +
                "\nCan only have one AWE item equiped at a time");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            string itemName = player.armor[slot].Name;

            if (itemName == "Tear of the Goddess" || itemName == "Archangel's Staff" || itemName == "Seraph's Embrase" || itemName == "Manamune" || itemName == "Muramana")
            {
                base.CanEquipAccessory(player, slot);
                return true;
            }
            else if (modPlayer.awe)
            {
                return false;
            }
            else
            {
                base.CanEquipAccessory(player, slot);
                return true;
            }

        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;

            Active = new ManaShield(4, 15, 200, 50, 90);
            Passives = new Passive[]
            {
                new Awe(8, 0, 50),
                new Haste()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.GetDamage(DamageClass.Magic) += 0.05f;
            modPlayer.TrueMinionDamage += 0.05;
            modPlayer.abilityHaste += 15;
            player.statManaMax2 += 100;

            base.UpdateAccessory(player, hideVisual);
        }

        public override string GetStatText()
        {
            if (Active.currentlyActive)
            {
                if (Active.cooldownCount > 0)
                    return (Active.cooldownCount / 60).ToString();
            }
            return "";
        }

        public override bool OnCooldown(Player player)
        {
            if (Active.cooldownCount > 0 || !Active.currentlyActive)
                return true;
            else
                return false;
        }
    }
}
