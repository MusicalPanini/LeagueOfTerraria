using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.PetrifiedWood
{
    public class PetSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Petrified Wood Sword");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.DamageType = DamageClass.Melee;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = 100;
            Item.rare = ItemRarityID.White;
            Item.UseSound = SoundID.Item1;
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<PetWood>(), 7)
            .AddTile(TileID.WorkBenches)
            .Register();
            base.AddRecipes();
        }

    }

}
