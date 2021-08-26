using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.SummonerSpells
{
    public class VialofTrueMagic : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vial of True Magic");
            
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Orange;
            Item.width = 12;
            Item.height = 30;
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<RawMagic>(), 10)
            .AddIngredient(ItemID.Bottle)
            .AddTile(TileID.Bottles)
            .Register();


            CreateRecipe()
            .AddIngredient(ItemType<RawMagic>(), 5)
            .AddIngredient(ItemID.Bottle)
            .AddTile(TileID.CrystalBall)
            .Register();
        }
    }
}
