using System.Collections.Generic;
using System.Linq;
using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class ForbiddenIdol : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forbidden Idol");
            Tooltip.SetDefault("Increases ability haste by 10" +
                "\nIncreases mana regeneration by 15%"+
                "\n5% increased healing power");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
            Item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.15;
            player.GetModPlayer<PLAYERGLOBAL>().healPower += 0.05;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<FaerieCharm>(), 2)
            .AddIngredient(ItemType<VoidbornFlesh>(), 20)
            .AddRecipeGroup("TerraLeague:DemonPartGroup", 10)
            .AddTile(TileID.DemonAltar)
            .Register();
            
        }
    }
}
