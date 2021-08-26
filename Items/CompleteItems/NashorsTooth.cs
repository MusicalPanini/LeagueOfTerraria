﻿using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class NashorsTooth : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nashor's Tooth");
            Tooltip.SetDefault("7% increased summon damage" +
                "\n25% increased melee speed" +
                "\nIncreases ability haste by 20");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 45, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new VoidSharpened(15, 25)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Summon) += 0.07f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 20;
            player.meleeSpeed += 0.25f;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Stinger>(), 1)
            .AddIngredient(ItemType<Codex>(), 1)
            .AddIngredient(ItemID.Excalibur, 1)
            .AddIngredient(ItemType<VoidBar>(), 8)
            .AddIngredient(ItemID.SoulofFright, 4)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
