using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items
{
    public class PrototypeHexCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prototype Hex Core");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 99;
            Item.width = 26;
            Item.height = 16;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 0, 90, 0);
            Item.uniqueStack = false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Amethyst, 10)
            .AddIngredient(ItemID.Bottle, 1)
            .AddIngredient(ItemID.MeteoriteBar, 2)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
