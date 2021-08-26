using TerraLeague.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Ammo
{
    public class MagicArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Arrow");
            Tooltip.SetDefault("Creates a homing magic projectile on hit");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.shootSpeed = 3f;
            Item.shoot = ProjectileType<Arrow_MagicArrow>();
            Item.damage = 9;
            Item.width = 10;
            Item.height = 28;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.ammo = AmmoID.Arrow;
            Item.knockBack = 3f;
            Item.value = 40;
            Item.rare = ItemRarityID.Blue;
            Item.DamageType = DamageClass.Ranged;
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            CreateRecipe(100)
            .AddIngredient(ItemType<ManaBar>(), 1)
            .AddIngredient(ItemID.WoodenArrow, 100)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
