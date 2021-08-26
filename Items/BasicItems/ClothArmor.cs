using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.BasicItems
{
    public class ClothArmor : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cloth Armor");
            Tooltip.SetDefault("Increases armor by 3");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 2, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
            Item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 3;

        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Silk, 6)
            .AddIngredient(ItemID.Leather, 4)
            .AddTile(TileID.Loom)
            .Register();
            
        }
    }
}
