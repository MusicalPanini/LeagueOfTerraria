﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using TerraLeague.Items.AdvItems;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Athenes : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Athene's Unholy Grail");
            Tooltip.SetDefault("4% increased magic and summon damage" +
                "\nIncreases resist by 4" +
                "\nIncreases mana regeneration by 30%" +
                "\nIncreases ability haste by 10");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 45, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new Dissonance(1, 40),
                new BloodPool(250)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.3;
            player.GetDamage(DamageClass.Magic) += 0.04f;
            player.GetDamage(DamageClass.Summon) += 0.04f;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Codex>(), 1)
            .AddIngredient(ItemType<Chalice>(), 1)
            .AddIngredient(ItemID.SoulofFright, 6)
            .AddIngredient(ItemID.SoulofMight, 6)
            .AddIngredient(ItemID.HallowedBar, 12)
            .AddIngredient(ItemID.Deathweed, 2)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string GetStatText()
        {
            if (Passives[1].currentlyActive)
                return ((int)Passives[1].passiveStat).ToString();
            else
                return "";
        }
    }
}
