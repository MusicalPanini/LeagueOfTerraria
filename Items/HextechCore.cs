using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items
{
    public class HextechCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Artifical Hex Core");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();

            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 99;
            Item.width = 20;
            Item.height = 24;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 0, 44 * 5, 20);
            Item.uniqueStack = false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.CrystalShard, 30)
            .AddIngredient(ItemID.Bottle, 1)
            .AddIngredient(ItemID.MeteoriteBar, 2)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
