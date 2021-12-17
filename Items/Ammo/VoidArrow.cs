using TerraLeague.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Ammo
{
    public class VoidArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Arrow");
            Tooltip.SetDefault("Gains 25% damage on hit");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.shootSpeed = 3f;
            Item.shoot = ProjectileType<Arrow_VoidArrow>();
            Item.damage = 8;
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
            CreateRecipe(10)
            .AddIngredient(ItemType<VoidFragment>(), 10)
            .AddIngredient(ItemType<VoidbornFlesh>(), 1)
            .AddIngredient(ItemID.WoodenArrow, 10)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
