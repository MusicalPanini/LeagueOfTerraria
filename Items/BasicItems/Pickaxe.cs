using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.BasicItems
{
    public class Pickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pickaxe");
            Tooltip.SetDefault("3% increased melee and ranged damage");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 3, 75, 0);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            Item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.03f;
            player.GetDamage(DamageClass.Ranged) += 0.03f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.IronPickaxe, 1)
            .AddIngredient(ItemID.IronShortsword, 1)
            .AddTile(TileID.Anvils)
            .Register();


            CreateRecipe()
            .AddIngredient(ItemID.LeadPickaxe, 1)
            .AddIngredient(ItemID.LeadShortsword, 1)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
