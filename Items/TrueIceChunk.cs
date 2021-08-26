using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items
{
    public class TrueIceChunk : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Ice Chunk");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.width = 24;
            Item.height = 22;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 0, 7, 50);
        }

        public override void AddRecipes()
        {
            CreateRecipe(2)
            .AddIngredient(ItemID.IceBlock, 32)
            .AddTile(TileID.IceMachine)
            .Register();
            
        }
    }
}
