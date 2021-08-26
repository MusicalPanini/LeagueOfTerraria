using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Tools
{
    public class BrassAxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bilgewater Brass Axe");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.DamageType = DamageClass.Melee;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.width = 32;
            Item.height = 28;
            Item.useTime = 20;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4.5f;
            Item.value = 1600;
            Item.rare = ItemRarityID.Blue;
            Item.axe = 11;
            Item.scale = 1.1f;
            Item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<BrassBar>(), 9)
            .AddRecipeGroup("Wood", 3)
            .AddTile(TileID.Anvils)
            .Register();
            
            base.AddRecipes();
        }
    }
}
