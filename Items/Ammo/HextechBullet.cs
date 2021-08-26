using System;
using System.Collections.Generic;
using TerraLeague.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Ammo
{
    public class HextechBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech Bullet");
            Tooltip.SetDefault("An experimental round that splits into 3 shots");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.shootSpeed = 3f;
            Item.shoot = ProjectileType<Bullet_HextechShot>();
            Item.damage = 10;
            Item.width = 8;
            Item.height = 16;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.ammo = AmmoID.Bullet;
            Item.knockBack = 2.5f;
            Item.value = 40;
            Item.DamageType = DamageClass.Ranged;
            Item.rare = ItemRarityID.Orange;

            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            CreateRecipe(100)
            .AddIngredient(ItemType<HextechCore>(), 1)
            .AddIngredient(ItemID.EmptyBullet, 100)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
    }
}
