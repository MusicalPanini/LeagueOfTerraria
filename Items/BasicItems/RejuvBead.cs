using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.BasicItems
{
    public class RejuvBead : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rejuvenation Bead");
            Tooltip.SetDefault("Increases life regeneration by 1");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 28;
            Item.value = Item.buyPrice(0, 2, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
            Item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.lifeRegen += 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.StoneBlock, 5)
            .AddIngredient(ItemID.Rope, 1)
            .AddIngredient(ItemID.Daybloom, 1)
            .AddTile(TileID.WorkBenches)
            .Register();
            
        }
    }
}
