using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.StartingItems
{
    public class DoransShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Doran's Shield");
            Tooltip.SetDefault("Increases defence, armor and resist by 2" +
                "\nIncreases health by 15");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 2;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().dsheild = true;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 2;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 2;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<DoransBag>(), 1)
            .Register();
        }
    }
}
