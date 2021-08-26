using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.ManaBarItems
{
    public class ManaPickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mana Infused Pickaxe");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Melee;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 20;
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3f;
            Item.value = 12000;
            Item.rare = ItemRarityID.Orange;
            Item.pick = 70;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.tileBoost += 3;
            Item.scale = 1.2f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<ManaBar>(), 16)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
