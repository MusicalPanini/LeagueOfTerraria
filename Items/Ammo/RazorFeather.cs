using TerraLeague.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Ammo
{
    public class RazorFeather : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Razor Feather");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.shootSpeed = 1f;
            Item.shoot = ProjectileType<MagicalPlumage_DeadlyFeather>();
            Item.damage = 6;
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.ammo = ItemType<RazorFeather>();
            Item.notAmmo = false;
            Item.knockBack = 1f;
            Item.value = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.rare = ItemRarityID.Blue;

            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            CreateRecipe(100)
            .AddIngredient(GetInstance<ManaBar>(), 1)
            .AddIngredient(ItemID.Feather, 1)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
