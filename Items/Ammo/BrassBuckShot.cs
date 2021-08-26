using TerraLeague.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Ammo
{
    public class BrassBuckShot : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brass Buckshot");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.shootSpeed = 3f;
            Item.shoot = ProjectileType<Bullet_BrassShot>();
            Item.damage = 6;
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.ammo = AmmoID.Bullet;
            Item.value = 5;
            Item.knockBack = 4f;
            Item.value = 5;
            Item.DamageType = DamageClass.Ranged;
            Item.rare = ItemRarityID.Blue;

            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            CreateRecipe(70)
            .AddIngredient(ItemType<BrassBar>(), 1)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
