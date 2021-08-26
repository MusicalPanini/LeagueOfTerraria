using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class VoidBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Matter Bar");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 99;
            Item.width = 30;
            Item.height = 24;
            Item.uniqueStack = false;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.createTile = TileType<Tiles.VoidBarTile>();
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(4)
            .AddIngredient(ItemID.ChlorophyteBar, 4)
            .AddIngredient(ItemType<VoidFragment>(), 16)
            .AddTile(TileID.AdamantiteForge)
            .Register();
            
        }
    }
}
