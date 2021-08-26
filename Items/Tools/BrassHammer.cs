using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Tools
{
    public class BrassHammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bilgewater Brass Hammer");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Melee;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 20;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5.5f;
            Item.value = 1600;
            Item.rare = ItemRarityID.Blue;
            Item.hammer = 48;
            Item.scale = 1.2f;
            Item.UseSound = SoundID.Item1;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(GetInstance<BrassBar>(), 9)
            .AddRecipeGroup("Wood", 3)
            .AddTile(TileID.Anvils)
            .Register();
            
            base.AddRecipes();
        }
    }
}
