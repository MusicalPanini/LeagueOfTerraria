using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.BasicItems
{
    public class RubyCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ruby Crystal");
            Tooltip.SetDefault("Increases maximum life by 10");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            Item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 10;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LifeCrystal, 1)
            .AddIngredient(ItemID.Ruby, 2)
            .AddTile(TileID.Furnaces)
            .Register();
            
        }
    }
}
