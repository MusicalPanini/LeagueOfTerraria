using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Ammo
{
    public class DuskFeather : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dusk Feather");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.shootSpeed = 3f;
            Item.shoot = ProjectileType<Projectiles.MagicalPlumage_DuskFeather>();
            Item.damage = 9;
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.ammo = ItemType<RazorFeather>();
            Item.notAmmo = false;
            Item.knockBack = 1f;
            Item.value = 15;
            Item.DamageType = DamageClass.Ranged;
            Item.rare = ItemRarityID.Blue;

            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            CreateRecipe(100)
            .AddIngredient(ItemType<CelestialBar>(), 1)
            .AddIngredient(ItemType<RazorFeather>(), 100)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
