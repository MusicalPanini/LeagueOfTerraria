using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.StartingItems
{
    public class DoransBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Doran's Blade");
            Tooltip.SetDefault("+4 melee and ranged damage" +
                "\nIncreases health by 10");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().dblade = true;
            player.statLifeMax2 += 10;
            player.GetModPlayer<PLAYERGLOBAL>().meleeFlatDamage += 4;
            player.GetModPlayer<PLAYERGLOBAL>().rangedFlatDamage += 4;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<DoransBag>(), 1)
            .Register();
        }
    }
}
