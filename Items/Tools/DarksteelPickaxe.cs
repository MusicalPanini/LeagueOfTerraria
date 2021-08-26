using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Tools
{
    public class DarksteelPickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darksteel Pick-Axe");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Melee;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 10;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3f;
            Item.value = 22500;
            Item.rare = ItemRarityID.Orange;
            Item.pick = 90;
            Item.axe = 25;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.scale = 1.2f;
            Item.tileBoost -= 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<DarksteelBar>(), 18)
            .AddTile(TileID.Anvils)
            .Register();
            
            base.AddRecipes();
        }
    }
}
