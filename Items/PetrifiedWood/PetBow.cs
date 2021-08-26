using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.PetrifiedWood
{
    public class PetBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Petrified Wood Bow");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.width = 16;
            Item.height = 32;
            Item.useTime = 27;
            Item.useAnimation = 27;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 0f;
            Item.value = 100;
            Item.rare = ItemRarityID.White;
            Item.shootSpeed = 6.6f;
            Item.UseSound = SoundID.Item5;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool CanUseItem(Player player)
        {
            Item.shoot = player.inventory.Where(x => x.ammo == AmmoID.Arrow).First().shoot;
            return base.CanUseItem(player);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<PetWood>(), 10)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
    }
}
