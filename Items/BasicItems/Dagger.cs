using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.BasicItems
{
    public class Dagger : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dagger");
            Tooltip.SetDefault("5% increased melee and ranged attack speed");
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
            player.meleeSpeed += 0.05f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.05;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup("TerraLeague:IronGroup", 4)
            .AddRecipeGroup("Wood", 4)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
