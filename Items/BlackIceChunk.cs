using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class BlackIceChunk : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Black Ice Chunk");
            Tooltip.SetDefault("What was once pure is now cursed..");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.width = 24;
            Item.height = 22;
            Item.rare = ItemRarityID.LightRed;
            Item.value = 20000;
        }

        public override void AddRecipes()
        {
            CreateRecipe(2)
            .AddIngredient(ItemType<TrueIceChunk>(), 2)
            .AddIngredient(ItemID.PurpleIceBlock, 32)
            .AddIngredient(ItemID.SoulofNight, 8)
            .AddTile(TileID.IceMachine)
            .Register();

            CreateRecipe(2)
            .AddIngredient(ItemType<TrueIceChunk>(), 2)
            .AddIngredient(ItemID.RedIceBlock, 32)
            .AddIngredient(ItemID.SoulofNight, 8)
            .AddTile(TileID.IceMachine)
            .Register();
        }
    }
}
