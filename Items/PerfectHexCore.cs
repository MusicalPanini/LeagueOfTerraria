using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items
{
    public class PerfectHexCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Perfect Hex Core");
            Tooltip.SetDefault("The perfect power source");
            base.SetStaticDefaults();

            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 99;
            Item.width = 26;
            Item.height = 32;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(0, 6, 0, 0);
            Item.uniqueStack = false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<HexCrystal>())
            .AddIngredient(ItemID.Bottle, 1)
            .AddIngredient(ItemID.MeteoriteBar, 2)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }
    }
}
