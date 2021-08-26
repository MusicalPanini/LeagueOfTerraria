using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.Items.Placeable;
using TerraLeague.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class SilversteelBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Silver-Steel Bar");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 99;
            Item.width = 30;
            Item.height = 24;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 2, 75, 0);
            Item.uniqueStack = false;
            Item.createTile = TileType<SilversteelBarTile>();
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
            .AddIngredient(ItemType<Petricite>(), 1)
            .AddRecipeGroup("TerraLeague:SilverGroup", 2)
            .AddIngredient(ItemID.HellstoneBar, 1)
            .AddTile(TileID.Hellforge)
            .Register();
            
        }
    }
}
