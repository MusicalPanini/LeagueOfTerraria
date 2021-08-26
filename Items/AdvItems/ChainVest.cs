using System.Collections.Generic;
using System.Linq;
using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class ChainVest : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chain Vest");
            Tooltip.SetDefault("Armor increased by 4");
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
            player.GetModPlayer<PLAYERGLOBAL>().armor += 4;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<ClothArmor>(), 1)
            .AddIngredient(ItemID.Granite, 20)
            .AddIngredient(ItemID.Chain, 10)
            .AddIngredient(ItemID.Leather, 5)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
