using TerraLeague.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Ammo
{
    public class PetriciteArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Petricite Arrow");
            Tooltip.SetDefault("Critical strikes deal 50% more damage");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.shootSpeed = 3f;
            Item.shoot = ProjectileType<Arrow_PetriciteArrow>();
            Item.damage = 9;
            Item.width = 10;
            Item.height = 28;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.ammo = AmmoID.Arrow;
            Item.knockBack = 1f;
            Item.value = 40;
            Item.rare = ItemRarityID.Blue;
            Item.DamageType = DamageClass.Ranged;
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            CreateRecipe(100)
            .AddIngredient(GetInstance<Petricite>(), 1)
            .AddIngredient(ItemID.WoodenArrow, 100)
            .AddTile(TileID.Anvils)
            .Register();

        }
    }
}
