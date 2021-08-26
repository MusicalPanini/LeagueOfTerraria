using TerraLeague.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Ammo
{
    public class HextechChloroBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech CH-300 Balistic");
            Tooltip.SetDefault("A experimental round that splits into 3 Chlorophite Bolts" +
                "\n...But not always");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.shootSpeed = 3f;
            Item.shoot = ProjectileType<Bullet_HextechChloroShot>();
            Item.damage = 14;
            Item.width = 8;
            Item.height = 16;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.ammo = AmmoID.Bullet;
            Item.knockBack = 2.5f;
            Item.value = 50;
            Item.DamageType = DamageClass.Ranged;
            Item.rare = ItemRarityID.Orange;

            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            CreateRecipe(100)
            .AddIngredient(GetInstance<HextechCore>(), 1)
            .AddIngredient(ItemID.EmptyBullet, 100)
            .AddIngredient(ItemID.ChlorophyteBullet, 300)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
    }
}
