using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.BasicItems
{
    public class BFSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("B.F.Sword");
            Tooltip.SetDefault("4% increased melee and ranged damage");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            Item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.04f;
            player.GetDamage(DamageClass.Ranged) += 0.04f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.IronBroadsword, 1)
            .AddRecipeGroup("TerraLeague:IronGroup", 5)
            .AddIngredient(ItemID.Leather, 2)
            .AddTile(TileID.Anvils)
            .Register();


            CreateRecipe()
            .AddIngredient(ItemID.LeadBroadsword, 1)
            .AddRecipeGroup("TerraLeague:IronGroup", 5)
            .AddIngredient(ItemID.Leather, 2)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
