using TerraLeague.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Ammo
{
    public class DarksteelBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darksteel Bullet");
            Tooltip.SetDefault("Struck enemies will start to 'Hemorrhage'" +
                "\nHemorrage stacks up to 5 times");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.shootSpeed = 3f;
            Item.shoot = ProjectileType<Bullet_DarksteelShot>();
            Item.damage = 7;
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 999;
            Item.value = 7;
            Item.consumable = true;
            Item.ammo = AmmoID.Bullet;
            Item.knockBack = 1f;
            Item.value = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.rare = ItemRarityID.Blue;


            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            CreateRecipe(150)
            .AddIngredient(ItemType<DarksteelBar>(), 1)
            .AddIngredient(ItemID.MusketBall, 150)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
