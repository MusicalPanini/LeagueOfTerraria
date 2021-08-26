using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.BasicItems
{
    public class FaerieCharm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Faerie Charm");
            Tooltip.SetDefault("Increases mana regeneration by 15%");
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
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.15;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.FallenStar, 1)
            .AddIngredient(ItemID.Chain, 1)
            .AddIngredient(ItemID.Moonglow, 2)
            .AddTile(TileID.WorkBenches)
            .Register();
            
        }
    }
}
