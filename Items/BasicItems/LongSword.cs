using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.BasicItems
{
    public class LongSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Long Sword");
            Tooltip.SetDefault("2% increased melee and ranged damage");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 2, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
            Item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.02f;
            player.GetDamage(DamageClass.Ranged) += 0.02f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup("TerraLeague:IronGroup", 5)
            .AddRecipeGroup("Wood", 5)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
